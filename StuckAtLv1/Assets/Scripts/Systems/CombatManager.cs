using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    /*
    Handles both combat and survival modes.
    */

    [SerializeField] private TextMeshProUGUI uIObjective;
    [SerializeField] private TextMeshProUGUI uIObjectiveNumber;
    private int enemiesToKill;
    private int timeLeft;
    private string objective;
    
    [SerializeField] private MapManager mapProgress;
    [SerializeField]private GameManager notify;
    [SerializeField] private EnemyManager spawner;
    private bool active;

    void Update()
    {
        //for now, handles checking when combat is done in here. there's prob a better way to do this
        if (objective == "combat" && active) {
            if (enemiesToKill <= 0) {
                Finish();
            }
        }
    }

    public void Setup(string format) {
        /*
        Set-up includes:
        - getting which type of combat it is (combat vs survival)
        - setting fight to active
        - enabling the spawner
        - setting the # of enemies or time to survive based on the progress in the map
        - updating the UI accordingly
        */
        switch (format) {
            case "combat":
            objective = "combat";
            active = true;
            spawner.enabled = true;
            enemiesToKill = mapProgress.GetWorld()*(20 + (2 * mapProgress.GetLevel()));
            uIObjectiveNumber.text = enemiesToKill.ToString(); uIObjective.text = "Defeat!";
            break;

            case "survival":
            objective = "survival";
            active = true;
            spawner.enabled = true;
            timeLeft = mapProgress.GetWorld()*(30 + mapProgress.GetLevel());
            uIObjectiveNumber.text = timeLeft.ToString(); uIObjective.text = "Survive!";
            StartCoroutine(SurvivalTimer());
            break;
        }
    }

    public void EnemyKilled() {
        if(objective == "combat") {
            enemiesToKill--; uIObjectiveNumber.text = enemiesToKill.ToString();
        }
    }

    private IEnumerator SurvivalTimer() {
        //handles timer countdown for survival format
        while (timeLeft > 0) {
            yield return new WaitForSeconds(1);
            timeLeft--; uIObjectiveNumber.text = timeLeft.ToString();
        }
        Finish();
    }

    private void Finish() {         //Disable the spawner, kill all remaining enemies, rend the combat manager inactive, then notify the game manager that the objective has been completed
        spawner.enabled = false;

        Enemy[] remainingEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy straggler in remainingEnemies) {
            straggler.TakeDamage(999999999);
        }

        active = false;
        objective = "";
        notify.ReceiveCommand("map");
        //normally go back to the upgrade screen, but go to map just for testing
    }
}
