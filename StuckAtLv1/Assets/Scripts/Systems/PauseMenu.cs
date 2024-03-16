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
    [SerializeField] private BuffManager buffManager;
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
    [SerializeField] private Sprite[] buffIcons, debuffIcons;
    [SerializeField] private Image[] activeBuffs, activeDebuffs;
    [SerializeField] private TextMeshProUGUI[] activeBuffDuration, activeDebuffDuration;
    [SerializeField] private GameObject[] activeBuffTooltip, activeDebuffTooltip;
    [SerializeField] private TextMeshProUGUI[] activeBuffTooltipText, activeDebuffTooltipText;
    private bool[] buffTooltipInteractable = new bool[5];
    private bool[] debuffTooltipInteractable = new bool[5];
    
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
            basicAtkSpdText.text = "2.00 / s"; tempAtkSpdBoost.text = "0%"; permAtkSpdBoost.text = "0%";
            moveSpeedText.text = "5.0"; moveSpdBuff.text = "0%"; moveSpdDebuff.text = "0%";
        } else {
            basicAtkSpdText.text = (1 / (0.5f * slotManager.GetTempAtkSpd() * slotManager.GetPermanentAtkSpd())).ToString("f2") + " / s";
            tempAtkSpdBoost.text = (((1 / slotManager.GetTempAtkSpd()) - 1) * 100).ToString("f0") + "%";
            permAtkSpdBoost.text = (((1 / slotManager.GetPermanentAtkSpd()) - 1) * 100).ToString("f0") + "%";

            moveSpeedText.text = movement.GetSpeed().ToString("f1");
            moveSpdBuff.text = ((movement.SpeedModifier - 1) * 100).ToString("f0") + "%";
            moveSpdDebuff.text = ((movement.SpeedDebuff - 1) * -100).ToString("f0") + "%";
        }
        
        dashCooldown.text = movement.GetDashCooldown().ToString("f1");

        (int buffs, int debuffs) = buffManager.GetNumberOfBuffsDebuffs();
        if (buffs > 0 || debuffs > 0) {
            PopulateBuffsDebuffs();
        }

        //Center
        worldNumber.text = mapManager.GetWorld().ToString();
        levelNumber.text = mapManager.GetLevel().ToString();

        //Right side
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
        resumeButton.image.raycastTarget = false;
        resumeButton.GetComponentInChildren<TextMeshProUGUI>().raycastTarget = false;
        optionsButton.interactable = false;
        optionsButton.image.raycastTarget = false;
        optionsButton.GetComponentInChildren<TextMeshProUGUI>().raycastTarget = false;
        quitButton.interactable = false;
        quitButton.image.raycastTarget = false;
        quitButton.GetComponentInChildren<TextMeshProUGUI>().raycastTarget = false;
    }

    private void EnableCenterButtons() {
        resumeButton.interactable = true;
        resumeButton.image.raycastTarget = true;
        resumeButton.GetComponentInChildren<TextMeshProUGUI>().raycastTarget = true;
        optionsButton.interactable = true;
        optionsButton.image.raycastTarget = true;
        optionsButton.GetComponentInChildren<TextMeshProUGUI>().raycastTarget = true;
        quitButton.interactable = true;
        quitButton.image.raycastTarget = true;
        quitButton.GetComponentInChildren<TextMeshProUGUI>().raycastTarget = true;
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

    private void PopulateBuffsDebuffs() {
        for (int i = 0; i < 5 ; i++) {
            if (buffManager.doesBuffExist(i)) {
                (string buffType, float efficacy, float duration) = buffManager.GetBuffInfo(i);
                switch (buffType) {
                    case "power":
                        activeBuffs[i].sprite = buffIcons[1];
                        activeBuffTooltipText[i].text = "Increase skill damage by " + (efficacy * 100) + "%.";
                        break;
                    case "speed":
                        activeBuffs[i].sprite = buffIcons[2];
                        activeBuffTooltipText[i].text = "Increase movement speed by " + (efficacy * 100) + "%.";
                        break;
                    case "bloodsucker": 
                        activeBuffs[i].sprite = buffIcons[3];
                        activeBuffTooltipText[i].text = "Basic attacks inflict Anemia(1).";
                        break;
                    case "bulwark": 
                        activeBuffs[i].sprite = buffIcons[4];
                        activeBuffTooltipText[i].text = "If overhealing, health increases over time instead.";
                        break;
                    case "critdmg": 
                        activeBuffs[i].sprite = buffIcons[5];
                        activeBuffTooltipText[i].text = "Increase skill critical damage by " + (efficacy * 100) + "%.";
                        break;
                    case "penetration": 
                        activeBuffs[i].sprite = buffIcons[6];
                        activeBuffTooltipText[i].text = "Basic attacks pierce through enemies.";
                        break;
                    case "avarice":
                        activeBuffs[i].sprite = buffIcons[7];
                        activeBuffTooltipText[i].text = "If not overhealing, basic attacks have +50% crit chance and grant 1 gold when they hit an enemy.";
                        break;
                }
                activeBuffDuration[i].text = duration.ToString("f1") + "s";
                buffTooltipInteractable[i] = true;
                Debug.Log("Buff populated: " + i);
            } else {
                activeBuffs[i].sprite = buffIcons[0];
                activeBuffDuration[i].text = "";
                activeBuffTooltipText[i].text = "";
                buffTooltipInteractable[i] = false;
            }

            if (buffManager.doesDebuffExist(i)) {
                (string debuffType, float severity, float duration) = buffManager.GetDebuffInfo(i);
                switch (debuffType) {
                    case "slow":
                        activeDebuffs[i].sprite = debuffIcons[1];
                        activeDebuffTooltipText[i].text = "Reduces movement speed by " + (-1 * ((1 - severity) * 100)) + "%.";
                        break;
                    case "bleed":
                        activeDebuffs[i].sprite = debuffIcons[2];
                        activeDebuffTooltipText[i].text = "Health drain is increased by " + (severity * 100) + "%.";
                        break;
                    case "anemia":
                        activeDebuffs[i].sprite = debuffIcons[3];
                        activeDebuffTooltipText[i].text = "Lose " + (severity * 100) + "% of your current health per second.";
                        break;
                }
                activeDebuffDuration[i].text = duration.ToString("f1") + "s";
                debuffTooltipInteractable[i] = true;
                Debug.Log("Debuff populated: " + i);
            } else {
                activeDebuffs[i].sprite = debuffIcons[0];
                activeDebuffDuration[i].text = "";
                activeDebuffTooltipText[i].text = "";
                debuffTooltipInteractable[i] = false;
            }
        }
    }

    public void ShowBuffTooltip(int buff) {
        if (buffTooltipInteractable[buff]) {
            activeBuffTooltip[buff].SetActive(true);
            Debug.Log("Buff tooltip " + buff + " activated");
        }
    }

    public void HideBuffTooltip(int buff) {
        activeBuffTooltip[buff].SetActive(false);
    }

    public void ShowDebuffTooltip(int debuff) {
        if (debuffTooltipInteractable[debuff]) {
            activeDebuffTooltip[debuff].SetActive(true);
            Debug.Log("Debuff tooltip " + debuff + " activated");
        }
    }

    public void HideDebuffTooltip(int debuff) {
        activeDebuffTooltip[debuff].SetActive(false);
    }

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
        //Left side
    }
}
