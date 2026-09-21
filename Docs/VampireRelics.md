# Vampire keys and locks

The earlier camera HUD and name-based inventory have been removed. This feature uses physical props, direct hand grabbing, and three matching socket locks.

## Three different pairs

| Key | Matching lock | Unlock event |
| --- | --- | --- |
| Ruby blood vial with gold collars | Gold chalice at the Blood Tithe altar | Reliquary lid opens to reveal a heart |
| Pointed silver stake with a wrapped handle | Stake receptacle beside the bone-colored coffin | Coffin lid swings open |
| Purple-and-gold winged bat medallion | Bat-shaped crest on the crypt | Iron gate rises |

Each pairing differs in silhouette, theme, color, label, and moving mechanism. Drop a prop anywhere over its matching lock table, then release your grip or left mouse button in simulator Point and Click mode. Each lock has a 2 m wide, 1.72 m deep box trigger from local Y=0.8 to 1.8, covering the tabletop with a small edge margin. The matching prop snaps into its original socket seat when accepted. A matching offer changes the indicator to gold and displays **RELEASE TO OFFER**. Once accepted, the indicator turns green, the sign reads **UNLOCKED**, and the mechanism opens. Wrong props cannot hover or select that socket. Each unlock fires once; installed props remain latched.

## Scene and controls

- Open `Assets/Ken/Scenes/Ken's_room.unity` and enter Play Mode.
- The **Vampire Relics** group contains separate **Keys** and **Locks** groups. Keys stand on their own pedestals at world Z=1.2; the lock stations are farther back at Z=3.6.
- Both XR controllers have direct-grab children with 12 cm trigger volumes. They use the existing XRI left/right **Select** grip actions and join the existing hand interaction groups with priority over distant grabbing.
- Aim the **right controller's ray** at a key. Its accent turns **gold** to show it can be grabbed. Hold the **grip button** to pull it to your hand; its accent turns **cyan** while held. Carry it to the matching lock and release grip to offer it. Aiming alone never collects a key.
- In the XR Interaction Simulator, focus the Game view, select the right controller with `]`, aim at a key, and hold `G` for grip. Release `G` to drop or place it. These are the existing simulator bindings.
- The existing floor retains its MeshCollider. All three props have gravity-enabled, non-kinematic Rigidbodies, colliders, and XR Grab Interactable behavior.

## Editing

`Assets/Ken/VampireProps/VampireRelics.prefab` arranges the separate key displays and lock stations. `BloodVialKey.prefab`, `SilverStakeKey.prefab`, and `BatMedallionKey.prefab` are independent movable props. `ChaliceLock.prefab`, `CoffinLock.prefab`, and `CryptLock.prefab` contain only the accepting mechanism, with no key inside. Move or reuse each independently. Shapes and materials are saved Unity assets; Blender is not required.

`VampireKey` derives from XRGrabInteractable. Its **Relic** enum defines the identity. The saved **Far Attach Mode = Near** setting brings a ray-selected key to the hand. Hover/select events drive the targeting and held colors. `VampireLock` derives from XRSocketInteractor; **Required Relic** chooses its match, and **On Unlocked** can trigger another scene event later. **Moving Part**, **Opened Local Position**, and **Opened Local Euler Angles** configure the visible opening.

`LeftDirectGrab.prefab` and `RightDirectGrab.prefab` are attached to their matching controller in Ken's_room. `VampireDirectInteractor` joins its parent hand's XRInteractionGroup at startup to avoid competing with that hand's ray interactor. Existing input assets and project settings are unchanged.

Unlock and victory state reset when the scene reloads. There is no inventory UI or persistent save system.

## Escape goal and signifiers

`VampireEscape` on the arrangement prefab updates the original scene object named **Canvas**, on the back wall. It contains the escape heading, goal, progress, grab/offer instructions, and the original controls in five **TextMeshProUGUI** labels on an opaque background. The separate floating Canvas has been removed. Key names, raised lock titles, and lock status use opaque **TextMeshPro** 3D labels, with no legacy TextMesh components. General grabbing and offering explanations appear only on the Canvas. Original controls remain: Right Primary â€” Quit; Right Secondary â€” Change Light Color; Left Primary â€” Switch View Location. Edit the table prefabs or the existing scene Canvas children in the Inspector. The scene VampireRelics instance overrides its heading/progress references to point to that Canvas. It teaches that gold highlights mark grabbable objects, green marks solved locks, and all three named key/lock pairs are required. In simulator Point and Click mode, holding left mouse on a key simulates grip; releasing it offers the key to the table.

Progress advances from 0/3 to 3/3. Victory requires all three distinct locks to finish opening AND retain their matching installed props. Only then does the sign show **YOU ESCAPED!**, fade to green, and invoke **On Escaped** once. The completed scene stays visible for the walkthrough; locomotion is not frozen and the application does not close.

Each lock's indicator blends to green over 1.1 seconds with SmoothStep easing, alongside the existing eased lid or gate motion. Matching props remain in their original visible socket seats. A new round requires reloading the scene.

The pairs share a taught offering rule but differ in shape, theme, and result (heart revealed, coffin opened, crypt gate lifted). Their input mechanic is deliberately the same broad table drop; they do not yet provide increasing puzzle complexity.

### Walkthrough verification

1. Show the goal sign and the three separate key props.
2. Target a key to show its gold highlight, then hold left mouse to grab it (cyan).
3. Drop a wrong prop on a table to demonstrate rejection; retrieve it.
4. Drop each matching prop near a table edge. Show its eased indicator and mechanism, and the progress counter.
5. Confirm one or two solved locks do not win. Frame all three installed keys as the final opening completes and the victory message appears.
## Boarded wall door and hidden Night Crest room

The wall under the existing Canvas now has a 2.4 m wide, 2.5 m tall doorway at world Z=7.5. Four nailed wooden planks seal an opaque oak door. The door and its collision barrier remain closed; entry uses the teleport interaction.

Solve Blood Tithe and Hunter's Seal, in either order. Once both lock animations finish, the boards slide away over 1.1 seconds. The door stays closed. Aim the left controller at the door and press **Left Secondary** to teleport inside. In the simulator use **Shift + 2**. A matching return door on the inside wall uses the same left-controller aim and button to teleport back to the main room.

Night Crest and the bat-medallion pedestal are inside the enclosed crypt beyond the wall. The room has a floor, walls, ceiling, and warm sconces. Its key and socket interactions stay disabled until the boards have cleared, preventing a ray grab or solve through the sealed wall. Night Crest still supplies the third lock required for victory.

Edit **VampireRelics > Boarded wall doorway and hidden crypt**. `BoardedCryptDoor` has explicit references to the two required locks, boards, door hinge, passage collider, status label, and gated interactions. `CryptDoorTeleport` handles both doors, using the existing LeftHand/SecondaryButton input action. Scene overrides assign the XR Origin and left controller. A clear pointing ray within 10 m is required; other surfaces block the ray. Each press teleports once, with a short cooldown, preserving head height and facing the destination. The original `front_plane` is disabled in Ken's_room; three solid wall sections in the relic arrangement replace it around the opening. Keep the arrangement's scene position at (0,0,3) so those sections align with the room boundary.

## Five gate layout (current)

Aim with the left controller and press Left Secondary (simulator Shift + 2). There are five destination pairs; return doors/portals are the reverse route, not additional destinations.

| Entrance | Destination and contents |
| --- | --- |
| Existing house door | Enclosed Blood Tithe chamber, blood vial pedestal and chalice lock |
| Blue car rear side door | Simulated back-seat cabin, coffin on the rear bench |
| Green car rear side door | Separate ordinary back-seat cabin, no key or lock |
| Portal in an east parking bay | Landing terrace on top of the existing planet, silver stake pedestal |
| Boarded wall crypt | Existing Night Crest and bat medallion, available after the first two seals |

The two car doors swing when activated, then teleport the player. Every destination has a return route. The planet's solid landing cap and low perimeter rim are stationary while the original planet continues rotating. Carry the silver stake through the return portal and into the blue car, then release it at the coffin's accepting area. Blood Tithe remains independent and can be solved before or after Hunter's Seal. No extra keys or locks were duplicated.

The new geometry is saved in VampireRelics.prefab: house/cabins are offset beyond the main room, and the green car is a nested instance of the same native PrimitiveCar prefab with green body material overrides. The original blue car and cottage remain in their existing scene locations. Thin gate surfaces align with their visible doors. Keep VampireRelics at (0,0,3); moving the original cottage/car requires moving its corresponding gate and outside-arrival marker too.

CryptDoorTeleport owns all gate input to avoid multiple handlers teleporting on one press. Its Additional Gates list stores the four added entrance/return pairs, destination markers, hints and optional opening hinges. Existing scene XR references are retained. VampireKey moves a held key with the rig during direct gate travel and briefly excludes the jump from its throw velocity.

The original Canvas includes the route instructions and all original controller controls. Table labels remain solid 3D TMP text. For the walkthrough, show the installed vial and stake at their separate destinations before entering the final crypt and installing the bat medallion.

## Blood bat collectibles

One blood bat circles slowly above each of the four existing trees, with animated wings and a gold targeting highlight. Aim the right controller and hold grip, as when grabbing a key. A catch immediately removes that bat and adds one to the house's wall-mounted BAT CAUGHT scoreboard. Left-hand interaction and aiming without grip do not collect it. Each bat becomes visible and catchable again five game seconds after collection; the same respawned bat may be caught again.

The blood vial is authored inactive, leaving its house pedestal empty. The scoreboard initially says MORE BLOOD BATS NEEDED and shows progress toward three catches. At three total catches, BloodBatCollection activates the original vial once and changes the message to BLOOD VIAL READY. The vial remains a normal physical VampireKey and fits the original Blood Tithe lock. Further catches keep increasing the total without creating another vial or moving a vial that has already been taken or installed. A new scene round resets the count and hides the vial again.

BloodBatCollectible derives from XRBaseInteractable, sharing the existing XRI grip/Select input but collecting instead of staying in the hand. Its kinematic body follows a 0.35 m orbit at 18 degrees/second, 2.5 m above each tree origin. Only the model/collider hide during respawn, so its timer continues running. Collection is completed after XRI selection callbacks finish. All bat meshes/materials and the scoreboard are saved native Unity assets in VampireRelics.prefab; Ken's_room assigns BloodBatCollection.rightController to the existing right-hand transform. No input asset or project settings changes are required. General instructions stay on the original Canvas and contain controller controls only.

## Silver stake sequence puzzle

The planet landing plate now holds a compact clue board, three symbol buttons, and a caged silver stake. The stake display is 60% of its former size, and the return portal is smaller and moved aside to leave space. The clue reads: "What is the night's order? When the moon rises, the bat flies, then the vampire rests." Enter Moon, Bat, Coffin by aiming the right controller and pressing grip, releasing between presses. Physical button order is Bat, Coffin, Moon.

PlanetStakePuzzle tracks the ordered sequence. Wrong input resets progress with red feedback; correct symbols turn green. Hover, left-hand input, and moving a held grip across buttons do not enter symbols. After three correct presses the cage rises over 1.2 seconds with eased motion. Only after it clears does the original silver stake become grabbable and its Rigidbody become dynamic. Solving is permanent for that scene round. The same key still fits Hunter's Seal and can travel through gates.

PlanetSymbolButton shares the existing XRI Select input. Ken's_room assigns PlanetStakePuzzle.rightController to the existing right controller. Geometry, references, and the locked initial key state are authored in VampireRelics.prefab. The original Canvas includes the planet puzzle instruction; the local question and buttons use solid TMP text. No input asset changes are needed.

Validated in isolated Unity Play Mode with actual NearFarInteractor selection and controller input events: hand filtering, hover, fresh presses, wrong-order reset, ordered success, intermediate cage movement, delayed key release, right-ray key grab, original coffin acceptance, solved-state persistence, and return portal targeting. Main-project C# compilation also passed. A full main-scene/headset walkthrough has not been performed. Preview images: PlanetPuzzle-locked.png and PlanetPuzzle-unlocked.png.

## Colour-matched car keys

Both car entrances now start locked. Pedestrian 1 wears blue and provides the blue car key; Pedestrian 2 wears green and provides the green car key. Their shirt/sleeve materials match the visible keys. There are no overhead pedestrian hints or sign boards. Approach within 2.5 metres of the right controller, aim at the pedestrian, and make a fresh right-hand grip/Select press. Hover, left-hand selection, and long-distance targeting cannot collect a key. Each pedestrian gives one key per scene round.

The collected key model moves to a small key-ring position beside the right controller, leaving normal relic grabbing available. Both keys can be carried together. Aim left at the matching car door and press Left Secondary to consume/destroy that key, animate the door, and teleport into the cabin. Without its matching key, the door remains shut and displays "car locked". Collecting the matching key immediately changes this to "car unlocked"; the same text remains after the key is consumed on entry. The wrong car does not consume another key. Once unlocked, the car remains accessible for that round; return gates do not require keys. Other destinations and the three relic locks are unchanged.

PedestrianCarKey owns its one-shot collection and consumption state. CryptDoorTeleport.Gate adds requiresCarKey and an explicit carKey reference. ParkingLot.prefab stores the two pickup components, colliders and key models. Ken's_room binds each to the right controller and binds the two outer car gates to their corresponding pedestrians. No new input actions or packages are required.

## Pickup sounds and decoy test cube

All five useful keys play Assets/Ken/sound_effect/yeah.mp3 when collected: blood vial, silver stake, bat medallion, and the blue/green car keys. Normal relics play on each player grab, including re-grabs, but not on hover, release, or socket installation. Car keys play after successful collection onto the controller. AudioSources are native prefab components, with play-on-awake and looping off, spatial blend 1, no Doppler, volume 0.8 and attenuation over 1-8 m. Both short sound clips import as mono with audio data preloaded.

DecoyCube.prefab is a 0.3 m grey cube with a BoxCollider, gravity Rigidbody and standard XRGrabInteractable. One instance sits in front of the house, beside the entrance, initially at world (-2.5, 0.6, 4.2). It falls to the existing floor, can be grabbed and thrown, and plays hmm.mp3 on each pickup. It has no VampireKey identity, so none of the three relic locks accept it.

For more decoys, use distinct models with Rigidbody, collider and XRGrabInteractable; add an AudioSource using hmm.mp3 and PickupSound referencing the grab component. Duplicate the test prefab as a starting point, then replace the model. Copies of the same cube are not unique models for the rubric. This step adds only one test decoy.

## Twelve additional decoy props

The scene now contains 13 distinct decoys, including the original grey cube. The twelve new native prefabs live in Assets/Ken/VampireProps/Decoys. Each has a different compound model, a solid collider, a gravity Rigidbody, a standard XRGrabInteractable, and PickupSound using hmm.mp3. They use the same grab controls as the relics and have no VampireKey identity, so no relic lock accepts them. No extra key-like symbols or overhead hints were added.

| Prop | Placement |
| --- | --- |
| Book | New house side table |
| Candle holder | Outside beside the house entrance |
| Mug | House side table |
| Picture frame | New shelf inside the house |
| Pumpkin | Centre of the empty west parking bay next to the green car, world (-4.55, 0.22, -2.4) |
| Garden trowel | Beside the northeast tree |
| Watering can | Outside beside the house |
| Traffic cone | An empty parking area |
| Soda can | Green car rear seat |
| Sunglasses | Green car rear seat |
| Toy rocket | Planet landing plate, beside the puzzle |
| Dust brush | Crypt floor, away from the relic pedestal |

The existing cube remains in front of the house. Useful key pickups still play yeah. Models use saved native Unity primitive combinations, with saved cone meshes for the traffic cone and rocket nose; no external model imports or runtime model generation are required. Box colliders approximate the props as solid objects. Future decoys can duplicate a prefab and replace its model.

Model preview: Docs/Decoy-models.png. Read left-to-right by row: book/candle holder/mug/picture frame; pumpkin/trowel/watering can/cone; soda can/sunglasses/rocket/brush. This preview uses equivalent built-in shading.

The original test cube is now an ivory die with black pips numbered 1-6; opposing faces sum to seven. It retains the DecoyCube.prefab path/GUID, physical collider, grab settings, hmm sound, and house-front position. Scene name: DecoyDie. There are still 13 distinct decoys.

## Invisible writing on the planet

The planet board now keeps its question and viewing hint visible: "What is the night's order? The moon reveals its secret from the left. Stand on the crescent." Its answer, MOON > BAT > COFFIN, is a separate solid TMP text object using AngleRevealedTMP.shader and MoonSecretText.mat. The original visible answer sentence has been removed.

Stand on the small gold crescent on the left side of the landing plate and look at the board. The shader reveals the answer near a 40-degree left viewing angle: fully visible within 6 degrees, with a 4-degree soft fade outside that band. Straight-on, right-side, and rear views remain invisible. It uses horizontal viewing angle so player height does not prevent discovery. Camera eye selection is stereo-aware; actual headset comfort still needs a walkthrough.

Visibility is calculated in the fragment shader, including fading the TMP outline, with no runtime script toggling the text. The shader adapts the project's existing TMP mobile SDF shader and retains its font atlas and text rendering features. DisableBatching preserves the per-object viewing origin. The material inspector exposes reveal angle, half-width and fade width. The gold crescent is native mesh geometry without a collider, at planet-local (-0.85, 0.004, 0.18), safely within the platform and separate from the return portal and toy rocket.
