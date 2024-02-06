using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossPlaceholder : MonoBehaviour
{
    [SerializeField] private Button startGame;
    [SerializeField] private GameManager gameManager;
    void Start() {
        InitializeButtons();
    }

    private void InitializeButtons() {
        startGame.onClick.AddListener(() => Continue());
    }

    private void Continue() {
        gameManager.ReceiveCommand("dialogue");
    }
}
