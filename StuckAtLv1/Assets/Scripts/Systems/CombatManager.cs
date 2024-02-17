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
    [SerializeField] private Movement charMovement;
    [SerializeField] private MapManager mapProgress;
    [SerializeField] private GameManager notify;
    [SerializeField] private EnemyManager spawner;
    [SerializeField] private BuffManager buffManager;
    [SerializeField] private GameObject bossUI;
    [SerializeField] private Slot[] slots = new Slot[5];
    [SerializeField] private GameObject[] ruinsRooms = new GameObject[5];
    [SerializeField] private GameObject[] ruinsSpawn = new GameObject[5];
    [SerializeField] private GameObject[] forestRooms = new GameObject[5];
    [SerializeField] private GameObject[] forestSpawn = new GameObject[5];
    [SerializeField] private GameObject[] sewerRooms = new GameObject[5];
    [SerializeField] private GameObject[] sewerSpawn = new GameObject[5];
    //todo, abyss?
    private int enemiesToKill, timeLeft, condition, roomChosen;
    private bool bossIsAlive;
    private GameObject room;
    private string objective;
    private bool specialCondition;

    private void Awake() {
        specialCondition = false;
        condition = -1;
    }

    public void Setup(string format) {
        bossUI.SetActive(false);
        /*
        Set-up includes:
        - choosing the map and placing a character in it
        - checking for special event fights
        - getting which type of combat it is (combat vs survival)
        - setting fight to active
        - enabling the spawner
        - setting the # of enemies or time to survive based on the progress in the map
        - updating the UI accordingly
        */

        switch (mapProgress.GetWorld()) {
            //choose a room, set it to be active, position the character to the spawn point
            case 1: //Ruins
                switch (format) {
                    case "combat":
                    case "survival":
                        roomChosen = Random.Range(0, 4);
                        break;
                    case "miniboss":
                        roomChosen = 4;
                        break;
                    case "boss":
                        roomChosen = 5; //todo
                        break;
                }
                ruinsRooms[roomChosen].SetActive(true); 
                room = ruinsRooms[roomChosen];
                character.gameObject.transform.position = ruinsSpawn[roomChosen].gameObject.transform.position;
                Debug.Log(roomChosen);
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }

        if (specialCondition) { 
            //when adding a special condition, it needs to be mentioned here, and on the enemy manager.
            switch(condition) {
                case 2: //Ruins Event 2
                    spawner.SetCondition(condition);
                    objective = "combat";
                    spawner.enabled = true;
                    enemiesToKill = 20;
                    uIObjectiveNumber.text = enemiesToKill.ToString();
                    StartCoroutine(CombatTracker());
                    break;
                case 8: //Ruins Event 5
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

            case "miniboss":
                objective = "miniboss";
                bossIsAlive = true;
                spawner.enabled = true;
                uIObjective.text = "Defeat miniboss!!";
                uIObjectiveNumber.text = "∞";
                StartCoroutine(BossTracker());
                break;

            case "boss":
                objective = "boss";
                bossIsAlive = true;
                spawner.enabled = true;
                uIObjective.text = "Defeat boss!!";
                uIObjectiveNumber.text = "∞";
                break;
            }
        }
        character.Heal(0);
    }

    public void EnemyKilled() { 
        //for combat-type encounter use only
        if(objective == "combat") {
            enemiesToKill--; uIObjectiveNumber.text = enemiesToKill.ToString();
        }
        if(objective == "miniboss") {
            bossIsAlive = false;
        }
    }

    public void ReceiveCondition(int c) {
        specialCondition = true;
        condition = c;
    }
    
    public void BossDied() {
        bossUI.SetActive(false);
        bossIsAlive = false;
    }

    private IEnumerator SurvivalTimer() { 
        //handles timer countdown for survival-type encounter format
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

    private IEnumerator BossTracker() {
        yield return new WaitUntil(() => !bossIsAlive);
        Finish();
    }

    public string GetObjective() {
        return objective;
    }

    private void Finish() {         
        //Disable the spawner & kill all remaining enemies
        spawner.enabled = false;
        Enemy[] remainingEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy straggler in remainingEnemies) {
            straggler.TakeDamage(999);
        }

        //Reset the objective and stop coroutines in the character script to prevent errors
        objective = "";
        character.Interrupt();
        charMovement.CombatEnd();
        buffManager.EndBattle();

        //reset the cooldown when the battle ends.
        foreach (Slot slot in slots) {
            if (slot != null) {
                slot.BattleEnd();
            }
        }

        //Get rid of any special conditions
        if (specialCondition) {
            specialCondition = false;
            condition = -1;
            spawner.SetCondition(-1);
        }

        //Disable the current map
        room.SetActive(false);

        //Notify the game manager.
        notify.ReceiveCommand("upgrade");
    }
}
