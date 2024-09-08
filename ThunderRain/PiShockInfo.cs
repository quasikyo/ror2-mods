using System;
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
		/// Username of the PiShock owner.
		/// </summary>
		[JsonProperty("Username")]
		internal string Username { get; set; }

		/// <summary>
		/// API key belonging to the PiShock owner's account.
		/// </summary>
		[JsonProperty("Apikey")]
		internal string ApiKey { get; set; }

		/// <summary>
		/// Share code of the shocker to operate.
		/// </summary>
		[JsonProperty("Code")]
		internal string Code { get; set; }

		/// <summary>
		/// Name of the entity executing the operation.
		/// </summary>
		[JsonProperty("Name")]
		internal string Name { get; set; }

		/// <summary>
		/// <see cref="PiShockOperation">Operation type.</see>
		/// 0 = Shock
		/// 1 = Vibrate
		/// 2 = Beep
		/// </summary>
		[JsonProperty("Op")]
		internal int Operation { get; set; }

		/// <summary>
		/// Duration of the operation in seconds from <c>[0, 15]</c>.
		/// </summary>
		[JsonProperty("Duration")]
		internal int DurationSeconds { get; set; }

		/// <summary>
		/// Intensity of the operations from <c>[0, 100]</c>.
		/// </summary>
		[JsonProperty("Intensity")]
		internal int Intensity { get; set; }
	}

	/// <summary>
	/// Represents an individual shocker that can be targeted for operation.
	/// </summary>
	internal class PiShockShocker {

		/// <summary>
		/// The share code of the shocker generated on the PiShock portal.
		/// </summary>
		internal string ShareCode { get; set; }

		internal PiShockShocker(string shareCode) {
			ShareCode = shareCode;
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
		/// Duration of the operation from <c>[0, 15]</c> seconds.
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
