using UnityEngine;

public class WolfClawParent : MonoBehaviour
{
    private readonly float WOLFCLAW_BASE_DURATION = 3f;
    private readonly float ROTATION_REFRESH_RATE = 0.3f;
    private float rotation;
    private float duration;
    private float size;
    private Slot slot;
    void Start() {
        slot = GetComponentInParent<Slot>();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        rotation = 0.3f;

        duration = asb.GetDurationBonus(slot, WOLFCLAW_BASE_DURATION);

        size = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(size, size);
    }

    void Update() {
        duration -= Time.deltaTime;
        rotation -= Time.deltaTime;

        if (duration <= 0) {
            Destroy(gameObject);
        }

        if (rotation <= 0) {
            RandomRotate();
        }
    }

    public Slot GetSlot() {
        return slot;
    }

    private void RandomRotate() {
        transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0,361));
        rotation = ROTATION_REFRESH_RATE;
    }
}