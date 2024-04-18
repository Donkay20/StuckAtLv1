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
    [SerializeField] GameObject targetGameObject;
    Character targetCharacter;
    [SerializeField] private GameObject moneyDropPrefab;
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] GameManager gameManager;
    [SerializeField] float baseSpeed;
    [SerializeField] private int hp;
    [SerializeField] private int baseHP;
    [SerializeField] public int maxHP;
    [SerializeField] int damage;
    [SerializeField] private float alteredSpeed, alteredSpeedTimer;
    [Space]
    [SerializeField] private bool tutorial, fromSpecialEnemy;
    private bool anemiaApplied; 
    private float anemiaTimer;
    private float anemiaActiveTick = 1;
    private float anemiaTick = 1; 
    private int anemiaDamage;
    private bool stunApplied;
    private bool anemiaCheck, critCheck;
    Rigidbody2D body;
    Animator anim;
    SpriteRenderer rend;
    public delegate void DamageTakenEvent();
    public event DamageTakenEvent OnDamageTaken;
    private BuffManager buffManager;
    public GameObject particlePrefab;
    private Vector3 force;
    private int moneyOnKill;
    private bool finalBoss, abyssMidboss;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    public void SetTarget(GameObject target) {
        targetGameObject = target;
        targetDestination = target.transform;
    }

    private void Start() {
        buffManager = FindAnyObjectByType<BuffManager>();
        gameManager = FindAnyObjectByType<GameManager>();
        hp = baseHP + gameManager.ScaleDifficulty();
        maxHP = baseHP + gameManager.ScaleDifficulty();
        moneyOnKill = 5;
    }

    private void OnEnable() {
        if (FindObjectOfType<MapManager>(includeInactive: true).GetWorld() == 5 && FindAnyObjectByType<FinalBossManager>().GetPhase() == 1) {
            finalBoss = true;
        } else {
            finalBoss = false;
        }

        if (FindObjectOfType<MapManager>(includeInactive: true).GetWorld() == 4 && FindObjectOfType<MapManager>(includeInactive: true).GetLevel() == 5) {
            abyssMidboss = true;
        } else {
            abyssMidboss = false;
        }

        buffManager = FindAnyObjectByType<BuffManager>();
        gameManager = FindAnyObjectByType<GameManager>();

        if (!gameObject.CompareTag("FinalBossPhase1") || !gameObject.CompareTag("FinalBossPhase2") || !gameObject.CompareTag("FinalBossPhase3")) {
            hp = baseHP + gameManager.ScaleDifficulty();
            maxHP = baseHP + gameManager.ScaleDifficulty();
        }
    }
    
    private void FixedUpdate() {
        force = (targetDestination.position - transform.position).normalized;
        if (alteredSpeedTimer > 0) {
            if (baseSpeed > 0) {
                body.velocity = force * alteredSpeed;
            }
        } else {
            if (baseSpeed > 0) {
                body.velocity = force * baseSpeed;
            }    
        }

        if (this.CompareTag("LichEffigy") == false) {
            Flip(force.x);
        }
    }

    private void Update() {
        if (alteredSpeedTimer > 0) {
            alteredSpeedTimer -= Time.deltaTime;
        }

        if (anemiaApplied) {
            anemiaActiveTick -= Time.deltaTime;
            anemiaTimer -= Time.deltaTime;
            if (anemiaActiveTick <= 0) {
                anemiaCheck = true;
                TakeDamage(anemiaDamage);
                anemiaActiveTick = anemiaTick;
            }
        }

        if (anemiaTimer <= 0) {
            anemiaApplied = false;
            anemiaDamage = 0;
            anemiaActiveTick = 1;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject == targetGameObject) {
            Attack();
        }
    }

    private void Attack() {
        if (!stunApplied) { //stunned enemies can't deal damage
            targetCharacter = targetGameObject.GetComponent<Character>();
        }

        if (targetCharacter != null) {
            targetCharacter.TakeDamage(damage);
            switch (this.gameObject.tag) {
                case "LichEffigy":
                    buffManager.AddDebuff("slow", 0.9f, 3f);
                    break;
            }
        }
    }

    public void TakeDamage(int damage) {
        int actualDamage; 
        string additionalText = "";

        //Debug.Log("damage taken: " + damage);
        switch (gameObject.tag) {
            case "Knight": //special rules for the knight enemy
                Knight knight = FindAnyObjectByType<Knight>();
                if (!knight.IsVulnerable()) {
                    hp -= 0; actualDamage = 0; additionalText = "RESIST";
                } else {
                    hp -= damage; actualDamage = damage;
                }
                break;
            case "KnightSword": //sword resists absorb bullet
                hp -= damage - 1; actualDamage = damage - 1; additionalText = "RESIST";
                break;
            case "Lich":
                Lich lich  = FindAnyObjectByType<Lich>();
                if (!lich.IsVulnerable()) {
                    hp -= 0; actualDamage = 0; additionalText = "RESIST";
                } else {
                    hp -= damage; actualDamage = damage;
                }
                break;
            case "FinalBossPhase2":
                Phase2Enemy p2  = FindAnyObjectByType<Phase2Enemy>();
                if (!p2.GetActivity()) {
                    hp -= 0; actualDamage = 0; additionalText = "RESIST";
                } else {
                    hp -= damage; actualDamage = damage;
                }
                break;
            default:
                hp -= damage; actualDamage = damage;
                break;
        }

        anim.SetTrigger("Hit");
        OnDamageTaken?.Invoke();

        if (damageTextPrefab) {
            var dmg = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
            dmg.GetComponentInChildren<DamageNumber>().Setup(actualDamage, additionalText, critCheck, anemiaCheck);
            if (anemiaCheck) {anemiaCheck = false;}
            if (critCheck) {critCheck = false;}
        }

        if (hp < 1) {
            if (!tutorial) {
                CombatManager c = FindAnyObjectByType<CombatManager>();
                if (c.GetObjective() == "miniboss" && (gameObject.CompareTag("Knight") || gameObject.CompareTag("DeerNymph")) || gameObject.CompareTag("BigSlime") || gameObject.CompareTag("Swarm")) {
                    c.EnemyKilled();
                } else if (c.GetObjective() == "boss" && (gameObject.CompareTag("Lich") || gameObject.CompareTag("VenusFlyTrap") || gameObject.CompareTag("Sandworm") || gameObject.CompareTag("FinalBossPhase3")) ) {
                    c.EnemyKilled();
                } else if (c.GetObjective() == "combat") {
                    c.EnemyKilled();
                }
            }
            Instantiate(particlePrefab, this.transform.position, this.transform.rotation);
            Character character = FindAnyObjectByType<Character>();
            character.GainMoney(moneyOnKill);
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

    private void Flip(float x) {
        if(x > 0) {
            rend.flipX = false;
        } else if(x < 0) {
            rend.flipX = true;
        }
    }

    private void ResolveEnemy() {
        switch (this.gameObject.tag) {
            case "KnightSword": //special rules for the knight's sword; set it to disable instead of destroy so it can be spawned again. then, alert the knight that its sword has died.
                BossAnemiaCleanse();
                anemiaApplied = false;  anemiaDamage = 0; anemiaTimer = 0; anemiaActiveTick = anemiaTick; //clear poison (but keep acceleration debuff)
                Knight knight = FindAnyObjectByType<Knight>();
                knight.SwordDied();
                gameObject.SetActive(false);
                break;
            case "LichEffigy":
                BossAnemiaCleanse();
                anemiaApplied = false;  anemiaDamage = 0; anemiaTimer = 0; anemiaActiveTick = anemiaTick; //clear poison (but keep acceleration debuff)
                Lich lich = FindAnyObjectByType<Lich>();
                lich.EffigyDied();
                gameObject.SetActive(false);
                break;
            case "Knight":  
            case "Lich":
            case "DeerNymph":
            case "Swarm":
                Destroy(gameObject);
                break;
            case "VenusFlyTrap":
                Destroy(gameObject.transform.parent.gameObject);
                break;
            case "FinalBossPhase1":
                FindAnyObjectByType<FinalBossManager>().BossDied(1);
                Destroy(gameObject);
                break;
            case "FinalBossPhase2":
                FindAnyObjectByType<FinalBossManager>().BossDied(2);
                Destroy(gameObject);
                break;
            case "FinalBossPhase3":
                Destroy(gameObject);
                break;
            default:
                if (finalBoss && FindAnyObjectByType<FinalBossManager>().GetPhase() == 1) {
                    FinalBossManager finalBossManager = FindAnyObjectByType<FinalBossManager>();
                    if (finalBossManager != null) {
                        finalBossManager.Phase1EnemyDied();
                    }
                }
                if (abyssMidboss) {
                    MonsterSwarm swarm = FindAnyObjectByType<MonsterSwarm>();
                    if (swarm != null) {
                        swarm.EnemyDied();
                    }
                }
                if (fromSpecialEnemy) {
                    Destroy(gameObject);
                }
                EnemyPool.Instance.ReturnEnemy(gameObject);
                break;
        }
    }

    public int GetHealth() {
        return hp;
    }

    public void CriticalHit() {
        critCheck = true;
    }

    public void SetHealth(int health) {
        hp = health;
    }

    public bool IsAnemic() {
        return anemiaApplied;
    }

    public int AnemiaSeverity() {
        return anemiaDamage;
    }

    public void AnemicShock() {
        if (anemiaApplied) {
            anemiaCheck = true;
            TakeDamage((int) (anemiaDamage * anemiaTimer));
        }
    }

    public void AnemiaAcceleration() {
        anemiaTick *= 0.7f;
    }

    public void DropMoney(int additionalChance) {
        int chance = 5 + additionalChance;
        if (Random.Range(0, 101) < chance) {
            if (moneyDropPrefab) {
                Instantiate(moneyDropPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    public void Knockback(int force) {
        Vector2 kbForce = (targetDestination.transform.position - body.transform.position).normalized;
        Vector2 finalForce = -kbForce * force;
        body.AddForce(finalForce);
    }

    public void RaiseReward(int bonusMoney) {
        moneyOnKill += bonusMoney;
    }

    public void SelfDestruct() {
        ResolveEnemy();
    }

    public void BossAnemiaCleanse() { //clears the boss of active anemia (but keeps the acceleration)
        anemiaApplied = false;  
        anemiaDamage = 0; 
        anemiaTimer = 0; 
        anemiaActiveTick = 1;
    }

    public void Cleanse() {
        transform.localScale = new Vector2(1, 1);
        anemiaApplied = false;  
        anemiaDamage = 0; 
        anemiaTimer = 0; 
        anemiaActiveTick = 1;
        anemiaTick = 1;
        alteredSpeedTimer = 0;
        stunApplied = false;
        alteredSpeed = baseSpeed;
        critCheck = false;
        rend.color = new Color(1,1,1);
    }

    public void SetSpeed(float speedToSet) {
        baseSpeed = speedToSet;
    }

    public bool IsStunned() {
        return stunApplied;
    }

    public float GetBaseSpeed()
    {
        return baseSpeed;
    }

    public float GetAlteredSpeed()
    {
        return alteredSpeed;
    }

    public float GetAlteredSpeedTimer()
    {
        return alteredSpeedTimer;
    }
}