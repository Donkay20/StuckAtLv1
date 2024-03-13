using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private KeyCode initializePauseMenu;
    [Header("Associated Scripts")]
    [SerializeField] private Character character;
    [SerializeField] private Movement movement;
    [SerializeField] private SlotManager slotManager;
    private Animator pauseAnimator;
    private bool pauseMenuOpen;
    private String status;
    [Space] //left side
    [Header("Left Section | Status")]
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private TextMeshProUGUI afterimageText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI basicAtkDmgText;
    [SerializeField] private TextMeshProUGUI basicAtkSpdText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;
    [Space] //center
    [Header("Center | Pause Menu")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    void Awake() {
        pauseAnimator = GetComponent<Animator>();
    }

    void Update() {
        if (Input.GetKeyDown(initializePauseMenu)) {
            if (!pauseMenuOpen) {
                UpdateAllStatuses();
                PauseMenuStart();
            } else {
                PauseMenuEnd();
            }
        }
    }

    private void UpdateAllStatuses() {
        HPText.text = character.currentHp.ToString();
        if (character.currentHp > 10) {
            HPText.color = Color.green;
        } else if (character.currentHp < 10) {
            HPText.color = Color.red;
        } else {
            HPText.color = Color.white;
        }
        
        afterimageText.text = character.afterimage.ToString("f1") + "s";
        moneyText.text = character.money.ToString();
        basicAtkDmgText.text = (2 + slotManager.GetTempAtkDmg() + slotManager.GetPermanentAtkDmg()).ToString();
        basicAtkSpdText.text = (1 / (0.5f * slotManager.GetTempAtkSpd() * slotManager.GetPermanentAtkSpd())).ToString("f1") + " / second";
        moveSpeedText.text = movement.GetSpeed().ToString("f1");
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
