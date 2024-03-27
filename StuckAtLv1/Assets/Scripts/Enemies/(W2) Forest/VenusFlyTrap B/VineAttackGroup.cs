using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineAttackGroup : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 forceDirection = Vector2.down;
    private readonly float MAGNITUDE = 6f;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = forceDirection * MAGNITUDE;
    }

    private void OnTriggerEnter2D(Collider2D col) {
       if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) {
        Destroy(gameObject);
       }
    }
}
