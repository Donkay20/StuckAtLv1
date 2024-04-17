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
    [SerializeField] private GameObject sandSwirl;
    //backend
    private readonly float MAX_SPEED = 6;
    private readonly float MIN_SPEED = 1;
    private readonly float SPEED_BOOST_FACTOR = 0.1f;
    private readonly float SLOWDOWN_FACTOR = 1f;
    private readonly float ATTACK_COOLDOWN = 15;
    private float cameraSize = 8;
    private float attackTimer;
    private int bossMaxHP;
    private float speed = 3;
    
    void Start() {
        attackTimer = ATTACK_COOLDOWN;
        bossMaxHP = enemyScript.maxHP;
        enemyScript.SetTarget(FindAnyObjectByType<Character>().gameObject);
    }

    private void OnEnable() {
        bossHPBar.SetActive(true);
        bossName.text = "The Sandworm.";

        momentumBar.SetActive(true);
        momentumName.text = "Momentum";
    }

    void Update() {
        bossHPBarFill.fillAmount = (float) enemyScript.GetHealth() / bossMaxHP;
        momentumBarFill.fillAmount = speed / MAX_SPEED;

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0) {
            Attack();
        }

        if (cameraSize < 12) {
            cameraSize += Time.deltaTime;
            cam.m_Lens.OrthographicSize = cameraSize;
        }
    }

    private void Attack() {
        switch (Random.Range(0,2)) {
            case 0:
                Instantiate(mapwideAttack);
                break;
            case 1:
                Instantiate(sandSwirl, target.transform.position, Quaternion.identity);
                break;
        }
        attackTimer = ATTACK_COOLDOWN;
    }

    private void SpeedUp() {
        if (speed + SPEED_BOOST_FACTOR > MAX_SPEED) {
            speed = MAX_SPEED;
        } else {
            speed += SPEED_BOOST_FACTOR;
        }
        enemyScript.SetSpeed(speed);
    }

    public void Slowdown() {
        if (speed - SLOWDOWN_FACTOR < MIN_SPEED) {
            speed = MIN_SPEED;
        } else {
            speed -= SLOWDOWN_FACTOR;
        }
        enemyScript.SetSpeed(speed);
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.collider.TryGetComponent<Enemy>(out var enemy)) {
            enemy.TakeDamage(50);
            SpeedUp();
        }
    }
}