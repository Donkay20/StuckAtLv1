using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
/*
This class manages gameflow. We go from state to state to connect each portion of the game to each other.
Ideally, we should only need these eight states, plus any cutscenes in the middle we need to play for lore purposes.

All UI is in the canvas.
There are separate combat, map, event, and upgrade scripts that manage each event individually and report back to this script.
*/
{
    private enum GameState {
        Map,
        Combat,
        Survival,
        Miniboss,
        Boss,
        Event,
        Shop,
        Upgrade,
        Dialogue,
    }

    [SerializeField] private GameObject combatUI;
    [SerializeField] private GameObject combat;
    [SerializeField] private GameObject mapUI;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private GameObject mouseCursorUI;
    [SerializeField] private GameObject eventUI;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject minibossTempUI;
    [SerializeField] private GameObject bossTempUI;
    [SerializeField] private MapManager mapManager;
    private GameState currentState;
    private GameState previousState;
    private int scaling;

    private int[] slotUpgrades = new int[5];
    private int[] weight = new int[5];
    private int maxSlots;
    void Start() //default to the map when the game launches.
    {
        previousState = GameState.Map;
        currentState = GameState.Map;
        maxSlots = 2; //default amt of slots
        UpdateState();
    }

    private void UpdateState() {
        /*
        Updates the game's state, aptly named. 
        The last line before the break for each state change switches the previous state to the current state to reference for the next one after that.
        */
        
        switch(currentState) {
            //non map focused
            case GameState.Map:
                //do stuff
                if (previousState == GameState.Combat || previousState == GameState.Survival) { //Not sure if this is needed anymore?
                    combat.SetActive(false);
                    mapUI.SetActive(true); mouseCursorUI.SetActive(true);
                }
                if (previousState == GameState.Upgrade) {
                    upgradeUI.SetActive(false);
                    mapUI.SetActive(true);
                }
                if (previousState == GameState.Event) {
                    eventUI.SetActive(false);
                    mapUI.SetActive(true);
                }
                if (previousState == GameState.Shop) {
                    shopUI.SetActive(false);
                    mapUI.SetActive(true);
                }
                if (previousState == GameState.Miniboss) {
                    minibossTempUI.SetActive(false);
                    mapUI.SetActive(true);
                }
                if (previousState == GameState.Boss) {
                    bossTempUI.SetActive(false);
                    mapUI.SetActive(true);
                }
                if (previousState == GameState.Dialogue) {
                    //unload dialogue scene
                    switch(mapManager.GetWorld()) {
                        case 1:
                            if (mapManager.GetLevel() == 5) {
                                SceneManager.UnloadSceneAsync("RuinsMiniBossEnd");
                            }

                            if (mapManager.GetLevel() == 11) {
                                SceneManager.UnloadSceneAsync("RuinsBossEnd");
                            }
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                    }
                    mapUI.SetActive(true);
                }
                previousState = GameState.Map;
                break;

            case GameState.Upgrade:
                if (previousState == GameState.Combat || previousState == GameState.Survival) {
                    combat.SetActive(false); combatUI.SetActive(false);
                    upgradeUI.SetActive(true); upgradeUI.GetComponent<UpgradeManager>().Setup("normal");
                    mouseCursorUI.SetActive(true);
                    //give normal upgrades
                }
                if (previousState == GameState.Miniboss || previousState == GameState.Boss) {
                    combat.SetActive(false); combatUI.SetActive(false);
                    upgradeUI.SetActive(true); upgradeUI.GetComponent<UpgradeManager>().Setup("legendary");
                    mouseCursorUI.SetActive(true);
                    //give legendary upgrades
                }
                if(previousState == GameState.Event) {
                    eventUI.SetActive(false);
                    upgradeUI.SetActive(true); upgradeUI.GetComponent<UpgradeManager>().Setup("normal");
                    //I don't foresee any events that would give legendary upgrades, but if they do this'll need to be changed.
                }
                if (previousState == GameState.Shop) {
                    upgradeUI.SetActive(true); upgradeUI.GetComponent<UpgradeManager>().Shop(); upgradeUI.GetComponent<UpgradeManager>().Setup("normal"); //proc a special flag to let the upgrade manager know to go back to the shop
                    shopUI.SetActive(false);
                }
                previousState = GameState.Upgrade;
                break;

            //map focused
            case GameState.Combat:
                if (previousState == GameState.Map) {
                    mapUI.SetActive(false); mouseCursorUI.SetActive(false);
                    combat.SetActive(true); combatUI.SetActive(true);
                    combat.GetComponent<CombatManager>().Setup("combat");
                    //disable map stuff and go into combat
                }
                if (previousState == GameState.Event) {
                    eventUI.SetActive(false);
                    combat.SetActive(true); combatUI.SetActive(true);
                    combat.GetComponent<CombatManager>().Setup("combat");
                    Debug.Log("event to combat");
                }
                previousState = GameState.Combat;
                break;

            case GameState.Survival:
                if (previousState == GameState.Map) {
                    mapUI.SetActive(false); mouseCursorUI.SetActive(false);
                    combat.SetActive(true); combatUI.SetActive(true);
                    combat.GetComponent<CombatManager>().Setup("survival");
                    //disable map stuff and go into survival
                    Debug.Log("map to survival");
                }
                if (previousState == GameState.Event) {
                    //disable event stuff and go into survival, TODO
                    Debug.Log("event to survival");
                }
                previousState = GameState.Survival;
                break;

            case GameState.Event:
                eventUI.SetActive(true); eventUI.GetComponent<EventManager>().InitializeEvent();
                mapUI.SetActive(false);
                previousState = GameState.Event;
                Debug.Log("event state");
                break;

            case GameState.Shop:
                if (previousState == GameState.Map) {
                    shopUI.SetActive(true);
                    mapUI.SetActive(false);
                    Debug.Log("map to shop");
                }

                if (previousState == GameState.Upgrade) {
                    upgradeUI.SetActive(false);
                    shopUI.SetActive(true);
                    Debug.Log("upgrade to shop");
                }
                
                previousState = GameState.Shop;
                Debug.Log("shop state");
                break;

            case GameState.Miniboss:
                if (previousState == GameState.Dialogue) {
                    switch (mapManager.GetWorld()) {
                        case 1:
                            previousState = GameState.Miniboss;
                            minibossTempUI.SetActive(true);
                            SceneManager.UnloadSceneAsync("RuinsMiniBossIntro");
                            break;
                        case 2:
                            //todo 
                            break;
                        case 3:
                            //todo
                            break;
                    }
                }
                
                if (previousState == GameState.Map) {
                    mapUI.SetActive(false);
                    ReceiveCommand("dialogue");
                }
                
                Debug.Log("miniboss state");
                break;

            case GameState.Boss:
                if (previousState == GameState.Dialogue) {
                    switch (mapManager.GetWorld()) {
                        case 1:
                            previousState = GameState.Boss;
                            bossTempUI.SetActive(true);
                            SceneManager.UnloadSceneAsync("RuinsBossIntro");
                            break;
                        case 2:
                            //todo 
                            break;
                        case 3:
                            //todo
                            break;
                    }
                    //unload dialogue scene
                }

                if (previousState == GameState.Map) {
                    mapUI.SetActive(false);
                    ReceiveCommand("dialogue");
                }
                
                Debug.Log("boss state");
                break;

            case GameState.Dialogue:
                //From the map, we play the boss' intro scene. To determine which one to play, we grab the level and world.
                if (previousState == GameState.Map) {
                    //load dialogue scene
                    switch (mapManager.GetWorld()) {
                        case 1:
                            if (mapManager.GetLevel() == 5) {
                                SceneManager.LoadScene("RuinsMiniBossIntro", LoadSceneMode.Additive);
                            }

                            if (mapManager.GetLevel() == 11) {
                                SceneManager.LoadScene("RuinsBossIntro", LoadSceneMode.Additive);
                            }
                            break;
                        case 2:
                            //todo
                            break;
                        case 3:
                            //todo
                            break;
                    }
                    mapUI.SetActive(false);
                }

                //From the miniboss/boss, we play the boss' outro scene
                if (previousState == GameState.Miniboss) {
                    //load dialogue scene
                    switch(mapManager.GetWorld()) {
                        case 1:
                                SceneManager.LoadScene("RuinsMiniBossEnd", LoadSceneMode.Additive);
                            break;
                        case 2:
                            //todo
                            break;
                        case 3:
                            //todo
                            break;
                    }
                    minibossTempUI.SetActive(false);
                }
                if (previousState == GameState.Boss) {
                    //load dialogue scene
                    switch(mapManager.GetWorld()) {
                        case 1:
                            SceneManager.LoadScene("RuinsBossEnd", LoadSceneMode.Additive);
                            break;
                        case 2:
                            //todo
                            break;
                        case 3:
                            //todo
                            break;
                    }
                    bossTempUI.SetActive(false);
                }
                previousState = GameState.Dialogue;
                Debug.Log("dialogue state");
                break;
        }
    }

    public void ReceiveCommand(String name) {
        switch (name) {
            //non-map focused
            case "map":
            currentState = GameState.Map;
                break;
            case "upgrade":
            currentState = GameState.Upgrade;
                break;
            case "dialogue":
            currentState = GameState.Dialogue;
                break;

            //map focused
            case "combat":
            currentState = GameState.Combat;
                break;
            case "survival":
            currentState = GameState.Survival;
                break;
            case "event":
            currentState = GameState.Event;
                break;
            case "shop":
            currentState = GameState.Shop;
                break;
            case "miniboss":
            currentState = GameState.Miniboss;
                break;
            case "boss":
            currentState = GameState.Boss;
                break;
        }
        UpdateState();
    }

    public void AdjustScaling() {
        scaling = mapManager.GetWorld() * mapManager.GetLevel();
    }

    public int ScaleDifficulty() {
        return scaling;
    }

    public void AddMaxSlots() {
        maxSlots++;
    }

    public void AdjustSlotUpgradeCounter(int identity) {
        //gamemanager holds the amt of upgrades for each slot.
        slotUpgrades[identity-1]++;
        CalculateWeight();
    }

    private void CalculateWeight() {
        int minValue = slotUpgrades[0];
        for (int i = 0; i < maxSlots-1; i++) {
            Debug.Log(i);
            if (slotUpgrades[i] < minValue) {minValue = slotUpgrades[i];}
        }
        for (int i = 0; i < maxSlots-1; i++) {
            Debug.Log(i);
            weight[i] = slotUpgrades[i] - minValue;
        }
    }

    public int GetWeight(int identity) {
        return weight[identity-1];
    }
}
