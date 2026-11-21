using UnityEngine;

/// <summary>
/// Rotates a real, separate gull-wing door mesh around its roof hinge.
/// Attach to the pivot GameObject, not the imported OBJ mesh child.
/// </summary>
public class ToyTimeMachineDoor : MonoBehaviour
{
    [Tooltip("-1 for left, +1 for right; used to determine upward opening direction.")]
    public int side = -1;

    [Header("Hinge movement")]
    [Range(10f, 100f)] public float openAngle = 67f;
    [Min(10f)] public float angularSpeedDegreesPerSecond = 115f;
    public bool startOpen = false;

    [SerializeField] private bool isOpen;
    private float currentAngle;

    public bool IsOpen { get { return isOpen; } }

    private void Awake()
    {
        isOpen = startOpen;
        currentAngle = isOpen ? openAngle : 0f;
        ApplyAngle();
    }

    private void Update()
    {
        float targetAngle = isOpen ? openAngle : 0f;
        currentAngle = Mathf.MoveTowards(currentAngle, targetAngle,
            angularSpeedDegreesPerSecond * Time.deltaTime);
        ApplyAngle();
    }

    public void Toggle()
    {
        isOpen = !isOpen;
    }

    public void SetOpen(bool open)
    {
        isOpen = open;
    }

    private void ApplyAngle()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f,
            (side < 0 ? -1f : 1f) * currentAngle);
    }
}
