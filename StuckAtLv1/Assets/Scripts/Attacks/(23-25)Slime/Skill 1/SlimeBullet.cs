using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBullet : MonoBehaviour
{
    private Enemy hitEnemy;
    private CircleCollider2D bulletCollider;
    private Slot slot;
    private Vector2 initialOffset;
    private readonly int SLIME_BULLET_BASE_DMG = 3;
    private readonly float SLIME_BULLET_BASE_DURATION = 1.5f;
    private int damage;
    private float duration;
    private float size;
    private bool enemyHit;
    void Start() {
        bulletCollider = GetComponent<CircleCollider2D>();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        damage = asb.GetDamageBonus(slot, SLIME_BULLET_BASE_DMG);
        size = asb.GetSizeBonus(slot); transform.localScale = new Vector2(size, size);
        duration = asb.GetDurationBonus(slot, SLIME_BULLET_BASE_DURATION);
    }

    void Update() {
        duration -= Time.deltaTime;
        if (duration <= 0) {
            if (hitEnemy != null) {                             //bullet deals damage when it expires
                FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, hitEnemy, damage);
            } 
            Destroy(gameObject);
            return; 
        }

        if (enemyHit && !hitEnemy.isActiveAndEnabled) {         //if the enemy dies before the bullet explodes
            Destroy(gameObject);
        }
        
        if (hitEnemy != null) {
            if (hitEnemy.isActiveAndEnabled && enemyHit) {      //while the bullet is stuck, keep it stuck on the enemy
                transform.position = transform.position = (Vector2) hitEnemy.transform.position + initialOffset;
            }
        }
    }

    public void AssignSlot(Slot s) {
        slot = s;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Enemy>(out var enemy) && !enemyHit) {
            bulletCollider.enabled = false;
            enemyHit = true; hitEnemy = enemy;
            enemy.ApplySlow(0.7f, duration);

            transform.SetParent(enemy.transform, true); //stick on enemy
            initialOffset = (Vector2)transform.position - (Vector2)hitEnemy.transform.position;
        }
    }  
}