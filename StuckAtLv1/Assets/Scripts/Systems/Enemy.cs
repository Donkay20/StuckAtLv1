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
    
    [SerializeField] float baseSpeed;
    [SerializeField] int hp = 1000;
    [SerializeField] int damage = 1;
    [SerializeField] private float alteredSpeed, alteredSpeedTimer;

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
        //add condition here, depending on type of enemy, especially bosses
        Vector3 direction = (targetDestination.position - transform.position).normalized;
        if (alteredSpeedTimer > 0) {
            body.velocity = direction * alteredSpeed;
        } else {
            body.velocity = direction * baseSpeed;
        }
        Flip(direction.x);
    }

    private void Update() {
        if (alteredSpeedTimer > 0) {
            alteredSpeedTimer -= Time.deltaTime;
        }
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

    public void ApplySlow(float percentage, float duration) {
        alteredSpeed = baseSpeed - (baseSpeed*percentage);
        Debug.Log("Altered speed: " + alteredSpeed);
        alteredSpeedTimer = duration;
        Debug.Log("Altered speed duration: " + alteredSpeedTimer);
        //To apply a slow, it needs to take in the severity of the slow, + the duration for how long the slow lasts.
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
