using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelTail : MonoBehaviour
{
    private Slot slot;
    private readonly int SQUIRREL_TAIL_BASE_DMG = 2;
    private float size;
    private int damage;

    public void Activate(Slot s) {
        slot = s;
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        damage = asb.GetDamageBonus(slot, SQUIRREL_TAIL_BASE_DMG);
        size = asb.GetSizeBonus(slot); transform.localScale = new Vector2(size, size);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
    }
}