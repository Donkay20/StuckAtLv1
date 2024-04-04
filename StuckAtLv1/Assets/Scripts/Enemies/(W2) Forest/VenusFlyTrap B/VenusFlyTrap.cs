using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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
    [SerializeField] private GameObject vineGround;
    [SerializeField] private GameObject hyperWarningCircle;
    [SerializeField] private FlyTrapHead leftHead, centerHead, rightHead;
    [SerializeField] private GameObject[] vineWavesH, vineWavesV;
    [Space]
    [Header("UI")]
    [SerializeField] private GameObject bossHPBar;
    [SerializeField] private GameObject enrageBar;
    [SerializeField] private Image bossHPBarFill, enrageBarFill;
    [SerializeField] private TextMeshProUGUI bossName, enrageName;
    [SerializeField] private float attackTimerResetValue;
    [SerializeField] private float suctionTimerResetValue;
    [SerializeField] private CinemachineVirtualCamera cam;
    private float resetTimer;
    private float attackTimer;
    private float suctionTimer;
    private readonly int MAX_RAGE = 360;
    private int bossMaxHP, rage;
    private bool mapwideSuctionPrep, mapwideSuction, hyperSuction; private float mapwideSuctionPrepTimer, hyperSuctionPrepTimer;
    void Start() {
        resetTimer = 1;
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

        if (attackTimer > 0 && !mapwideSuctionPrep && !mapwideSuction) {
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
            float tc = Mathf.Lerp(8, 12, t);
            warningCircle.transform.localScale = new Vector2(tr, tr);
            cam.m_Lens.OrthographicSize = tc;
        } else {
            mapwideSuctionPrepTimer = 0;
        }

        if (mapwideSuction) {
            hyperSuctionPrepTimer += Time.deltaTime;
            float t = Mathf.Clamp01(hyperSuctionPrepTimer/5f);
            float tr = Mathf.Lerp(0, 20, t);
            hyperWarningCircle.transform.localScale = new Vector2(tr, tr);
        } else {
            hyperSuctionPrepTimer = 0;
        }

        if (rage >= MAX_RAGE) {
            attackTimerResetValue = 2;
        } else {
            attackTimerResetValue = 8;
        }

        if (resetTimer <= 0.5f) {
            resetTimer += Time.deltaTime;
            float t = Mathf.Clamp01(resetTimer / 0.5f);
            float tc = Mathf.Lerp(12, 8, t); // Interpolate from 12 to 8
            cam.m_Lens.OrthographicSize = tc;
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

        mapwideSuction = true; suction.enabled = true; hyperWarningCircle.SetActive(true);
        yield return new WaitForSeconds(5);                 //suction duration
        mapwideSuction = false; suction.enabled = false; 

        hyperSuctionPrepTimer = 0; hyperWarningCircle.SetActive(false);  vineGround.SetActive(true); hyperSuction = true;
        leftHead.BiteAttack(); centerHead.BiteAttack(); rightHead.BiteAttack();
        yield return new WaitForSeconds(1);                 //hyper suction duration
        vineGround.SetActive(false); hyperSuction = false;
        ResetCameraAfterSuction();

        suctionTimer = suctionTimerResetValue;
    }

    private void ResetCameraAfterSuction() {
        resetTimer = 0;
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

    public bool IsSuction() {
        if (mapwideSuction || hyperSuction) {
            return true;
        } else {
            return false;
        }
    }
}
