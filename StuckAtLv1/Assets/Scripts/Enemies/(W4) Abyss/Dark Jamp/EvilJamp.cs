using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvilJamp : MonoBehaviour
{
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private EnemyManager spawner;
    [SerializeField] private GameObject target;
    [Space]
    [Header("UI")]
    [SerializeField] private GameObject bossHPBar;
    [SerializeField] private GameObject experienceBar;
    [SerializeField] private GameObject additionalInfo;
    [SerializeField] private Image bossHPBarFill, experienceBarFill;
    [SerializeField] private TextMeshProUGUI bossName, experienceBarTextName, additionalInfoText;
    [Header("Attacks")]
    [SerializeField] private GameObject orbitingBats;   //level 20 bonus (level 60 bonus)
    [SerializeField] private GameObject quickSlash;     //enhanced at lv.40 & lv.80
    //backend
    private readonly int EXPERIENCE_TO_LEVEL = 5;
    private readonly int MAX_LEVEL = 100;
    private int quickAttackSize, orbitAttackSize;
    private int level;
    private float experience;
    private int bossMaxHP;
    void Start() {
        level = 10;
        quickAttackSize = 1;
        orbitAttackSize = 1;
        bossMaxHP = enemyScript.maxHP;
        enemyScript.SetTarget(FindAnyObjectByType<Character>().gameObject);
    }

    private void OnEnable() {
        bossHPBar.SetActive(true);
        bossName.text = "Evil Jamp.";
        bossHPBarFill.fillAmount = 1;
        //boss hp bar
        experienceBar.SetActive(true);
        experienceBarTextName.text = "Experience:";
        experienceBarFill.fillAmount = 0;
        //secondary hp bar
        additionalInfo.SetActive(true);
        additionalInfoText.text = "Level " + level;
        //additional info
    }

    void Update() {
        bossHPBarFill.fillAmount = (float) enemyScript.GetHealth() / bossMaxHP;
        experienceBarFill.fillAmount =  experience / EXPERIENCE_TO_LEVEL;

        if (level < MAX_LEVEL) {
            experience += Time.deltaTime;
        }

        if (experience >= EXPERIENCE_TO_LEVEL && level < MAX_LEVEL) {
            LevelUp();
        }
    }

    private void LevelUp() {
        level++; additionalInfoText.text = "Level " + level;
        switch (level) {
            case 20:
                orbitingBats.SetActive(true);
                break;
            case 30:
                spawner.SetSpawnTimer(1f);
                break;
            case 40:
                quickAttackSize = 2;
                break;
            case 50:
                enemyScript.SetSpeed(3.5f);
                break;
            case 60:
                UpgradeOrbitingBats();
                break;
            case 70:
                spawner.SetSpawnTimer(0.75f);
                break;
            case 80:
                quickAttackSize = 3;
                break;
            case 90:
                Nuke();
                break;
            case 100:
                spawner.SetSpawnTimer(0.5f);
                UpgradeOrbitingBats();
                enemyScript.SetSpeed(5);
                StartCoroutine(Level100Attack());
                break;
        }
        experience = 0;
        Attack();
    }

    private void Attack() { //Quick Slash
        GameObject qs = Instantiate(quickSlash, target.transform.position, Quaternion.Euler(0, 0, Random.Range(0,361)));
        qs.transform.localScale = new Vector2(quickAttackSize, quickAttackSize);
    }

    private void UpgradeOrbitingBats() {
        orbitAttackSize++;
        orbitingBats.transform.localScale = new Vector2(orbitAttackSize, orbitAttackSize);
    }

    private IEnumerator Level100Attack() {
        while (true) {
            yield return new WaitForSeconds(1f);
            Attack();
        }
    }

    private void Nuke() {
        //todo
    }
}
