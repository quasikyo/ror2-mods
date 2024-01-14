using System;

namespace ThunderRain {

	/// <summary>
	/// Creates <see cref="PiShockValues"/> for each <see cref="PiShockOperation" />.
	/// </summary>
	internal class ValuePool {

		internal enum PoolStatus {
			Empty,
			Active
		}

		internal PoolStatus Status { get; private set; }

		/// <summary>
		/// Values for <see cref="PiShockOperation.Vibrate" />
		/// </summary>
		internal PiShockValues VibrationValues { get; set; }

		/// <summary>
		/// Values for <see cref="PiShockOperation.Shock" />
		/// </summary>
		internal PiShockValues ShockValues { get; set; }

		internal ValuePool() {
			Reset();
			ConfigManager.MaximumShockDuration.SettingChanged += ResetOnSettingChange;
			ConfigManager.MaximumShockIntensity.SettingChanged += ResetOnSettingChange;
			ConfigManager.MaximumVibrationDuration.SettingChanged += ResetOnSettingChange;
			ConfigManager.MaximumVibrationIntensity.SettingChanged += ResetOnSettingChange;
		}

		internal void SetActive() {
			Status = PoolStatus.Active;
		}

		private void ResetOnSettingChange(object sender, EventArgs e) {
			Reset();
		}

		internal void Reset() {
			Status = PoolStatus.Empty;
			VibrationValues = new PiShockValues(ConfigManager.MaximumVibrationIntensity.Value, ConfigManager.MaximumVibrationDuration.Value);
			ShockValues = new PiShockValues(ConfigManager.MaximumShockIntensity.Value, ConfigManager.MaximumShockDuration.Value);
		}
	}
}
