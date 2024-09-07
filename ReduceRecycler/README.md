# ReduceRecycler
Sets Recycler cooldown to instant and adds the cooldown to the run timer.

I have not tested this in multiplayer, but users have reported that it works as of 1.1.3.

## Changelog - 1.3.0
- Add config option to dynamically enable/disable mod
- Implement cooldown reset options:
  - After Use: cooldown is reset and time is added **after** rerolling an item
  - Before Use: cooldown is reset and time is added **before** rerolling an item

For the full changelog, see:
- [the GitHub releases page](https://github.com/quasikyo/ror2-mods/releases?q=ReduceRecycler),
- [the Thunderstore changelog page](https://thunderstore.io/package/quasikyo/ReduceRecycler/changelog), or
- [the local `CHANGELOG.md` file](./Thunderstore/CHANGELOG.md)

## Credits
- Boof, for initial mod concept and in-game general/edge-case testing

## Configuration
- Whether or not no-cooldown should apply only after teleporter
- Cooldown refresh strategy:
  - After Use: cooldown is reset and time is added **after** rerolling an item
  - Before Use: cooldown is reset and time is added **before** rerolling an item

## Issue Reporting and Suggestions
Contact me on Discord (`quasikyo`) or submit a new issue [here](https://github.com/quasikyo/ror2-mods/issues).

## FAQ

#### Does the time added to the run timer reflect any equipment cooldown reduction (e.g., fuel cells, Gesture)?
Yes.

#### If I have multiple fuel cells, will the mod only activate when the last charge is used?
Yes.

#### Does the config option work in stages without a teleporter (Hidden Realms, Void Fields, Commencement)?
Yes, but enabling the config option will simply make the Recycler behave as normal.
