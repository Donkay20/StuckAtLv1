using UnityEngine;

public class SpinningSlashRotation : MonoBehaviour
{
    [SerializeField] private SpinningSlashSword sword;
    private readonly float KNIGHTSWORD_BASE_DURATION = 2f;
    private readonly float KNIGHTSWORD_ROTATION_SPEED = -9f;  //inconsistent rotation on build, fix later
    private float duration;
    private Slot slot;
    void Start() {
        slot = GetComponentInParent<Slot>();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        sword.Activate(slot);
        duration = asb.GetDurationBonus(slot, KNIGHTSWORD_BASE_DURATION);
    }

    void Update() {
        duration -= Time.deltaTime;
        transform.Rotate(0, 0, KNIGHTSWORD_ROTATION_SPEED);
        if (duration <= 0) {
            Destroy(gameObject);
        }
    }
}
