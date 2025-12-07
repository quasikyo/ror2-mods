# Quasikyo's RoR2 Mods
Mono-repository for all of my RoR2 mods.

## Building
```bash
dotnet build [ProjectPath] -c [Debug|Release]
```
Omitting `[ProjectPath]` will compile all projects.

`ror2-mods.targets` handles the following:
- Compiling the assembly
- Copying the assembly to the testing profile
- Setting the Plugin metadata (GUID, author, version)
- [`-c Release`] Packaging the Thunderstore artifacts
- [`-c Release -t:PublishThunderstore`] Publishing the packaged Thunderstore artifacts

### Environment Variables
In `.env`, you can set the following variables:
- `TCLI_AUTH_TOKEN`: Authentication token for publishing with `tcli`.
- `TEST_PROFILE`: The directory of a profile you want to copy `<ModName>.[dll|pdb]`  to; `/BepInEx/plugins/$(Owner)-$(AssemblyName)/` will be appended to it.

Remember to `source .env` each time you begin a new terminal session.

### Build Dependencies
1. [`MinVer`](https://www.nuget.org/packages/MinVer) for setting the version based on git tags.
2. [`BepInEx.PluginInfoProps`](https://nuget.bepinex.dev/packages/BepInEx.PluginInfoProps) for generating Plugin metadata.
3. [`tcli`](https://github.com/thunderstore-io/thunderstore-cli) for packaging and publishing build artifacts for [Thunderstore](https://thunderstore.io).

The build file relies on `MinVer` for generating the version and passes it to `BepInEx.PluginInfoProps` and `tcli`.

## Releasing
1. Add a new entry to `<ModName>/CHANGELOG.md` and copy-paste the new entry to `<ModName>/README.md`
2. Commit changes and push with tag `<ModName>-<Major>.<Minor>.<Patch>`
3. `.github/workflows/publish.yml` will auto-publish to Thunderstore
4. Manually create a new GitHub release
   - The description should be the same as step 1
   - Attach `<ModName>/Thunderstore/dist/quasikyo-<ModName>-<Major>.<Minor>.<Patch>.zip`

### Non-workflow Release
1. Run `dotnet build <ModName> -c Release` to generate build artifacts
2. Run `dotnet build <ModName> -c Release -t:PublishThunderstore` to publish to Thunderstore
