using System;

namespace RumbleRain {
	/// <summary>
	/// Class that provides vibration information in relation to inputs and time.
	/// </summary>
	internal abstract class VibrationInfoProvider {

		internal enum VibrationBehavior {
			AdditiveWithLinearDecay,
			AdditiveWithExponentialDecay
		}

		protected internal VibrationInfo VibrationInfo { get; set; }

		protected internal VibrationInfoProvider() {
			VibrationInfo = new VibrationInfo();
		}

		protected internal static VibrationInfoProvider From(VibrationBehavior vibrationBehavior) {
			switch (vibrationBehavior) {
				case VibrationBehavior.AdditiveWithLinearDecay:
					return new AdditiveInfoProviderWithLinearDecay();
				case VibrationBehavior.AdditiveWithExponentialDecay:
					return new AdditiveInfoProvierWithExponentialDecay();
				default:
					throw new ArgumentException($"{vibrationBehavior} is not a valid behavior.");
			}
		}

		/// <summary>
		/// Used to feed the <c>VibrationInfoProvider</c> new information.
		/// </summary>
		protected internal abstract void Input(VibrationInfo newVibrationInfo);

		/// <summary>
		/// <c>UpdateVibrationInfo</c> should be called periodically to update vibration info.
		/// </summary>
		/// <param name="timeSinceLastUpdate">Time since this method was last called by its caller.</param>
		protected internal abstract void UpdateVibrationInfo(TimeSpan timeSinceLastUpdate);
	}

	/// <summary>
	/// Simply adds new <c>VibrationInfo</c> to current values and decays the intensity linearly with the duration.
	/// </summary>
	internal class AdditiveInfoProviderWithLinearDecay : VibrationInfoProvider {
		protected internal override void Input(VibrationInfo newVibrationInfo) {
			VibrationInfo += newVibrationInfo;
			VibrationInfo.SnapshotValues();
		}

		protected internal override void UpdateVibrationInfo(TimeSpan timeSinceLastUpdate) {
			// This also works but is harder to follow.
			//TimeSpan remainingDuration = VibrationInfo.Duration - timeSinceLastUpdate;
			//double intensityScalar = remainingDuration.TotalSeconds / VibrationInfo.Duration.TotalSeconds;
			//VibrationInfo.Intensity *= intensityScalar;
			//VibrationInfo.Duration = remainingDuration;

			VibrationInfo.Duration -= timeSinceLastUpdate;
			double intensityScalar = VibrationInfo.Duration.TotalSeconds / VibrationInfo.DurationSnapshot.TotalSeconds;
			VibrationInfo.Intensity = VibrationInfo.IntensitySnapshot * intensityScalar;
		}
	}

	/// <summary>
	/// Simply adds new <c>VibrationInfo</c> to current values and decays the intensity exponentially with the duration.
	/// </summary>
	internal class AdditiveInfoProvierWithExponentialDecay : VibrationInfoProvider {
		protected internal override void Input(VibrationInfo newVibrationInfo) {
			VibrationInfo += newVibrationInfo;
			VibrationInfo.SnapshotValues();
		}

		protected internal override void UpdateVibrationInfo(TimeSpan timeSinceLastUpdate) {
			VibrationInfo.Duration -= timeSinceLastUpdate;
			double intensityScalar = VibrationInfo.Duration.TotalSeconds / VibrationInfo.DurationSnapshot.TotalSeconds;
			VibrationInfo.Intensity *= intensityScalar;
		}
	}
}

// Adaptive = depends on amount of damage dealt/taken relative to max health of entity receiving damage
// Static = unchanging amount that is configurable

// AdaptiveIntensity AdaptiveDuration
// AdaptiveIntensity StaticDuration
// StaticIntensity AdaptiveDuration
// StaticIntensity StaticDuration
// RelativeToCurrentHealth

// how to add TimeDecay?
// lot's of repetition; how to abstract to reduce code duplication
// could add a Factory that passes in booleans
