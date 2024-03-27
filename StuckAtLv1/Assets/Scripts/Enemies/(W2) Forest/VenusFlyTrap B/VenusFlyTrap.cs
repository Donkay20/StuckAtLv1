using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VenusFlyTrap : MonoBehaviour
{
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private GameObject target;

    [Space]
    [Header("Boss Attacks")]
    [SerializeField] private PointEffector2D suction;
    [SerializeField] private GameObject warningCircle;
    [SerializeField] private PointEffector2D hyperSuction;
    [SerializeField] private FlyTrapHead leftHead, centerHead, rightHead;
    [SerializeField] private GameObject[] vineWavesH, vineWavesV;
    [Space]
    [Header("UI")]
    [SerializeField] private GameObject bossHPBar;
    [SerializeField] private GameObject enrageBar;
    [SerializeField] private Image bossHPBarFill, enrageBarFill;
    [SerializeField] private TextMeshProUGUI bossName, enrageName;
    [SerializeField] private float attackTimer;
    private readonly int MAX_RAGE = 360;
    private int bossMaxHP, rage;
    private float attackTimerResetValue = 5;
    private readonly float suctionTimerResetValue = 20;
    [SerializeField] private float suctionTimer;
    private bool mapwideSuctionPrep, mapwideSuction; private float mapwideSuctionPrepTimer;
    void Start() {
        attackTimer = attackTimerResetValue;
        suctionTimer = suctionTimerResetValue;
        bossMaxHP = enemyScript.maxHP;
        enemyScript.SetTarget(FindAnyObjectByType<Character>().gameObject);
    }

    private void OnEnable() {
        bossHPBar.SetActive(true);
        bossName.text = "The Venus Flytrap.";
        bossHPBarFill.fillAmount = 1;

        enrageBar.SetActive(true);
        enrageName.text = "Rage";
        enrageBarFill.fillAmount = 0;

        StartCoroutine(IncrementRage());
    }

    void Update() {
        bossHPBarFill.fillAmount = (float) enemyScript.GetHealth() / bossMaxHP;
        enrageBarFill.fillAmount = (float) rage / MAX_RAGE;

        if (attackTimer > 0) {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0) {
                Attack();
            }
        }

        if (suctionTimer > 0) {
            suctionTimer -= Time.deltaTime;
            if (suctionTimer <= 0 && !mapwideSuctionPrep) {
                StartCoroutine(SuctionAttack());
            }
        }

        if (mapwideSuctionPrep) {
            mapwideSuctionPrepTimer += Time.deltaTime;
            float t = Mathf.Clamp01(mapwideSuctionPrepTimer/3f);
            float tr = Mathf.Lerp(0, 87, t);
            warningCircle.transform.localScale = new Vector2(tr, tr);
        } else {
            mapwideSuctionPrepTimer = 0;
        }

        if (rage >= MAX_RAGE) {
            attackTimerResetValue = 1;
        } else {
            attackTimerResetValue = 5;
        }
    }

    private IEnumerator IncrementRage() {
        yield return new WaitForSeconds(3);
        while (rage < MAX_RAGE) {
            yield return new WaitForSeconds(1);
            rage++;
        }
    }

    private void Attack() {
        if (Random.Range(0, 2) == 0) {
            StartCoroutine(VineAttackV());
        } else {
            StartCoroutine(VineAttackH());
        }
        attackTimer = attackTimerResetValue;
    }

    private IEnumerator SuctionAttack() {
        mapwideSuctionPrep = true; warningCircle.SetActive(true);
        yield return new WaitForSeconds(3);                 //provide a warning
        mapwideSuctionPrep = false; mapwideSuctionPrepTimer = 0; warningCircle.SetActive(false);

        mapwideSuction = true; suction.enabled = true;
        yield return new WaitForSeconds(5);                 //suction duration
        mapwideSuction = false; suction.enabled = false;

        suctionTimer = suctionTimerResetValue;
    }

    private IEnumerator VineAttackH() {
        Instantiate(vineWavesH[0]);
        yield return new WaitForSeconds(1.5f);
        Instantiate(vineWavesH[1]);
    }

    private IEnumerator VineAttackV() {
        Instantiate(vineWavesV[0]);
        Instantiate(vineWavesV[1]);
        Instantiate(vineWavesV[2]);
        Instantiate(vineWavesV[3]);

        yield return new WaitForSeconds(2f);
        
        Instantiate(vineWavesV[4]);
        Instantiate(vineWavesV[5]);
        Instantiate(vineWavesV[6]);
        Instantiate(vineWavesV[7]);
        Instantiate(vineWavesV[8]);
    }
}
