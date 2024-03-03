using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnemiaScreenBlast : MonoBehaviour
{
    private Slot slot;
    private readonly int ANEMIA_SCREEN_BLAST_BASE_DMG = 2;
    private int damage;
    void Start() {
        slot = GetComponentInParent<Slot>();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        damage = asb.GetDamageBonus(slot, ANEMIA_SCREEN_BLAST_BASE_DMG);
        Destroy(gameObject, 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null && enemy.IsAnemic()) {
            FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
        }
    }
}
