using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    //Handles both combat and survival modes.
    [SerializeField] private TextMeshProUGUI uIObjective;
    [SerializeField] private TextMeshProUGUI uIObjectiveNumber;
    [SerializeField] private TextMeshProUGUI uIObjectiveReminderText;
    [SerializeField] private Character character;
    [SerializeField] private Movement charMovement;
    [SerializeField] private MapManager mapProgress;
    [SerializeField] private GameManager notify;
    [SerializeField] private EnemyManager spawner;
    [SerializeField] private BuffManager buffManager;
    [SerializeField] private SlotManager slotManager;
    [SerializeField] private CombatUIAnimation uIAnimation;
    [SerializeField] private GameObject bossUI, bossSecondaryUI;
    [SerializeField] private GameObject survivalHourglass;
    [SerializeField] private GameObject combatSkull;
    [SerializeField] private GameObject warningLeft, warningRight;
    [SerializeField] private Slot[] slots = new Slot[5];
    [SerializeField] private GameObject[] ruinsRooms = new GameObject[5];
    [SerializeField] private GameObject[] ruinsSpawn = new GameObject[5];
    [SerializeField] private GameObject[] forestRooms = new GameObject[5];
    [SerializeField] private GameObject[] forestSpawn = new GameObject[5];
    [SerializeField] private GameObject[] sewerRooms = new GameObject[5];
    [SerializeField] private GameObject[] sewerSpawn = new GameObject[5];
    [SerializeField] private GameObject tiffanyRoom;
    [SerializeField] private GameObject tiffanySpawn;
    //todo, abyss
    private int enemiesToKill, timeLeft, condition, roomChosen;
    private bool bossIsAlive;
    private GameObject room;
    private string objective;
    private bool specialCondition;
    [SerializeField] private Animator combatAnimation, combatUIAnimation;

    private void Awake() {
        specialCondition = false;
        condition = -1;
        combatAnimation = GetComponent<Animator>();
        combatAnimation.SetTrigger("Intro");
        combatUIAnimation.SetTrigger("Intro");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            Finish(); //debugging
        }
    }

    private void OnEnable() {
        combatAnimation.SetTrigger("Intro");
        combatUIAnimation.SetTrigger("Intro");  
    }

    public void Setup(string format) {
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

        bossUI.SetActive(false);
        bossSecondaryUI.SetActive(false);

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
                character.gameObject.transform.position = ruinsSpawn[roomChosen].transform.position;
                break;
            case 2: //Forest
                switch (format) {
                    case "combat":
                    case "survival":
                        roomChosen = Random.Range(0, 2); //adjust when more forest rooms are made available.
                        break;
                    case "miniboss":
                        roomChosen = 4;
                        break;
                    case "boss":
                        roomChosen = 5;
                        break;
                }
                forestRooms[roomChosen].SetActive(true);
                room = forestRooms[roomChosen];
                character.gameObject.transform.position = forestSpawn[roomChosen].transform.position;
                break;
            case 3:
                tiffanyRoom.SetActive(true);
                room = tiffanyRoom;
                character.gameObject.transform.position = tiffanySpawn.transform.position;
                break;
            case 4:
                break;
            case 5:
                break;
        }

        if (specialCondition) {     //when adding a special condition, it needs to be mentioned here, and on the enemy manager.
            spawner.SetCondition(condition);
            
            switch(condition) {
                case 2:     //Ruins Event 2
                    uIObjective.text = "Defeat!";
                    objective = "combat";
                    combatSkull.SetActive(true);
                    enemiesToKill = 20;
                    spawner.SetSpawnTimer(0.4f);
                    uIObjectiveNumber.text = enemiesToKill.ToString();
                    StartCoroutine(CombatTracker());
                    break;
                case 8:     //Ruins Event 5
                    uIObjective.text = "Defeat!";
                    objective = "combat";
                    combatSkull.SetActive(true);
                    enemiesToKill = 15;
                    spawner.SetSpawnTimer(0.4f);
                    uIObjectiveNumber.text = enemiesToKill.ToString();
                    StartCoroutine(CombatTracker());
                    break;
                case 11:    //Forest Event 1
                    uIObjective.text = "Defeat!";
                    objective = "combat";
                    combatSkull.SetActive(true);
                    enemiesToKill = 30;
                    spawner.SetSpawnTimer(0.4f);
                    uIObjectiveNumber.text = enemiesToKill.ToString();
                    StartCoroutine(CombatTracker());
                    break;
                case 12:    //Forest Event 3
                    uIObjective.text = "Defeat!";
                    objective = "combat";
                    combatSkull.SetActive(true);
                    enemiesToKill = 30;
                    spawner.SetSpawnTimer(0.4f);
                    uIObjectiveNumber.text = enemiesToKill.ToString();
                    StartCoroutine(CombatTracker());
                    break;
                case 19:    //Forest Event 5
                    uIObjective.text = "Defeat!";
                    objective = "combat";
                    combatSkull.SetActive(true);
                    enemiesToKill = 50;
                    spawner.SetSpawnTimer(0.4f);
                    uIObjectiveNumber.text = enemiesToKill.ToString();
                    StartCoroutine(CombatTracker());
                    break;
            }
        } else {
            switch (format) {
            case "combat":
                objective = "combat";
                combatSkull.SetActive(true);
                spawner.SetSpawnTimer(0.5f);
                enemiesToKill = mapProgress.GetWorld() * (10 + (2 * mapProgress.GetLevel())); //orig 10
                uIObjectiveNumber.text = enemiesToKill.ToString(); uIObjective.text = "Defeat!";
                StartCoroutine(CombatTracker());
                break;

            case "survival":
                objective = "survival";
                survivalHourglass.SetActive(true);
                spawner.SetSpawnTimer(0.5f);
                timeLeft = mapProgress.GetWorld() * (20 + mapProgress.GetLevel()); //orig 20
                uIObjectiveNumber.text = timeLeft.ToString(); uIObjective.text = "Survive!";
                StartCoroutine(SurvivalTimer());
                break;

            case "miniboss":
                objective = "miniboss";
                bossIsAlive = true;
                spawner.SetSpawnTimer(3f);
                uIObjective.text = "Defeat miniboss!!";
                uIObjectiveNumber.text = "∞";
                StartCoroutine(BossTracker());
                break;

            case "boss":
                objective = "boss";
                bossIsAlive = true;
                spawner.SetSpawnTimer(2f);
                uIObjective.text = "Defeat boss!!";

                if (mapProgress.GetWorld() == 3) {
                    spawner.SetSpawnTimer(1f);
                    uIObjective.text = "Kill Tiffany!!";
                }
                
                uIObjectiveNumber.text = "∞";
                StartCoroutine(BossTracker());
                break;
            }
        }
        character.Heal(0);
    }

    public void EnemyKilled() {
        //for combat-type encounter use only
        if(objective == "combat") {
            enemiesToKill--;
            if (enemiesToKill <= 0) {
                uIObjectiveNumber.text = "Clear!";
            } else {
                uIObjectiveNumber.text = enemiesToKill.ToString();
            }
        }

        if(objective == "miniboss" || objective == "boss") {
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
            if (timeLeft == 10 || (timeLeft < 4 && timeLeft > 0)) {
                uIObjectiveReminderText.text = timeLeft.ToString();
                combatUIAnimation.SetTrigger("Objective");
            } 
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

    public void WarningAnimation(string side) {
        combatUIAnimation.SetTrigger(side);
    }

    public void SetObjective(string objective) {
        this.objective = objective;
    }

    private void Finish() {
        //Disable the spawner & get rid of all remaining enemies, damage numbers, money, and drops
        spawner.enabled = false;

        Enemy[] remainingEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy straggler in remainingEnemies) {
            straggler.SelfDestruct();
        }

        DamageNumberParent[] remainingDamageNumbers = FindObjectsByType<DamageNumberParent>(FindObjectsSortMode.None);
        foreach (DamageNumberParent d in remainingDamageNumbers) {
            d.BattleOver();
        }

        ExtraDrop[] remainingMoneyDrops = FindObjectsByType<ExtraDrop>(FindObjectsSortMode.None);
        foreach (ExtraDrop moneybags in remainingMoneyDrops) {
            moneybags.DestroyExtraDrops();
        }

        EnemyGroup[] remainingEnemyGroups = FindObjectsByType<EnemyGroup>(FindObjectsSortMode.None);
        foreach (EnemyGroup enemyGroup in remainingEnemyGroups) {
            enemyGroup.BattleEnd();
        }

        //Disable UI
        survivalHourglass.SetActive(false);
        combatSkull.SetActive(false);
        if (objective == "miniboss" || objective == "boss") {
            bossSecondaryUI.SetActive(false);
            bossUI.SetActive(false);
        }

        //Reset the objective and stop coroutines in various scripts to prevent errors
        objective = "";
        character.Interrupt();
        charMovement.BattleEnd();
        buffManager.BattleEnd();
        slotManager.BattleEnd();

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

        //Do the outro animation
        //combatAnimation.SetTrigger("Outro");
        combatUIAnimation.SetTrigger("Outro");
    }

    public void IntroTransitionAnimationComplete() {
        Debug.Log("Intro animation method called");
        spawner.enabled = true; 
    }

    public void OutroTransitionAnimationComplete() {
        //Notify the game manager.
        notify.ReceiveCommand("upgrade");
    }
}
