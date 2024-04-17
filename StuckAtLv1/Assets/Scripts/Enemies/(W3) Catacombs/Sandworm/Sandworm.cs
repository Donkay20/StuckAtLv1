using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sandworm : MonoBehaviour {
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private GameObject target;
    [Space]
    [Header("UI")]
    [SerializeField] private GameObject bossHPBar;
    [SerializeField] private GameObject momentumBar;
    [SerializeField] private Image bossHPBarFill, momentumBarFill;
    [SerializeField] private TextMeshProUGUI bossName, momentumName;
    [SerializeField] private CinemachineVirtualCamera cam;
    [Header("Boss Attacks")]
    [SerializeField] private GameObject mapwideAttack;
    //backend
    private readonly float MAX_SPEED = 6;
    private readonly float MIN_SPEED = 1;
    private readonly float ATTACK_COOLDOWN = 10;
    private float attackTimer;
    private int bossMaxHP;
    private float speed;
    
    void Start() {
        attackTimer = ATTACK_COOLDOWN;
        bossMaxHP = enemyScript.maxHP;
    }

    private void OnEnable() {
        bossHPBar.SetActive(true);
        bossName.text = "The Sandworm.";
        bossHPBarFill.fillAmount = 1;

        momentumBar.SetActive(true);
        momentumName.text = "Momentum";
        momentumBarFill.fillAmount = 0;
    }

    void Update() {
        bossHPBarFill.fillAmount = (float) enemyScript.GetHealth() / bossMaxHP;
        momentumBarFill.fillAmount = speed / MAX_SPEED;

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0) {
            Attack();
        }
    }

    private void Attack() {
        switch (Random.Range(0,2)) {
            case 0:
                StartCoroutine(MapwideAttack());
                break;
            case 1:

                break;
        }
    }

    private IEnumerator Quicksand() {
        yield return new WaitForSeconds(0);
    }

    private IEnumerator MapwideAttack() {
        Instantiate(mapwideAttack);
        yield return new WaitForSeconds(7.5f);
    }

    private void SpeedUp() {
        if (speed + 0.1f > MAX_SPEED) {
            speed = MAX_SPEED;
        } else {
            speed += 0.1f;
        }
        enemyScript.SetSpeed(speed);
    }

    public void Slowdown() {
        if (speed - 0.5f < MIN_SPEED) {
            speed = MIN_SPEED;
        } else {
            speed -= 0.5f;
        }
        enemyScript.SetSpeed(speed);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Enemy>(out var enemy)) {
            enemy.TakeDamage(50);
            SpeedUp();
        }
    }
}