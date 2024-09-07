# Quasikyo's RoR2 Mods
Mono-repository for all of my RoR2 mods.

## Usage of `build.sh`
```
./build.sh [ProjectName] [Debug|Release]
```

## Things to Keep in Mind

#### Updating Version
- Update `<ModName>/<ModName>.cs::PluginVersion`
- Update `version_number` in `<ModName>/Thunderstore/manifest.json`
- Upate version number for the most recent changelog entry in `<ModName>/README.md`
- The git tag and `.zip` name (see [Release](#releasing)) should match as well

#### Releasing
- Create a tag `<ModName>-<Major>.<Minor>.<Patch>` for GitHub release
- Ensure any DLL dependencies are included in `<ModName>/Thunderstore/plugins/<ModName>`
- Run `./build.sh <ModName> Release`
- Create `.zip` of `<ModName>/Thunderstore/*` named `<AuthorName>-<ModName>-<Version>`

###### Changelog Notes
- Update `<ModName>/README.md` to have notes for most recent update
- Add entry in `<ModName>/Thunderstore/CHANGELOG.md`
- Write notes in GitHub release

#### Archived Links
- Check for links to old per-mod repositores in `<ModName>/Thunderstore/CHANGELOG.md`, `<ModName>/README.md`, and `<ModName>/Thunderstore/manifest.json`
