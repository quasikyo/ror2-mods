using System;
using UnityEngine;

namespace ThunderRain {

	enum PiShockOperation {
		Shock = 0,
		Vibrate = 1,
		Beep = 2
	}

	internal class PiShockRequest {

		internal string Username { get; set; }
		internal string Apikey { get; set; }
		internal string Code { get; set; }
		internal string Name { get; set; }

		internal int Op { get; set; }
		internal int Duration { get; set; }
		internal int Intensity { get; set; }
	}

	internal class PiShockShocker {

		internal string ShareCode { get; set; }

		internal PiShockShocker(string shareCode) {
			ShareCode = shareCode;
		}

	}

	internal class PiShockValues {

		private const int MaxApiDurationSeconds = 15;
		private const int MaxApiIntensity = 100;

		private TimeSpan _duration = TimeSpan.Zero;
		internal TimeSpan Duration {
			get => _duration;
			set {
				float seconds = Mathf.Clamp((float) value.TotalSeconds, 0, MaxApiDurationSeconds);
				_duration = TimeSpan.FromSeconds(seconds);
			}
		}

		private int _intensity = 0;
		internal int Intensity {
			get => _intensity;
			set {
				_intensity = Mathf.Clamp(value, 0, MaxApiIntensity);
			}
		}
	}
}
