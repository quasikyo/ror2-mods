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
		}

		internal void SetActive() {
			Status = PoolStatus.Active;
		}

		internal void Reset() {
			Status = PoolStatus.Empty;
			VibrationValues = new PiShockValues();
			ShockValues = new PiShockValues();
		}
	}
}
