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
    [SerializeField] private AttackSlotBonus attackSlotBonus;
    [SerializeField] private Slot[] slots;
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

    [Space]
    [Header ("Right | Artifact")]
    [SerializeField] private TextMeshProUGUI[] slotDamage;
    [SerializeField] private TextMeshProUGUI[] slotSize;
    [SerializeField] private TextMeshProUGUI[] slotDuration;
    [SerializeField] private TextMeshProUGUI[] slotCritChance;
    [SerializeField] private TextMeshProUGUI[] slotCritDamage;
    [SerializeField] private GameObject slot3Group, slot4Group, slot5Group;
    [SerializeField] private GameObject slot3LockImage, slot4LockImage, slot5LockImage;
    [SerializeField] private Button[] slotDetailButtons;
    [SerializeField] private GameObject artifactUpgradeDetailPanel;
    [SerializeField] private Button closeArtifactUpgradeDetailPanel;
    [SerializeField] private TextMeshProUGUI detailPanelText;
    //end
    //variables
    private Animator pauseAnimator;
    private bool pauseMenuOpen;
    private string status;
    private bool okToClose, okToOpen;
    private bool[] buffTooltipInteractable = new bool[5];
    private bool[] debuffTooltipInteractable = new bool[5];

    void Awake() {
        okToOpen = true;
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

        if (pauseMenuOpen) {
            switch (status) {
                case "center":
                    if (Input.GetKeyDown(KeyCode.A)) {
                        Left();
                    } else if (Input.GetKeyDown(KeyCode.D)) {
                        Right();
                    }
                    break;
                case "left":
                    if (Input.GetKeyDown(KeyCode.D)) {
                        Center();
                    } else if (Input.GetKeyDown(KeyCode.A)) {
                        Right();
                    }
                    break;
                case "right":
                    if (Input.GetKeyDown(KeyCode.A)) {
                        Center();
                    } else if (Input.GetKeyDown(KeyCode.D)) {
                        Left();
                    }
                    break;
            }
        }
    }

    private void PauseMenuStart() {
        if (okToOpen) {
            okToOpen = false;
            EnableCenterButtons();
            pauseAnimator.SetTrigger("Intro");
            Time.timeScale = 0;
            pauseMenuOpen = true;
            status = "center";
        }
        
    }

    private void IntroAnimationFinished() {
        okToClose = true;
    }

    private void OutroAnimationFinished() {
        okToOpen = true;
    }

    private void PauseMenuEnd() {
        if (okToClose) {
            okToClose = false;
            OptionCancel();
            QuitCancel();
            CloseAdditionalDetailPanel();
            pauseAnimator.SetTrigger("Outro");
            Time.timeScale = 1;
            pauseMenuOpen = false;
        }
    }

    private void UpdateAllStatuses() {  //takes a snapshot of all relevant stats right before pausing.
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

        if (mapManager.GetWorld() == 1 && mapManager.GetLevel() == 0) {     //stats won't display correctly if the player hasn't entered at least one level
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
        PopulateBuffsDebuffs();

        //Center
        worldNumber.text = mapManager.GetWorld().ToString();
        levelNumber.text = mapManager.GetLevel().ToString();

        //Right side
        GetSlotStatistics();
    }

    //CENTER CENTER CENTER CENTER CENTER CENTER CENTER CENTER CENTER CENTER CENTER 
    public void Center() {  //swap screen to center view
        if (status != "center") {
            EnableCenterButtons();
            pauseAnimator.SetTrigger("Center");
            status = "center";
        }
    }

    private void QuitButtonClicked() {quitContextMenu.SetActive(true);}

    private void QuitConfirm() {Time.timeScale = 1; SceneManager.LoadScene("TitleScreen");}

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

    //RIGHT SIDE RIGHT SIDE RIGHT SIDE RIGHT SIDE RIGHT SIDE RIGHT SIDE RIGHT SIDE RIGHT SIDE 
    public void Right() {   //swap screen to right view
        if (status != "right") {
            DisableCenterButtons();
            pauseAnimator.SetTrigger("Right");
            status = "right";
        }
    }

    private void GetSlotStatistics() {
        for (int i = 0; i < gameManager.GetMaxSlots(); i++) {
            Debug.Log(gameManager.GetMaxSlots());
            (float damage, float size, float duration) = attackSlotBonus.GetUpgradeCalculation(slots[i]);
            int critChance = slots[i].GetCritChance();
            float critDamage = slots[i].CriticalDamage();

            if (mapManager.GetWorld() == 1 && mapManager.GetLevel() == 0) { //breaks if paused before the game starts
                slotDamage[i].text = "+0%";
                slotSize[i].text = "+0%" ;
                slotDuration[i].text = "+0%" ;
                slotCritChance[i].text = "5%";
                slotCritDamage[i].text = "200%";
            } else {
                slotDamage[i].text = "+" + ((damage - 1) * 100).ToString("f0") + "%";
                slotSize[i].text = "+" + ((size - 1) * 100).ToString("f0") + "%";
                slotDuration[i].text = "+" + ((duration - 1) * 100).ToString("f0") + "%";
                slotCritChance[i].text = critChance.ToString("f0") + "%";
                slotCritDamage[i].text = (critDamage * 100).ToString("f0") + "%";
            }

            
        }
    }

    private void GenerateAdditionalDetailPanel(int slot) {
        artifactUpgradeDetailPanel.SetActive(true);

        //Common
        if (slots[slot].GetCommonUpgrade(0) > 0) {
            detailPanelText.text += "Skill damage +10% (" + slots[slot].GetCommonUpgrade(0) + ")\n";
        }
        if (slots[slot].GetCommonUpgrade(1) > 0) {
            detailPanelText.text += "Skill size +5% (" + slots[slot].GetCommonUpgrade(1) + ")\n";
        }
        if (slots[slot].GetCommonUpgrade(2) > 0) {
            detailPanelText.text += "Skill duration +20% (" + slots[slot].GetCommonUpgrade(2) + ")\n";
        }
        if (slots[slot].GetCommonUpgrade(3) > 0) {
            detailPanelText.text += "Overheal 2 HP on skill cast (" + slots[slot].GetCommonUpgrade(3) + ")\n";
        }
        if (slots[slot].GetCommonUpgrade(4) > 0) {
            detailPanelText.text += "Critical chance +10% (" + slots[slot].GetCommonUpgrade(4) + ")\n";
        }
        if (slots[slot].GetCommonUpgrade(5) > 0) {
            detailPanelText.text += "5% skill damage buff on skill cast (" + slots[slot].GetCommonUpgrade(5) + ")\n";
        }
        if (slots[slot].GetCommonUpgrade(6) > 0) {
            detailPanelText.text += "Critical damage +20% (" + slots[slot].GetCommonUpgrade(6) + ")\n";
        }
        if (slots[slot].GetCommonUpgrade(7) > 0) {
            detailPanelText.text += "Skills inflict 20% slow for 5 seconds (" + slots[slot].GetCommonUpgrade(7) + ")\n";
        }
        if (slots[slot].GetCommonUpgrade(8) > 0) {
            detailPanelText.text += "5% movement speed buff on skill cast (" + slots[slot].GetCommonUpgrade(8) + ")\n";
        }
        if (slots[slot].GetCommonUpgrade(9) > 0) {
            detailPanelText.text += "+1 temp. basic attack damage on-kill (" + slots[slot].GetCommonUpgrade(9) + ")\n";
        }
        if (slots[slot].GetCommonUpgrade(10) > 0) {
            detailPanelText.text += "Skill chance to inflict anemia on-hit +50% (" + slots[slot].GetCommonUpgrade(10) + ")\n";
        }
        if (slots[slot].GetCommonUpgrade(11) > 0) {
            detailPanelText.text += "Push away nearby enemies on skill cast (" + slots[slot].GetCommonUpgrade(11) + ")\n";
        }
        if (slots[slot].GetCommonUpgrade(12) > 0) {
            detailPanelText.text += "Treasure Chest spawn chance on-kill +5% (" + slots[slot].GetCommonUpgrade(12) + ")\n";
        }
        if (slots[slot].GetCommonUpgrade(13) > 0) {
            detailPanelText.text += "Enemy kills grant increased gold (" + slots[slot].GetCommonUpgrade(13) + ")\n";
        }
        if (slots[slot].GetCommonUpgrade(14) > 0) {
            detailPanelText.text += "+1% temp. attack speed on-cast (" + slots[slot].GetCommonUpgrade(14) + ")\n";
        }

        //Rare 
        detailPanelText.text += "\n";
        if (slots[slot].GetRareUpgrade(0) > 0) {
            detailPanelText.text += "+1 temp. basic attack damage / +5% basic attack speed on-kill (" + slots[slot].GetRareUpgrade(0) + ")\n";
        }
        if (slots[slot].GetRareUpgrade(1) > 0) {
            detailPanelText.text += "+5% skill size / +10% skill damage / +20% skill duration (" + slots[slot].GetRareUpgrade(1) + ")\n";
        }
        if (slots[slot].GetRareUpgrade(2) > 0) {
            detailPanelText.text += "+3 slot weight (instead of 1) / +1000g (" + slots[slot].GetRareUpgrade(2) + ")\n";
        }
        if (slots[slot].GetRareUpgrade(3) > 0) {
            detailPanelText.text += "No overheal = +30% crit chance (" + slots[slot].GetRareUpgrade(3) + ")\n";
        }
        if (slots[slot].GetRareUpgrade(4) > 0) {
            detailPanelText.text += "No overheal & critical hit = +0.5s afterimage-time (" + slots[slot].GetRareUpgrade(4) + ")\n";
        }
        if (slots[slot].GetRareUpgrade(5) > 0) {
            detailPanelText.text += "+5% move speed buff per second of afterimage-time on-cast (" + slots[slot].GetRareUpgrade(5) + ")\n";
        }
        if (slots[slot].GetRareUpgrade(6) > 0) {
            detailPanelText.text += "Cannot gain overheal, gain temp. attack speed instead (" + slots[slot].GetRareUpgrade(6) + ")\n";
        }
        if (slots[slot].GetRareUpgrade(7) > 0) {
            detailPanelText.text += "Doubles overhealing gained (" + slots[slot].GetRareUpgrade(7) + ")\n";
        }
        if (slots[slot].GetRareUpgrade(8) > 0) {
            detailPanelText.text += "Overheal 1 HP on-hit (" + slots[slot].GetRareUpgrade(8) + ")\n";
        }
        if (slots[slot].GetRareUpgrade(9) > 0) {
            detailPanelText.text += "Skill damage +0.5% of HP (" + slots[slot].GetRareUpgrade(9) + ")\n";
        }
        if (slots[slot].GetRareUpgrade(10) > 0) {
            detailPanelText.text += "Skill size +1% of overheal (" + slots[slot].GetRareUpgrade(10) + ")\n";
        }
        if (slots[slot].GetRareUpgrade(11) > 0) {
            detailPanelText.text += "Inflict anemia on-hit (" + slots[slot].GetRareUpgrade(11) + ")\n";
        }
        if (slots[slot].GetRareUpgrade(12) > 0) {
            detailPanelText.text += "Grants gold when hitting an anemic enemy (" + slots[slot].GetRareUpgrade(12) + ")\n";
        }
        if (slots[slot].GetRareUpgrade(13) > 0) {
            detailPanelText.text += "Spreads anemia nearby when hitting an anemic enemy (" + slots[slot].GetRareUpgrade(13) + ")\n";
        }
        if (slots[slot].GetRareUpgrade(14) > 0) {
            detailPanelText.text += "+20% skill damage boost upon inflicting anemia (" + slots[slot].GetRareUpgrade(14) + ")\n";
        }

        //Legendary
        detailPanelText.text += "\n";
        if (slots[slot].GetLegendaryUpgrade(0) > 0) {
            detailPanelText.text += "Enemy explodes on-kill (" + slots[slot].GetLegendaryUpgrade(0) + ")\n";
        }
        if (slots[slot].GetLegendaryUpgrade(1) > 0) {
            detailPanelText.text += "Grant Penetration on-kill for 3 seconds (" + slots[slot].GetLegendaryUpgrade(1) + ")\n";
        }
        if (slots[slot].GetLegendaryUpgrade(2) > 0) {
            detailPanelText.text += "Slot upgrades applied to this slot are also applied to adjacent slots (" + slots[slot].GetLegendaryUpgrade(2) + ")\n";
        }
        if (slots[slot].GetLegendaryUpgrade(3) > 0) {
            detailPanelText.text += "No overheal = +1s afterimage-time on-hit (+3s on critical hit) (" + slots[slot].GetLegendaryUpgrade(3) + ")\n";
        }
        if (slots[slot].GetLegendaryUpgrade(4) > 0) {
            detailPanelText.text += "Critical hit = Avarice for 3 seconds (" + slots[slot].GetLegendaryUpgrade(4) + ")\n";
        }
        if (slots[slot].GetLegendaryUpgrade(5) > 0) {
            detailPanelText.text += "+1 permanent attack speed on-kill with critical hit (" + slots[slot].GetLegendaryUpgrade(5) + ")\n";
        }
        if (slots[slot].GetLegendaryUpgrade(6) > 0) {
            detailPanelText.text += "Critical hit = +50% critical damage buff for 3 seconds & clear status condition (" + slots[slot].GetLegendaryUpgrade(6) + ")\n";
        }
        if (slots[slot].GetLegendaryUpgrade(7) > 0) {
            detailPanelText.text += "Damage all anemic enemies on-screen on-kill and on-cast (" + slots[slot].GetLegendaryUpgrade(7) + ")\n";
        }
        if (slots[slot].GetLegendaryUpgrade(8) > 0) {
            detailPanelText.text += "Anemic Shock upon inflicting an anemic enemy with anemia (" + slots[slot].GetLegendaryUpgrade(8) + ")\n";
        }
        if (slots[slot].GetLegendaryUpgrade(9) > 0) {
            detailPanelText.text += "Bloodsucker buff for 2 seconds on-cast (" + slots[slot].GetLegendaryUpgrade(9) + ")\n";
        }
        if (slots[slot].GetLegendaryUpgrade(10) > 0) {
            detailPanelText.text += "Damaging an anemic enemy increases their damage rate speed on-hit (" + slots[slot].GetLegendaryUpgrade(10) + ")\n";
        }
        if (slots[slot].GetLegendaryUpgrade(11) > 0) {
            detailPanelText.text += "Treasure chest spawn chance += 5% of overheal (" + slots[slot].GetLegendaryUpgrade(11) + ")\n";
        }
        if (slots[slot].GetLegendaryUpgrade(12) > 0) {
            detailPanelText.text += "Slows enemy equal to 1% of overheal on-hit (" + slots[slot].GetLegendaryUpgrade(12) + ")\n";
        }
        if (slots[slot].GetLegendaryUpgrade(13) > 0) {
            detailPanelText.text += "Grants Bulwark buff on-kill (" + slots[slot].GetLegendaryUpgrade(13) + ")\n";
        }
        if (slots[slot].GetLegendaryUpgrade(14) > 0) {
            detailPanelText.text += "+1 permanent basic attack damage on-kill per 100 HP (" + slots[slot].GetLegendaryUpgrade(14) + ")\n";
        }
    }

    private void CloseAdditionalDetailPanel() {
        detailPanelText.text = "";
        artifactUpgradeDetailPanel.SetActive(false);
    }

    public void IncreaseMaxSlots() {
        switch (gameManager.GetMaxSlots()) {
            case 3:
                slot3LockImage.SetActive(false);
                slot3Group.SetActive(true);
                break;
            case 4:
                slot4LockImage.SetActive(false);
                slot4Group.SetActive(true);
                break;
            case 5:
                slot5LockImage.SetActive(false);
                slot5Group.SetActive(true);
                break;
        }
    }

    //LEFT SIDE LEFT SIDE LEFT SIDE LEFT SIDE LEFT SIDE LEFT SIDE LEFT SIDE
    public void Left() {    //swap screen to left view
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

    private void PopulateBuffsDebuffs() {   //looks at the buff manager and grabs all of the buffs and debuffs currently affecting Jamp
        for (int i = 0; i < 5 ; i++) {
            if (buffManager.DoesBuffExist(i)) {
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
            } else {
                activeBuffs[i].sprite = buffIcons[0];
                activeBuffDuration[i].text = "";
                activeBuffTooltipText[i].text = "";
                buffTooltipInteractable[i] = false;
            }

            if (buffManager.DoesDebuffExist(i)) {
                (string debuffType, float severity, float duration) = buffManager.GetDebuffInfo(i);
                switch (debuffType) {
                    case "slow":
                        activeDebuffs[i].sprite = debuffIcons[1];
                        activeDebuffTooltipText[i].text = "Reduces movement speed by " + ((1 - severity)* 100) + "%.";
                        break;
                    case "bleed":
                        activeDebuffs[i].sprite = debuffIcons[2];
                        activeDebuffTooltipText[i].text = "Overhealth drains " + (severity * 100) + "% faster.";
                        break;
                    case "anemia":
                        activeDebuffs[i].sprite = debuffIcons[3];
                        activeDebuffTooltipText[i].text = "Lose " + (severity * 100) + "% of your current health per second.";
                        break;
                }
                activeDebuffDuration[i].text = duration.ToString("f1") + "s";
                debuffTooltipInteractable[i] = true;
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
        }
    }

    public void HideBuffTooltip(int buff) {
        activeBuffTooltip[buff].SetActive(false);
    }

    public void ShowDebuffTooltip(int debuff) {
        if (debuffTooltipInteractable[debuff]) {
            activeDebuffTooltip[debuff].SetActive(true);
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
        slotDetailButtons[0].onClick.AddListener(() => GenerateAdditionalDetailPanel(0));
        slotDetailButtons[1].onClick.AddListener(() => GenerateAdditionalDetailPanel(1));
        slotDetailButtons[2].onClick.AddListener(() => GenerateAdditionalDetailPanel(2));
        slotDetailButtons[3].onClick.AddListener(() => GenerateAdditionalDetailPanel(3));
        slotDetailButtons[4].onClick.AddListener(() => GenerateAdditionalDetailPanel(4));
        closeArtifactUpgradeDetailPanel.onClick.AddListener(() => CloseAdditionalDetailPanel());
    }
}