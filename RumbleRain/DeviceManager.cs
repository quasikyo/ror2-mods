using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Buttplug.Core;
using Buttplug.Client;
using Buttplug.Client.Connectors.WebsocketConnector;

namespace RumbleRain {
	/// <summary>
	/// Manages connections to ButtplugIO enabled devices and controlls their vibrations.
	/// </summary>
	internal class DeviceManager {

		/// <summary>
		/// Different possible states the <c>DeviceManager</c> can be in.
		/// </summary>
		private enum DeviceState {
			/// <summary>
			/// Open to input and devices are vibrating.
			/// </summary>
			Active,
			/// <summary>
			/// Open to input and devices are not vibrating.
			/// </summary>
			Inactive,
			/// <summary>
			/// Closed to input and devices are not vibrating.
			/// </summary>
			Paused
		}

		private DeviceState _state;
		private DeviceState State {
			get => _state;
			set {
				Log.Debug($"State updated from {_state} to {value}");
				_state = value;
			}
		}

		private ButtplugClient ButtplugClient { get; set; }
		private List<ButtplugClientDevice> ConnectedDevices { get; set; }

		private VibrationInfoProvider _infoProvider;
		internal VibrationInfoProvider InfoProvider {
			get => _infoProvider;
			set {
				StopConnectedDevices();
				_infoProvider = value;
			}
		}

		public DeviceManager(VibrationInfoProvider vibrationInfoProvider, string clientName) {
			State = DeviceState.Inactive;

			ConnectedDevices = new List<ButtplugClientDevice>();
			ButtplugClient = new ButtplugClient(clientName);
			Log.Info("BP client created for " + clientName);
			ButtplugClient.DeviceAdded += HandleDeviceAdded;
			ButtplugClient.DeviceRemoved += HandleDeviceRemoved;

			InfoProvider = vibrationInfoProvider;
		}

		/// <summary>
		/// Connects to the ButtplugClient and begins scanning for and connecting found devices.
		/// </summary>
		public async void ConnectDevices() {
			try {
				Log.Info($"Attempting to connect to Intiface server at \"{ConfigManager.ServerUri.Value}\"");
				await ButtplugClient.ConnectAsync(new ButtplugWebsocketConnector(new Uri(ConfigManager.ServerUri.Value)));
				Log.Info("Connection successful. Beginning scan for devices");
				await ButtplugClient.StartScanningAsync();
			} catch (ButtplugHandshakeException exception) {
				Log.Error($"Attempt to connect to Intiface server failed: {exception}");
			} catch (ButtplugException exception) {
				Log.Error($"ButtplugIO error occured while connecting devices: {exception}");
			}
		}

		/// <summary>
		/// Causes the connected devices to vibrate according to periodically performed calculations.
		/// </summary>
		internal IEnumerator PollVibrations() {
			Log.Info($"Beginning polling every {ConfigManager.PollingRateSeconds.Value} seconds");
			while (true) {
				// calculations become desynced if config is updated while waiting so snapshot the value
				float secondsToWaitFor = ConfigManager.PollingRateSeconds.Value;
				yield return new WaitForSeconds(secondsToWaitFor);

				if (!ButtplugClient.Connected || State != DeviceState.Active) {
					continue;
				} else if (InfoProvider.Info.IsImpotent() && State == DeviceState.Active) {
					StopConnectedDevices();
					continue;
				}

				Log.Debug($"{InfoProvider.Info}");
				InfoProvider.UpdateVibrationInfo(TimeSpan.FromSeconds(secondsToWaitFor));
				VibrateConnectedDevices(InfoProvider.Info.Intensity);
			}
		}

		/// <summary>
		/// Updates the <c>VibrationInfoProvider</c> with the given <paramref name="vibrationInfo"/>
		/// and immediately causes the connected devices to vibrate with the newly provided info.
		/// </summary>
		internal void SendVibrationInfo(VibrationInfo vibrationInfo) {
			if (State == DeviceState.Paused) { return; }

			InfoProvider.Input(vibrationInfo);
			VibrateConnectedDevices(InfoProvider.Info.Intensity);
		}

		/// <summary>
		/// Vibrates each connected device for the given <paramref name="intensity"/>.
		/// </summary>
		/// <param name="intensity">Value in the range <c>[0, 1]</c>.</param>
		private void VibrateConnectedDevices(double intensity) {
			State = DeviceState.Active;

			// new List<ButtplugClientDevice>(ButtplugClient.Devices).ForEach(x => Log.Info(x.Name));
			ConnectedDevices.ForEach(async (ButtplugClientDevice device) => {
				await device.VibrateAsync(intensity);
			});
		}

		/// <summary>
		/// Stops all connected devices and sets internal state to <paramref name="newState"/>.
		/// </summary>
		/// <param name="newState">Either or <c>Inactive</c> or <c>Paused</c>.</param>
		private void StopConnectedDevices(DeviceState newState = DeviceState.Inactive) {
			if (newState == DeviceState.Active) {
				throw new ArgumentException($"{nameof(newState)}={newState} is invalid. Expecting {DeviceState.Inactive} or {DeviceState.Active}.");
			}

			State = newState;
			// await ButtplugClient?.StopAllDevicesAsync(); // throwing NullReferenceException
			ConnectedDevices.ForEach(async (ButtplugClientDevice device) => await device.Stop());
		}

		/// <summary>
		///	Convenience method for pausing and resuming vibrations.
		/// </summary>
		internal void ToggleConnectedDevices() {
			switch (State) {
				case DeviceState.Active:
				case DeviceState.Inactive:
					StopConnectedDevices(DeviceState.Paused);
					break;
				case DeviceState.Paused:
					VibrateConnectedDevices(InfoProvider.Info.Intensity);
					break;
			}
		}

		/// <summary>
		/// Stops all connected devices and resets the calculations used for vibrations.
		/// </summary>
		internal void CleanUp() {
			StopConnectedDevices();
			InfoProvider.Reset();
		}

		private void HandleDeviceAdded(object sender, DeviceAddedEventArgs args) {
			if (!IsVibratableDevice(args.Device)) {
				Log.Info($"{args.Device.Name} was detected but ignored due to it not being vibratable.");
				return;
			}

			Log.Info($"{args.Device.Name} connected to client {ButtplugClient.Name}");
			ConnectedDevices.Add(args.Device);
		}

		private void HandleDeviceRemoved(object sender, DeviceRemovedEventArgs args) {
			if (!IsVibratableDevice(args.Device)) {
				Log.Info($"{args.Device.Name} was detected but ignored due to it not being vibratable.");
				return;
			}

			Log.Info($"{args.Device.Name} disconnected from client {ButtplugClient.Name}");
			ConnectedDevices.Remove(args.Device);
		}

		private bool IsVibratableDevice(ButtplugClientDevice device) {
			return device.VibrateAttributes.Count > 0;
		}
	}
}
