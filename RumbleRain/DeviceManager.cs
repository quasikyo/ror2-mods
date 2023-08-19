using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using BepInEx.Logging;

using Buttplug.Core;
using Buttplug.Client;
using Buttplug.Client.Connectors.WebsocketConnector;

namespace RumbleRain {
	/// <summary>
	/// Manages connections to ButtplugIO enabled devices and controlls their vibrations.
	/// </summary>
	internal class DeviceManager {

		internal readonly static TimeSpan VibrationPollingRate = TimeSpan.FromSeconds(ConfigManager.PollingRateSeconds.Value);

		private ManualLogSource Logger { get; set; }
		private ButtplugClient ButtplugClient { get; set; }
		private List<ButtplugClientDevice> ConnectedDevices { get; set; }
		private bool DevicesAreStopped { get; set; }

		private VibrationInfoProvider VibrationInfoProvider { get; set; }

		public DeviceManager(VibrationInfoProvider vibrationInfoProvider, ManualLogSource logger, string clientName = "RumbleRain") {
			VibrationInfoProvider = vibrationInfoProvider;
			Logger = logger;

			ConnectedDevices = new List<ButtplugClientDevice>();
			ButtplugClient = new ButtplugClient(clientName);
			Logger.LogInfo("BP client created for " + clientName);
			ButtplugClient.DeviceAdded += HandleDeviceAdded;
			ButtplugClient.DeviceRemoved += HandleDeviceRemoved;
			DevicesAreStopped = true;
		}

		/// <summary>
		/// Connects to the ButtplugClient and begins scanning for and connecting found devices.
		/// </summary>
		public async void ConnectDevices() {
			try {
				Logger.LogInfo("Connecting to websocket server");
				await ButtplugClient.ConnectAsync(new ButtplugWebsocketConnector(new Uri(ConfigManager.ServerUri.Value)));
			} catch (ButtplugException exception) {
				Logger.LogError($"Connection failed: {exception}");
			}

			try {
				await ButtplugClient.StartScanningAsync();
			} catch (ButtplugException exception) {
				Logger.LogError($"Scanning for devices failed: {exception}");
			}
		}

		/// <summary>
		/// Causes the connected devices to vibrate according to periodically performed calculations.
		/// </summary>
		internal IEnumerator PollVibrations() {
			while (true) {
				yield return new WaitForSeconds((float)VibrationPollingRate.TotalSeconds);

				if (!ButtplugClient.Connected || (VibrationInfoProvider.VibrationInfo.IsImpotent() && DevicesAreStopped)) {
					continue;
				}  else if (VibrationInfoProvider.VibrationInfo.IsImpotent() && !DevicesAreStopped) {
					StopConnectedDevices();
					continue;
				}

				VibrationInfoProvider.UpdateVibrationInfo(VibrationPollingRate);
				VibrateConnectedDevices(VibrationInfoProvider.VibrationInfo.Intensity);
				Logger.LogDebug($"{VibrationInfoProvider.VibrationInfo} from {VibrationInfoProvider}");
			}
		}

		/// <summary>
		/// Updates the calculations used in <c>PollVibrations</c> and immediately
		/// causes the connected devices to vibrate with the given <paramref name="vibrationInfo"/>.
		/// </summary>
		internal void SendVibrationInfo(VibrationInfo vibrationInfo) {
			VibrationInfoProvider.Input(vibrationInfo);
			VibrateConnectedDevices(VibrationInfoProvider.VibrationInfo.Intensity);
		}

		/// <summary>
		/// Vibrates each connected device for the given <paramref name="intensity"/>.
		/// </summary>
		/// <param name="intensity">Value in the range <c>[0, 1]</c>.</param>
		private void VibrateConnectedDevices(double intensity) {
			DevicesAreStopped = false;
			ConnectedDevices.ForEach(async (ButtplugClientDevice device) => {
				int vibratorMotorCount = device.VibrateAttributes.Count;
				double[] intensityPerMotor = new double[vibratorMotorCount];
				for (int i = 0; i < intensityPerMotor.Length; ++i) {
					intensityPerMotor[i] = intensity;
				}

				await device.VibrateAsync(intensityPerMotor);
			});
		}

		private void StopConnectedDevices() {
			Logger.LogDebug($"Stop called");
			ConnectedDevices.ForEach(async device => await device.Stop());
			DevicesAreStopped = true;
		}

		private void HandleDeviceAdded(object sender, DeviceAddedEventArgs args) {
			if (!IsVibratableDevice(args.Device)) return;
			Logger.LogInfo($"{args.Device.Name} connected to client {ButtplugClient.Name}");
			ConnectedDevices.Add(args.Device);
		}

		private void HandleDeviceRemoved(object sender, DeviceRemovedEventArgs args) {
			if (!IsVibratableDevice(args.Device)) return;
			Logger.LogInfo($"{args.Device.Name} disconnected from client {ButtplugClient.Name}");
			ConnectedDevices.Remove(args.Device);
		}

		private bool IsVibratableDevice(ButtplugClientDevice device) {
			return device.VibrateAttributes.Count > 0;
		}
	}
}
