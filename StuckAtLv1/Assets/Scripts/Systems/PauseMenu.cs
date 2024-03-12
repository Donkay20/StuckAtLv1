using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private KeyCode initPauseMenu;
    private Animator pauseAnimator;
    private bool pauseMenuOpen;

    void Awake() {
        pauseAnimator = GetComponent<Animator>();
    }

    void Update() {
        if (Input.GetKeyDown(initPauseMenu)) {
            if (!pauseMenuOpen) {
                PauseMenuStart();
            } else {
                PauseMenuEnd();
            }
        }
    }

    private void PauseMenuStart() {
        pauseAnimator.SetTrigger("Intro");
        Time.timeScale = 0;
        pauseMenuOpen = true;
    }

    private void PauseMenuEnd() {
        pauseAnimator.SetTrigger("Outro");
        Time.timeScale = 1;
        pauseMenuOpen = false;
    }
}
