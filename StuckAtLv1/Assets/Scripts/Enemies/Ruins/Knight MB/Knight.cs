using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Knight : MonoBehaviour
{
    private readonly int KNIGHT_MAX_HP = 1000;
    [SerializeField] private GameObject sword; 
    [SerializeField] private Enemy enemyScript, swordScript;
    //handles scripts and objects
    [SerializeField] private GameObject bossHPBar;
    [SerializeField] private Image bossHPBarFill;
    [SerializeField] private TextMeshProUGUI bossName;
    //UI stuff
    [SerializeField] private GameObject swordBeamPrefab, lateralSlashPrefab; 
    [SerializeField] private GameObject swordBeamParent, lateralSlashParent;
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

        if (attackCooldown > 0 && !vulnerable) {
            attackCooldown -= Time.deltaTime;
        }

        if (attackCooldown <= 0 && !attackCheck && !vulnerable) {
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
        enemyScript.ApplyStun(5f);
    }

    public void Attack() {
        //Swaps between the sword beam and the lateral slash.
        switch (swapAttack) {
            case true: //Sword Beam
                Instantiate(swordBeamPrefab, lateralSlashParent.transform);
                swapAttack = false;
                break;
            case false: //Lateral Slash
                Instantiate(lateralSlashPrefab, lateralSlashParent.transform);
                swapAttack = true;
                break;
        }
        attackCooldown = 1f + (4f *((float) enemyScript.GetHealth() / KNIGHT_MAX_HP)); //cd between attacks decreases as hp decreases
        attackCheck = false;
    }
}
