using System.Collections;
using System.Collections.Generic;
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
    private readonly int CALAMITY_MAX;
    private int swarmMaxHP;
    private int calamity;
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
    }

    public void EnemyDied() {
        enemyScript.TakeDamage(1);
    }
}
