# Changelog
This project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html), and the format of this changelog is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## [0.5.2](https://github.com/quasikyo/ror2-mods/releases/tag/RumbleRain-0.5.2) - 2025-12-08

### Changed
- Depend on R2API submodules [#23](https://github.com/quasikyo/ror2-mods/pull/23) ([@1Naim](https://github.com/1Naim))

## [0.5.1](https://github.com/quasikyo/ror2-mods/releases/tag/RumbleRain-0.5.1) - 2025-12-07

### Changed
- Bump versions for DLC3

## [0.5.0](https://github.com/quasikyo/ror2-mods/releases/tag/RumbleRain-0.5.0) - 2024-09-08

### Changed
- Update for SotS
- Update Buttplug dependency to 4.0.0 from 3.0.1

### Removed
- Remove Fody Costura

## [0.4.2](https://github.com/quasikyo/ror2-mods/releases/tag/RumbleRain-0.4.2) - 2023-12-07

### Fixed
- Fix null error when damaging asteroids on Sky Meadow

## [0.4.1](https://github.com/quasikyo/ror2-mods/releases/tag/RumbleRain-0.4.1) - 2023-12-02

### Fixed
- Prevent null reference exception that came from accounting for minions

## [0.4.0](https://github.com/quasikyo/ror2-mods/releases/tag/v0.4.0) - 2023-11-18

### Added
- Account for minions (turrets, drones) dealing and receiving damage
  - Add corresponding config options

## [0.3.1](https://github.com/quasikyo/ror2-mods/releases/tag/v0.3.1) - 2023-09-27

### Added
- Add in-game button to reconnect devices (no longer need to have Intiface running beforehand)

## [0.3.0](https://github.com/quasikyo/ror2-mods/releases/tag/v0.3.0) - 2023-08-30

### Added
- Add configurable toggle for pausing and resuming vibrations
  - Improved internal state management as a consequence

### Changed
- Conditional debug logging
- Streamlined build pipeline

## [0.2.2](https://github.com/quasikyo/ror2-mods/releases/tag/v0.2.2) - 2023-08-29

### Changed
- Drastically reduce download size by not compiling all dependencies into a single `.dll`

## [0.2.1](https://github.com/quasikyo/ror2-mods/releases/tag/v0.2.1) - 2023-08-28

### Added
- Updated config to clamp values inside the mod manager

## [0.2.0](https://github.com/quasikyo/ror2-mods/releases/tag/v0.2.0) - 2023-08-28

### Added
- Allow changing of vibration behaviors without needing a restart

## [0.1.0](https://github.com/quasikyo/ror2-mods/releases/tag/v0.1.0) - 2023-08-23

### Added
- Vibration on taking or receiving damage
- Configurable values for vibrations
