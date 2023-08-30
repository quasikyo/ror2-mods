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

		private ManualLogSource Logger { get; set; }
		private ButtplugClient ButtplugClient { get; set; }
		private List<ButtplugClientDevice> ConnectedDevices { get; set; }
		private bool DevicesAreStopped { get; set; }

		private VibrationInfoProvider _vibrationInfoProvider;
		internal VibrationInfoProvider VibrationInfoProvider {
			get => _vibrationInfoProvider;
			set {
				StopConnectedDevices();
				_vibrationInfoProvider = value;
			}
		}

		public DeviceManager(VibrationInfoProvider vibrationInfoProvider, ManualLogSource logger, string clientName = "RumbleRain") {
			Logger = logger;

			ConnectedDevices = new List<ButtplugClientDevice>();
			ButtplugClient = new ButtplugClient(clientName);
			Logger.LogInfo("BP client created for " + clientName);
			ButtplugClient.DeviceAdded += HandleDeviceAdded;
			ButtplugClient.DeviceRemoved += HandleDeviceRemoved;

			VibrationInfoProvider = vibrationInfoProvider;
			DevicesAreStopped = true;
		}

		/// <summary>
		/// Connects to the ButtplugClient and begins scanning for and connecting found devices.
		/// </summary>
		public async void ConnectDevices() {
			try {
				Logger.LogInfo($"Attempting to connect to Intiface server at \"{ConfigManager.ServerUri.Value}\"");
				await ButtplugClient.ConnectAsync(new ButtplugWebsocketConnector(new Uri(ConfigManager.ServerUri.Value)));
				Logger.LogInfo("Connection successful. Beginning scan for devices");
				await ButtplugClient.StartScanningAsync();
			} catch (ButtplugHandshakeException exception) {
				Logger.LogError($"Attempt to connect to Intiface server failed: {exception}");
			} catch (ButtplugException exception) {
				Logger.LogError($"ButtplugIO error occured while connecting devices: {exception}");
			}
		}

		/// <summary>
		/// Causes the connected devices to vibrate according to periodically performed calculations.
		/// </summary>
		internal IEnumerator PollVibrations() {
			Logger.LogInfo($"Beginning polling every {ConfigManager.PollingRateSeconds.Value} seconds");
			while (true) {
				// calculations become desynced if updated while waiting so snapshot the value
				float secondsToWaitFor = ConfigManager.PollingRateSeconds.Value;
				yield return new WaitForSeconds(secondsToWaitFor);

				if (!ButtplugClient.Connected || DevicesAreStopped) {
					continue;
				} else if (VibrationInfoProvider.VibrationInfo.IsImpotent() && !DevicesAreStopped) {
					StopConnectedDevices();
					continue;
				}

				Logger.LogDebug($"{VibrationInfoProvider.VibrationInfo}");
				VibrationInfoProvider.UpdateVibrationInfo(TimeSpan.FromSeconds(secondsToWaitFor));
				VibrateConnectedDevices(VibrationInfoProvider.VibrationInfo.Intensity);
			}
		}

		/// <summary>
		/// Updates the <c>VibrationInfoProvider</c> with the given <paramref name="vibrationInfo"/>
		/// and immediately causes the connected devices to vibrate with the newly provided info.
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

		/// <summary>
		/// Stops all connected devices.
		/// </summary>
		private void StopConnectedDevices() {
			ConnectedDevices.ForEach(async device => await device.Stop());
			DevicesAreStopped = true;
		}

		/// <summary>
		///	Convenience method for toggling the devices on and off.
		/// </summary>
		internal void ToggleConnectedDevices() {
			bool devicesNeedToBeStopped = !DevicesAreStopped;
			if (devicesNeedToBeStopped) {
				StopConnectedDevices();
			} else {
				VibrateConnectedDevices(VibrationInfoProvider.VibrationInfo.Intensity);
			}
		}

		/// <summary>
		/// Stops all connected devices and resets the calculations used for vibrations.
		/// </summary>
		internal void CleanUp() {
			StopConnectedDevices();
			VibrationInfoProvider.Reset();
		}

		private void HandleDeviceAdded(object sender, DeviceAddedEventArgs args) {
			if (!IsVibratableDevice(args.Device)) { return; }

			Logger.LogInfo($"{args.Device.Name} connected to client {ButtplugClient.Name}");
			ConnectedDevices.Add(args.Device);
		}

		private void HandleDeviceRemoved(object sender, DeviceRemovedEventArgs args) {
			if (!IsVibratableDevice(args.Device)) { return; }

			Logger.LogInfo($"{args.Device.Name} disconnected from client {ButtplugClient.Name}");
			ConnectedDevices.Remove(args.Device);
		}

		private bool IsVibratableDevice(ButtplugClientDevice device) {
			return device.VibrateAttributes.Count > 0;
		}
	}
}
