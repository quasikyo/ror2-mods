# RumbleRain
Vibrates [BPio-capable](https://buttplug.io) devices in response to in-game damage events.

## Installation and Running
Before launching the game, start an [Intiface Central](https://intiface.com/central) server. If you have changed the server address from the default (`ws://localhost:12345`), be sure to update the "Server Uri" field in the configuration for this mod.

#### Thunderstore
1. Allow NSFW in the mod manager's filter categories
2. Search "RumbleRain" and download

#### Manual
1. Download the `.zip` from either:
   - [the relases page on GitHub](https://github.com/quasikyo/rumble-rain/releases)
   - the Manual Download button on [the Thunderstore page](https://thunderstore.io/package/quasikyo/RumbleRain)
2. Go to the settings in the mod manager for the profile you want to install to and select "Import local mod".
3. Select the `.zip` downloaded in step 1.

## Configuration
- Whether to trigger on dealing and/or receiving damage
- Severity of vibrations
- Duration of vibrations
- Vibration falloff

The config file and in-game [Risk Of Options]() menu provide descriptions and valid values for each of the specific options.

## Issue Reporting and Suggestions
Submit a new issue [here](https://github.com/quasikyo/rumble-rain/issues).

## Note About Pre-releases
I aim to have all releases of this mod be stable. v1.0.0 will be released once I have added all the features I have envisioned.

I would like to support the following before v1.0.0:
- Kill switch
- Heal events
- Able to set negative intensities to reduce vibration
- and potentially more.

## Justification
There is a [pre-existing mod by MisterKinky](https://thunderstore.io/package/MisterKinky/RiskOfRumble) that also vibrates devices, but it is outdated, no longer maintained, doesn't use Intiface Central, has limited configurability, and will tank your performance. It also is unusable in multiplayer as it doesn't isolate the appropriate user.
This mod aims to solve all of that.

## Using Lovense Connect
If you're having connectivity issues with Bluetooth, Lovense Connect ([iOS](https://apps.apple.com/us/app/lovense-connect/id1273067916), [Android](https://play.google.com/store/apps/details?id=com.lovense.connect)) may be a better option.

1. Install the app on a phone that is on the same network as your computer
2. Connect the device to the phone via Bluetooth
3. Enable "Lovense Connect Service" in Intiface

See [here](https://docs.intiface.com/docs/intiface-central/brands/lovense#i-cant-get-intifacebuttplug-to-find-lovense-connect-devices) for troubleshooting Lovense Connect, and see [here](https://docs.intiface.com/docs/intiface-central/brands/intro) for other brands.
