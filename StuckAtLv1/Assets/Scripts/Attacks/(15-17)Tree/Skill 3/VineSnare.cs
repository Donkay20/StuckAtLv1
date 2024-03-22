using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VineSnare : MonoBehaviour
{
    private readonly int VINESNARE_BASE_DMG = 10;
    private readonly float VINESNARE_BASE_DURATION = 3f;
    private readonly float VINESNARE_STUN_DURATION = 2f;
    private readonly float VINESNARE_SPEED = 5f;
    private int damage;
    private float duration;
    private float size;
    private Slot slot;
    Rigidbody2D rb;
    private Vector3 mousePosition;
    private Camera mainCamera;
    void Start() {
        RotateTowardsMouse();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        Vector2 direction = mousePosition - transform.position;
        rb.velocity = direction.normalized * VINESNARE_SPEED;

        size = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(size, size);

        duration = asb.GetDurationBonus(slot, VINESNARE_BASE_DURATION);

        damage = asb.GetDamageBonus(slot, VINESNARE_BASE_DMG);
    }

    void Update() {
        duration -= Time.deltaTime;
        if (duration <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.ApplyStun(VINESNARE_STUN_DURATION);
        }
        FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
    }

    private void RotateTowardsMouse() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }
}