# CS 417 MP1B — Group 8

Team Unity VR project, starting from Zijian's saved MP1A project on September 21, 2026, including local scene/settings edits and Resources assets.

## Open the project

1. Install Git, Git LFS, and Unity **6000.5.6f1** through Unity Hub. Install Android Build Support (SDK/NDK and OpenJDK) for Quest builds.
2. Clone the repository:

   ```sh
   git lfs install
   git clone https://github.com/zijianzzzz/cs417_f26_mp1b_group8.git
   cd cs417_f26_mp1b_group8
   git lfs pull
   ```

3. In Unity Hub, add this repository folder as a project. It directly contains `Assets`, `Packages`, and `ProjectSettings`; there is no nested `mp1a` folder.
4. Let Unity finish importing assets and resolving packages. Open `Assets/Ken/Scenes/Ken's_room.unity`.
5. Enter Play Mode and check the Console. The first import regenerates `Library` and can take several minutes.

The baseline uses URP 17.5.0, Input System 1.20.0, XR Interaction Toolkit 3.5.1, and OpenXR 1.17.1. Use the committed package manifest and lockfile; coordinate any editor or package upgrades with the team.

## Team contributions

Read [CONTRIBUTING.md](CONTRIBUTING.md) before adding a scene. Each teammate owns a separate scene and asset folder, works on a branch, and opens a pull request. Commit asset `.meta` files alongside the assets.

Owner folders: `Assets/Ken`, `Assets/Minh`, and `Assets/Michael`. Ken's room assets are grouped under `Ken`; the other two folders are ready for new contributions. Shared XR assets, samples, fonts, input actions, and project configuration remain at the top level.

The repository owner must invite teammates through GitHub repository Settings → Collaborators. Write access is needed to push branches to this repository.

## Existing gameplay and documentation

- [MP1A gameplay reference and controls](Docs/MP1A-Reference.md)
- [Vampire puzzle setup](Docs/VampireRelics.md)
- [Project context and inherited development notes](Docs/AI/UnityProjectContext.md)

The build starts in `Ken's_room`, followed by `MichaelManorHall`. After solving all three seals and seeing the win message, click the **left controller thumbstick** or press **N** on the keyboard to enter Michael's room. Only one room is loaded at a time, so both rooms can stay at the origin. See [room transition setup and checks](Docs/RoomTransitions.md). The project keeps its inherited Unity application settings; agree on any MP1B application-name or Android package-identifier changes before a release.

This repository starts with a fresh Git history. MP1A's repository remains separate. Generated caches, builds, debug backups, and individual submission documents/videos are not part of this baseline. The room transition has passed an isolated Unity Play Mode check with a simulated win; full puzzle and headset validation remain manual checks.
