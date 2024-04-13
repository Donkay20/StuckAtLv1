using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleOuter : MonoBehaviour
{
    [SerializeField] private MagicCircle magicCircle;
    private readonly int MAGIC_CIRCLE_BASE_DAMAGE = 10;
    private Slot slot;
    private int damage;

    void Start() {
        slot = magicCircle.GetSlot();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        damage = asb.GetDamageBonus(slot, MAGIC_CIRCLE_BASE_DAMAGE);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent<Enemy>(out var enemy)) {
            FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
            enemy.ApplySlow(0.8f, 1f);
        }
    }
}