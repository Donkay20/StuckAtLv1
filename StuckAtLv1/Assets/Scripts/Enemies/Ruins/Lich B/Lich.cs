using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lich : MonoBehaviour
{
    private readonly int LICH_MAX_HP = 1500;
    private readonly int EFFIGY_MAX_HP = 100;
    [SerializeField] private Enemy[] effigies;
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private GameObject forceField;
    [SerializeField] private GameObject skullWaveAttackPrefab;
    [SerializeField] private GameObject HomingSkullPrefab;
    [SerializeField] private GameObject attackParent;
    //stuff
    [SerializeField] private GameObject bossHPBar;
    [SerializeField] private Image bossHPBarFill;
    [SerializeField] private TextMeshProUGUI bossName;
    [SerializeField] private GameObject effigyHPBar;
    [SerializeField] private Image effigyHPBarFill;
    [SerializeField] private TextMeshProUGUI effigyName;
    //UI stuff
    private bool vulnerable, attackCheck, attackSwitch; 
    private float attackTimer;
    private int effigyHP;
    //variables
    void Start() {
        enemyScript.SetTarget(FindAnyObjectByType<Character>().gameObject);
        foreach (Enemy effigy in effigies) {
            effigy.SetTarget(FindAnyObjectByType<Character>().gameObject);
        }
        effigyHP = 100;
        attackTimer = 5;
        vulnerable = false;
        attackSwitch = false;
    }

    void OnEnable() {
        bossHPBar.SetActive(true);
        bossName.text = "The Lich.";
        bossHPBarFill.fillAmount = 1;
        //boss hp bar
        effigyHPBar.SetActive(true);
        effigyName.text = "Effigy";
        effigyHPBarFill.fillAmount = 1;
        //secondary hp bar
    }

    void Update() {
        bossHPBarFill.fillAmount = (float) enemyScript.GetHealth() / LICH_MAX_HP;
        effigyHPBarFill.fillAmount = (float) effigyHP / EFFIGY_MAX_HP;

        if (attackTimer >= 0 && !vulnerable) {
            attackTimer -= Time.deltaTime;
        }

        if (attackTimer <= 0 && !vulnerable && !attackCheck) {
            attackCheck = true;
            Attack();
        }
    }

    public void EffigyDied() {
        effigyHP -= 25;
        if (effigyHP == 0) {
            forceField.SetActive(false);
            vulnerable = true;
            InvokeRepeating(nameof(Regenerate), 1, 0.1f);
        }
    }

    public void Regenerate() {
        effigyHP++;
        if (effigyHP >= EFFIGY_MAX_HP) {
            effigyHP = EFFIGY_MAX_HP;
            CancelInvoke();
            ReactivateEffigies();
        }
    }

    public void ReactivateEffigies() {
        foreach (Enemy deadEffigy in effigies) {
            deadEffigy.SetHealth(50);
            deadEffigy.gameObject.SetActive(true);
        }
        forceField.SetActive(true);
        vulnerable = false;
    }

    public void Attack() {
        if (attackSwitch) {
            Instantiate(skullWaveAttackPrefab, attackParent.transform);
            attackSwitch = false;
        } else {
            Instantiate(HomingSkullPrefab, attackParent.transform);
            attackSwitch = true;
        }
        
        attackTimer = 3f + (2f * ((float) enemyScript.GetHealth() / LICH_MAX_HP)); //cd between attacks decreases as hp decreases
        attackCheck = false;
    }

    public bool IsVulnerable() {
        return vulnerable;
    }
}
