using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeerNymph : MonoBehaviour
{
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private GameObject target;
    [Space]
    [Header("Boss Attacks")]
    [SerializeField] private GameObject[] flowerBombPattern;
    [SerializeField] private GameObject thornSpike;
    [SerializeField] private GameObject chargeLine;

    [Space]
    [Header("UI")]
    [SerializeField] private GameObject bossHPBar;
    [SerializeField] private GameObject angerBar;
    [SerializeField] private Image bossHPBarFill, angerBarFill;
    [SerializeField] private TextMeshProUGUI bossName, secondaryBarTextName;
    private readonly int MAX_ANGER = 100;
    private readonly int BASE_SPEED = 2;
    private readonly float CHARGE_SPEED = 2;
    private int deerwomanMaxHP, anger;
    private float attackTimer, angerDrainAttackTimer;
    private bool charging, chargePrep;
    private Vector2 angleToCharge;
    Rigidbody2D rb;

    private Animator anim;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        deerwomanMaxHP = enemyScript.maxHP;
        enemyScript.SetTarget(FindAnyObjectByType<Character>().gameObject);
        attackTimer = 5;
        angerDrainAttackTimer = 1;
        anim = GetComponent<Animator>();
    }

    private void OnEnable() {
        bossHPBar.SetActive(true);
        bossName.text = "The Deer-Nymph.";
        bossHPBarFill.fillAmount = 1;
        //boss hp bar
        angerBar.SetActive(true);
        secondaryBarTextName.text = "Anger";
        angerBarFill.fillAmount = 0;
        //secondary hp bar
    }

    void Update() {
        bossHPBarFill.fillAmount = (float) enemyScript.GetHealth() / deerwomanMaxHP;
        angerBarFill.fillAmount = (float) anger / MAX_ANGER;

        attackTimer -= Time.deltaTime;
        angerDrainAttackTimer -= Time.deltaTime;

        if (angerDrainAttackTimer <= 0) {
            if (anger > 0 && anger < 100) {
                anger--; angerDrainAttackTimer = 1;
            }
        }

        if (attackTimer <= 0) {
            Attack();
        }

        if (chargePrep) {
            angleToCharge = (target.transform.position - transform.position) * CHARGE_SPEED;
        }

    }

    private void Attack() {
        switch (Random.Range(0,2)) {
            case 0:
                StartCoroutine(SpikeBarrage());
                break;
            case 1:
                FlowerBombAttack();
                break;
        }
        attackTimer = 5;
    }

    public void RaiseAnger() {  //when the deer pheromone attack hits the deer nymph, call this method
        if (anger < MAX_ANGER && !charging && !chargePrep) {
            if (anger + 30 > MAX_ANGER) {
                anger = 100;
            } else {
                anger += 30;
            }
        }

        if (anger == MAX_ANGER) {
            StartCoroutine(ChargeAttack());
        }
    }

    private IEnumerator SpikeBarrage() {
        int spikes = 10;
        while (spikes > 0) {
            yield return new WaitForSeconds(0.2f);
            Instantiate(thornSpike);
            spikes--;
        }
    }

    private void FlowerBombAttack() {
        Instantiate(flowerBombPattern[Random.Range(0,3)], transform.position, Quaternion.identity);
    }

    private IEnumerator ChargeAttack() {
        chargePrep = true; 
        anim.SetBool("Charge", true);
        enemyScript.SetSpeed(0); 
        rb.velocity = new Vector2(0, 0);
        chargeLine.SetActive(true); 

        yield return new WaitForSeconds(4); //track player during this time w/ the charge line
        
        chargePrep = false; charging = true; chargeLine.SetActive(false);
        anim.SetBool("Charge",false);
        anim.SetBool("Charging", true);
        rb.velocity = angleToCharge;

        yield return new WaitForSeconds(2); //if hasn't hit anything in 2 seconds, just reset
        rb.velocity = new Vector2(0, 0);
        enemyScript.SetSpeed(BASE_SPEED);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("CrystalRock") && charging) {
            HitSomething(true);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Wall") && charging) {
            HitSomething(false);
        }

        if (other.gameObject == target) {
            HitSomething(false);
        }
    }

    private void HitSomething(bool crystal) {
        anim.SetBool("Charging",false);
        charging = false; StopAllCoroutines();
        if (crystal) {  //stop momentum, deal a bunch of damage to the boss and stun
            rb.velocity = new Vector2(0, 0);
            enemyScript.TakeDamage(deerwomanMaxHP / 5);
            enemyScript.ApplySlow(0.1f, 3);
        } else {        //just stop momentum
            enemyScript.ApplySlow(0.1f, 1);
            rb.velocity = new Vector2(0, 0);
        }
        enemyScript.SetSpeed(BASE_SPEED);
        anger = 0;
    }
}