using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RottenApple : MonoBehaviour
{
    [SerializeField] GameObject healthDrop;
    private Slot slot;
    private readonly int ROTTEN_APPLE_DMG = 10;
    private int damage;
    private float size;

    private void Start() {
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        damage = asb.GetDamageBonus(slot, ROTTEN_APPLE_DMG);
        size = asb.GetSizeBonus(slot); transform.localScale = new Vector2(size, size);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Enemy>(out var enemy)) {
            if (enemy.GetHealth() < damage) {
                Instantiate(healthDrop, transform.position, Quaternion.identity);
            }
            FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
            Destroy(gameObject);
        }
    }   
}