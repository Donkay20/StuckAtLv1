using System.Collections;
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
    [Header("Attacks")]
    [SerializeField] private GameObject slimeBarrageBullet;
    [SerializeField] private GameObject slimeSlice;
    private readonly float MAX_SIZE = 4;
    private readonly float MIN_SIZE = 0.5f;
    private readonly float ATTACK_TIMER_COOLDOWN = 10f;
    private int goopMaxHp;
    private float size;
    private float attackTimer;

    /*
        When the boss takes damage from normal attacks, it increases in size, to a maximum of 5x.
        Getting electrified causes the boss to shrink, to a minimum of 0.5x.

        All of the boss' attacks scale with its size.
    */
    
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
        int i = Random.Range(0,2);
        switch (i) {
            case 0:
                StartCoroutine(SlimeBarrage());
                break;
            case 1:
                Instantiate(slimeSlice, transform.position, Quaternion.identity, transform);
                break;
        }
        attackTimer = ATTACK_TIMER_COOLDOWN;
    }

    private IEnumerator SlimeBarrage() {
        int bulletAmount;
        if (enemyScript.GetHealth() < (goopMaxHp / 2)) {
            bulletAmount = 5;
        } else {
            bulletAmount = 3;
        }
        while (bulletAmount > 0) {
            Vector2 trajectory = target.transform.position - transform.position;
            GameObject b = Instantiate(slimeBarrageBullet, transform.position, Quaternion.identity);
            b.GetComponent<Rigidbody2D>().velocity = trajectory;
            b.transform.localScale = new Vector2(size, size);
            bulletAmount--;
            yield return new WaitForSeconds(1);
        }
    }

    private void TookDamage() {     //whenever the boss takes damage, it increases in size
        if (size < MAX_SIZE) {
            size += 0.025f;
            transform.localScale = new Vector2(size, size);
        }
    }

    public void Electrified() {     //whenever the boss is hit by a conductor bolt, its size is reduced
        if (size - 0.05f <= MIN_SIZE) {
            size = MIN_SIZE;
        } else {
            size -= 0.05f;
        }
        transform.localScale = new Vector2(size, size);
    }
}