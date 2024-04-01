using UnityEngine;

public class FinalBossManager : MonoBehaviour
{
    [SerializeField] private Phase1Enemy p1Script;
    [SerializeField] private Phase2Enemy p2Script;
    [SerializeField] private Phase3Enemy p3Script;
    [Space]
    [SerializeField] private Enemy p1EnemyScript;
    [SerializeField] private Enemy p2EnemyScript;
    [SerializeField] private Enemy p3EnemyScript;
    [Space]
    [SerializeField] private GameObject phase3Enemy;
    [Space]
    [SerializeField] private GameObject bossHPBar;
    [Space]
    [SerializeField] private EnemyManager enemyManager;
    private int phase;
    void Start() {
        phase = 1;
    }

    public void Phase1EnemyDied() {
        if (p1EnemyScript != null) {
            if (phase == 1) {
                p1EnemyScript.TakeDamage(1);
            }
        }
    }

    public void BossDied(int phase) {
        phase++;
        switch (phase) {
            case 2:
                Debug.Log("Phase 2 Activated.");
                enemyManager.SetSpawnTimer(60f);
                p2Script.Activate();
                break;
            case 3:
                enemyManager.enabled = true;
                enemyManager.SetSpawnTimer(2f);
                phase3Enemy.SetActive(true);
                break;
        }
    }

    public int GetPhase() {
        return phase;
    }
}
