# Changelog
This project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html), and the format of this changelog is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## [1.3.4](https://github.com/quasikyo/ror2-mods/releases/tag/ReduceRecycler-1.3.4) - 2025-12-08

### Changed
- Depend on R2API submodules [#23](https://github.com/quasikyo/ror2-mods/pull/23) ([@1Naim](https://github.com/1Naim))

## [1.3.3](https://github.com/quasikyo/ror2-mods/releases/tag/ReduceRecycler-1.3.3) - 2025-12-06

### Fixed
- Update outdated reference `Inventory::GetEquipment(uint)` to `Invetory::GetEquipment(uint, uint)`

## [1.3.2](https://github.com/quasikyo/ror2-mods/releases/tag/ReduceRecycler-1.3.2) - 2025-11-30

### Fixed
- Fix compatibility with new Alloyed Collective Update [#19](https://github.com/quasikyo/ror2-mods/issues/19)
- Address log spam [#18](https://github.com/quasikyo/ror2-mods/issues/18)

## [1.3.1](https://github.com/quasikyo/ror2-mods/releases/tag/ReduceRecycler-1.3.1) - 2024-09-07
- Update README

## [1.3.0](https://github.com/quasikyo/ror2-mods/releases/tag/ReduceRecycler-1.3.0) - 2024-09-07

### Added
- Add config option to dynamically enable/disable mod
- Implement cooldown reset options:
  - AfterUse: cooldown is reset and time is added **after** rerolling an item
  - OnDemand: cooldown is reset on button press

## [1.2.2](https://github.com/quasikyo/ror2-mods/releases/tag/ReduceRecycler-1.2.2) - 2024-09-01

### Changed
- Update RiskOfOptions dependency to use remote nuget package
- Update link to old GitHub repo

## [1.2.1](https://github.com/quasikyo/ror2-mods/releases/tag/ReduceRecycler-1.2.1) - 2024-09-01

### Changed
- Update versions for SotS update

## [1.2.0](https://github.com/quasikyo/ror2-mods/releases/tag/ReduceRecycler-1.2.0) - 2024-06-12

### Changed
- Avoid adding time if stopwatch is paused for whatever reason

## [1.1.3](https://github.com/quasikyo/reduce-recycler/releases/tag/v1.1.3) - 2023-10-10

### Changed
- Makes it so the config option is still functional on Hidden Realms, Void Fields, and Commencement
  - Just makes the Recycler behave as normal but also fixes an error preventing the immediate refresh

## [1.1.2](https://github.com/quasikyo/reduce-recycler/releases/tag/v1.1.2) - 2023-09-10

### Added
- Update FAQ question regarding fuel cells

### Fixed
- Fix typo in CHANGELOG.md file name

## [1.1.1](https://github.com/quasikyo/reduce-recycler/releases/tag/v1.1.1) - 2023-09-07

### Changed
- Update README

## [1.1.0](https://github.com/quasikyo/reduce-recycler/releases/tag/v1.1.0) - 2023-09-07

### Added
- Add credits section

### Changed
- Only activate when consuming last charge

## [1.0.1](https://github.com/quasikyo/reduce-recycler/releases/tag/v1.0.1) - 2023-09-07

### Added
- Add icon to code repo

### Fixed
- Fix descriptions

## [1.0.0](https://github.com/quasikyo/reduce-recycler/releases/tag/v1.0.0) - 2023-09-07

### Added
- Public release
- Neutralizes the cooldown on the Recycler equipment
