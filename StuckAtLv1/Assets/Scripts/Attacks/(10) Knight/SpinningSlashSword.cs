using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningSlashSword : MonoBehaviour
{
    private Slot slot;
    private readonly int KNIGHTSWORD_BASE_DAMAGE = 50;
    private int damage;
    private float size;

    public void Activate(Slot s) {
        slot = s;
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        damage = asb.GetDamageBonus(slot, KNIGHTSWORD_BASE_DAMAGE);
        size = asb.GetSizeBonus(slot); transform.localScale = new Vector2(size, size);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
    }
}
