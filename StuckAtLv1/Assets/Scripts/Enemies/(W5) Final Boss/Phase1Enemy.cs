using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phase1Enemy : MonoBehaviour
{
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private GameObject target;
    [SerializeField] private FinalBossManager finalBossManager;
    [SerializeField] private GameObject bossHPBar;
    [SerializeField] private TextMeshProUGUI bossTitle;
    [SerializeField] private Image hpBarFill;
    private int maxHP;
    void Awake() {
        enemyScript.SetTarget(target);
        maxHP = enemyScript.maxHP;
    }

    private void OnEnable() {
        bossHPBar.SetActive(true);
        bossTitle.text = "Tiffany's Royal Guard.";
        hpBarFill.fillAmount = 1;
    }

    void Update() {
        hpBarFill.fillAmount = (float) enemyScript.GetHealth() / maxHP;
    }
}
