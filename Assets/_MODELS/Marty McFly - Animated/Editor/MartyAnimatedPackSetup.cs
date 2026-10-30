namespace MartyAnimationPack.Editor
{
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;

// This setup applies only to the model distributed beside this editor script.
[InitializeOnLoad]
public static class MartyAnimatedPackSetup
{
    static MartyAnimatedPackSetup() { EditorApplication.delayCall += AutoSetup; }

    static string Folder()
    {
        var guid = AssetDatabase.FindAssets("MartyAnimatedPackSetup t:MonoScript").FirstOrDefault();
        if (string.IsNullOrEmpty(guid)) return null;
        return Path.GetDirectoryName(Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(guid))).Replace('\\', '/');
    }

    static void AutoSetup()
    {
        if (EditorApplication.isCompiling || EditorApplication.isUpdating) { EditorApplication.delayCall += AutoSetup; return; }
        var folder = Folder();
        if (folder != null && (!File.Exists(folder + "/Marty_Animated.prefab") || !File.Exists(folder + "/Marty_Locomotion.controller"))) Setup();
    }

    [MenuItem("Tools/Marty Animated Pack/Build or Refresh Animated Prefab")]
    public static void Setup()
    {
        var folder = Folder();
        if (folder == null) return;
        var modelPath = folder + "/Marty_Humanoid.fbx";
        var importer = AssetImporter.GetAtPath(modelPath) as ModelImporter;
        if (!importer) { Debug.LogError("Marty FBX not found beside setup script."); return; }
        importer.animationType = ModelImporterAnimationType.Human;
        importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
        importer.importAnimation = false;
        importer.materialImportMode = ModelImporterMaterialImportMode.ImportStandard;
        importer.SaveAndReimport();

        var model = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);
        var animator = model.GetComponent<Animator>();
        if (!animator || !animator.avatar || !animator.avatar.isValid || !animator.avatar.isHuman)
        {
            Debug.LogError("Marty: Unity could not build the Humanoid avatar. Select Marty_Humanoid.fbx > Rig > Configure to inspect the bone mapping.");
            return;
        }
        var matFolder = folder + "/Materials";
        if (!AssetDatabase.IsValidFolder(matFolder)) AssetDatabase.CreateFolder(folder, "Materials");
        var isURP = UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline != null;
        var shader = Shader.Find(isURP ? "Universal Render Pipeline/Lit" : "Standard");
        if (!shader) shader = Shader.Find("Standard");
        foreach (var name in new[] { "body", "eyes", "face", "hair", "teeth" })
        {
            var matPath = matFolder + "/" + name + ".mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
            if (!mat) { mat = new Material(shader); AssetDatabase.CreateAsset(mat, matPath); }
            mat.shader = shader;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(folder + "/" + name + ".png");
            if (mat.HasProperty("_BaseMap")) mat.SetTexture("_BaseMap", texture);
            if (mat.HasProperty("_MainTex")) mat.SetTexture("_MainTex", texture);
            if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", .15f);
            if (mat.HasProperty("_Glossiness")) mat.SetFloat("_Glossiness", .15f);
            EditorUtility.SetDirty(mat);
            importer.AddRemap(new AssetImporter.SourceAssetIdentifier(typeof(Material), name), mat);
        }
        importer.SaveAndReimport();
        model = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);
        var visual = (GameObject)PrefabUtility.InstantiatePrefab(model);
        try
        {
            visual.name = "Marty_Visual";
            var a = visual.GetComponent<Animator>();
            a.applyRootMotion = false;
            a.runtimeAnimatorController = BuildAnimator(folder, a.avatar);
            visual.AddComponent<MartyAnimationPack.MartyAnimationDriver>();
            PrefabUtility.SaveAsPrefabAsset(visual, folder + "/Marty_Animated.prefab");
            var skin = visual.GetComponentInChildren<SkinnedMeshRenderer>();
            var report = "Unity version: " + Application.unityVersion + "\nAvatar valid: " + a.avatar.isValid + "\nHumanoid: " + a.avatar.isHuman + "\nVertices: " + skin.sharedMesh.vertexCount + "\nSkin bones: " + skin.bones.Length + "\n";
            foreach (HumanBodyBones bone in Enum.GetValues(typeof(HumanBodyBones)))
            {
                if (bone == HumanBodyBones.LastBone) continue;
                var t = a.GetBoneTransform(bone);
                if (t) report += bone + ": " + t.name + "\n";
            }
            File.WriteAllText(folder + "/Unity_Validation.txt", report);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Marty Humanoid avatar is valid. Use " + folder + "/Marty_Animated.prefab as your controller's visual model.");
        }
        finally { UnityEngine.Object.DestroyImmediate(visual); }
    }
    static readonly string[] ClipNames = { "Idle", "Walk", "Run", "EnterCar", "SeatedIdle", "ExitCar", "TakeDamage", "Die" };
    static AnimatorController BuildAnimator(string folder, Avatar sourceAvatar)
    {
        var clips = new System.Collections.Generic.Dictionary<string, AnimationClip>();
        foreach (var name in ClipNames)
        {
            var path = folder + "/Animations/" + name + ".fbx";
            var imp = AssetImporter.GetAtPath(path) as ModelImporter;
            if (!imp) throw new Exception("Missing animation: " + path);
            imp.animationType = ModelImporterAnimationType.Human;
            imp.avatarSetup = ModelImporterAvatarSetup.CopyFromOther;
            imp.sourceAvatar = sourceAvatar;
            imp.importAnimation = true;
            imp.animationCompression = ModelImporterAnimationCompression.Off;
            var takes = imp.defaultClipAnimations;
            if (takes.Length == 0) throw new Exception("No animation take found in " + path);
            var clip = takes[0];
            clip.name = name;
            clip.loopTime = name == "Idle" || name == "Walk" || name == "Run" || name == "SeatedIdle";
            clip.loopPose = clip.loopTime;
            clip.lockRootRotation = true;
            clip.keepOriginalOrientation = true;
            clip.lockRootPositionXZ = true;
            clip.keepOriginalPositionXZ = true;
            clip.lockRootHeightY = true;
            clip.keepOriginalPositionY = true;
            clip.heightFromFeet = false;
            imp.clipAnimations = new[] { clip };
            imp.SaveAndReimport();
            var animation = AssetDatabase.LoadAllAssetsAtPath(path).OfType<AnimationClip>()
                .FirstOrDefault(c => c.name == name && !c.name.StartsWith("__preview__"));
            if (!animation) throw new Exception("Animation import failed: " + name);
            clips.Add(name, animation);
        }
        var controllerPath = folder + "/Marty_Locomotion.controller";
        var existing = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
        if (existing) return existing; // Preserve any edits to the generated controller.
        var controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
        controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
        controller.AddParameter("EnterCar", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("ExitCar", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("TakeDamage", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("Dead", AnimatorControllerParameterType.Bool);
        var machine = controller.layers[0].stateMachine;
        var locomotion = machine.AddState("Locomotion", new Vector3(240, 30));
        machine.defaultState = locomotion;
        locomotion.writeDefaultValues = false;
        var tree = new BlendTree { name = "Idle Walk Run", blendType = BlendTreeType.Simple1D, blendParameter = "Speed", useAutomaticThresholds = false };
        AssetDatabase.AddObjectToAsset(tree, controller);
        tree.AddChild(clips["Idle"], 0);
        tree.AddChild(clips["Walk"], 1.6f);
        tree.AddChild(clips["Run"], 5.5f);
        locomotion.motion = tree;
        var enter = State(machine, "EnterCar", clips["EnterCar"], "Vehicle", new Vector3(500, -80));
        var seated = State(machine, "SeatedIdle", clips["SeatedIdle"], "Vehicle", new Vector3(760, -80));
        var exit = State(machine, "ExitCar", clips["ExitCar"], "Vehicle", new Vector3(760, 70));
        var hit = State(machine, "TakeDamage", clips["TakeDamage"], "Action", new Vector3(500, 220));
        var die = State(machine, "Die", clips["Die"], "Dead", new Vector3(240, 390));
        Trigger(locomotion, enter, "EnterCar", .12f);
        Finish(enter, seated, .10f);
        Trigger(seated, exit, "ExitCar", .10f);
        Finish(exit, locomotion, .15f);
        Trigger(locomotion, hit, "TakeDamage", .05f);
        Finish(hit, locomotion, .12f);
        var death = machine.AddAnyStateTransition(die);
        death.hasExitTime = false;
        death.hasFixedDuration = true;
        death.duration = .08f;
        death.canTransitionToSelf = false;
        death.AddCondition(AnimatorConditionMode.If, 0, "Dead");
        EditorUtility.SetDirty(controller);
        AssetDatabase.SaveAssets();
        return controller;
    }
    static AnimatorState State(AnimatorStateMachine machine, string name, Motion motion, string tag, Vector3 position)
    {
        var state = machine.AddState(name, position);
        state.motion = motion; state.tag = tag; state.writeDefaultValues = false;
        return state;
    }
    static void Trigger(AnimatorState from, AnimatorState to, string trigger, float duration)
    {
        var transition = from.AddTransition(to);
        transition.hasExitTime = false; transition.hasFixedDuration = true; transition.duration = duration;
        transition.AddCondition(AnimatorConditionMode.If, 0, trigger);
    }
    static void Finish(AnimatorState from, AnimatorState to, float duration)
    {
        var transition = from.AddTransition(to);
        transition.hasExitTime = true; transition.exitTime = 1; transition.hasFixedDuration = true; transition.duration = duration;
    }

}

}
