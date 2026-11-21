using UnityEngine;

/// <summary>
/// At 88 miles per hour (39.33952 m/s), starts WORLD-SPACE flame particles at
/// the contact patches of both rear wheels. Leftover particles linger/fade on
/// the road as the vehicle moves away. Stops new emission under 86 mph to
/// prevent flickering around the threshold.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ToyTimeMachineFireTrails : MonoBehaviour
{
    [Header("Vehicle / emitters")]
    public Rigidbody vehicleBody;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;
    public ParticleSystem leftFire;
    public ParticleSystem rightFire;
    public ParticleSystem leftEmbers;
    public ParticleSystem rightEmbers;

    [Header("Speed thresholds (MPH)")]
    [Min(1f)] public float igniteAboveMph = 88f;
    [Min(0f)] public float stopBelowMph = 86f;
    public bool showSpeedHUD = true;

    [Header("Debug (off by default)")]
    public bool previewFireWithoutDriving = false;

    public float CurrentSpeedMph { get; private set; }
    public bool IsBurning { get; private set; }
    private const float MetersPerSecondToMph = 2.2369362921f;

    private void Awake()
    {
        if (vehicleBody == null) vehicleBody = GetComponent<Rigidbody>();
        SetEmitting(false);
    }

    private void Update()
    {
        if (vehicleBody == null) return;
#if UNITY_6000_0_OR_NEWER
        Vector3 velocity = vehicleBody.linearVelocity;
#else
        Vector3 velocity = vehicleBody.velocity;
#endif
        // Horizontal WORLD speed, not tire RPM or falling velocity.
        CurrentSpeedMph = Vector3.ProjectOnPlane(velocity, Vector3.up).magnitude
                           * MetersPerSecondToMph;

        bool shouldBurn = IsBurning
            ? CurrentSpeedMph >= Mathf.Min(stopBelowMph, igniteAboveMph)
            : CurrentSpeedMph >= igniteAboveMph;
        if (previewFireWithoutDriving) shouldBurn = true;
        if (shouldBurn != IsBurning) SetEmitting(shouldBurn);
    }

    private void LateUpdate()
    {
        // Keep the emitters at actual wheel-suspension positions. The emitted
        // particles have WORLD simulation space, so they do NOT follow the car.
        PositionAtContactPatch(rearLeftWheel, leftFire);
        PositionAtContactPatch(rearRightWheel, rightFire);
        PositionAtContactPatch(rearLeftWheel, leftEmbers);
        PositionAtContactPatch(rearRightWheel, rightEmbers);
    }

    private void PositionAtContactPatch(WheelCollider wheel, ParticleSystem effect)
    {
        if (wheel == null || effect == null) return;
        wheel.GetWorldPose(out Vector3 axle, out Quaternion _);
        effect.transform.position = axle
            - transform.up * (wheel.radius - .045f)
            - transform.forward * .12f;
        effect.transform.rotation = transform.rotation * Quaternion.Euler(0f, 180f, 0f);
    }

    public void SetEmitting(bool emit)
    {
        IsBurning = emit;
        UpdateParticle(leftFire, emit);
        UpdateParticle(rightFire, emit);
        UpdateParticle(leftEmbers, emit);
        UpdateParticle(rightEmbers, emit);
    }

    private static void UpdateParticle(ParticleSystem ps, bool emit)
    {
        if (ps == null) return;
        if (emit)
        {
            // Resume emission even if the previous burst still has live particles.
            ps.Play(true);
        }
        else
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    private void OnGUI()
    {
        if (!showSpeedHUD) return;
        // Unstyled debug readout; disable the inspector toggle when using your UI.
        GUI.Box(new Rect(15f, 15f, 216f, 66f),
            "TOY TIME MACHINE     " + CurrentSpeedMph.ToString("0") + " MPH\n"
            + (IsBurning ? "REAR TIRE FIRE: ON" : "FIRE AT 88 MPH"));
    }
}
