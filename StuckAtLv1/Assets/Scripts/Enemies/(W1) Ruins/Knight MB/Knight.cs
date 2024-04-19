using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Knight : MonoBehaviour
{
    private readonly int KNIGHT_MAX_HP = 800;
    [SerializeField] private GameObject sword; 
    [SerializeField] private Enemy enemyScript, swordScript;
    //handles scripts and objects
    [SerializeField] private GameObject bossHPBar, swordHPBar, additionalInfo;
    [SerializeField] private Image bossHPBarFill, swordHPBarFill;
    [SerializeField] private TextMeshProUGUI bossName, swordName, additionalInfoText;
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
        //hp bar
        swordHPBar.SetActive(true);
        swordName.text = "Sword";
        swordHPBarFill.fillAmount = 0;
        //secondary hp bar
        additionalInfo.SetActive(true);
        additionalInfoText.text = "";
        //additional info
    }

    void Update() {
        bossHPBarFill.fillAmount = (float) enemyScript.GetHealth() / KNIGHT_MAX_HP;
        swordHPBarFill.fillAmount = (float) swordScript.GetHealth() / 30;

        if (vulnerable) {
            swordCooldown -= Time.deltaTime;
            additionalInfoText.text = "Sword regen in " + swordCooldown.ToString("f1");
        } else {
            additionalInfoText.text = "";
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
                Instantiate(swordBeamPrefab, swordBeamParent.transform);
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
