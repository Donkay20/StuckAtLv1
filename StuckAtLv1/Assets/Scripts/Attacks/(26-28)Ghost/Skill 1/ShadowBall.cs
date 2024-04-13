using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBall : MonoBehaviour
{
    private readonly int SHADOWBALL_BASE_DMG = 10;
    private readonly float SHADOWBALL_BASE_DURATION = 5f;
    private readonly float SHADOWBALL_SPEED = 5f;
    private readonly float SHADOWBALL_SLOW_DURATION = 3f;
    private Slot slot;
    Rigidbody2D rb;
    private Vector3 mousePosition;
    private Camera mainCamera;
    private int damage;
    private float duration;
    private float size;
    private float hitSize;
    private bool hitEnemy;
    
    void Start() {
        RotateTowardsMouse();
        rb = GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        Vector2 direction = mousePosition - transform.position;
        rb.velocity = direction.normalized * SHADOWBALL_SPEED;

        size = asb.GetSizeBonus(slot); hitSize = size * 2;
        transform.localScale = new Vector2(size, size);

        duration = asb.GetDurationBonus(slot, SHADOWBALL_BASE_DURATION);

        damage = asb.GetDamageBonus(slot, SHADOWBALL_BASE_DMG);
    }

    void Update() {
        duration -= Time.deltaTime;
        if (duration <= 0) {
            Destroy(gameObject);
        }
    }

    private void HitEnemy() {
        hitEnemy = true;
        transform.localScale = new Vector2(hitSize, hitSize);
        rb.velocity /= 2;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Enemy>(out var enemy)) {
            enemy.ApplySlow(0.7f, SHADOWBALL_SLOW_DURATION);
            FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
            if (!hitEnemy) {
                HitEnemy();
            }
        }
    }

    private void RotateTowardsMouse() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }
}