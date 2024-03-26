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
    [SerializeField] private PointEffector2D hyperSuction;
    [SerializeField] private Animator leftHead, centerHead, rightHead;
    [SerializeField] private GameObject vineWavesVertical, vinesWavesHorizontal;
    [Space]
    [Header("UI")]
    [SerializeField] private GameObject bossHPBar;
    [SerializeField] private GameObject enrageBar;
    [SerializeField] private Image bossHPBarFill, enrageBarFill;
    [SerializeField] private TextMeshProUGUI bossName, enrageName;
    private readonly int MAX_RAGE = 360;
    private int bossMaxHP, rage;
    private float attackTimer, attackTimerResetValue;
    private readonly float suctionTimerResetValue = 20;
    private float suctionTimer;
    void Start() {
        attackTimer = 5;
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

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0) {
            Attack();
        }

        suctionTimer -= Time.deltaTime;

        if (suctionTimer <= 0) {
            StartCoroutine(SuctionAttack());
        }

        if (rage == MAX_RAGE) {
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

    private IEnumerator SuctionAttack() {
        yield return new WaitForSeconds(1);
        //todo
        suctionTimer = suctionTimerResetValue;
    }

    private void Attack() {
        attackTimer = attackTimerResetValue;
    }
}
