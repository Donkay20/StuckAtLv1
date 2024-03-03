using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSkull : MonoBehaviour
{
    private Slot slot;
    private float timer = 2f; //constant & mutable, unaffected by duration bonuses
    private readonly float SKULL_SPEED = 10f;
    private readonly int SKULL_BASE_DAMAGE = 1; 
    private Vector3 mousePosition;
    private Camera mainCamera;
    Rigidbody2D rb;
    private int damage;
    private float size;

    void Start() {
        slot = GetComponentInParent<Slot>();
        rb = GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float angleModifier = Random.Range(-30f, 30f);
        direction = Quaternion.Euler(0f, 0f, angleModifier) * direction;
        rb.velocity = direction.normalized * SKULL_SPEED;
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        damage = asb.GetDamageBonus(slot, SKULL_BASE_DAMAGE);

        size = asb.GetSizeBonus(slot); transform.localScale = new Vector2(0.25f * size, 0.25f * size);
    }

    void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
    }
}
