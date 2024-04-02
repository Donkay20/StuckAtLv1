using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phase3Enemy : MonoBehaviour
{
    [Header("Back-end")]
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject bossHPBar, executeBar;
    [SerializeField] private TextMeshProUGUI bossTitle, executeTitle;
    [SerializeField] private Image hpBarFill, executeFill;
    [Header("Attacks")]
    [SerializeField] private BuffManager buffManager;
    [SerializeField] private GameObject crissCross;
    [SerializeField] private GameObject crissCrossSpawnPoint;
    [SerializeField] private GameObject halfMap;
    [SerializeField] private GameObject halfMapSpawnPointTop;
    [SerializeField] private GameObject halfMapSpawnPointBottom;
    [SerializeField] private GameObject jailAttack;
    [SerializeField] private Character scourgeTarget;
    
    private readonly float ATTACK_RESET_TIME = 6f;
    private readonly int EXECUTE_TIME_LIMIT = 300;
    private int maxHP, executeTimer;
    private float attackTimer;
    void Awake() {
        enemyScript.SetTarget(target);
        maxHP = enemyScript.maxHP;
        attackTimer = ATTACK_RESET_TIME;
    }

    private void OnEnable() {
        bossHPBar.SetActive(true);
        bossTitle.text = "Tiffany, Scourge of Vespera.";
        hpBarFill.fillAmount = 1;

        executeBar.SetActive(true);
        executeTitle.text = "Sanguine Sonata";
        executeFill.fillAmount = 1;

        StartCoroutine(IncrementSanguineScourge());
    }

    void Update() {
        hpBarFill.fillAmount = (float) enemyScript.GetHealth() / maxHP;
        executeFill.fillAmount = (float) executeTimer/EXECUTE_TIME_LIMIT;

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0) {
            Attack();
        }
    }

    private void Attack() {
        enemyScript.BossAnemiaCleanse();
        switch (Random.Range(0,3)) {
            case 0:
                Instantiate(crissCross, crissCrossSpawnPoint.transform.position, Quaternion.identity);
                break;
            case 1:
                switch (Random.Range(0,2)) {
                    case 0:
                        Instantiate(halfMap, halfMapSpawnPointTop.transform.position, Quaternion.identity);
                        break;
                    case 1:
                        Instantiate(halfMap, halfMapSpawnPointBottom.transform.position, Quaternion.identity);
                        break;
                }
                break;
            case 2:
                Instantiate(jailAttack, target.transform.position, Quaternion.identity);
                break;
        }
        attackTimer = ATTACK_RESET_TIME;
    }

    private IEnumerator IncrementSanguineScourge() {
        while (executeTimer < EXECUTE_TIME_LIMIT) {
            yield return new WaitForSeconds(1);
            executeTimer++;
        }

        while (executeTimer >= EXECUTE_TIME_LIMIT) {
            yield return new WaitForSeconds(2);
            scourgeTarget.TakeDamage((scourgeTarget.currentHp / 2) + 10);
            buffManager.AddDebuff("slow", 0.5f, 2);
        }
    }
}
