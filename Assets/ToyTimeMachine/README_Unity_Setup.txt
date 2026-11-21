TOY TIME MACHINE — UNITY VEHICLE PACKAGE v2
==========================================

UPGRADE FEATURES (v2)
- Separate DoorLeft.obj and DoorRight.obj with actual hinged meshes including the
  outer roof wing, side window, door panel, handles and mirror; doors do NOT leave
  their original outer body panels behind when opened.
- Click/tap a door in the Game view to toggle it, or press Q (left) / E (right).
  Open angle 67 degrees; angle and open/close speed are inspector-adjustable.
- A basic interior is visible when the doors are open.
- FIRE FROM BOTH REAR TIRES when the Rigidbody moves >= 88 mph (~39.34 m/s).
  World-space particles linger behind the moving car to form two tire trails.
  Emission stops below 86 mph to avoid flicker around the threshold.
- Real-time MPH debug readout, and optional inspector preview of the fire FX.

SOURCE / ACCURACY
Stylized, low-poly mesh approximated from the supplied ONE PHOTO of a gray toy
DeLorean-style time machine coupe. The unseen sides, underside and interior are
estimated. This is an authored game asset, NOT a photogrammetric 3D scan.

FILES
- CarBody.obj                 Body and cabin, excluding the moving doors.
- DoorLeft.obj                Left door, pivoted at its roof hinge.
- DoorRight.obj               Right door, pivoted at its roof hinge.
- Wheel.obj                   Single axle-centered wheel (instantiate four times).
- CarComplete_Preview.obj     STATIC closed car for non-interactive previews ONLY.
- ToyTimeMachine.mtl          Material color definitions for OBJ import.
- FireFlame.png               Generated alpha-blended particle sprite.
- ToyTimeMachineController.cs Rigidbody/WheelCollider driving physics.
- ToyTimeMachineDoor.cs       Individually animatable door hinge component.
- ToyTimeMachineInteraction.cs Mouse/touch raycast and Q/E door toggles.
- ToyTimeMachineFireTrails.cs Speed check + 4 tire particle emitters.
- Editor/BuildToyCarPrefab.cs Automatic prefab/material/FX builder.
- mesh_info.json             Model dimensions and door/wheel pivots.
- ModelPreview.png            Static preview with doors closed.
- ModelPreview_DoorsOpen.png  STATIC illustration with doors open.
- build_toycar_source.py     Python source used to generate OBJ models.

IMPORT / UPDATE AN EXISTING INSTALL
1. Delete or replace the previous Assets/ToyTimeMachine folder's package files;
   DO NOT use the older CarBody.obj or older Editor/BuildToyCarPrefab.cs.
   You can leave a separately customized prefab elsewhere in your project,
   but update it manually if you need to preserve your custom settings.
2. Extract the ZIP. Place all files from ToyTimeMachine_Unity inside the folder
      Assets/ToyTimeMachine/
   Preserve the Editor subfolder. A folder named ToyTimeMachine (rather than
   ToyTimeMachine_Unity) is necessary for the included automatic prefab builder.
3. Wait for Unity to import the files and compile the C# scripts.
4. In the Unity toolbar, choose:
      Tools > Toy Time Machine > Create / Update Drivable Prefab
   This creates or replaces:
      Assets/ToyTimeMachine/ToyTimeMachine_Drivable.prefab
   and auto-creates FireParticles.mat if it does not already exist.
5. Drag this PREFAB into your scene. Remove any previous old prefab instance.
6. Add a ground Plane/Terrain with a non-trigger Collider. Ensure the car is
   a little above the ground to begin with, and use a Camera tagged MainCamera
   facing the car if you want click-to-open doors.
7. Press Play.

CONTROLS (DEFAULT)
- W / Up                 Drive forward
- S / Down               Reverse
- A / D, Left / Right    Steer
- Space                  Brake
- Q                      Toggle LEFT gull-wing door
- E                      Toggle RIGHT gull-wing door
- Left click on a door   Toggle clicked door (requires Camera tagged MainCamera)
- Tap on a door          Toggle when New Input System touchscreen enabled

SPEED AND FIRE
- The trigger uses the car Rigidbody's horizontal WORLD speed in meters per
  second, converted to miles per hour (1 m/s = 2.2369362921 mph).
- Crossing 88 mph starts two flame AND two ember particle systems, one pair
  attached to each REAR wheel's contact patch (not an exhaust pipe).
- Emitted particles simulate in WORLD space and persist briefly behind the
  moving vehicle. Falling vertically does not count toward the speed threshold.
- When speed dips below 86 mph, no new fire particles are created; those already
  emitted fade naturally.
- In the Inspector on ToyTimeMachineFireTrails you can customize the ignition
  speed, extinction speed, and toggle showSpeedHUD. To view the effect at rest,
  check previewFireWithoutDriving ONLY FOR TESTING; leave disabled for gameplay.
- No scripting hooks are required: the prefab builder links the wheels and
  fire particle systems automatically.

DOOR INTERACTION / INTEGRATION WITH YOUR OWN CONTROLLER
- Doors are separate imported assets parented under hinge transforms.
- Door_L(eft)/Door_R(ight)_Hinge hold ToyTimeMachineDoor components and click
  trigger hitboxes that move together with the visual mesh.
- Q/E and left-click are handled by ToyTimeMachineInteraction; they do not
  require a separate first-person or third-person character controller.
- You can call leftDoor.SetOpen(true), leftDoor.SetOpen(false), leftDoor.Toggle()
  (same on rightDoor) from your own Unity UI, buttons, or interaction system.
- Your existing driving script can replace ToyTimeMachineController as long as
  the Rigidbody and rear WheelColliders are assigned to ToyTimeMachineFireTrails.
- Door hinges are LOCAL transforms. Left hinge (-.325,1.478,-.25), right hinge
  (.325,1.478,-.25), all meters; +Z points toward the front of the car.

UNITY / RENDER PIPELINE NOTES
- Mesh scale: 1 Unity unit = 1 meter; set OBJ importer Scale Factor to 1 if needed.
- Unity 2022.3 / 2023 / 6 (expected); both Input Manager and new Input System
  have conditional keyboard/mouse support.
- FireParticles.mat is generated with a suitable built-in or URP particle shader.
  For HDRP, if you see pink/missing fire, assign an HDRP particle-compatible
  transparent material to the four particle renderers, using FireFlame.png.
- OBJ .mtl colors may require manual reassignment with URP/HDRP Lit shaders.
- If Unity already generated a FireParticles.mat in an older run, you may delete
  that file to have the builder regenerate it with the active render pipeline.
- Do not use CarComplete_Preview.obj as the drivable prefab visual: it includes
  wheels and both doors baked shut. Use CarBody, DoorLeft, DoorRight and Wheel.
- A live Unity Editor/runtime is not available in this generation environment;
  please test physics, particle material appearance, input and door hit testing
  in your target Unity version after importing.

LICENSE
Generated for the requesting user based on their supplied photo. You may
modify and use these generated assets in your own Unity project.
