using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sandworm : MonoBehaviour {
    [SerializeField] private Enemy enemyScript;
    [Header("UI")]
    [SerializeField] private GameObject bossHPBar;
    [SerializeField] private GameObject enrageBar;
    [SerializeField] private Image bossHPBarFill, enrageBarFill;
    [SerializeField] private TextMeshProUGUI bossName, enrageName;
    [SerializeField] private CinemachineVirtualCamera cam;
    [Header("BossAttacks")]
    [SerializeField] private GameObject attack1;

    //backend
    private readonly float BASE_SPEED = 3;
    private readonly float MAX_SPEED = 6;
    private readonly float MIN_SPEED = 1;
    private readonly float ATTACK_COOLDOWN = 10;
    private float attackTimer;
    private int bossMaxHP;
    private float bonusSpeed;
    
    void Start() {
        attackTimer = ATTACK_COOLDOWN;
    }

    private void OnEnable() {
        bossHPBar.SetActive(true);
        bossName.text = "The Sandworm.";
        bossHPBarFill.fillAmount = 1;

        enrageBar.SetActive(true);
        enrageName.text = "Momentum";
        enrageBarFill.fillAmount = 0;
    }

    void Update() {
        bossHPBarFill.fillAmount = (float) enemyScript.GetHealth() / bossMaxHP;
        enrageBarFill.fillAmount = (float) (BASE_SPEED + bonusSpeed) / MAX_SPEED;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0) {
            Attack();
        }
    }

    private void Attack() {
        //todo
    }

    private IEnumerator Quicksand() {
        yield return new WaitForSeconds(0);
    }
}
