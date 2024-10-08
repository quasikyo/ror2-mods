using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace ThunderRain {

	/// <summary>
	/// Manages connection and sends operations to PiShock.
	/// </summary>
	internal class DeviceManager {

		private const string PiShockApiUri = "https://do.pishock.com/api/apioperate/";
		private static readonly HttpClient HttpClient = new HttpClient() {
			BaseAddress = new Uri(PiShockApiUri)
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
			ConfigManager.PiShockShareCodes.SettingChanged += UpdateShockerList;
		}

		private void UpdateShockerList(object _, EventArgs __) {
			Log.Info(ConfigManager.PiShockShareCodes.Value);
			Shockers = ConfigManager.PiShockShareCodes.Value
					.Split(',')
					.Select(code => new PiShockShocker(code.Trim()))
					.ToArray();
			Log.Info($"Amount of shockers: {Shockers.Length}");
			foreach (PiShockShocker shocker in Shockers) {
				Log.Info($"Shocker share code: {shocker.ShareCode}");
			};
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
			Log.Info($"Sending {operation} with {values} to {shocker.ShareCode}");
			string requestContent = JsonConvert.SerializeObject(new PiShockRequest {
				Username = ConfigManager.PiShockUsername.Value,
				ApiKey = ConfigManager.PiShockApiKey.Value,
				Code = shocker.ShareCode,
				Name = DisplayName,
				Operation = (int) operation,
				DurationSeconds = (int) values.Duration.TotalSeconds,
				Intensity = (int) values.Intensity
			});
			Log.Debug($"Request content: {requestContent}");
			HttpContent requestBody = new StringContent(requestContent, Encoding.UTF8, "application/json");

			HttpResponseMessage response = await HttpClient.PostAsync("", requestBody);
			Log.Debug($"Received response status code: {response.StatusCode}");
			string responseContent = await response.Content.ReadAsStringAsync();
			switch (responseContent) {
				case "Operation Succeeded.":
				case "Operation Attempted.":
					Log.Debug("Request successful");
					break;
				default:
					Log.Error($"Request failed for reason: {responseContent}");
					break;
			}
		}
	}
}
