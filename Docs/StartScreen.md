# Start screen

Open `Assets/Shared/Scenes/StartScreen.unity` and enter Play Mode to test the full game from the beginning. Built players start here automatically because it is the first enabled build scene. Unity's Play button otherwise plays whichever scene is currently open.

The player arrives in a separate gothic foyer with a world-space **START GAME** button. Aim at it and use the XR UI trigger, click with the mouse, or press **Enter**. The button loads `Assets/Ken/Scenes/Ken's_room.unity` with `LoadSceneMode.Single`. The foyer and its player rig unload; Michael remains unloaded until Ken's existing win-gated transition. No challenge runs before Start Game.

This scene fulfills the Start Screen requirement and is not counted as a new story/escape-room scene.

`Assets/Shared/Scripts/StartScreenController.cs` owns loading, an on-screen loading/error message, keyboard fallback, and repeated-click protection. The native Button has a saved OnClick listener. The canvas has standard and tracked-device graphic raycasters, and one XRUIInputModule handles mouse and XR UI input. Shared fonts and the existing XR Origin prefab are reused; no packages were added.

Enabled scene order: StartScreen, Ken's_room, MichaelManorHall. The Web profile inherits this list. To work on an individual puzzle, open that room directly in the Editor.

Ken and the foyer reuse the existing `MichaelManor.RoomSpawnAlignment` component to compensate for persistent simulator/headset tracking offsets. Ken's outside-view action now records the current inside pose when leaving, after arrival alignment, so returning cannot use a stale pre-alignment position.

## Validation

Unity 6000.5.6f1 compiled the changes in an isolated project copy. Play Mode checks passed for build scene zero, absence of the escape-room puzzle before starting, the saved Button listener, UI raycast and pointer-click dispatch, repeated-click protection, keyboard Enter, unloading the foyer, a single destination XR Origin, a fresh Ken puzzle, compensated arrival with tracking offset `(12, 0, -6)`, and Michael's pre-win gate. A rendered 1600 x 1000 preview was reviewed for readability. These checks do not simulate a physical headset trigger. The editor also reported the previously observed SearchDatabase indexing exception; gameplay checks completed successfully.

Use the title scene to check that no escape room appears in the Hierarchy before starting, the button highlights and activates, and only Ken's room remains after loading. Try keyboard Enter separately. Walk in the simulator before starting and confirm arrival remains on Ken's floor. Confirm Michael is still inaccessible before winning Ken's challenge. Physical headset pointing, trigger input, readability, and comfort need a device check.
