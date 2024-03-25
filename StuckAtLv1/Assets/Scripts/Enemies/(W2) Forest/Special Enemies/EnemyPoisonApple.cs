using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoisonApple : MonoBehaviour
{
    private readonly int POISON_APPLE_DMG = 2;
    private readonly float POISON_APPLE_SPD = .5f;
    private float lifetime = 10f;
    private Character targetCharacter;
    private GameObject targetGameObject;
    private Rigidbody2D rb;
    private Vector2 direction;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        targetCharacter = FindAnyObjectByType<Character>();
        targetGameObject = targetCharacter.gameObject;
        //transform.right = targetGameObject.transform.position - transform.position;
        direction = (targetGameObject.transform.position - transform.position) * POISON_APPLE_SPD;
    }

    void Update() {
        lifetime -= Time.deltaTime;
        rb.velocity = direction;
        //rb.AddForce(direction, ForceMode2D.Force);
        if (lifetime <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Attack();
            Destroy(gameObject);
        }
    }

    private void Attack() {
        Debug.Log("attacked player");
        if (targetCharacter != null) {
            targetCharacter.TakeDamage(POISON_APPLE_DMG);
        } else {
            throw new NullReferenceException("ERROR: Null instance found; Tree's Poison Apple.");
        }
    }
}
