using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vortex : MonoBehaviour
{
    private Camera mainCamera;
    private Vector2 mousePosition;
    private CircleCollider2D vortexCollider;
    private readonly int VORTEX_BASE_DAMAGE = 3;
    private readonly float VORTEX_BASE_DURATION = 5f;
    private readonly float VORTEX_TICK_TIME = 0.3f;
    private Slot slot;
    private int damage;
    private float size;
    private float duration;
    private bool hitCheck;
    private float hitRefreshRate;
    void Start() {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
        vortexCollider = GetComponent<CircleCollider2D>();
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();

        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        size = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(size + 1.5f, size + 1.5f);

        duration = asb.GetDurationBonus(slot, VORTEX_BASE_DURATION);

        damage = asb.GetDamageBonus(slot, VORTEX_BASE_DAMAGE);
    }

    private void ResetHitCheck() {
        hitCheck = false;
        hitRefreshRate = VORTEX_TICK_TIME;
    }

    void Update() {
        duration -= Time.deltaTime;
        if (duration <= 0) {
            Destroy(gameObject);
        }

        if (hitRefreshRate > 0 && !hitCheck) {
            hitRefreshRate -= Time.deltaTime;
        }

        if (hitRefreshRate <= 0 && !hitCheck) {
            hitCheck = true;
        }
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (hitCheck) {
            Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(transform.position, vortexCollider.radius, LayerMask.GetMask("Enemy"));
            Collider2D[] passThroughEnemyColliders = Physics2D.OverlapCircleAll(transform.position, vortexCollider.radius, LayerMask.GetMask("PassThroughEnemy"));
            foreach (Collider2D c in enemyColliders.Concat(passThroughEnemyColliders)) {
                Enemy enemy = c.GetComponent<Enemy>();
                FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
            }
            ResetHitCheck();
        }
    }
}