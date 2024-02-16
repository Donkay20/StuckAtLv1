using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Knight : MonoBehaviour
{
    private readonly int KNIGHT_MAX_HP = 100;
    [SerializeField] private GameObject sword; //floating sword hovering around boss
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private GameObject bossHPBar;
    [SerializeField] private Image bossHPBarFill;
    [SerializeField] private TextMeshProUGUI bossName;
    [SerializeField] private Enemy swordScript;
    private bool vulnerable;
    [SerializeField] private float swordCooldown;
    void Start() {
        enemyScript.SetTarget(FindAnyObjectByType<Character>().gameObject);
        swordScript.SetTarget(FindAnyObjectByType<Character>().gameObject);
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
    }

    public bool IsVulnerable() {
        return vulnerable;
    }

    public void SwordDied() {
        vulnerable = true;
        swordCooldown = 5;
    }

    //todo, add the attacks.
}
