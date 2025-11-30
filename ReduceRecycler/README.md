# ReduceRecycler
Sets Recycler cooldown to instant and adds the cooldown to the run timer.

I have not tested this in multiplayer, but users have reported that it works as of 1.1.3.

## [1.3.2](https://github.com/quasikyo/ror2-mods/releases/tag/ReduceRecycler-1.3.2) - 2025-11-22

### Fixed
- Fix compatibility with new Alloyed Collective Update
- Address log spam https://github.com/quasikyo/ror2-mods/issues/18

For the full changelog, see:
- [the GitHub releases page](https://github.com/quasikyo/ror2-mods/releases?q=ReduceRecycler),
- [the Thunderstore changelog page](https://thunderstore.io/package/quasikyo/ReduceRecycler/changelog), or
- [the local `CHANGELOG.md` file](./Thunderstore/CHANGELOG.md)

## Credits
- Boof, for initial mod concept and in-game general/edge-case testing

## Configuration
- Whether the mod is enabled or not
- Whether or not no-cooldown should apply only after teleporter
- Cooldown refresh strategy:
  - AfterUse: cooldown is reset and time is added immediately after rerolling an item
  - OnDemand: cooldown is reset on button press

## Issue Reporting and Suggestions
Contact me on Discord (`quasikyo`) or submit a new issue [here](https://github.com/quasikyo/ror2-mods/issues).

## FAQ

#### Does the time added to the run timer reflect any equipment cooldown reduction (e.g., fuel cells, Gesture)?
Yes.

#### If I have multiple fuel cells, will the mod only activate when the last charge is used?
Yes.

#### Does the config option work in stages without a teleporter (Hidden Realms, Void Fields, Commencement)?
Yes, but enabling the config option will simply make the Recycler behave as normal.
