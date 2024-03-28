using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSkull : MonoBehaviour
{
    private readonly int HOMING_SKULL_DMG = 4;
    private readonly float HOMING_SKULL_SPD = 4f;
    private float homingTimer = 5f;
    private float lifetime = 7f;
    private GameObject lichPosition;
    private GameObject targetPosition;
    private Character targetCharacter;
    private Vector2 previousDirection;
    private Vector2 directionToTarget;
    Rigidbody2D rb;

    private Animator anim;
    
    void Start() {
        lichPosition = FindAnyObjectByType<Lich>().gameObject;
        transform.position = lichPosition.transform.position;
        targetCharacter = FindAnyObjectByType<Character>();
        targetPosition = targetCharacter.gameObject;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate() {
        directionToTarget = (targetPosition.transform.position - transform.position).normalized;

        if (homingTimer > 0) {
            // Move towards the target player
            Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition.transform.position, HOMING_SKULL_SPD * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);

            // Store the direction for later use
            previousDirection = directionToTarget;
        } else {
            // Move forward in the previous direction
            Vector2 newPosition = rb.position + previousDirection * HOMING_SKULL_SPD * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }

    private void FacePlayer()
    {
        if(Mathf.Abs(directionToTarget.x) > Mathf.Abs(directionToTarget.y))
        {
            if(directionToTarget.x > 0)
            {
                anim.SetBool("Up", false);
                anim.SetBool("Down", false);
                anim.SetBool("Left", false);
                anim.SetBool("Right", true);
            }
            else
            {
                anim.SetBool("Up", false);
                anim.SetBool("Down", false);
                anim.SetBool("Right", false);
                anim.SetBool("Left", true);
            }
        }
        else if(Mathf.Abs(directionToTarget.x) < Mathf.Abs(directionToTarget.y))
        {
            if(directionToTarget.y > 0)
            {
                anim.SetBool("Down", false);
                anim.SetBool("Left", false);
                anim.SetBool("Right", false);
                anim.SetBool("Up", true);
            }
            else{
                anim.SetBool("Up", false);
                anim.SetBool("Left", false);
                anim.SetBool("Right", false);
                anim.SetBool("Down", true);
            }
        }
    }

    private void Update() {
        homingTimer -= Time.deltaTime;
        lifetime -= Time.deltaTime;

        if (lifetime <= 0) {
            Destroy(gameObject);
        }

        FacePlayer();
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
            targetCharacter.TakeDamage(HOMING_SKULL_DMG);
        } else {
            throw new NullReferenceException("ERROR: Null instance found; Lich's Homing Skull.");
        }
    }
}
