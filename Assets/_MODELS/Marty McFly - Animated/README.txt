MARTY MCFLY - UNITY ANIMATION PACK

QUICK START
1. Extract this entire folder into your Unity project's Assets folder. Keep its
   Editor, Scripts, Animations and texture files together. Python is not required.
2. Let Unity compile and import the assets. The included editor helper builds:
   - Marty_Locomotion.controller (Unity Animator Controller)
   - Marty_Animated.prefab (model, Animator and animation driver)
   - Materials/ (Universal RP or Built-in materials)
   - Unity_Validation.txt, after Unity confirms a valid Humanoid avatar.
   If these are missing, use Tools > Marty Animated Pack > Build or Refresh
   Animated Prefab, and check the Console for any avatar/import error.
3. Use Marty_Animated.prefab as your existing third-person player's visual model.
   Match its feet to the player's ground origin and use scale 1.
4. The animation driver finds a CharacterController on a parent and reads its
   horizontal velocity to blend Idle, Walk and Run. It does not move the player.
   If your motor uses another movement system, turn off Read Controller Velocity
   and call SetSpeed(metresPerSecond) on the driver from your movement script.
5. If your movement script has an Animator field, assign the Animator on this
   visual model. Keep one active Animator driving the skeleton.

ANIMATIONS (30 FPS)
Idle         3.20 s  looping breathing and weight shift
Walk         1.00 s  looping in-place gait
Run          0.67 s  looping in-place gait
EnterCar     2.40 s  reach, duck and lower into a seated pose
SeatedIdle   3.20 s  looping seated hold/breathing
ExitCar      2.40 s  rise from the matching seated pose
TakeDamage   0.70 s  recoil and recover
Die          2.00 s  buckle, fall backward and hold the last pose

These are generated starter animations, editable in the included Blender source.
The seven requested actions plus SeatedIdle are included. No independent finger,
facial or car-door animation is present.

ANIMATOR CONTROLLER
Float: Speed. Thresholds in metres/second: Idle 0, Walk 1.6, Run 5.5.
Triggers: EnterCar, ExitCar, TakeDamage.
Bool: Dead. True transitions from any state to Die; no automatic return.
Locomotion -> EnterCar -> SeatedIdle -> ExitCar -> Locomotion.
Locomotion -> TakeDamage -> Locomotion.
Root motion is disabled. The character controller supplies travel and turning;
clip pelvis motion supplies visual bobbing, sitting and falling.

GAMEPLAY CONNECTION
The prefab includes MartyAnimationPack.MartyAnimationDriver, with public methods:
SetSpeed(float metresPerSecond), EnterCar(), ExitCar(), TakeDamage(), Die(),
ResetToIdle(). CanMove, IsSeated and IsDead expose the current action status.

Your movement script should skip its movement and turning while CanMove is false.
The driver does not disable your motor, collider, health system or vehicle system.
Wire the appropriate method from your existing interaction or health code.
The UnityEvents On Seated, On Exited Car, and On Death Animation Finished can call
existing gameplay methods from the Inspector.

Car animations are performed relative to the character's current root. Your
vehicle interaction system must align that root with a car-specific entry/seat
anchor, stop player movement, and handle the door, collision and camera. The
seated pelvis ends about 0.5 metres above the root's ground plane. Match your seat
anchor and use On Seated / On Exited Car to complete your vehicle handoff. These
clips do not automatically find a car, open its door, or position the player.

The driver handles visuals and state signals; this is not a replacement movement
controller, vehicle controller, ragdoll or health system.

SOURCE AND PREVIEWS
Animations/*.fbx are native Unity-importable skeletal animation files.
Editor/*.cs and Scripts/*.cs are Unity C# files.
Previews~/*.gif and Animation_Preview.jpg show the actual skinned animation.
Source~/Marty_Animated.blend contains all eight editable actions with textures
packed. Source~/Marty_Humanoid.blend is the original rig source. Unity ignores
folders ending in ~; neither Blender nor Python is required in your game.

VALIDATION
All eight FBX files were re-imported successfully; skeleton counts, animation
keyframes, durations and finite animated values were checked. All four loops
have matching deformed-mesh endpoints. Skinned previews were visually inspected.
The editor and runtime C# scripts compile against the installed Unity 6000.6.6f1
assemblies. Actual Unity Humanoid retargeting, generated-controller behavior and
Play Mode remain unverified because the previous Unity launch could not connect
to the local license service. The helper validates the avatar during import in
your normal Unity session. See animation_manifest.json for the recorded checks.

The helper preserves an existing generated Animator Controller on refresh so
that your own edits are retained. To regenerate its default graph, back up your
edited controller, remove it from this imported pack, and run the menu again.

This package was saved in the chat's outputs folder. Copying into the E: project
was not permitted in this session; extract or move this folder there manually.
