using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// <summary>
/// Simple WheelCollider-powered controller for the photo-inspired toy car.
/// Coordinate convention: +Z forward, Y up, X across the axles.
/// W/S or Up/Down accelerate and reverse, A/D or Left/Right steer, Space brakes.
/// This uses Unity physics (Rigidbody + four WheelColliders), not an animation.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ToyTimeMachineController : MonoBehaviour
{
    [Header("Physics wheels")]
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    [Header("Independent wheel visuals")]
    public Transform frontLeftVisual;
    public Transform frontRightVisual;
    public Transform rearLeftVisual;
    public Transform rearRightVisual;

    [Header("Driving")]
    [Min(0)] public float maximumMotorTorque = 850f;
    [Range(0, 45)] public float maximumSteerAngle = 29f;
    [Min(0)] public float maximumBrakeTorque = 2600f;
    [Min(1)] public float steeringFadeSpeed = 28f;
    [Min(0)] public float aerodynamicDownforce = 22f;
    public bool fourWheelDrive = false;

    private Rigidbody body;
    private float throttleInput;
    private float steerInput;
    private bool brakeInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        // Root origin is near ground level; lower the COM to resist tipping.
        body.centerOfMass = new Vector3(0f, 0.48f, -0.1f);
    }

    private void Update()
    {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        Keyboard kb = Keyboard.current;
        if (kb != null)
        {
            throttleInput = (kb.wKey.isPressed || kb.upArrowKey.isPressed ? 1f : 0f)
                          - (kb.sKey.isPressed || kb.downArrowKey.isPressed ? 1f : 0f);
            steerInput = (kb.dKey.isPressed || kb.rightArrowKey.isPressed ? 1f : 0f)
                       - (kb.aKey.isPressed || kb.leftArrowKey.isPressed ? 1f : 0f);
            brakeInput = kb.spaceKey.isPressed;
        }
        else { throttleInput = 0; steerInput = 0; brakeInput = true; }
#else
        throttleInput = Input.GetAxisRaw("Vertical");
        steerInput = Input.GetAxisRaw("Horizontal");
        brakeInput = Input.GetKey(KeyCode.Space);
#endif
    }

    private void FixedUpdate()
    {
        if (frontLeft == null || frontRight == null || rearLeft == null || rearRight == null)
            return;

#if UNITY_6000_0_OR_NEWER
        float speed = body.linearVelocity.magnitude;
#else
        float speed = body.velocity.magnitude;
#endif
        float steerFactor = Mathf.Lerp(1f, 0.45f, Mathf.Clamp01(speed / steeringFadeSpeed));
        float steer = steerInput * maximumSteerAngle * steerFactor;
        frontLeft.steerAngle = steer;
        frontRight.steerAngle = steer;

        float motor = throttleInput * maximumMotorTorque;
        float brake = brakeInput ? maximumBrakeTorque : 0f;
        float factor = fourWheelDrive ? .25f : .5f;

        frontLeft.motorTorque = fourWheelDrive ? motor * factor : 0f;
        frontRight.motorTorque = fourWheelDrive ? motor * factor : 0f;
        rearLeft.motorTorque = motor * factor;
        rearRight.motorTorque = motor * factor;

        frontLeft.brakeTorque = brake;
        frontRight.brakeTorque = brake;
        rearLeft.brakeTorque = brake;
        rearRight.brakeTorque = brake;

        body.AddForce(-transform.up * (aerodynamicDownforce * speed), ForceMode.Force);
    }

    private void LateUpdate()
    {
        UpdateVisual(frontLeft, frontLeftVisual);
        UpdateVisual(frontRight, frontRightVisual);
        UpdateVisual(rearLeft, rearLeftVisual);
        UpdateVisual(rearRight, rearRightVisual);
    }

    private static void UpdateVisual(WheelCollider physicsWheel, Transform visual)
    {
        if (physicsWheel == null || visual == null) return;
        physicsWheel.GetWorldPose(out Vector3 position, out Quaternion rotation);
        visual.SetPositionAndRotation(position, rotation);
    }
}
