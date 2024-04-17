using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSwarm : MonoBehaviour
{
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject bossHPBar, calamityBar;
    [SerializeField] private TextMeshProUGUI bossTitle, calamityTitle;
    [SerializeField] private Image hpBarFill, calamityBarFill;
    [SerializeField] private CinemachineVirtualCamera cam;
    private readonly float CALAMITY_MAX = 20;
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
        calamityBarFill.fillAmount = (float) calamity / CALAMITY_MAX;

        calamity += Time.deltaTime;
        if (calamity >= CALAMITY_MAX) {
            Calamity();
        }
    }

    public void EnemyDied() {
        enemyScript.TakeDamage(1);
    }

    private void Calamity() {
        //todo
        calamity = 0;
    }
}