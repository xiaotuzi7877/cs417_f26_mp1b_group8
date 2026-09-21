# Contributing scenes

## Start a contribution

Save your scene and exit Unity before pulling or switching branches. Start with a clean working tree; commit your work on your own branch before switching.

```sh
git switch main
git pull --ff-only
git switch -c scene/your-name
```

Open the project with Unity 6000.5.6f1. The three owner folders are `Assets/Ken`, `Assets/Minh`, and `Assets/Michael`. Ken's existing room is `Assets/Ken/Scenes/Ken's_room.unity`. Minh and Michael can create subfolders through Unity's Project window, for example:

```text
Assets/Minh/
  Scenes/
  Prefabs/
  Materials/
  Scripts/
  Audio/
  Models/
```

Use a distinct scene name such as `Minh_room` or `Michael_room`. Put your scene and its own assets under your owner folder. Shared XR assets, imported samples, TextMesh Pro, input actions, and project configuration remain outside these folders. Coordinate before editing shared assets or another teammate's room. Use unique script class names or a teammate-specific namespace to prevent duplicate class errors.

## Bring an existing scene from another project

1. Back up your original project. Match the team's Unity version, render pipeline, and required packages before integration.
2. In the original project, organize your own assets into a uniquely named folder using Unity, preserving their `.meta` files. Do not move shared package assets.
3. Select the scene and choose Assets → Export Package. Include dependencies and verify that your scripts, materials, textures, prefabs, audio, and lighting data are included.
4. In your team-project branch, choose Assets → Import Package → Custom Package. Review the import list. Resolve shared-asset conflicts deliberately; do not overwrite team assets or reimport a second copy of the XR sample packages indiscriminately.
5. Package Manager dependencies are not supplied by a `.unitypackage`; coordinate any required changes to `Packages/manifest.json` and `packages-lock.json`.
6. Open the imported scene, resolve missing scripts/materials and compile errors, and test interaction in Play Mode. Test on the target headset before calling it ready for submission.

Do not submit just the `.unity` file: its dependencies and `.meta` files are needed. Commit the imported assets, not the exported `.unitypackage` archive.

## Commit and submit

Save everything in Unity and review changes before committing. For a contribution confined to your folder:

```sh
git status --short
git add Assets/Minh Assets/Minh.meta
git diff --cached --stat
git commit -m "Add YourName room scene and dependencies"
git push -u origin scene/your-name
```

Replace `Minh` with your own folder name in the example. Explicitly stage any required dependencies outside your own folder and their `.meta` files after reviewing them. Always inspect staged changes; do not commit `Library`, `Temp`, `Logs`, local settings, APKs, or debug/backup output.

Open a pull request into `main` describing your scene path, controls, added dependencies, and tests performed. Have a teammate review it before merging. If Git reports conflicts in a scene or prefab, coordinate with its owner; never resolve by blindly discarding either version. Unity's YAML merge driver requires local configuration even though `.gitattributes` names it.

Git LFS must be installed before adding assets; `.gitattributes` routes images, audio, models, and other configured binary types through LFS. After pulling a branch, run `git lfs pull` if assets are missing locally.

## Integration checklist

- Each contribution opens with no missing references or compilation errors.
- One person coordinates the build scene list and scene transitions.
- Separately loaded scenes can each own a player rig; scenes loaded together additively need coordinated ownership of the XR Origin, camera, AudioListener, EventSystem, input, and XR interaction systems.
- Agree on spawn points, scale, orientation, locomotion, and controls before combining rooms.
- Verify Play Mode and the target headset after integration.

References: [Unity asset organization](https://unity.com/how-to/organizing-your-project) and [exporting asset packages](https://docs.unity.com/en-us/engine/6000.3/manual/assets-and-media/asset-packages/create).
