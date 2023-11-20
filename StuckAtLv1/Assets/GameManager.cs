using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum GameState {
        Map,
        Combat,
        Upgrade,
    }

    [SerializeField] private GameObject combatUI;
    [SerializeField] private GameObject mapUI;
    [SerializeField] private GameObject upgradeUI;
    private GameState state;
    void Start()
    {
        state = GameState.Map;
        UpdateState();
    }

    private void UpdateState() {

    }
}
