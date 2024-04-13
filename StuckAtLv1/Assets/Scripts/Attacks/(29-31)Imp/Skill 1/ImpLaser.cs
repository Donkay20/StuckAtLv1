using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImpLaser : MonoBehaviour
{
    [SerializeField] private ImpLaserParent ilp;
    private Slot slot;
    private int IMP_BEAM_BASE_DAMAGE = 5;
    BoxCollider2D laserCollider;
    private bool hitCheck;
    private float hitRefreshRate = 0.2f;
    private int damage;
    void Start() {
        slot = ilp.GetSlot();
        laserCollider = GetComponent<BoxCollider2D>();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        damage = asb.GetDamageBonus(slot, IMP_BEAM_BASE_DAMAGE);
    }

    void Update() {
        if (hitRefreshRate > 0 && !hitCheck) {
            hitRefreshRate -= Time.deltaTime;
        }

        if (hitRefreshRate <= 0 && !hitCheck) {
            hitCheck = true;
        }
    }

    private void ResetHitCheck() {
        hitCheck = false;
        hitRefreshRate = 0.2f;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Enemy>(out var enemy)) {
            FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
        }
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (hitCheck) {
            Vector2 boxSize = laserCollider.size;

            Vector2 boxCenter = transform.position;

            Collider2D[] enemyColliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0, LayerMask.GetMask("Enemy"));
            Collider2D[] passThroughEnemyColliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0, LayerMask.GetMask("PassThroughEnemy"));

            foreach (Collider2D c in enemyColliders.Concat(passThroughEnemyColliders)) {
                Enemy enemy = c.GetComponent<Enemy>();
                FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
            }
            ResetHitCheck();
        }
    }


}