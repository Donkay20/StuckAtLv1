using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSkull : MonoBehaviour
{
    private Slot slot;
    private float timer = 2f;
    private readonly float SKULL_SPEED = 10f;
    private Vector3 mousePosition;
    private Camera mainCamera;
    private int damage;
    Rigidbody2D rb;

    void Start() {
        slot = GetComponentInParent<Slot>();
        rb = GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mousePosition - transform.position;
        float angleModifier = Random.Range(-30f, 30f);
        direction = Quaternion.Euler(0f, 0f, angleModifier) * direction;
        rb.velocity = direction.normalized * SKULL_SPEED;

        damage = (int) (1 * (1+(slot.GetCommonUpgrade(0)*0.2f + slot.GetRareUpgrade(0)*0.4f + slot.GetLegendaryUpgrade(0)*0.6f)));

        float scalingFactor = 1 + slot.GetCommonUpgrade(1)*0.2f + slot.GetRareUpgrade(1)*0.3f + slot.GetLegendaryUpgrade(1)*0.4f;
        transform.localScale = new Vector2(0.25f * scalingFactor, 0.25f * scalingFactor);
    }

    void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(damage);   //if a modifier increases damage, it would call back to the parent slot and acquire the modifier for calculation
        }
    }
}
