using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phase2Enemy : MonoBehaviour
{
    [Header("Back-end")]
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private GameObject target;
    [SerializeField] private FinalBossManager finalBossManager;
    [SerializeField] private GameObject bossHPBar, summonBar;
    [SerializeField] private TextMeshProUGUI bossTitle, extraTitle;
    [SerializeField] private Image hpBarFill, summonBarFill;
    [Header("Limiter")]
    [SerializeField] private GameObject forceField;
    [Header("Attacks")]
    [SerializeField] private GameObject[] summonPositions;
    [SerializeField] private GameObject laserSwivel;
    [SerializeField] private GameObject crissCross;
    [SerializeField] private GameObject jailAttack;
    private int maxHP;
    private int summonTimer;
    private float attackTimer;
    private readonly int SUMMON_MAX = 20;
    private readonly int ATTACK_RESET_TIME = 10;
    private bool active;
    void Awake() {
        enemyScript.SetTarget(target);
        maxHP = enemyScript.maxHP;
        attackTimer = ATTACK_RESET_TIME;
    }

    void Update() {
        if (active) {
            hpBarFill.fillAmount = (float) enemyScript.GetHealth() / maxHP;
            summonBarFill.fillAmount = (float) summonTimer/SUMMON_MAX;

            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0) {
                Attack();
            }
        }

        if (enemyScript.GetHealth() <= maxHP/2) {
            if (laserSwivel != null && !laserSwivel.activeInHierarchy) { 
                laserSwivel.SetActive(true);
            }
        }
    }

    private void Attack() {
        switch (Random.Range(0,2)) {
            case 0:
                Instantiate(crissCross, transform.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(jailAttack, target.transform.position, Quaternion.identity);
                break;
        }
        attackTimer = ATTACK_RESET_TIME;
    }

    private IEnumerator IncrementSummon() {
        while (active) {
            yield return new WaitForSeconds(1);
            summonTimer++;
            if (summonTimer == SUMMON_MAX) {
                summonTimer = 0;
                for (int i = 0; i < 9; i++) {
                    GameObject newEnemy = EnemyPool.Instance.GetEnemy(Random.Range(0,17)); //change this later for sewer enemies
                    newEnemy.transform.position = summonPositions[i].transform.position;
                    newEnemy.GetComponent<Enemy>().SetTarget(target);
                }
            }
        }
    }

    public bool GetActivity() {
        return active;
    }

    public void Activate() {
        active = true;

        Destroy(forceField);

        bossHPBar.SetActive(true);
        bossTitle.text = "Tiffany, Queen of Vespera";
        hpBarFill.fillAmount = 1;

        summonBar.SetActive(true);
        extraTitle.text = "Summon Royal Guard";
        summonBarFill.fillAmount = 1;
        
        active = true;
        StartCoroutine(IncrementSummon());
    }
}
