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
    [SerializeField] private Character character;
    private int enemiesToKill;
    private int timeLeft;
    private string objective;

    private bool specialCondition;

    private int condition;
    
    [SerializeField] private MapManager mapProgress;
    [SerializeField]private GameManager notify;
    [SerializeField] private EnemyManager spawner;
    
    private void Awake() {
        specialCondition = false;
        condition = -1;
    }

    public void Setup(string format) {
        /*
        Set-up includes:
        - checking for special event fights
        - getting which type of combat it is (combat vs survival)
        - setting fight to active
        - enabling the spawner
        - setting the # of enemies or time to survive based on the progress in the map
        - updating the UI accordingly
        */

        if (specialCondition) { //when adding a special condition, it needs to be mentioned here, and on the enemy manager.
            switch(condition) {
                case 2:
                    spawner.SetCondition(condition);
                    objective = "combat";
                    spawner.enabled = true;
                    enemiesToKill = 20;
                    uIObjectiveNumber.text = enemiesToKill.ToString();
                    StartCoroutine(CombatTracker());
                    break;
                case 8:
                    spawner.SetCondition(condition);
                    objective = "combat";
                    spawner.enabled = true;
                    enemiesToKill = 10;
                    uIObjectiveNumber.text = enemiesToKill.ToString();
                    StartCoroutine(CombatTracker());
                    break;
            }
        } else {
            switch (format) {
            case "combat":
            objective = "combat";
            spawner.enabled = true;
            enemiesToKill = mapProgress.GetWorld()*(1 + (2 * mapProgress.GetLevel())); //orig 10
            uIObjectiveNumber.text = enemiesToKill.ToString(); uIObjective.text = "Defeat!";
            StartCoroutine(CombatTracker());
            break;

            case "survival":
            objective = "survival";
            spawner.enabled = true;
            timeLeft = mapProgress.GetWorld()*(2 + mapProgress.GetLevel()); //orig 20
            uIObjectiveNumber.text = timeLeft.ToString(); uIObjective.text = "Survive!";
            StartCoroutine(SurvivalTimer());
            break;
            }
        }
        character.Heal(0);
    }

    public void EnemyKilled() { //for combat-type encounter use only
        if(objective == "combat") {
            enemiesToKill--; uIObjectiveNumber.text = enemiesToKill.ToString();
        }
    }

    public void ReceiveCondition(int c) {
        specialCondition = true;
        condition = c;
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
        character.Interrupt();

        if (specialCondition) {
            specialCondition = false;
            condition = -1;
            spawner.SetCondition(-1);
        }
        
        notify.ReceiveCommand("upgrade");
    }
}
