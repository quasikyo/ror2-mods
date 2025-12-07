# Changelog
This project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html), and, as of version 1.3.2, the format of this changelog is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## [1.3.3](https://github.com/quasikyo/ror2-mods/releases/tag/ReduceRecycler-1.3.3) - 2025-12-06

### Fixed
- Update `Inventory::GetEquipment(uint)` to `Invetory::GetEquipment(uint, uint)`

## [1.3.2](https://github.com/quasikyo/ror2-mods/releases/tag/ReduceRecycler-1.3.2) - 2025-11-30

### Fixed
- Fix compatibility with new Alloyed Collective Update https://github.com/quasikyo/ror2-mods/issues/19
- Address log spam https://github.com/quasikyo/ror2-mods/issues/18

## [1.3.1](https://github.com/quasikyo/ror2-mods/releases/tag/ReduceRecycler-1.3.1)
- Update README

## [1.3.0](https://github.com/quasikyo/ror2-mods/releases/tag/ReduceRecycler-1.3.0)
- Add config option to dynamically enable/disable mod
- Implement cooldown reset options:
  - AfterUse: cooldown is reset and time is added **after** rerolling an item
  - OnDemand: cooldown is reset on button press

## [1.2.2](https://github.com/quasikyo/ror2-mods/releases/tag/ReduceRecycler-1.2.2)
- Update RiskOfOptions dependency to use remote nuget package
- Update link to old GitHub repo

## [1.2.1](https://github.com/quasikyo/ror2-mods/releases/tag/ReduceRecycler-1.2.1)
- Update versions for SotS update

## [1.2.0](https://github.com/quasikyo/ror2-mods/releases/tag/ReduceRecycler-1.2.0)
- Avoid adding time if stopwatch is paused for whatever reason

## [1.1.3](https://github.com/quasikyo/reduce-recycler/releases/tag/v1.1.3)
- Makes it so the config option is still functional on Hidden Realms, Void Fields, and Commencement
  - Just makes the Recycler behave as normal but also fixes an error preventing the immediate refresh

## [1.1.2](https://github.com/quasikyo/reduce-recycler/releases/tag/v1.1.2)
- Update FAQ section
- Fix typo in CHANGELOG.md file name

## [1.1.1](https://github.com/quasikyo/reduce-recycler/releases/tag/v1.1.1)
- Update README

## [1.1.0](https://github.com/quasikyo/reduce-recycler/releases/tag/v1.1.0)
- Add credits section
- Only activate when consuming last charge

## [1.0.1](https://github.com/quasikyo/reduce-recycler/releases/tag/v1.0.1)
- Add icon to code repo
- Fix descriptions

## [1.0.0](https://github.com/quasikyo/reduce-recycler/releases/tag/v1.0.0)
- Public release
- Neutralizes the cooldown on the Recycler equipment
