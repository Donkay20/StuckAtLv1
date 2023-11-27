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
            spawner.enabled = true;
            enemiesToKill = mapProgress.GetWorld()*(2 + (2 * mapProgress.GetLevel())); //orig 20
            uIObjectiveNumber.text = enemiesToKill.ToString(); uIObjective.text = "Defeat!";
            StartCoroutine(CombatTracker());
            break;

            case "survival":
            objective = "survival";
            spawner.enabled = true;
            timeLeft = mapProgress.GetWorld()*(10 + mapProgress.GetLevel()); //orig 30
            uIObjectiveNumber.text = timeLeft.ToString(); uIObjective.text = "Survive!";
            StartCoroutine(SurvivalTimer());
            break;
        }
    }

    public void EnemyKilled() { //for combat-type encounter use only
        if(objective == "combat") {
            enemiesToKill--; uIObjectiveNumber.text = enemiesToKill.ToString();
        }
    }

    private IEnumerator SurvivalTimer() { //handles timer countdown for survival-type encounter format
        while (timeLeft > 0) {
            yield return new WaitForSeconds(1);
            timeLeft--; uIObjectiveNumber.text = timeLeft.ToString();
        }
        Finish();
    }

    private IEnumerator CombatTracker() {
        //handles tracker for combat format
        yield return new WaitUntil(() => enemiesToKill <= 0);
        Finish();
    }

    private void Finish() {         //Disable the spawner, kill all remaining enemies, rend the combat manager inactive, then notify the game manager that the objective has been completed
        spawner.enabled = false;

        Enemy[] remainingEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy straggler in remainingEnemies) {
            straggler.TakeDamage(999999999);
        }
        objective = "";
        notify.ReceiveCommand("upgrade");
    }
}
