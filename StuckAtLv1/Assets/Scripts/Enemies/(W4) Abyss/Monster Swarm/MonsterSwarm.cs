using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSwarm : MonoBehaviour
{
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject bossHPBar, calamityBar;
    [SerializeField] private TextMeshProUGUI bossTitle, calamityTitle;
    [SerializeField] private Image hpBarFill, calamityBarFill;
    [SerializeField] private CinemachineVirtualCamera cam;
    [Header("Attacks")]
    [SerializeField] private GameObject meteor;
    [SerializeField] private GameObject chaosFissure;
    [SerializeField] private GameObject[] meteorSpawnPosition;
    [SerializeField] private GameObject[] chaosFissurePosition;
    private float calamityMaximum = 20;
    private int swarmMaxHP;
    private float calamity;
    void Start() {
        swarmMaxHP = enemyScript.maxHP;
        enemyScript.SetTarget(FindAnyObjectByType<Character>().gameObject);
    }

    private void OnEnable() {
        //boss
        bossHPBar.SetActive(true);
        bossTitle.text = "The Monster Swarm.";
        hpBarFill.fillAmount = 1;
        //calamity
        calamityBar.SetActive(true);
        calamityTitle.text = "Calamity";
        calamityBarFill.fillAmount = 0;
    }

    void Update() {
        hpBarFill.fillAmount = (float) enemyScript.GetHealth() / swarmMaxHP;
        calamityBarFill.fillAmount = (float) calamity / calamityMaximum;

        calamity += Time.deltaTime;
        if (calamity >= calamityMaximum) {
            Calamity();
        }
    }

    public void EnemyDied() {
        enemyScript.TakeDamage(1);
        switch (enemyScript.GetHealth()) {
            case int n when (n < swarmMaxHP && n >= 100):
                enemyManager.SetSpawnTimer(1f);
                break;
            case int n when (n < 100 && n >= 50):
                enemyManager.SetSpawnTimer(0.75f);
                calamityMaximum = 15;
                break;
            case int n when (n < 50 && n >= 25):
                enemyManager.SetSpawnTimer(0.75f);
                calamityMaximum = 10;
                break;
            case int n when (n < 25 && n > 0):
                enemyManager.SetSpawnTimer(0.75f);
                calamityMaximum = 5;
                break;
        }
    }

    private void Calamity() {
        switch (Random.Range(0,2)) {
            case 0:
                ChaosFissure();
                break;
            case 1:
                StartCoroutine(MeteorFall());
                break;
        }
        calamity = 0;
    }

    private void ChaosFissure() {
        int randomSpawnPoint = Random.Range(0, chaosFissurePosition.Length);
        Instantiate(chaosFissure, chaosFissurePosition[randomSpawnPoint].transform.position, Quaternion.identity);
    }

    private IEnumerator MeteorFall() {
        int numberOfMeteors = Random.Range(9, 13);
        while (numberOfMeteors > 0) {
            yield return new WaitForSeconds(0.5f);
            int randomSpawnPoint = Random.Range(0, meteorSpawnPosition.Length);
            Instantiate(meteor, meteorSpawnPosition[randomSpawnPoint].transform.position, Quaternion.identity);
            numberOfMeteors--;
        }
    }
}