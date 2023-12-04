# ThunderRain
Operates [PiShocks](https://pishock.com) in response to in-game damage events.

## Safety
Please configure maximums in the share code.

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

#### Operation Behavior
- Shocker selection
  - `All`: affect all shockers
  - `Random`: only affect a random shocker
