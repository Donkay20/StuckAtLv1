using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineAttackGroup : MonoBehaviour
{
    private Rigidbody2D rb;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.down);
    }

    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D col) {
       if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) {
        Destroy(gameObject);
       }
    }
}
