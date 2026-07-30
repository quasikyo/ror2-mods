using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace ThunderRain {

	/// <summary>
	/// Manages connection and sends operations to PiShock.
	/// </summary>
	internal class DeviceManager {

		private const string PiShockApiBaseUri = "https://api.pishock.com/";
		private static readonly HttpClient HttpClient = new HttpClient() {
			BaseAddress = new Uri(PiShockApiBaseUri)
		};

		/// <summary>
		/// The name used to send operations.
		/// </summary>
		internal string DisplayName { get; private set; }
		/// <summary>
		/// List of shockers that operations can be sent to.
		/// </summary>
		private PiShockShocker[] Shockers { get; set; }

		/// <summary>
		/// Controls which shockers are selected when operations are sent.
		/// </summary>
		internal enum ShockerSelection {
			/// <summary>
			/// Operation is sent to all shockers.
			/// </summary>
			All,
			/// <summary>
			/// Operation is sent to a random shocker.
			/// </summary>
			Random
		}

		public DeviceManager(string displayName) {
			DisplayName = displayName;
			UpdateShockerList(null, null);
			ConfigManager.PiShockShockerIds.SettingChanged += UpdateShockerList;
		}

		private void UpdateShockerList(object _, EventArgs __) {
			Log.Info($"Parsing \"{ConfigManager.PiShockShockerIds.Value}\" for shocker IDs");
			Shockers = ConfigManager.PiShockShockerIds.Value
					.Split(',')
					.Select((string id, int index) => {
						Log.Info($"Shocker ID #{index + 1}: {id}");
						return new PiShockShocker(id.Trim());
					})
					.ToArray();
			Log.Info($"Amount of shockers: {Shockers.Length}");
		}

		private PiShockShocker GetRandomShocker() {
			return Shockers[new Random().Next(0, Shockers.Length)];
		}

		/// <summary>
		/// Uses a <paramref name="pool"/> of <see cref="PiShockValues" /> to send PiShock requests.
		/// </summary>
		internal void ProcessValuePool(ValuePool pool) {
			if (!pool.VibrationValues.IsNill()) {
				Operate(PiShockOperation.Vibrate, pool.VibrationValues);
			}
			if (!pool.ShockValues.IsNill()) {
				Operate(PiShockOperation.Shock, pool.ShockValues);
			}
		}

		/// <summary>
		/// Selects a shocker to be sent a shock, vibrate, or beep <paramref name="operation"/>.
		/// The duration and intensity are determined by <paramref name="values"/>.
		/// </summary>
		/// <param name="operation">Shock, vibrate, or beep.</param>
		/// <param name="values">Duration and intensity of the operation.</param>
		private void Operate(PiShockOperation operation, PiShockValues values) {
			switch (ConfigManager.ShockerSelection.Value) {
				case ShockerSelection.All:
					foreach (PiShockShocker shocker in Shockers) {
						SendOperation(operation, values, shocker);
					};
					break;
				case ShockerSelection.Random:
					SendOperation(operation, values, GetRandomShocker());
					break;
			}
		}

		/// <summary>
		/// Composes and sends an API request to execute
		/// <paramref name="operation"/> on a <paramref name="shocker"/>.
		/// </summary>
		/// <param name="operation">Shock, vibrate, or beep.</param>
		/// <param name="values">Duration and intensity of the operation.</param>
		/// <param name="shocker">Which shocker to send the operation to.</param>
		async private void SendOperation(PiShockOperation operation, PiShockValues values, PiShockShocker shocker) {
			string requestMatcher = $"[{operation}/{shocker.ShockerId}]";
			Log.Info($"{requestMatcher} Sending {operation} to {shocker.ShockerId} with {values}");
			string requestContent = JsonConvert.SerializeObject(new PiShockRequest {
				AgentName = DisplayName,
				DurationMilliseconds = (int) values.Duration.TotalMilliseconds,
				Intensity = (int) values.Intensity,
				IntensityAsPercentage = false,
				Operation = (int) operation,
			});
			HttpRequestMessage pishockRequest = new HttpRequestMessage {
				Method = HttpMethod.Post,
				RequestUri = new Uri( $"/Shockers/{shocker.ShockerId}", UriKind.Relative),
				Headers = {
					{ "X-PiShock-Api-Key", ConfigManager.PiShockApiKey.Value }
				},
				Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
			};
			Log.Debug($"{requestMatcher} Request content: {requestContent}");

			HttpResponseMessage response = await HttpClient.SendAsync(pishockRequest);
			HandleOperationResponse(response, operation, shocker);
		}

		private void HandleOperationResponse(HttpResponseMessage response, PiShockOperation operation, PiShockShocker shocker) {
			string requestMatcher = $"[{operation}/{shocker.ShockerId}]";
			Log.Debug($"{requestMatcher} resulted in {response.StatusCode}");
			if (response.IsSuccessStatusCode) {
				return;
			}

            string userFacingErrorMessage;
            switch (response.StatusCode) {
                case HttpStatusCode.Unauthorized:
				case HttpStatusCode.Forbidden:
					userFacingErrorMessage = "Request could not be authorized. Please ensure your username, API key, and shocker ID(s) are correct.";
					break;
				case HttpStatusCode.NotFound:
					userFacingErrorMessage = $"Shocker ID of {shocker.ShockerId} could not be found.";
					break;
				case HttpStatusCode.MethodNotAllowed:
					userFacingErrorMessage = $"{operation} is not enabled for shocker {shocker.ShockerId}.";
					break;
				case HttpStatusCode.NotAcceptable:
					userFacingErrorMessage = $"Shocker {shocker.ShockerId} is not flashed to V3. Please consult https://docs.pishock.com/pishock/pishock-v3-documentation.html.";
					break;
				default:
					userFacingErrorMessage = $"Shocker ${shocker.ShockerId} failed to ${operation}: error code {response.StatusCode}";
					break;
			}
			Log.Error($"{requestMatcher} {userFacingErrorMessage}");
		}
	}
}
