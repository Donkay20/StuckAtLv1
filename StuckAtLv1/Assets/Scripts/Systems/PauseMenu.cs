using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private KeyCode initializePauseMenu;
    [Header("Associated Scripts")]
    [SerializeField] private Character character;
    [SerializeField] private Movement movement;
    [SerializeField] private SlotManager slotManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private Slot[] slots;
    private string status;
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
    [SerializeField] private TextMeshProUGUI worldNumber;
    [SerializeField] private TextMeshProUGUI levelNumber;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button optionCancel;
    [SerializeField] private Toggle easyModeToggle;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button quitYes, quitNo;
    [SerializeField] private GameObject quitContextMenu;
    [SerializeField] private GameObject optionContextMenu;
    private Animator pauseAnimator;
    private bool pauseMenuOpen;

    void Awake() {
        pauseAnimator = GetComponent<Animator>();
        InitializeButtons();
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

        if (mapManager.GetWorld() == 1 && mapManager.GetLevel() == 0) { //stats break if the player hasn't entered at least one level, for some reason
            basicAtkSpdText.text = "2.00 / second";
            tempAtkSpdBoost.text = "0%";
            permAtkSpdBoost.text = "0%";

            moveSpeedText.text = "5.0";
            moveSpdBuff.text = "0%";
            moveSpdDebuff.text = "0%";
        } else {
            basicAtkSpdText.text = (1 / (0.5f * slotManager.GetTempAtkSpd() * slotManager.GetPermanentAtkSpd())).ToString("f2") + " / second";
            tempAtkSpdBoost.text = (((1 / slotManager.GetTempAtkSpd()) - 1) * 100).ToString("f0") + "%";
            permAtkSpdBoost.text = (((1 / slotManager.GetPermanentAtkSpd()) - 1) * 100).ToString("f0") + "%";

            moveSpeedText.text = movement.GetSpeed().ToString("f1");
            moveSpdBuff.text = ((movement.SpeedModifier - 1) * 100).ToString("f0") + "%";
            moveSpdDebuff.text = ((movement.SpeedDebuff - 1) * -100).ToString("f0") + "%";
        }
        
        

        dashCooldown.text = movement.GetDashCooldown().ToString("f1");

        //Center
        worldNumber.text = mapManager.GetWorld().ToString();
        levelNumber.text = mapManager.GetLevel().ToString();

        //Right side
    }

    private void PauseMenuStart() {
        EnableCenterButtons();
        pauseAnimator.SetTrigger("Intro");
        Time.timeScale = 0;
        pauseMenuOpen = true;
        status = "center";
    }

    private void PauseMenuEnd() {
        OptionCancel();
        QuitCancel();
        pauseAnimator.SetTrigger("Outro");
        Time.timeScale = 1;
        pauseMenuOpen = false;
    }

    //Center
    public void Center() {
        if (status != "center") {
            EnableCenterButtons();
            pauseAnimator.SetTrigger("Center");
            status = "center";
        }
    }

    private void QuitButtonClicked() {quitContextMenu.SetActive(true);}

    private void QuitConfirm() {SceneManager.LoadScene("TitleScreen");}

    private void QuitCancel() {quitContextMenu.SetActive(false);}

    private void OptionButtonClicked() {optionContextMenu.SetActive(true);}

    private void OptionToggleClicked(bool isEasyMode) {gameManager.EasyModeToggle(easyModeToggle.isOn);}

    private void OptionCancel() {optionContextMenu.SetActive(false);}

    private void DisableCenterButtons() {
        resumeButton.interactable = false;
        optionsButton.interactable = false;
        quitButton.interactable = false;
    }

    private void EnableCenterButtons() {
        resumeButton.interactable = true;
        optionsButton.interactable = true;
        quitButton.interactable = true;
    }

    //Right
    public void Right() {
        if (status != "right") {
            DisableCenterButtons();
            pauseAnimator.SetTrigger("Right");
            status = "right";
        }
    }

    //Left
    public void Left() {
        if (status != "left") {
            DisableCenterButtons();
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

    private void InitializeButtons() {
        //Center
        resumeButton.onClick.AddListener(() => PauseMenuEnd());

        optionsButton.onClick.AddListener(() => OptionButtonClicked());
        optionCancel.onClick.AddListener(() => OptionCancel());
        easyModeToggle.onValueChanged.AddListener(OptionToggleClicked);

        quitButton.onClick.AddListener(() => QuitButtonClicked());
        quitYes.onClick.AddListener(() => QuitConfirm());
        quitNo.onClick.AddListener(() => QuitCancel());
        
        //Right side
    }
}
