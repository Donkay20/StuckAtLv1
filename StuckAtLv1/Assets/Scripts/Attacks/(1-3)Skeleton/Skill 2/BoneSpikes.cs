using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneSpikes : MonoBehaviour
{
    private float timer; private float baseTime;
    Rigidbody2D rb;
    Vector2 baseVelocity;
    Slot slot;
    private Vector3 mousePosition;
    private Camera mainCamera;
    private float speed = 10;
    private int damage;
    private readonly int BONESPIKES_BASE_DMG = 15;
    private readonly float BONESPIKES_BASE_TIMER = 0.8f;

    private float size;
    void Start() {
        FaceMouse();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        rb.velocity = direction.normalized * speed;
        baseVelocity = rb.velocity;
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        damage = asb.GetDamageBonus(slot, BONESPIKES_BASE_DMG);

        size = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(size, size);

        timer = asb.GetDurationBonus(slot, BONESPIKES_BASE_TIMER);
        baseTime = timer;
    }

    void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }
        rb.velocity = baseVelocity * (timer/baseTime);
    }
    
    private void FaceMouse() {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
    }
}