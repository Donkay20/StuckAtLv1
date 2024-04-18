using System.Collections;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EvilJamp : MonoBehaviour
{
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private EnemyManager spawner;
    [SerializeField] private GameObject target;
    [SerializeField] private CinemachineVirtualCamera cam;
    [Space]
    [Header("UI")]
    [SerializeField] private GameObject bossHPBar;
    [SerializeField] private GameObject experienceBar;
    [SerializeField] private GameObject additionalInfo;
    [SerializeField] private Image bossHPBarFill, experienceBarFill;
    [SerializeField] private TextMeshProUGUI bossName, experienceBarTextName, additionalInfoText;
    [Header("Attacks")]
    [SerializeField] private GameObject orbitingBats;   //level 20 bonus (level 60 bonus | lv 100 bonus)
    [SerializeField] private GameObject quickSlash;     //enhanced at lv.40 & lv.80
    [SerializeField] private GameObject batWave;        //lv90
    //backend
    private int experienceToLevel = 5;
    private readonly int MAX_LEVEL = 100;
    private int quickAttackSize, orbitAttackSize;
    private int level;
    private float cameraSize = 8;
    private bool raiseCamera;
    private float experience;
    private int bossMaxHP;
    void Start() {
        level = 10;
        additionalInfoText.text = "Level " + level;
        quickAttackSize = 1;
        orbitAttackSize = 1;
        bossMaxHP = enemyScript.maxHP;
        enemyScript.SetTarget(FindAnyObjectByType<Character>().gameObject);
    }

    private void OnEnable() {
        bossHPBar.SetActive(true);
        bossName.text = "Tiffany's Son.";
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
        experienceBarFill.fillAmount =  experience / experienceToLevel;
        cam.m_Lens.OrthographicSize = cameraSize;

        if (level < MAX_LEVEL) {
            experience += Time.deltaTime;
        }

        if (experience >= experienceToLevel && level < MAX_LEVEL) {
            LevelUp();
        }

        if (raiseCamera && cameraSize < 12) {
            cameraSize += Time.deltaTime;
        }

        if (!raiseCamera && cameraSize > 8) {
            cameraSize -= Time.deltaTime;
        }

        if (enemyScript.GetHealth() < bossMaxHP / 2) {
            experienceToLevel = 4;
        } else if (enemyScript.GetHealth() < bossMaxHP / 3) {
            experienceToLevel = 3;
        } else if (enemyScript.GetHealth() < bossMaxHP / 4) {
            experienceToLevel = 2;
        } else {
            experienceToLevel = 5;
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
                StartCoroutine(SuperBatWave());
                break;
            case 100:
                spawner.SetSpawnTimer(0.5f);
                UpgradeOrbitingBats();
                enemyScript.SetSpeed(5);
                StartCoroutine(Level100Attack());
                break;
        }
        experience = 0;
        QuickSlash();
    }

    public void LevelDown() {
        if (level - 2 < 0) {
            level = 0;
        } else {
            level -= 2;
        }
        additionalInfoText.text = "Level " + level;
        switch (level) {
            case int n when n < 80 && n > 70:
                quickAttackSize = 2;
                break;
            case int n when n < 70 && n > 60:
                spawner.SetSpawnTimer(1f);
                break;
            case int n when n < 60 && n > 50:
                DowngradeOrbitingBats();
                break;
            case int n when n < 50 && n > 40:
                enemyScript.SetSpeed(2f);
                break;
            case int n when n < 40 && n > 30:
                quickAttackSize = 1;
                break;
            case int n when n < 30 && n > 20:
                spawner.SetSpawnTimer(2f);
                break;
            case int n when n < 20 && n > 10:
                orbitingBats.SetActive(false);
                break;
            case int n when n < 10 && n > 0:
                enemyScript.SetSpeed(1f);
                break;
            case 0:
                level += 19;
                enemyScript.TakeDamage(enemyScript.GetHealth() / 6);
                break;
        }
    }

    private void QuickSlash() {
        GameObject qs = Instantiate(quickSlash, target.transform.position, Quaternion.Euler(0, 0, Random.Range(0,361)));
        qs.transform.localScale = new Vector2(quickAttackSize, quickAttackSize);
    }

    private void UpgradeOrbitingBats() {
        orbitAttackSize++;
        orbitingBats.transform.localScale = new Vector2(orbitAttackSize, orbitAttackSize);
    }

    private void DowngradeOrbitingBats() {
        orbitAttackSize--;
        orbitingBats.transform.localScale = new Vector2(orbitAttackSize, orbitAttackSize);
    }

    private IEnumerator Level100Attack() {
        while (true) {
            yield return new WaitForSeconds(1.5f);
            QuickSlash();
        }
    }

    private IEnumerator SuperBatWave() {
        spawner.ClearEnemies(); spawner.SetSpawnTimer(10f);
        raiseCamera = true;
        enemyScript.SetSpeed(0);
        int batWaves = Random.Range(10, 15);
        int batToDelete = 5;
        while (batWaves > 0) {
            GameObject wave = Instantiate(batWave);
            wave.GetComponent<BatWave>().DestroyBat(batToDelete);
            if (batToDelete == 0) {
                batToDelete++;
            } else if (batToDelete == 9) {
                batToDelete--;
            } else {
                switch (Random.Range(0,2)) {
                    case 0:
                        batToDelete++;
                        break;
                    case 1:
                        batToDelete--;
                        break;
                }
            }
            yield return new WaitForSeconds(2);
            batWaves--;
        }
        enemyScript.SetSpeed(3.5f);
        spawner.SetSpawnTimer(0.75f);
        raiseCamera = false;
    }
}