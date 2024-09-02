# Quasikyo's RoR2 Mods
Mono-repository for all of my RoR2 mods.

## Usage of `build.sh`
```
./build.sh [ProjectName] [Debug|Release]
```

## Things to Keep in Mind

#### Updating Version
- Update `<ModName>.cs::PluginVersion`
- Update `version_number` in `manifest.json`
- The git tag and `.zip` name (see [Release](#releasing)) should match as well

#### Releasing
- Create a tag `<ModName>-<Major>.<Minor>.<Patch>` for GitHub release
  - Should match [Updating Version](#updating-version)
- Ensure any DLL dependencies are included in `<ModName>/Thunderstore/plugins/<ModName>`
- Run `./build.sh <ModName> Release`
- Create `.zip` of `<ModName>/Thunderstore/*` named `<AuthorName>-<ModName>-<Version>`

#### Archived Links
- Check for links to old per-mod repositores in `CHANGELOG.md`, `README.md`, and `manifest.json`
