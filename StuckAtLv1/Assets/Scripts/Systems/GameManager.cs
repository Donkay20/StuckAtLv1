using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
/*
This class manages gameflow. We go from state to state to connect each portion of the game to each other.
Ideally, we should only need these four states, plus any cutscenes in the middle we need to play for lore purposes.

All UI is in the canvas.
There are separate combat, map, event, and upgrade scripts that manage each event individually and report back to this script.
*/
{
    private enum GameState {
        Map,
        Combat,
        Event,
        Upgrade,
    }

    [SerializeField] private GameObject combatUI;
    [SerializeField] private GameObject mapUI;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private GameObject mouseCursorUI;
    private GameState currentState;
    private GameState previousState;
    void Start()
    {
        previousState = GameState.Map;
        currentState = GameState.Map;
        UpdateState(currentState);
    }

    private void UpdateState(GameState state) {
        switch(state) {
            case GameState.Map:
            break;

            case GameState.Combat:
            break;

            case GameState.Event:
            break;

            case GameState.Upgrade:
            break;
        }
    }

    public void ReceiveCommand(String name) {
        switch (name) {
            //non-map focused
            case "map":
            Debug.Log("Map command received.");
            break;
            case "upgrade":
            Debug.Log("Upgrade command received.");
            break;

            //map focused
            case "combat":
            Debug.Log("Combat command received.");
            break;
            case "survival":
            Debug.Log("Survival command received.");
            break;
            case "event":
            Debug.Log("Event command received.");
            break;
            case "shop":
            Debug.Log("Shop command received.");
            break;
            case "miniboss":
            Debug.Log("Miniboss command received.");
            break;
            case "boss":
            Debug.Log("Event command received.");
            break;
        }
    }
}
