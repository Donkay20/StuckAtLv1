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
}
