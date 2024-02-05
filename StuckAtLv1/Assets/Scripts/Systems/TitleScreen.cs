using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private Button startGame;
    void Start() {
        InitializeButtons();
    }

    private void InitializeButtons() {
        startGame.onClick.AddListener(() => StartGame());
    }

    private void StartGame() {
        SceneManager.LoadScene("OpeningScene");
    }
}
