using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfClaw : MonoBehaviour
{
    [SerializeField] private WolfClawParent parent;
    private readonly int WOLFCLAW_BASE_DAMAGE = 5;
    private int damage;
    private Slot slot;
    void Start() {
        slot = parent.GetSlot();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        damage = asb.GetDamageBonus(slot, WOLFCLAW_BASE_DAMAGE);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
    }
}
