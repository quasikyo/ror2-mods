using System;


namespace RumbleRain {
	/// <summary>
	/// Class describing vibrations.
	/// </summary>
	internal class VibrationInfo {
		private double _intensity;
		/// <summary>
		/// The percentage of intensity of the vibration in the range [0, 1].
		/// </summary>
		protected internal double Intensity {
			get => _intensity;
			set => _intensity = Clamp(value, 0, ConfigManager.MaximumVibrationIntensity.Value);
		}

		private TimeSpan _duration;
		/// <summary>
		/// The duration that the vibration should last.
		/// Treats <c>TimeSpan.Zero</c> as a minimum.
		/// </summary>
		protected internal TimeSpan Duration {
			get => _duration;
			set => _duration = Clamp(value, TimeSpan.Zero, TimeSpan.FromSeconds(ConfigManager.MaximumVibrationDurationSeconds.Value));
		}

		private double _intensitySnapshot;
		/// <summary>
		/// A way to snapshot an intensity for future comparison.
		/// </summary>
		protected internal double IntensitySnapshot {
			get => _intensitySnapshot;
			set => _intensitySnapshot = Clamp(value, 0, ConfigManager.MaximumVibrationIntensity.Value);
		}

		private TimeSpan _durationSnapshot;
		/// <summary>
		/// A way to snapshot a duration for future comparison.
		/// </summary>
		protected internal TimeSpan DurationSnapshot {
			get => _durationSnapshot;
			set => _durationSnapshot = Clamp(value, TimeSpan.Zero, TimeSpan.FromSeconds(ConfigManager.MaximumVibrationDurationSeconds.Value));
		}

		internal VibrationInfo() : this(0, TimeSpan.Zero) { }

		internal VibrationInfo(double intensity) : this(intensity, TimeSpan.Zero) { }

		internal VibrationInfo(TimeSpan duration) : this(0, duration) { }

		internal VibrationInfo(double intensity, TimeSpan duration) {
			Intensity = intensity;
			Duration = duration;
			SnapshotValues();
		}

		private T Clamp<T>(T value, T inclusiveMinimum, T inclusiveMaximum) where T : IComparable<T> {
			if (value.CompareTo(inclusiveMinimum) < 0) return inclusiveMinimum;
			else if (value.CompareTo(inclusiveMaximum) > 0) return inclusiveMaximum;
			return value;
		}

		public static VibrationInfo operator +(VibrationInfo a, VibrationInfo b) {
			return new VibrationInfo(a.Intensity + b.Intensity, a.Duration + b.Duration);
		}

		/// <summary>
		/// Convenience method to snapshot current values.
		/// </summary>
		protected internal void SnapshotValues() {
			IntensitySnapshot = Intensity;
			DurationSnapshot = Duration;
		}

		protected internal bool IsImpotent() {
			return Intensity <= 0 || Duration <= TimeSpan.Zero;
		}

		public override string ToString() {
			return $"{base.ToString()}(intensity={Intensity}, duration={Duration.TotalSeconds} seconds)";
		}
	}
}
