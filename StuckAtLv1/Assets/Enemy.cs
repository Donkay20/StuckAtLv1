using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform targetDestination;
    GameObject targetGameObject;
    [SerializeField] float speed;
    [SerializeField] int hp = 4;

    Rigidbody2D body;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        targetGameObject = targetDestination.gameObject;
    }
    
    private void FixedUpdate() {
        Vector3 direction = (targetDestination.position - transform.position).normalized;
        body.velocity = direction * speed;
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject == targetGameObject) {
            Attack();
        }
    }

    private void Attack() {
        //Debug.Log("Attacking the character!!");
    }

    public void TakeDamage(int damage) {
        hp -= damage;
        if (hp < 1) {
            Destroy(gameObject);
        }
    }
}
