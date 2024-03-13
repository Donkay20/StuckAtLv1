using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private KeyCode initPauseMenu;
    private Animator pauseAnimator;
    private bool pauseMenuOpen;
    private String status;

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
        status = "center";
    }

    private void PauseMenuEnd() {
        pauseAnimator.SetTrigger("Outro");
        Time.timeScale = 1;
        pauseMenuOpen = false;
    }

    public void Center() {
        if (status != "center") {
            pauseAnimator.SetTrigger("Center");
            status = "center";
        }
    }

    public void Right() {
        if (status != "right") {
            pauseAnimator.SetTrigger("Right");
            status = "right";
        }
    }

    public void Left() {
        if (status != "left") {
            pauseAnimator.SetTrigger("Left");
            status = "left";
        }
    }
}
