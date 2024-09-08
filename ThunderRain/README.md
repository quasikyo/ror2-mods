# ThunderRain
Operates [PiShocks](https://pishock.com) in response to in-game damage events.

## Safety
Please configure shock maximums in the config and the share code.

## Changelog - 1.1.1
- Switch to `Newtonsoft.Json` from `System.Text.Json` to reduce dependencies

For the full changelog, see:
- [the GitHub releases page](https://github.com/quasikyo/ror2-mods/releases?q=ThunderRain),
- [the Thunderstore changelog page](https://thunderstore.io/package/quasikyo/ThunderRain/changelog), or
- [the local `CHANGELOG.md` file](./Thunderstore/CHANGELOG.md)

## Issue Reporting and Suggestions
Contact me on Discord (`quasikyo`) or submit a new issue [here](https://github.com/quasikyo/ror2-mods/issues).

## Configuration

#### PiShock Info
- PiShock Username
- PiShock API Key generated on your [PiShock account page](https://pishock.com/#/account)
- PiShock Share Codes
  - These are the alphanumeric share codes generated per shocker on the [PiShock control panel](https://pishock.com/#/control)
  - Example: `2A1A18697BC`

#### Activated By
The in-game triggers to enable.

- Vibrations from dealing damage
- Vibrations from receiving damage
- Shocks from dealing damage
- Shocks from receiving damage
- Shocks on death
- Vibrations from minions dealing damage
- Vibrations from minions receiving damage
- Shocks from minions dealing damage
- Shocks from minions receiving damage

#### Operation Values
The base values that get scaled off of damage dealt relative to damaged entity's max health. In other words, the intensity of the operation is calculated by
```
baseValue * (damageDealt / maxHealthOfDamagedEntity)
```

- Base duration of vibrations
- Base intensity of vibrations when dealing damage
- Base intensity of vibrations when receiving damage
- Base duration of shocks
- Base intensity of shocks when dealing damage
- Base intensity of shocks when receiving damage
- Maximums for vibration and shock durations and intensities

#### Operation Behavior
- Time span over which to sum up intensity and duration
- Shocker selection
  - `All`: affect all shockers
  - `Random`: only affect a random shocker
- Account for damage dealt past the entity's max health
