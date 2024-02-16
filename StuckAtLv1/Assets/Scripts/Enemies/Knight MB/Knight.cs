using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Knight : MonoBehaviour
{
    private readonly int KNIGHT_MAX_HP = 100;
    [SerializeField] private GameObject sword; 
    [SerializeField] private Enemy enemyScript, swordScript; 
    //handles scripts and objects
    [SerializeField] private GameObject bossHPBar;
    [SerializeField] private Image bossHPBarFill;
    [SerializeField] private TextMeshProUGUI bossName;
    //UI stuff
    [SerializeField] private GameObject swordBeamPrefab, lateralSlashPrefab; 
    [SerializeField] private GameObject prefabParent;
    //knight attacks 
    private bool vulnerable, attackCheck, swapAttack;
    [SerializeField] private float swordCooldown, attackCooldown;
    //vars

    void Start() {
        enemyScript.SetTarget(FindAnyObjectByType<Character>().gameObject);
        swordScript.SetTarget(FindAnyObjectByType<Character>().gameObject);
        attackCooldown = 5f;
    }

    void OnEnable() {
        bossName.text = "The Knight.";
        bossHPBar.SetActive(true);
        bossHPBarFill.fillAmount = 1;
    }

    void Update() {
        bossHPBarFill.fillAmount = (float) enemyScript.GetHealth() / KNIGHT_MAX_HP;
        if (vulnerable) {
            swordCooldown -= Time.deltaTime;
        }

        if (swordCooldown <= 0 && vulnerable) {
            vulnerable = false;
            swordScript.SetHealth(10);
            sword.SetActive(true);
        }

        if (attackCooldown > 0) {
            attackCooldown -= Time.deltaTime;
        }

        if (attackCooldown <= 0 && !attackCheck) {
            attackCheck = true;
            Attack();
        }
    }

    public bool IsVulnerable() {
        return vulnerable;
    }

    public void SwordDied() {
        vulnerable = true;
        swordCooldown = 5;
    }

    public void Attack() {
        switch (swapAttack) {
            case true:
                Instantiate(swordBeamPrefab, prefabParent.transform);
                swapAttack = false;
                break;
            case false:
                Instantiate(lateralSlashPrefab, prefabParent.transform);
                swapAttack = true;
                break;
        }
        attackCooldown = 3f + (2f *((float) enemyScript.GetHealth() / KNIGHT_MAX_HP)); //cd between attacks decreases as hp decreases
        attackCheck = false;
    }
}
