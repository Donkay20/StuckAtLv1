using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinSpirit : MonoBehaviour
{
    private Slot slot;
    private Enemy target;
    private Rigidbody2D rb;
    private readonly int BASE_SPIRIT_DAMAGE = 20;
    private readonly float BASE_SPIRIT_DURATION = 5f;
    private readonly float BASE_SPIRIT_SPEED = 7f;
    private Vector2 force;
    private float size, duration;
    private int damage;

    public void AssignSlot(Slot s) {
        slot = s;

        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        slot = GetComponentInParent<TwinSpirit_Parent>().GetSlot();
        rb = GetComponent<Rigidbody2D>();

        size = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(size, size);

        duration = asb.GetDurationBonus(slot, BASE_SPIRIT_DURATION);

        damage = asb.GetDamageBonus(slot, BASE_SPIRIT_DAMAGE);
    }

    public Enemy FindNearestEnemy(Vector3 position) {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Enemy nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;
        foreach (Enemy enemy in enemies) {
            float distance = Vector3.Distance(position, enemy.transform.position);
            if (distance < nearestDistance) {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }

    void Update() {
        if (target == null) {
            target = FindNearestEnemy(transform.position);
        }

        duration -= Time.deltaTime;
        
        if (duration <= 0) {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() {
        if (target != null) {
            force = (target.transform.position - transform.position).normalized;
            rb.velocity = force * BASE_SPIRIT_SPEED;
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Enemy>(out var enemy)) {
            FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
            Destroy(gameObject);
        }
    }
}