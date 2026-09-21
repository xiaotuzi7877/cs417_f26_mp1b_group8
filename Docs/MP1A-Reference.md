# MP1A Group 8

Unity VR project for CS 417.

## Parking lot decorations

`SampleScene` includes a `Parking Lot Decorations` prefab with eight parking bays,
lane markings, a crosswalk, four planted trees, an attendant cottage, three people,
a bench, street lamps, a parking sign, bollards, and a litter bin. The center aisle
remains open. The car is built from Unity cubes and cylinders in
`Assets/Decorations/PrimitiveCar.prefab`, using the existing decoration
materials. Its body, roof, windows, wheels, mirrors and lights are editable
children, with one static collider. It requires no Blender installation or
external model import. The old car prefab and Blender model were removed.

Edit `Assets/Decorations/ParkingLot.prefab` to move or recolor individual props.
The decorations are static visual objects; the original floor provides collision.
They are visible in Edit mode and Play mode without adding a component or running
a generator. If the scene was already open when the files changed, reload
`Assets/Scenes/SampleScene.unity` from disk to see the new prefab instance.

## Controls

### Vampire relics

Three key pedestals stand separately in front of the lock stations. Aim the right
controller at a key (gold highlight), hold grip to bring it to your hand (cyan),
then drop it anywhere on its matching lock table. In the simulator, select the
right controller with `]` and hold left mouse in Point and Click mode (or `G`) to grip. Blood vial/chalice opens the
reliquary, silver stake/coffin opens its lid, and bat medallion/crypt raises the
gate. Wrong props are rejected. Both hands retain direct grabbing; all keys have
gravity and solid colliders. The goal sign teaches the pairings and tracks all three seals; victory requires all three matching props installed and their eased opening animations complete. See [Vampire relic setup](VampireRelics.md).

| Action | XR controller | XR simulator keyboard | Result |
| --- | --- | --- | --- |
| Use crypt / return door | Aim left controller + left secondary | `Shift + 2` | After both prerequisite locks, teleport through the aimed-at door. |
| Quit | Right-hand primary button | `1` | Plays the farewell cue, waits for it to finish, then stops Play mode or quits the build. |
| Toggle blood moon | Right-hand secondary button | `2` | Toggles between the original scene lighting and a dim, dark-red light. Press again to restore the original color and intensity. |
| Switch view location | Left-hand primary button | `Shift + 1` | Alternates between the room and the external viewing point and plays a small doorway fog puff. |

## Input setup

- Quit uses `ControllerSampleActions/RightHand/PrimaryButton`.
- Blood moon lighting uses `ControllerSampleActions/RightHand/SecondaryButton`. The scene starts with its original lighting; the Point Light's LightSwitch component exposes Blood Moon Color and Blood Moon Intensity Multiplier for tuning (default 65% intensity).
- The blood moon toggle plays existing ParticleSystem components saved in the hierarchy. Expand `planet > Planet Clouds`, `planet > moon > Moon Clouds`, and `comet > Comet Clouds`. Expand each tree and person under `Parking Lot Decorations` to find its `Blood Moon Burst` child. Each Street lamp also has a `Crimson Sparks` child; Parking attendant cottage has `Left Window Mist`, `Right Window Mist`, and `Doorway Fog` children.
- Edit each child's Particle System directly: Main controls lifetime/size/color, Emission controls burst count/rate, Shape controls spread, and Renderer controls the material. The three clouds loop; the seven tree/person emitters have one 28-particle burst, no continuous emission, and lifetimes of 0.35–0.8 seconds. The two lamps use 8 sparks each (0.5-second lifetime); the two windows use 10 mist particles each (1.4 seconds); the doorway uses 12 pale fog particles (1.6 seconds). All five new emitters are non-looping, have zero emission rate, and use one burst. All emitters have Play On Awake disabled.
- `BloodMoonEffects` on Point Light only holds Clouds and Bursts references and calls Play/Stop. It creates no objects, materials, or particle settings at runtime. Turning normal lighting back on clears every effect. Stop Play and reload `Assets/Scenes/SampleScene.unity` after this migration to see the saved children and new references.
- Gameplay actions bind only to XR controller buttons. The former direct `1`–`4` keyboard shortcuts are removed so they do not conflict with simulator input.
- The imported XR Interaction Simulator automatically starts in the Editor only. Enter Play mode and focus the Game window. In controller simulation, use `1` / `2` for the right controller's primary / secondary buttons; hold `Shift` for the left controller (`Shift + 1` / `Shift + 2`).
- Use `Tab` for device mode and `[` to manipulate the left controller. Keep it in Controller mode rather than Hand mode when testing these buttons. `3` / `4` now retain their simulator meaning (joystick clicks).

## Core Requirements

Current feature checklist (the object spawning/shooting feature has been removed):

- [x] Particle Bursts
- [x] Spatial Sound
- [x] Object Space
- [x] World Space
- [x] Materials
- [x] Highlight Outline
- [x] XR Tracked Camera
- [x] Euler Steady
- [x] Kinematic Double Integrators
- [x] XR Controller Inputs
- [x] Quit Key
- [ ] Object Spawning (removed)
- [x] Camera Teleport

## Side Quests

- Particle Feedback Content: fifteen input-triggered emitters: three celestial clouds, four tree bursts, three person bursts, two lamp sparks, two window mists, and one doorway puff. Fourteen use the blood moon toggle; the doorway uses view switching.

- [x] Arbitrary Orbiter: the comet's double integrator calculates acceleration from the live position of the assigned planet Transform rather than a hardcoded world-space point.

Blood moon visual check: enter Play mode with the Game window focused. Initially
there should be no clouds or droplet bursts. Press simulator `2`: all three bodies
should gain wispy red clouds, while the four trees and three people each flash a
short dark-red droplet burst. The droplets should fade within 0.8 seconds without looping. Watch the
moon and comet move to check that the clouds follow them. Press `2` again: particles
clear immediately and the original room light returns. Repeat
several times to confirm the hierarchy does not accumulate additional effects.

To check the added feedback, press simulator `2` to see lamp sparks and window mist. Press `Shift + 1` to switch views and trigger the doorway puff. All emitters are saved in the hierarchy; no particle objects are created at runtime.

## Spatial audio

All 13 AudioSources are saved in the hierarchy, with Spatial Blend 1, Play On Awake off, no looping, and distance attenuation. Used clips in `Assets/sound_effect` import as mono with preloaded audio data; the original MP3 files are unchanged. `SpatialSoundFeedback` on Point Light holds the source references and routes the existing gameplay events.

| Sources | Location | Trigger / clips |
| --- | --- | --- |
| 2 | Planet children | Normal: chime; blood moon: rumble |
| 2 | Moon children | Normal: bell1; blood moon: bell2 at lower pitch |
| 2 | Comet children | Whoosh, higher pitch for normal and lower for blood moon |
| 3 | One Startled Gasp child per person | gasp1 / gasp2 / gasp3 when blood moon begins |
| 2 | Spatial Audio Feedback group | Departure at the previous listener position; arrival at the new position when switching views |
| 1 | Spatial Audio Feedback / Welcome - First Input | Welcome traveler once on the first light toggle or view switch |
| 1 | Spatial Audio Feedback / Farewell - Before Quit | bye before quitting, with a clip-length delay |

The six body sources follow their moving objects. Gasps stop when normal mode returns, and switching modes stops the opposite body cues. Welcome, farewell, and teleport sources are placed in world space near the listener when triggered, rather than remaining parented to the headset. `car_drop.mp3` is not used for these cues.

Stop Play, allow the audio imports to finish, and reload SampleScene before testing the new references. Listen while turning your head to check direction and adjust each source's Volume in the Inspector as needed. The voice identities of gasp1-3 have not been confirmed by listening; swap clips between the person AudioSources if necessary.

Spatial Sound Content: 13 input-triggered spatial audio generators.

