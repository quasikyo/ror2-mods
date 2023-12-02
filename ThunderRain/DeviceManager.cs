using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ThunderRain {

	internal class DeviceManager {

		public DeviceManager(string clientName) {
			// UserInfo = new PiShockUser {
			// 	Username = ConfigManager.PiShockUsername.Value,
			// 	ApiKey = ConfigManager.PiShockApiKey.Value,
			// };
			// PiShock = new PiShockApiClient {
			// 	DisplayName = clientName
			// };
			// Log.Info(UserInfo);
			// Log.Info(PiShock);
			MakeRequest();
		}

		async private void MakeRequest() {
			Log.Info("hello");
			string httpRequestContent = JsonSerializer.Serialize(new {
				Username = ConfigManager.PiShockUsername.Value,
				Apikey = ConfigManager.PiShockApiKey.Value,
				Code = ConfigManager.PiShockShareCodes.Value,
				Name = nameof(ThunderRain),
				Op = 1,
				Duration = 1,
				Intensity = 1
			});
			Log.Info("hello2");

			HttpContent httpRequestBody = new StringContent(httpRequestContent, Encoding.UTF8, "application/json");
			Log.Info(await httpRequestBody.ReadAsStringAsync());

			HttpClient client = new HttpClient() {
				BaseAddress = new Uri("https://do.pishock.com/api/apioperate/")
			};
			HttpResponseMessage response = await client.PostAsync("", httpRequestBody);
			string responseContent = await response.Content.ReadAsStringAsync();
			Log.Info(response);
			Log.Info(responseContent);
		}
	}
}
