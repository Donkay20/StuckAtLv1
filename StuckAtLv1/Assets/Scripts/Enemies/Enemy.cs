using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] GameManager gameManager;
    [SerializeField] float baseSpeed;
    [SerializeField] int hp;
    [SerializeField] int damage;
    [SerializeField] private float alteredSpeed, alteredSpeedTimer;
    private bool anemiaApplied; private float anemiaTimer, anemiaTick; private int anemiaDamage;
    private bool stunApplied; private float stunDuration;
    Rigidbody2D body;
    Animator anim;
    SpriteRenderer rend;

    public GameObject particlePrefab;

    private void Awake() {
        if (baseSpeed > 0) {
            body = GetComponent<Rigidbody2D>();
        }
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        anemiaTick = 1;
    }

    public void SetTarget(GameObject target) {
        targetGameObject = target;
        targetDestination = target.transform;
    }

    private void Start() {
        gameManager = FindAnyObjectByType<GameManager>();
        hp += gameManager.ScaleDifficulty();
        baseSpeed += gameManager.ScaleDifficulty()/10;
    }
    
    private void FixedUpdate() {
        //add condition here, depending on type of enemy, especially bosses
        Vector3 direction = (targetDestination.position - transform.position).normalized;
        if (alteredSpeedTimer > 0) {
            if (baseSpeed > 0) {
                body.velocity = direction * alteredSpeed;
            }
        } else {
            if (baseSpeed > 0) {
                body.velocity = direction * baseSpeed;
            }    
        }
        Flip(direction.x);
    }

    private void Update() {
        if (alteredSpeedTimer > 0) {
            alteredSpeedTimer -= Time.deltaTime;
        }

        if (anemiaApplied) {
            anemiaTick -= Time.deltaTime;
            anemiaTimer -= Time.deltaTime;
            if (anemiaTick <= 0) {
                TakeDamage(anemiaDamage);
                anemiaTick = 1;
            }
        }

        if (anemiaTimer <= 0) {
            anemiaApplied = false;
            anemiaDamage = 0;
            anemiaTick = 1;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject == targetGameObject) {
            Attack();
        }
    }

    private void Attack() {
        if (targetCharacter == null && !stunApplied) { //stunned enemies can't deal damage
            targetCharacter = targetGameObject.GetComponent<Character>();
        }
        targetCharacter.TakeDamage(damage);
    }

    public void TakeDamage(int damage) {
        Debug.Log("damage taken: " + damage);
        switch (this.gameObject.tag) {
            case "Knight": //special rules for the knight enemy
                Knight knight = FindAnyObjectByType<Knight>();
                if (!knight.IsVulnerable()) {
                    hp -= 0;
                } else {
                    hp -= damage;
                }
                break;
            case "KnightSword": //sword resists absorb bullet
                hp -= damage - 1;
                break;
            default:
                hp -= damage;
                break;
        }
        anim.SetTrigger("Hit");
        if (hp < 1) {
            CombatManager c = FindAnyObjectByType<CombatManager>();
            if (c.GetObjective() == "miniboss" && this.gameObject.CompareTag("Knight")) {
                c.EnemyKilled();
            } else if (c.GetObjective() == "combat") {
                c.EnemyKilled();
            }
            Instantiate(particlePrefab, this.transform.position, this.transform.rotation);
            //Debug.Log(this.transform);
            Character character = FindAnyObjectByType<Character>();
            character.money += 5;
            ResolveEnemy();
        }
    }

    public void ApplySlow(float percentage, float duration) {
        alteredSpeed = baseSpeed - (baseSpeed * percentage);
        alteredSpeedTimer = duration;
    }

    public void ApplyAnemia(int damage, float duration) {
        anemiaDamage += damage;
        anemiaTimer += duration;
        anemiaApplied = true;
    }

    public void ApplyStun(float duration) {
        alteredSpeed = 0;
        alteredSpeedTimer += duration;
        stunApplied = true;
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

    private void ResolveEnemy() {
        switch (this.gameObject.tag) {
            case "KnightSword": //special rules for the knight's sword; set it to disable instead of destroy so it can be spawned again. then, alert the knight that its sword has died.
                Knight knight = FindAnyObjectByType<Knight>();
                knight.SwordDied();
                gameObject.SetActive(false);
                break;
            case "Knight":
                Destroy(gameObject);
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    public int GetHealth() {
        return hp;
    }

    public void SetHealth(int health) {
        hp = health;
    }
}
