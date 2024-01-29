using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrow : MonoBehaviour
{
    float timer = 0.2f;
    Rigidbody2D rb;
    public float speed;
    public Vector2 direction = new Vector2(0, 0);
    public int damage = 5;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        Launch(direction.x, direction.y);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }
    }

    void Launch(float xDir, float yDir)
    {
        rb.velocity = new Vector2(xDir, yDir).normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(damage);
        }
    }
}
