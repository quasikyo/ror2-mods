using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using R2API.Utils;

namespace ThunderRain {

	internal class DeviceManager {

		private const string PiShockApiUri = "https://do.pishock.com/api/apioperate/";
		private static HttpClient HttpClient = new HttpClient() {
			BaseAddress = new Uri(PiShockApiUri)
		};

		internal string DisplayName { get; private set; }
		private PiShockShocker[] Shockers { get; set; }

		public DeviceManager(string displayName) {
			DisplayName = displayName;
			UpdateShockerList(null, null);
			ConfigManager.PiShockShareCodes.SettingChanged += UpdateShockerList;
		}

        private void UpdateShockerList(object _, EventArgs __) {
			Log.Debug(ConfigManager.PiShockShareCodes.Value);
			Shockers = ConfigManager.PiShockShareCodes.Value
					.Split(',')
					.Select(code => new PiShockShocker(code.Trim()))
					.ToArray();
			Log.Debug($"Amount of shockers: {Shockers.Length}");
			Array.ForEach(Shockers, (shocker) => Log.Debug($"Shocker share code: {shocker.ShareCode}"));
        }

		private PiShockShocker GetRandomShocker() {
			return Shockers[new Random().Next(0, Shockers.Length)];
		}

        internal void OperateUniform(PiShockOperation operation, PiShockValues values) {
			Array.ForEach(Shockers, (PiShockShocker shocker) => {
				SendCommand(operation, values, shocker);
			});
		}

		internal void OperateRandom(PiShockOperation operation, PiShockValues values) {
			SendCommand(operation, values, GetRandomShocker());
		}

		async private void SendCommand(PiShockOperation operation, PiShockValues values, PiShockShocker shocker) {
			// string requestContent = JsonSerializer.Serialize(new PiShockRequest { // why no worky?
			string requestContent = JsonSerializer.Serialize(new {
				Username = ConfigManager.PiShockUsername.Value,
				Apikey = ConfigManager.PiShockApiKey.Value,
				Code = shocker.ShareCode,
				Name = DisplayName,
				Op = (int) operation,
				Duration = (int) values.Duration.TotalSeconds,
				Intensity = values.Intensity
			});
			Log.Debug($"Request content: {requestContent}");
			HttpContent requestBody = new StringContent(requestContent, Encoding.UTF8, "application/json");

			HttpResponseMessage response = await HttpClient.PostAsync("", requestBody);
			string responseContent = await response.Content.ReadAsStringAsync();
			switch (responseContent) {
				case "Operation Succeeded.":
				case "Operation Attempted.":
					Log.Debug("Request successful");
					break;
				default:
					Log.Error("Request failed for reason: ");
					break;
			}
		}
	}
}
