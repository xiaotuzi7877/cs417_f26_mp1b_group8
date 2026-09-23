# Michael Manor Hall

Michael's MP1b room is self-contained under `Assets/Michael`.

## Open the room

- Scene: `Assets/Michael/Scenes/MichaelManorHall.unity`
- Unity: `6000.5.6f1`
- Runtime packages are already provided by the group project (URP, Input System,
  XR Interaction Toolkit, OpenXR, and TextMesh Pro).

The scene is intentionally not added to `EditorBuildSettings.asset`. The teammate
responsible for scene integration can add it in the final progression order without
this import changing shared project settings.

## Room objective

Find and grab the Silver Fang, then place it into the matching pedestal socket by
the exit. The insertion sequence releases the door seal, opens the exit, plays the
win celebration, and shows the `Congratulations` message for five seconds.

## Presentation controls

These keyboard shortcuts work in Play Mode and make the room easy to demonstrate
without a headset:

- `K`: reset, insert the Silver Fang, unlock the door, and play the full sequence
- `W`: replay the win celebration
- `R`: reset the puzzle, door, and celebration
- `P`: spawn an orbiting object
- `L`: toggle the alternate hall lighting
- `B`: toggle the break-out/return presentation
- `Q`: quit Play Mode or the built player

The VR path remains the primary interaction: use the XR controllers to move through
the room, grab the Silver Fang, and place it in the socket.

## Layout

- `Manor/`: room-specific scripts, materials, models, textures, and furniture assets
- `Dependencies/`: only the legacy runtime prefabs, scripts, audio, shader, and
  skybox files referenced by this scene
- `Scenes/`: the completed Michael Manor Hall scene

Michael's compatibility scripts use the `MichaelManor` namespace so they do not
collide with teammate scripts that use common names such as `BreakOut` or
`LightSwitch`.

Poly Haven source and CC0 attribution notes are stored next to the imported assets.
