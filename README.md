# RumbleRain
Vibrates [BPio-capable](https://buttplug.io) devices in response to in-game damage events.

## Installation and Running
This mod has yet to be released on [Thudnerstore](https://thunderstore.io).

1. Download the `.zip` from [the relases page](https://github.com/quasikyo/rumble-rain/releases).
2. Go to the settings in the mod manager for the profile you want to install to and select "Import local mod".
3. Select the `.zip` downloaded in step 1.

Before launching the game, start an [Intiface Central](https://intiface.com/central) server. If you have changed the server address from the default (`ws://localhost:12345`), be sure to update the "Server Uri" field in the configuration for this mod.

## Note About Pre-releases
I aim to have all releases of this mod be stable. v1.0.0 will be released once I have added all the features I have envisioned.

I would like to support the following before v1.0.0:
- Heal events
- Able to set negative intensities to reduce vibration
- Kill switch
- Defined acceptable values for configuration options
- and potentially more.

## Justification
There is a [pre-existing mod by MisterKinky](https://thunderstore.io/package/MisterKinky/RiskOfRumble) that also vibrates devices, but it is outdated, no longer maintained, doesn't use Intiface Central, has limited configurability, and will tank your performance. It also is unusable in multiplayer as it doesn't isolate the appropriate user.
This aims to solve all of that.

## Using Lovense Connect
If you're having connectivity issues with Bluetooth, Lovense Connect ([iOS](https://apps.apple.com/us/app/lovense-connect/id1273067916), [Android](https://play.google.com/store/apps/details?id=com.lovense.connect)) may be a better option.

1. Install the app on a phone that is on the same network as your computer
2. Connect the device to the phone via Bluetooth
3. Enable "Lovense Connect Service" in Intiface

See [here](https://docs.intiface.com/docs/intiface-central/brands/lovense#i-cant-get-intifacebuttplug-to-find-lovense-connect-devices) for troubleshooting Lovense Connect, and see [here](https://docs.intiface.com/docs/intiface-central/brands/intro) for other brands.
