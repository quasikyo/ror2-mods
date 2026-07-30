using System;
using EntityStates.Seeker;
using Newtonsoft.Json;
using UnityEngine;

namespace ThunderRain {

	enum PiShockOperation {
		Shock = 0,
		Vibrate = 1,
		Beep = 2
	}

	/// <summary>
	/// Fields required to send a request to the PiShock API.
	/// </summary>
	internal class PiShockRequest {

		/// <summary>
		/// Name of the entity executing the operation.
		/// </summary>
		[JsonProperty("AgentName")]
		[Obsolete("This field is being deprecated by the PiShock API and will be replaced by the name of the API key")]
		internal string AgentName { get; set; }

		/// <summary>
		/// Duration of the operation in milliseconds from <c>[16, 15000]</c>.
		/// </summary>
		[JsonProperty("Duration")]
		internal int DurationMilliseconds { get; set; }

		/// <summary>
		/// Intensity of the operations from <c>[0, 100]</c>.
		/// </summary>
		[JsonProperty("Intensity")]
		internal int Intensity { get; set; }

		/// <summary>
		/// If <c>Intensity</c> should act a percentage of the shocker's configured maximum.
		/// </summary>
		[JsonProperty("IntensityAsPercentage")]
		internal bool IntensityAsPercentage { get; set; }

		/// <summary>
		/// <see cref="PiShockOperation">Operation type.</see>
		/// 0 = Shock
		/// 1 = Vibrate
		/// 2 = Beep
		/// </summary>
		[JsonProperty("Operation")]
		internal int Operation { get; set; }
	}

	/// <summary>
	/// Represents an individual shocker that can be targeted for operation.
	/// </summary>
	internal class PiShockShocker {

		/// <summary>
		/// The ID of the shocker found on the PiShock portal.
		/// </summary>
		internal string ShockerId { get; set; }

		internal PiShockShocker(string shockerId) {
			ShockerId = shockerId;
		}

	}

	/// <summary>
	/// Helper class that stores and clamps duration and intensity values for <see cref="PiShockOperation" />s.
	/// </summary>
	internal class PiShockValues {

		internal const int MaxApiDurationSeconds = 15;
		internal const int MaxApiIntensity = 100;

		private int MaxIntensity { get; set; }
		private TimeSpan MaxDuration { get; set; }

		// want to use floats to capture damage that results in <1

		private TimeSpan _duration = TimeSpan.Zero;
		/// <summary>
		/// Duration of the operation from <c>(0, 15]</c> seconds.
		/// </summary>
		internal TimeSpan Duration {
			get => _duration;
			set {
				float seconds = Mathf.Clamp((float) value.TotalSeconds, 0, (float) MaxDuration.TotalSeconds);
				_duration = TimeSpan.FromSeconds(seconds);
			}
		}

		private float _intensity = 0;
		/// <summary>
		/// Intensity of the operation from <c>[0, 100]</c>.
		/// </summary>
		internal float Intensity {
			get => _intensity;
			set {
				_intensity = Mathf.Clamp(value, 0, MaxIntensity);
			}
		}

		internal PiShockValues(int maxIntensity, int maxDurationSeconds) {
			MaxIntensity = maxIntensity;
			MaxDuration = TimeSpan.FromSeconds(maxDurationSeconds);
		}

		internal PiShockValues() : this(MaxApiIntensity, MaxApiDurationSeconds) {}

		internal bool IsNill() {
			bool isDurationNill = Duration.TotalSeconds < 1;
			bool isIntensityNill = Intensity < 1;
			return isDurationNill || isIntensityNill;
		}

		public override string ToString() {
			return $"PiShockValues(Intensity={Intensity}, Duration={Duration.TotalSeconds}s)";
		}
	}
}
