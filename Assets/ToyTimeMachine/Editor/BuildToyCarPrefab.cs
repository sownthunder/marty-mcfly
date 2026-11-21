#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// Import everything to Assets/ToyTimeMachine, then:
/// Tools > Toy Time Machine > Create / Update Drivable Prefab
/// Uses moving gull-wing roof/door OBJ meshes, clickable trigger hitboxes,
/// wheel colliders, and particle trails at each rear tire's contact patch.
/// </summary>
public static class BuildToyCarPrefab
{
    private const string Folder = "Assets/ToyTimeMachine";

    [MenuItem("Tools/Toy Time Machine/Create / Update Drivable Prefab")]
    [MenuItem("Tools/Toy Time Machine/Create Drivable Prefab")]
    public static void Create()
    {
        GameObject bodyAsset = LoadModel("CarBody.obj");
        GameObject wheelAsset = LoadModel("Wheel.obj");
        GameObject leftDoorAsset = LoadModel("DoorLeft.obj");
        GameObject rightDoorAsset = LoadModel("DoorRight.obj");
        if (!bodyAsset || !wheelAsset || !leftDoorAsset || !rightDoorAsset)
        {
            EditorUtility.DisplayDialog("Missing OBJ files",
                "Copy the entire current package into Assets/ToyTimeMachine. " +
                "CarBody.obj, Wheel.obj, DoorLeft.obj and DoorRight.obj must exist there.", "OK");
            return;
        }

        GameObject root = new GameObject("ToyTimeMachine_Drivable");
        root.transform.position = new Vector3(0f, .12f, 0f);
        try
        {
            AddImportedVisual(root.transform, bodyAsset, "CarBody_Visual");
            Rigidbody rb = root.AddComponent<Rigidbody>();
            rb.mass = 1100f;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
#if UNITY_6000_0_OR_NEWER
            rb.linearDamping = .1f;
            rb.angularDamping = .6f;
#else
            rb.drag = .1f;
            rb.angularDrag = .6f;
#endif

            BoxCollider chassis = root.AddComponent<BoxCollider>();
            chassis.center = new Vector3(0f, .70f, 0f);
            chassis.size = new Vector3(1.67f, .52f, 4.18f);

            GameObject cabin = NewChild(root.transform, "Cabin_Collider");
            BoxCollider cabCol = cabin.AddComponent<BoxCollider>();
            cabCol.center = new Vector3(0f, 1.23f, -.27f);
            cabCol.size = new Vector3(1.20f, .38f, 1.10f);

            ToyTimeMachineController driving = root.AddComponent<ToyTimeMachineController>();
            SetWheel(root.transform, wheelAsset, "FL", new Vector3(-.937f, .355f, 1.35f),
                out driving.frontLeft, out driving.frontLeftVisual);
            SetWheel(root.transform, wheelAsset, "FR", new Vector3(.937f, .355f, 1.35f),
                out driving.frontRight, out driving.frontRightVisual);
            SetWheel(root.transform, wheelAsset, "RL", new Vector3(-.937f, .355f, -1.34f),
                out driving.rearLeft, out driving.rearLeftVisual);
            SetWheel(root.transform, wheelAsset, "RR", new Vector3(.937f, .355f, -1.34f),
                out driving.rearRight, out driving.rearRightVisual);

            ToyTimeMachineDoor left = CreateDoor(root.transform, "Left", leftDoorAsset,
                -1, new Vector3(-.325f, 1.478f, -.25f));
            ToyTimeMachineDoor right = CreateDoor(root.transform, "Right", rightDoorAsset,
                +1, new Vector3(.325f, 1.478f, -.25f));
            ToyTimeMachineInteraction interactor = root.AddComponent<ToyTimeMachineInteraction>();
            interactor.leftDoor = left;
            interactor.rightDoor = right;

            Material fireMat = GetOrMakeFireMaterial();
            ToyTimeMachineFireTrails trails = root.AddComponent<ToyTimeMachineFireTrails>();
            trails.vehicleBody = rb;
            trails.rearLeftWheel = driving.rearLeft;
            trails.rearRightWheel = driving.rearRight;
            trails.leftFire = CreateParticles(root.transform, "FireTrail_LeftRear", fireMat, false);
            trails.rightFire = CreateParticles(root.transform, "FireTrail_RightRear", fireMat, false);
            trails.leftEmbers = CreateParticles(root.transform, "Embers_LeftRear", fireMat, true);
            trails.rightEmbers = CreateParticles(root.transform, "Embers_RightRear", fireMat, true);

            string target = Folder + "/ToyTimeMachine_Drivable.prefab";
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, target);
            if (prefab != null)
            {
                Selection.activeObject = prefab;
                EditorGUIUtility.PingObject(prefab);
                Debug.Log("Toy Time Machine prefab updated: Q/E gull-wing doors; click doors; " +
                    "WASD drives; rear tire fire at 88 MPH. Assign a MainCamera to use clicking.");
            }
        }
        finally
        {
            Object.DestroyImmediate(root);
        }
    }

    private static GameObject LoadModel(string name)
    {
        return AssetDatabase.LoadAssetAtPath<GameObject>(Folder + "/" + name);
    }

    private static GameObject NewChild(Transform parent, string name)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        return go;
    }

    private static GameObject AddImportedVisual(Transform parent, GameObject asset, string name)
    {
        GameObject visual = (GameObject)PrefabUtility.InstantiatePrefab(asset);
        visual.name = name;
        visual.transform.SetParent(parent, false);
        visual.transform.localPosition = Vector3.zero;
        visual.transform.localRotation = Quaternion.identity;
        return visual;
    }

    private static ToyTimeMachineDoor CreateDoor(Transform parent, string name,
        GameObject model, int side, Vector3 hingePosition)
    {
        GameObject pivot = NewChild(parent, "Door_" + name + "_Hinge");
        pivot.transform.localPosition = hingePosition;
        pivot.transform.localRotation = Quaternion.identity;
        AddImportedVisual(pivot.transform, model, "Door_" + name + "_Mesh");

        ToyTimeMachineDoor door = pivot.AddComponent<ToyTimeMachineDoor>();
        door.side = side;
        door.openAngle = 67f;

        // These are trigger colliders solely for interaction, NOT for physics.
        // They turn together with the animated mesh and catch raycasts.
        BoxCollider panelClick = pivot.AddComponent<BoxCollider>();
        panelClick.isTrigger = true;
        panelClick.center = new Vector3(side * .596f, -.706f, .31f);
        panelClick.size = new Vector3(.14f, .53f, 1.12f);

        BoxCollider windowClick = pivot.AddComponent<BoxCollider>();
        windowClick.isTrigger = true;
        windowClick.center = new Vector3(side * .430f, -.235f, .405f);
        windowClick.size = new Vector3(.16f, .40f, .80f);

        BoxCollider roofClick = pivot.AddComponent<BoxCollider>();
        roofClick.isTrigger = true;
        roofClick.center = new Vector3(side * .165f, .010f, .01f);
        roofClick.size = new Vector3(.31f, .05f, .90f);
        return door;
    }

    private static void SetWheel(Transform parent, GameObject wheelAsset, string id,
        Vector3 center, out WheelCollider wheelCollider, out Transform wheelVisual)
    {
        GameObject physicsGO = NewChild(parent, "WheelCollider_" + id);
        physicsGO.transform.localPosition = center;
        wheelCollider = physicsGO.AddComponent<WheelCollider>();
        wheelCollider.radius = .342f;
        wheelCollider.mass = 18f;
        wheelCollider.suspensionDistance = .17f;
        wheelCollider.forceAppPointDistance = .02f;
        JointSpring spring = wheelCollider.suspensionSpring;
        spring.spring = 29000f;
        spring.damper = 3900f;
        spring.targetPosition = .50f;
        wheelCollider.suspensionSpring = spring;

        GameObject visualRoot = NewChild(parent, "Wheel_" + id + "_VisualPivot");
        visualRoot.transform.localPosition = center;
        AddImportedVisual(visualRoot.transform, wheelAsset, "Wheel_Mesh");
        wheelVisual = visualRoot.transform;
    }

    private static Material GetOrMakeFireMaterial()
    {
        string target = Folder + "/FireParticles.mat";
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(target);
        if (mat != null) return mat;

        // Select a shader that can display transparent particles in the active RP.
        Shader shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
        if (shader == null) shader = Shader.Find("Particles/Additive");
        if (shader == null) shader = Shader.Find("Particles/Standard Unlit");
        if (shader == null) shader = Shader.Find("Sprites/Default");
        if (shader == null) shader = Shader.Find("Unlit/Transparent");
        if (shader == null)
            throw new System.InvalidOperationException("No compatible particle shader was found.");
        mat = new Material(shader);
        mat.name = "FireParticles";
        Texture2D fireSprite = AssetDatabase.LoadAssetAtPath<Texture2D>(Folder + "/FireFlame.png");
        if (fireSprite != null)
        {
            if (mat.HasProperty("_BaseMap")) mat.SetTexture("_BaseMap", fireSprite);
            if (mat.HasProperty("_MainTex")) mat.SetTexture("_MainTex", fireSprite);
        }
        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", Color.white);
        if (mat.HasProperty("_Color")) mat.SetColor("_Color", Color.white);
        // URP particle shader: alpha blended transparent rendering.
        if (mat.HasProperty("_Surface"))
        {
            mat.SetFloat("_Surface", 1f);
            if (mat.HasProperty("_Blend")) mat.SetFloat("_Blend", 0f);
            if (mat.HasProperty("_SrcBlend")) mat.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            if (mat.HasProperty("_DstBlend")) mat.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            if (mat.HasProperty("_ZWrite")) mat.SetFloat("_ZWrite", 0f);
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }
        AssetDatabase.CreateAsset(mat, target);
        return mat;
    }

    private static ParticleSystem CreateParticles(Transform parent, string name,
        Material material, bool embers)
    {
        GameObject go = NewChild(parent, name);
        go.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        ParticleSystem ps = go.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.playOnAwake = false;
        main.loop = true;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.startLifetime = embers
            ? new ParticleSystem.MinMaxCurve(.5f, 1.3f)
            : new ParticleSystem.MinMaxCurve(.5f, .93f);
        main.startSpeed = embers
            ? new ParticleSystem.MinMaxCurve(.18f, .8f)
            : new ParticleSystem.MinMaxCurve(.09f, .35f);
        main.startSize = embers
            ? new ParticleSystem.MinMaxCurve(.035f, .072f)
            : new ParticleSystem.MinMaxCurve(.17f, .27f);
        main.maxParticles = embers ? 400 : 650;
        main.startRotation = new ParticleSystem.MinMaxCurve(0f, Mathf.PI * 2f);

        var emission = ps.emission;
        emission.enabled = true;
        emission.rateOverTime = embers ? 36f : 105f;
        var shape = ps.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = embers ? 27f : 12f;
        shape.radius = embers ? .075f : .095f;
        shape.length = .02f;

        var color = ps.colorOverLifetime;
        color.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            embers
                ? new [] {
                    new GradientColorKey(new Color(1f, .80f, .29f), 0f),
                    new GradientColorKey(new Color(1f, .28f, .03f), .65f),
                    new GradientColorKey(new Color(.62f, .06f, .01f), 1f) }
                : new [] {
                    new GradientColorKey(new Color(1f, .95f, .46f), 0f),
                    new GradientColorKey(new Color(1f, .45f, .08f), .45f),
                    new GradientColorKey(new Color(.88f, .10f, .03f), 1f) },
            new [] {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(1f, .13f),
                new GradientAlphaKey(.80f, .48f),
                new GradientAlphaKey(0f, 1f) });
        color.color = new ParticleSystem.MinMaxGradient(gradient);

        var size = ps.sizeOverLifetime;
        size.enabled = true;
        size.size = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(
            new Keyframe(0f, .48f),
            new Keyframe(.25f, 1.0f),
            new Keyframe(1f, .02f)));

        var noise = ps.noise;
        noise.enabled = true;
        noise.strength = embers ? .20f : .15f;
        noise.frequency = .8f;

        ParticleSystemRenderer renderer = go.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.sharedMaterial = material;
        renderer.sortingFudge = 0f;
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        return ps;
    }
}
#endif
