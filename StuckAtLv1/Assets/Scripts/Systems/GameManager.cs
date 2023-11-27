using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

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
    }

    [SerializeField] private GameObject combatUI;
    [SerializeField] private GameObject combat;
    [SerializeField] private GameObject mapUI;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private GameObject mouseCursorUI;
    [SerializeField] private GameObject eventUI;
    [SerializeField] private GameObject shopUI;
    private GameState currentState;
    private GameState previousState;
    void Start() //default to the map when the game launches.
    {
        previousState = GameState.Map;
        currentState = GameState.Map;
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
                //disable event stuff and go into combat, TODO
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
            eventUI.SetActive(true);
            mapUI.SetActive(false);
            previousState = GameState.Event;
            Debug.Log("event state");
            break;

            case GameState.Shop:
            shopUI.SetActive(true);
            mapUI.SetActive(false);
            previousState = GameState.Shop;
            Debug.Log("shop state");
            break;

            case GameState.Miniboss:
            //TODO
            previousState = GameState.Miniboss;
            Debug.Log("miniboss state");
            break;

            case GameState.Boss:
            //TODO
            previousState = GameState.Boss;
            Debug.Log("boss state");
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
}
