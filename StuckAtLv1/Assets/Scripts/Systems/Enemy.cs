using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
/*
Class that handles enemy stats and HP values and taking damage, as well as attacking.

* This class will need to be changed so that it doesn't have the follow-enemy-functionality it currently has. 
*/
{
    Transform targetDestination;
    GameObject targetGameObject;
    Character targetCharacter;
    
    [SerializeField] float speed;
    [SerializeField] int hp = 1000;
    [SerializeField] int damage = 1;

    Rigidbody2D body;
    Animator anim;
    SpriteRenderer rend;

    public GameObject particlePrefab;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    public void SetTarget(GameObject target) {
        targetGameObject = target;
        targetDestination = target.transform;
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
        if (targetCharacter == null) {
            targetCharacter = targetGameObject.GetComponent<Character>();
        }
        targetCharacter.TakeDamage(damage);
    }

    public void TakeDamage(int damage) {
        Debug.Log("damage taken: " + damage);
        hp -= damage;
        anim.SetTrigger("Hit");
        if (hp < 1) {
            FindAnyObjectByType<CombatManager>().EnemyKilled();
            Instantiate(particlePrefab, this.transform.position, this.transform.rotation);
            //Debug.Log(this.transform);
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
