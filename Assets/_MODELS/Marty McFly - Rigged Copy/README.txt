MARTY M C F L Y - UNITY HUMANOID RIG

Contents
- Marty_Humanoid.fbx: textured, skinned humanoid model in T-pose.
- Five PNG textures.
- Editor/MartyAvatarSetup.cs: automatic Unity Humanoid/material/prefab setup.
- Source~/Marty_Humanoid.blend: editable Blender 4.2 source with packed textures.
  Unity ignores Source~ so it does not need Blender installed.
- Rig_Preview.png and Deformation_Preview.png: visual checks.
- rig_validation.json: actual checks and remaining validation status.

Use in Unity
1. Keep this entire folder together under Assets. Allow Unity to finish compiling.
2. The included helper configures the FBX as Humanoid, creates materials, and
   generates Marty_Visual.prefab. It supports Universal RP and Built-in RP.
   If needed, run Tools > Marty McFly > Build or Refresh Humanoid Prefab.
3. Use Marty_Visual as the visual model for your third-person player. Assign your
   existing Humanoid locomotion Animator Controller to its Animator. Set your
   movement script's Animator reference to that Animator if it exposes one.
4. If your controller requires the Animator on its player root, use its existing
   model-replacement workflow and select the Marty_HumanoidAvatar generated from
   this FBX. Keep a single Animator controlling this skeleton.
5. The model is approximately 1.656 metres tall at scale 1. Place its feet at the
   player ground origin. Match your player's capsule to its height. Root motion
   is disabled on the generated visual prefab for a movement-driven controller.

Scope
This is a model rig, not a third-person movement system or animation pack.
Use your existing Humanoid idle/walk/run/jump animations and controller.
The rig has 21 bones including shoulders, hands, feet and toes. Fingers and
facial features have no independent animation controls.

Validation
All 3,963 vertices are weighted, normalized and limited to four influences.
Automatic heat weighting succeeded without fallback assignments. FBX was
re-imported successfully in Blender and the skin data was checked. Bent-limb
preview shows shoulder, elbow, hip and knee deformation.
Unity 6000.6.6f1 was launched for validation but could not connect to its license
service in this session. Unity Humanoid validity therefore remains unconfirmed.
The helper checks avatar.isValid and avatar.isHuman in your normal Unity session
before creating the prefab, and writes Unity_Validation.txt on success.
If Unity reports a mapping issue, select the FBX > Rig > Configure; inspect the
bone mapping and use Pose > Enforce T-Pose, then Apply.

The original source folder and its files have not been changed.
