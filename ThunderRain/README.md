# ThunderRain
[![Thunderstore Version](https://img.shields.io/thunderstore/v/quasikyo/ThunderRain?style=for-the-badge&logo=thunderstore&logoColor=white)](https://thunderstore.io/package/quasikyo/ThunderRain)
[![Thunderstore Downloads](https://img.shields.io/thunderstore/dt/quasikyo/ThunderRain?style=for-the-badge)](https://thunderstore.io/package/quasikyo/ThunderRain)
[![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/quasikyo/ror2-mods/build.yml?style=for-the-badge)](https://github.com/quasikyo/ror2-mods/actions/workflows/build.yml)

Operates [PiShocks](https://pishock.com) in response to in-game damage events.

## Safety
Please configure shock maximums in the config and the share code.

## [1.1.2](https://github.com/quasikyo/ror2-mods/releases/tag/ThunderRain-1.1.1) - 2025-12-07

### Added
- FAQ to README

### Changed
- Bump versions for DLC3

For the full changelog, see:
- [the GitHub releases page](https://github.com/quasikyo/ror2-mods/releases?q=ThunderRain),
- [the Thunderstore changelog page](https://thunderstore.io/package/quasikyo/ThunderRain/changelog), or
- [the local `CHANGELOG.md` file](./CHANGELOG.md)

## Issue Reporting and Suggestions
1. Check [the FAQ](#faq).
2. Contact me on Discord (`quasikyo`) or submit a new issue [here](https://github.com/quasikyo/ror2-mods/issues).

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

## FAQ

### Nothing happens when I take/deal damage. Is it broken?
If you see nothing in the logs, this is likely because your configuration isn't setup properly. You'll want to check which events you want the mod to respond to and then set appropriate base values. Keep in mind that these base values are multipled by the percent of damage dealt/taken based off your max health, so you'll want to set them a bit higher. See [this thread](https://github.com/quasikyo/ror2-mods/issues/14#issuecomment-1907120773) for more.
