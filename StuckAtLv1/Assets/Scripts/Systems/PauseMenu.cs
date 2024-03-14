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
    [SerializeField] private TextMeshProUGUI tempAtkDmgBoost;
    [SerializeField] private TextMeshProUGUI permAtkDmgBoost;
    [SerializeField] private TextMeshProUGUI tempAtkSpdBoost;
    [SerializeField] private TextMeshProUGUI permAtkSpdBoost;
    [SerializeField] private TextMeshProUGUI moveSpdBuff;
    [SerializeField] private TextMeshProUGUI moveSpdDebuff;
    [SerializeField] private TextMeshProUGUI dashCooldown;
    [SerializeField] private GameObject atkDmgTooltip;
    [SerializeField] private GameObject atkSpdTooltip;
    [SerializeField] private GameObject moveSpdTooltip;
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
        //Left side
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
        tempAtkDmgBoost.text = slotManager.GetTempAtkDmg().ToString();
        permAtkDmgBoost.text = slotManager.GetPermanentAtkDmg().ToString();
        
        basicAtkSpdText.text = (1 / (0.5f * slotManager.GetTempAtkSpd() * slotManager.GetPermanentAtkSpd())).ToString("f2") + " / second";
        tempAtkSpdBoost.text = (((1 / slotManager.GetTempAtkSpd()) - 1) * 100).ToString("f0") + "%";
        permAtkSpdBoost.text = (((1 / slotManager.GetPermanentAtkSpd()) - 1) * 100).ToString("f0") + "%";

        moveSpeedText.text = movement.GetSpeed().ToString("f1");
        moveSpdBuff.text = ((movement.SpeedModifier - 1) * 100).ToString("f0") + "%";
        moveSpdDebuff.text = ((movement.SpeedDebuff - 1) * -100).ToString("f0") + "%";

        dashCooldown.text = movement.GetDashCooldown().ToString("f1");
        //Center

        //Right side
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

    //Center
    public void Center() {
        if (status != "center") {
            pauseAnimator.SetTrigger("Center");
            status = "center";
        }
    }

    //Right
    public void Right() {
        if (status != "right") {
            pauseAnimator.SetTrigger("Right");
            status = "right";
        }
    }

    //Left
    public void Left() {
        if (status != "left") {
            pauseAnimator.SetTrigger("Left");
            status = "left";
        }
    }

    public void ShowAtkDmgTooltip() {atkDmgTooltip.SetActive(true);}

    public void HideAtkDmgTooltip() {atkDmgTooltip.SetActive(false);}

    public void ShowAtkSpdTooltip() {atkSpdTooltip.SetActive(true);}

    public void HideAtkSpdTooltip() {atkSpdTooltip.SetActive(false);}

    public void ShowMoveSpdTooltip() {moveSpdTooltip.SetActive(true);}

    public void HideMoveSpdTooltip() {moveSpdTooltip.SetActive(false);}
}
