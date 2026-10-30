using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

// This setup applies only to the model distributed beside this editor script.
[InitializeOnLoad]
public static class MartyAvatarSetup
{
    static MartyAvatarSetup() { EditorApplication.delayCall += AutoSetup; }

    static string Folder()
    {
        var guid = AssetDatabase.FindAssets("MartyAvatarSetup t:MonoScript").FirstOrDefault();
        if (string.IsNullOrEmpty(guid)) return null;
        return Path.GetDirectoryName(Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(guid))).Replace('\\', '/');
    }

    static void AutoSetup()
    {
        if (EditorApplication.isCompiling || EditorApplication.isUpdating) { EditorApplication.delayCall += AutoSetup; return; }
        var folder = Folder();
        if (folder != null && !File.Exists(folder + "/Marty_Visual.prefab")) Setup();
    }

    [MenuItem("Tools/Marty McFly/Build or Refresh Humanoid Prefab")]
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
            PrefabUtility.SaveAsPrefabAsset(visual, folder + "/Marty_Visual.prefab");
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
            Debug.Log("Marty Humanoid avatar is valid. Use " + folder + "/Marty_Visual.prefab as your controller's visual model.");
        }
        finally { UnityEngine.Object.DestroyImmediate(visual); }
    }
}
