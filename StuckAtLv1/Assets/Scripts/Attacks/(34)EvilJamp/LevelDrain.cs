using UnityEngine;

public class LevelDrain : MonoBehaviour
{
    private readonly int LEVEL_DRAIN_BASE_DAMAGE = 75;
    private Slot slot;
    private int damage;
    private bool levelDrained;
    void Start() {
        slot = GetComponentInParent<LevelDrainRotate>().GetSlot();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        damage = asb.GetDamageBonus(slot, LEVEL_DRAIN_BASE_DAMAGE);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Enemy>(out var enemy)) {
            if (enemy.CompareTag("DarkJamp") && !levelDrained) {
                enemy.GetComponent<EvilJamp>().LevelDown();
                levelDrained = true;
            }
        }
        FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
    }
}