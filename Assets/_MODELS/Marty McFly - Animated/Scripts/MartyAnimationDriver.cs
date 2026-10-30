using UnityEngine;
using UnityEngine.Events;

namespace MartyAnimationPack
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public sealed class MartyAnimationDriver : MonoBehaviour
    {
        [Tooltip("Optional existing player CharacterController. This component never moves it.")]
        public CharacterController characterController;
        public bool readControllerVelocity = true;
        [Min(0)] public float speedDamping = 0.12f;
        public UnityEvent onSeated;
        public UnityEvent onExitedCar;
        public UnityEvent onDeathAnimationFinished;
        Animator animator;
        bool dead, vehicleLocked, exiting, notifiedSeated, notifiedDeath;
        static readonly int Speed = Animator.StringToHash("Speed");
        static readonly int Dead = Animator.StringToHash("Dead");
        public bool IsDead => dead;
        public bool IsSeated => Animator && Animator.GetCurrentAnimatorStateInfo(0).IsName("SeatedIdle");
        public bool CanMove => !dead && !vehicleLocked && !IsActionPlaying;
        public bool IsActionPlaying
        {
            get
            {
                if (!Animator) return false;
                var current = Animator.GetCurrentAnimatorStateInfo(0);
                if (current.IsTag("Action") || current.IsTag("Vehicle") || current.IsTag("Dead")) return true;
                if (!Animator.IsInTransition(0)) return false;
                var next = Animator.GetNextAnimatorStateInfo(0);
                return next.IsTag("Action") || next.IsTag("Vehicle") || next.IsTag("Dead");
            }
        }
        Animator Animator { get { if (!animator) animator = GetComponent<Animator>(); return animator; } }
        void Awake()
        {
            animator = GetComponent<Animator>();
            if (!characterController) characterController = GetComponentInParent<CharacterController>();
        }
        void LateUpdate()
        {
            if (readControllerVelocity && characterController)
            {
                var velocity = characterController.velocity;
                SetSpeed(new Vector2(velocity.x, velocity.z).magnitude);
            }
            if (IsSeated && !Animator.IsInTransition(0) && !notifiedSeated)
            {
                notifiedSeated = true;
                onSeated?.Invoke();
            }
            if (exiting && !Animator.IsInTransition(0) && Animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion"))
            {
                vehicleLocked = false;
                exiting = false;
                notifiedSeated = false;
                onExitedCar?.Invoke();
            }
            var state = Animator.GetCurrentAnimatorStateInfo(0);
            if (dead && state.IsName("Die") && state.normalizedTime >= 1 && !notifiedDeath)
            {
                notifiedDeath = true;
                onDeathAnimationFinished?.Invoke();
            }
        }
        // metres per second: Idle 0, Walk 1.6, Run 5.5.
        public void SetSpeed(float metresPerSecond)
        {
            Animator.SetFloat(Speed, CanMove ? Mathf.Max(0, metresPerSecond) : 0, speedDamping, Time.deltaTime);
        }
        public void EnterCar()
        {
            if (!CanMove) return;
            vehicleLocked = true;
            Animator.SetFloat(Speed, 0);
            Animator.SetTrigger("EnterCar");
        }
        public void ExitCar()
        {
            if (dead || !IsSeated || exiting) return;
            exiting = true;
            Animator.SetTrigger("ExitCar");
        }
        public void TakeDamage()
        {
            if (!CanMove) return;
            Animator.SetTrigger("TakeDamage");
        }
        public void Die()
        {
            if (dead) return;
            dead = true;
            Animator.ResetTrigger("EnterCar");
            Animator.ResetTrigger("ExitCar");
            Animator.ResetTrigger("TakeDamage");
            Animator.SetFloat(Speed, 0);
            Animator.SetBool(Dead, true);
        }
        public void ResetToIdle()
        {
            dead = vehicleLocked = exiting = notifiedSeated = notifiedDeath = false;
            Animator.ResetTrigger("EnterCar");
            Animator.ResetTrigger("ExitCar");
            Animator.ResetTrigger("TakeDamage");
            Animator.SetBool(Dead, false);
            Animator.SetFloat(Speed, 0);
            Animator.Play("Locomotion", 0, 0);
        }
    }
}
