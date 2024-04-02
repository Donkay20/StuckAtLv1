using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private Button startGame;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;
    private Animator anim;
    void Start() {
        anim = GetComponent<Animator>();
        InitializeButtons();
    }

    private void Update() {
        if (Input.GetMouseButton(0)) {
            anim.SetTrigger("Skip");
        }
    }

    private void OnEnable() {
        Time.timeScale = 1;
    }

    private void InitializeButtons() {
        startGame.onClick.AddListener(() => StartGame());
        creditsButton.onClick.AddListener(() => Credits());
        quitButton.onClick.AddListener(() => QuitGame());
    }

    private void StartGame() {
        SceneManager.LoadScene("OpeningScene");
    }

    private void Credits() {
        SceneManager.LoadScene("Credits");
    }

    private void QuitGame() {
        Application.Quit();
    }
}
