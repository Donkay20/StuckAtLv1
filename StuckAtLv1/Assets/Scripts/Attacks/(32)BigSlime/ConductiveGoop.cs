using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductiveGoop : MonoBehaviour
{
    private readonly int GOOP_BASE_DMG = 10;
    private readonly float GOOP_BASE_DURATION = 5f;
    private readonly float GOOP_SPEED = 5f;
    private Slot slot;
    Rigidbody2D rb;
    private Vector3 mousePosition;
    private Camera mainCamera;
    private int damage;
    private float duration;
    private float size;
    void Start() {
        RotateTowardsMouse();
        rb = GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        Vector2 direction = mousePosition - transform.position;
        rb.velocity = direction.normalized * GOOP_SPEED;

        size = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(size, size);

        duration = asb.GetDurationBonus(slot, GOOP_BASE_DURATION);

        damage = asb.GetDamageBonus(slot, GOOP_BASE_DMG);
    }

    void Update() {
        duration -= Time.deltaTime;

        if (duration < 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("Conductor")) {
            col.gameObject.GetComponent<Conductor>().Activate();
            Destroy(gameObject);
        }

        if (col.TryGetComponent<Enemy>(out var enemy)) {
            FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
        }
    }

    private void RotateTowardsMouse() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }
}
