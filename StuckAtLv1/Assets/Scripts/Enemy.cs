using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform targetDestination;
    GameObject targetGameObject;
    
    [SerializeField] float speed;
    [SerializeField] int hp = 4;

    Rigidbody2D body;
    Animator anim;
    SpriteRenderer rend;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        targetGameObject = targetDestination.gameObject;
    }
    
    private void FixedUpdate() {
        Vector3 direction = (targetDestination.position - transform.position).normalized;
        body.velocity = direction * speed;

        Flip(direction.x);
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
        anim.SetTrigger("Hit");
        if (hp < 1) {
            Destroy(gameObject);
        }
    }

    private void Flip(float x)
    {
        if(x > 0)
        {
            rend.flipX = false;
        }
        else if(x < 0)
        {
            rend.flipX = true;
        }
    }
}
