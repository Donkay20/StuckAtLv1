using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BigSlime : MonoBehaviour
{
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private GameObject target;
    [Space]
    [Header("UI")]
    [SerializeField] private GameObject bossHPBar;
    [SerializeField] private GameObject sizeBar;
    [SerializeField] private Image bossHPBarFill, sizeBarFill;
    [SerializeField] private TextMeshProUGUI bossName, secondaryBarTextName;
    private readonly float MAX_SIZE = 5;
    private readonly float ATTACK_TIMER_COOLDOWN = 7f;
    private int goopMaxHp;
    private float size;
    private float attackTimer;
    
    void Start() {
        size = 1;
        attackTimer = ATTACK_TIMER_COOLDOWN;
        goopMaxHp = enemyScript.maxHP;
        enemyScript.SetTarget(FindAnyObjectByType<Character>().gameObject);
        enemyScript.OnDamageTaken += TookDamage;
    }

    private void OnEnable() {
        bossHPBar.SetActive(true);
        bossName.text = "The Goop.";
        bossHPBarFill.fillAmount = 1;
        //boss hp bar
        sizeBar.SetActive(true);
        secondaryBarTextName.text = "Size";
        sizeBarFill.fillAmount = 0;
        //secondary hp bar
    }

    void Update() {
        bossHPBarFill.fillAmount = (float) enemyScript.GetHealth() / goopMaxHp;
        sizeBarFill.fillAmount = size / MAX_SIZE;

        attackTimer -= Time.deltaTime;
        if (attackTimer < 0) {
            Attack();
        }
    }

    private void Attack() {
        //todo
    }

    private void TookDamage() {     //whenever the boss takes damage, it increases in size
        if (size < 5) {
            size += 0.05f;
            transform.localScale = new Vector2(size, size);
        }
    }

    public void Electrified() {     //whenever the boss is hit by a conductor bolt, its size is reduced
        if (size <= 0.5f) {
            size = 0.5f;
        } else {
            size -= 0.05f;
        }
        transform.localScale = new Vector2(size, size);
    }
}