using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Howl : MonoBehaviour
{
    private readonly int HOWL_BASE_DAMAGE = 10;
    private readonly float HOWL_BASE_DURATION = 2f;
    private readonly float HOWL_SPEED = 4f;
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
        rb.velocity = direction.normalized * HOWL_SPEED;

        size = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(size, size);

        duration = asb.GetDurationBonus(slot, HOWL_BASE_DURATION);

        damage = asb.GetDamageBonus(slot, HOWL_BASE_DAMAGE);
    }

    void Update() {
        duration -= Time.deltaTime;
        transform.localScale = new Vector2(transform.localScale.x * 1.01f, transform.localScale.y * 1.01f);

        if (duration <= 0){
            Destroy(gameObject);
        }
    }

    private void RotateTowardsMouse() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }
    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
    }
}