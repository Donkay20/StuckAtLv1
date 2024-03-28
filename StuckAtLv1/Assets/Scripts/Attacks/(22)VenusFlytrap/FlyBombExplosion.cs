using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlyBombExplosion : MonoBehaviour
{   
    private readonly int FLYBOMB_BASE_DAMAGE = 75;
    private int damage;
    CircleCollider2D bombCollider;
    Slot slot;

    private void Start() {
        bombCollider = GetComponent<CircleCollider2D>();
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        damage = asb.GetDamageBonus(slot, FLYBOMB_BASE_DAMAGE);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(transform.position, bombCollider.radius, LayerMask.GetMask("Enemy"));
        Collider2D[] passThroughEnemyColliders = Physics2D.OverlapCircleAll(transform.position, bombCollider.radius, LayerMask.GetMask("PassThroughEnemy"));
        
        foreach (Collider2D c in enemyColliders.Concat(passThroughEnemyColliders)) {
            if (col.TryGetComponent<Enemy>(out var enemy)) {
                if (enemy.CompareTag("VenusFlyTrap")) {
                    damage *= 4;
                }
                FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
            }
        }
    }
}