using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WolfSummon : MonoBehaviour
{
    private readonly int WOLF_SUMMON_BASE_DAMAGE = 2;
    private readonly float WOLF_BASE_SPEED = 6f;
    private int damage;
    private float size;
    private Enemy target;
    private Slot slot;
    private Vector2 force;
    Rigidbody2D rb;
    BoxCollider2D wolfCollider;
    private bool hitCheck;
    private float hitRefreshRate = 0.3f;
    void Start() {
        slot = GetComponentInParent<WolfSummonParent>().GetSlot();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        wolfCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        size = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(size, size);

        damage = asb.GetDamageBonus(slot, WOLF_SUMMON_BASE_DAMAGE);
    }

    void Update() {
        if (target == null) {
            target = FindNearestEnemy(transform.position);
        }

        if (hitRefreshRate > 0 && !hitCheck) {
            hitRefreshRate -= Time.deltaTime;
        }

        if (hitRefreshRate <= 0 && !hitCheck) {
            hitCheck = true;
        }
    }

    private void FixedUpdate() {
        if (target != null) {
            force = (target.transform.position - transform.position).normalized;
            rb.velocity = force * WOLF_BASE_SPEED;
        }
    }

    public Enemy FindNearestEnemy(Vector3 position) {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Enemy nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
    }

    private void ResetHitCheck() {
        hitCheck = false;
        hitRefreshRate = 0.3f;
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (hitCheck) {
            Collider2D[] enemyColliders = Physics2D.OverlapBoxAll(transform.position, wolfCollider.size, 0, LayerMask.GetMask("Enemy"));
            Collider2D[] passThroughEnemyColliders = Physics2D.OverlapBoxAll(transform.position, wolfCollider.size, 0, LayerMask.GetMask("PassThroughEnemy"));
            
            foreach (Collider2D c in enemyColliders.Concat(passThroughEnemyColliders)) {
                Enemy enemy = c.GetComponent<Enemy>();
                FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
            }
            ResetHitCheck();
        }
    }
}
