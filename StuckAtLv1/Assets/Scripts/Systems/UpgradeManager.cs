using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UpgradeManager : MonoBehaviour
{
    private readonly int DEFAULT_REROLL_COST = 100;
    //back-end stuff
    private int[] commonUpgradePool = new int[15];
    private int[] rareUpgradePool = new int[15];
    private int[] legendaryUpgradePool = new int[15];
    private int[] upgradeRarities = new int[3];         //rarity storage for the 4 upgrades that are available. (rarity: 0-common, 1-rare, 2-legendary)
    private int[] upgradeSelection = new int[3];        //upgrade ID storage for the 4 upgrades that are available.

    //private bool commonUpgradesAvailable, rareUpgradesAvailable, legendaryUpgradeAvailable, allUpgradesTaken;   //for later use (maybe?)

    private int upgradePositionSelected, slotSelected;                      //determined which one that is clicked on in the game menu (goes from 0-2 for upgradeselected and 0-4 for slot selected)
    [SerializeField] private Slot[] slots = new Slot[5];                    //slots
    //in-game buttons
    [Space]
    [SerializeField] private Button[] upgradeButtons = new Button[3];       //buttons to click to select desired upgrade
    [SerializeField] private Button[] slotButtons = new Button[5];          //buttons to click to select desired slot
    [SerializeField] private Button confirmationButton;                     //click this after upgrade and slot to apply are selected
    //the following are for the interactable, visual stuff
    [SerializeField] private Button helpButton;                             //guide button to explain upgrade menu
    [SerializeField] private Button rerollButton;                           //get another upgrade attempt
    [SerializeField] private GameObject helpMenu;                           //help menu gameobject
    [SerializeField] private Image helpMenuImage;
    [SerializeField] private TextMeshProUGUI helpMenuText;                  //help menu image and text fields contained within the help menu gameobject
    [SerializeField] private Sprite[] helpMenuSprites;
    [TextArea(5,5)]
    [SerializeField] private string[] helpMenuStrings;                      //help menu image and text population stored for the help menu
    [SerializeField] private Button closeHelpMenu, advanceHelpMenu, backHelpMenu; //button stuff for the help menu
    [Space]
    private int helpMenuValue; private bool helpMenuSeen;
    //help menu stuff
    [SerializeField] private Image[] upgradeRarityBG = new Image[3];        //background of the actual clickable upgrade image that you select
    [SerializeField] private Image[] upgradeIcon = new Image[3];            //the image associated with each upgrade (eg. sword for dmg boost)
    [SerializeField] private TextMeshProUGUI[] upgradeText = new TextMeshProUGUI[3]; //the text that describes what the upgrade does
    [SerializeField] private TextMeshProUGUI[] weightText = new TextMeshProUGUI[5]; //text that shows slot weight
    [SerializeField] private TextMeshProUGUI equilibriumText;
    [Space]
    //the following are to be held for storage
    [SerializeField] private Sprite[] upgradeRarityImage = new Sprite[3];   //background image for different rarities
    [SerializeField] private Sprite[] commonIconPool = new Sprite[15];       //image associated w/ upgrades
    [SerializeField] private Sprite[] rareIconPool = new Sprite[15];         
    [SerializeField] private Sprite[] legendaryIconPool = new Sprite[15];   
    [TextArea(5,5)]
    [SerializeField] private string[] commonUpgradeText = new string[15];    //text that describes what the upgrade does 
    [TextArea(5,5)]
    [SerializeField] private string[] rareUpgradeText = new string[15];      
    [TextArea(5,5)]
    [SerializeField] private string[] legendaryUpgradeText = new string[15]; 
    [SerializeField] private Character character;
    [SerializeField] private TextMeshProUGUI rerollButtonText, characterMoney;
    [Space]
    //other
    [SerializeField] private GameManager notify;
    private bool fromShop; 
    private bool fromBoss; 
    private int maxSlots;
    private string upgradeType; private int rerollCost; 
    void Awake()    //initialize the buttons, and capacity for each upgrade at the beginning of the game.
    {
        maxSlots = 2;
        InitializeButtons();

        for (int i = 0; i < 15; i++) {
            commonUpgradePool[i] = 5;
            rareUpgradePool[i] = 3;
            legendaryUpgradePool[i] = 1;
        }
        upgradePositionSelected = -1;
        slotSelected = -1;
    }

    private void Update() {
        if (upgradePositionSelected != -1 && slotSelected != -1) {
            confirmationButton.interactable = true;
        } else {
            confirmationButton.interactable = false;
        }
    }

    private void OnEnable() {
        DisplayWeight();

        if (!helpMenuSeen) {
            OpenHelpMenu();
            helpMenuSeen = true;
        }

        rerollCost = DEFAULT_REROLL_COST;
        characterMoney.text = character.money.ToString();
        rerollButtonText.text = "Re-roll | " + DEFAULT_REROLL_COST + "g";
        if (character.money >= rerollCost) {
            rerollButton.interactable = true;
        } else {
            rerollButton.interactable = false;
        }
    }

    public void Setup(string type) {
        upgradeText[0].gameObject.SetActive(false); upgradeText[0].text = "";
        upgradeText[1].gameObject.SetActive(false); upgradeText[1].text = "";
        upgradeText[2].gameObject.SetActive(false); upgradeText[2].text = "";
          
        //Called from the gamemanager with a type of reward, dependent on type of battle fought. Should be called every time we go into this menu.
        //Sets up the upgrades that are displayed in-game. This logic will need to be re-written for if the upgrades run out entirely, although idk if that'll be possible.
        for (int i = 0; i < 3; i++) {       
            if (type == "normal") {
                upgradeType = "normal";
                int rarity = Random.Range(0, 101);
                if (rarity > 65) {          //for now, 30% chance to get a rare upgrade, can be tweaked
                    upgradeRarities[i] = 1; //give rare upgrade
                    upgradeRarityBG[i].sprite = upgradeRarityImage[1];  //Update the background for the appropriate rarity
                } else {
                    upgradeRarities[i] = 0; //give common upgrade
                    upgradeRarityBG[i].sprite = upgradeRarityImage[0];
                }
            }

            if (type == "legendary") {          //legendary upgrades are given after minibosses and bosses, sets all upgrades to legendary
                upgradeType = "legendary";
                upgradeRarities[0] = 2; upgradeRarities[1] = 2; upgradeRarities[2] = 2;
                upgradeRarityBG[i].sprite = upgradeRarityImage[2];
            }
        }

        for (int i = 0; i < 3; i++) {           //checks to see if the specific upgrade is empty. if so, reroll until you get one that isn't
            int roll = Random.Range(0, 15);
            switch (upgradeRarities[i]) {
                case 0:     //common
                     do {
                        if (i == 1 && upgradeRarities[0] == 0) {                                //reroll if you get dupes
                            while (roll == upgradeSelection[0]) {
                                roll = Random.Range(0, 15);
                            }
                        }

                        if (i == 2 && (upgradeRarities[0] == 0 || upgradeRarities[1] == 0)) {   //reroll if you get dupes
                            while ((upgradeRarities[0] == 0 && roll == upgradeSelection[0]) || (upgradeRarities[1] == 0 && roll == upgradeSelection[1])) {
                                roll = Random.Range(0, 15);
                            }
                        }
                    } while (commonUpgradePool[roll] == 0);
                    upgradeSelection[i] = roll;
                    upgradeIcon[i].sprite = commonIconPool[roll];
                    upgradeText[i].SetText(commonUpgradeText[roll]);
                    break;

                case 1:     //rare
                    do {
                        if (i == 1 && upgradeRarities[0] == 1) {                                //reroll if you get dupes
                            while (roll == upgradeSelection[0]) {
                                roll = Random.Range(0, 15);
                            }
                        }

                        if (i == 2 && (upgradeRarities[0] == 1 || upgradeRarities[1] == 1)) {   //reroll if you get dupes
                            while ((upgradeRarities[0] == 1 && roll == upgradeSelection[0]) || (upgradeRarities[1] == 1 && roll == upgradeSelection[1])) {
                                roll = Random.Range(0, 15);
                            }
                        }
                    } while (rareUpgradePool[roll] == 0);
                    upgradeSelection[i] = roll;
                    upgradeIcon[i].sprite = rareIconPool[roll];
                    upgradeText[i].SetText(rareUpgradeText[roll]);
                    break;

                case 2:     //legendary
                    do {
                        if (i == 1) { 
                            while (roll == upgradeSelection[0]) {                                   //reroll if you get dupes
                                roll = Random.Range(0, 15);
                            }
                        }
                        
                        if (i == 2) {
                            while (roll == upgradeSelection[0] || roll == upgradeSelection[1]) {    //reroll if you get dupes
                                roll = Random.Range(0, 15);
                            }
                        }
                    } while (legendaryUpgradePool[roll] == 0);
                    upgradeSelection[i] = roll;
                    upgradeIcon[i].sprite = legendaryIconPool[roll];
                    upgradeText[i].SetText(legendaryUpgradeText[roll]);
                    break;
            }
        }
        upgradeText[0].ForceMeshUpdate(); 
        upgradeText[1].ForceMeshUpdate(); 
        upgradeText[2].ForceMeshUpdate();
        upgradeText[0].gameObject.SetActive(true);
        upgradeText[1].gameObject.SetActive(true);
        upgradeText[2].gameObject.SetActive(true);
    }

    public void ClickedUpgrade(int position) {      
        //Select an upgrade. Selected button stays depressed. Updates the upgradeSelected variable
        Debug.Log("Clicked upgrade " + position);
        upgradePositionSelected = position;
        switch (position) {
            case 0:
                upgradeButtons[0].interactable = false;
                upgradeButtons[1].interactable = true;
                upgradeButtons[2].interactable = true;
                break;
            case 1:
                upgradeButtons[0].interactable = true;
                upgradeButtons[1].interactable = false;
                upgradeButtons[2].interactable = true;
                break;
            case 2:
                upgradeButtons[0].interactable = true;
                upgradeButtons[1].interactable = true;
                upgradeButtons[2].interactable = false;
                break;
        }
    }

    public void ClickedSlot(int slot) {     
        //select a slot, updates the slotSelected variable
        Debug.Log("Clicked slot " + slot);
        slotSelected = slot;
        for (int i = 0; i < maxSlots; i++) {
            slotButtons[i].interactable = true;
        }
        slotButtons[slot].interactable = false;
    }

    public void DisplayWeight() {
        //slot weight calculation here
        for (int i = 0; i < maxSlots; i++) {
            weightText[i].text = notify.GetWeight(i+1).ToString();

            if (notify.GetWeight(i+1) == 0) {weightText[i].color = Color.white;}
            if (notify.GetWeight(i+1) >= 1 && notify.GetWeight(i+1) <= 3) {weightText[i].color = new Color32(102, 175, 255, 255);} //lightblue
            if (notify.GetWeight(i+1) >= 4 && notify.GetWeight(i+1) <= 6) {weightText[i].color = new Color32(51, 255, 51, 255);} //green
            if (notify.GetWeight(i+1) >= 7 && notify.GetWeight(i+1) <= 9) {weightText[i].color = new Color32(255, 255, 51, 255);} //yellow
            if (notify.GetWeight(i+1) >= 10) {weightText[i].color = new Color32(255, 0, 0, 255);} //red

            if (notify.GetEquilibriumCheck()) {
                equilibriumText.gameObject.SetActive(true);
            } else {
                equilibriumText.gameObject.SetActive(false);
            }
        }
    }

    public void Shop() {
        fromShop = true;
    }

    public void FromBoss() { //boss or miniboss
        fromBoss = true;
    }

    private void UpdateHelpMenu(string command) {
        switch(command) {
            case "advance":
                helpMenuValue++;
                break;
            case "back":
                helpMenuValue--;
                break;
        }

        helpMenuImage.sprite = helpMenuSprites[helpMenuValue];
        helpMenuText.text = helpMenuStrings[helpMenuValue];

        if (helpMenuValue == 0) {
            backHelpMenu.gameObject.SetActive(false);
        } else {
            backHelpMenu.gameObject.SetActive(true);
        }

        if (helpMenuValue == 6) {
            advanceHelpMenu.gameObject.SetActive(false);
            closeHelpMenu.gameObject.SetActive(true);
        } else {
            advanceHelpMenu.gameObject.SetActive(true);
            closeHelpMenu.gameObject.SetActive(false);
        }
    }
    
    private void OpenHelpMenu() {
        helpMenu.SetActive(true);
    }

    private void CloseHelpMenu() {
        helpMenuImage.sprite = helpMenuSprites[0];
        helpMenuText.text = helpMenuStrings[0];
        helpMenuValue = 0;
        backHelpMenu.gameObject.SetActive(false);
        advanceHelpMenu.gameObject.SetActive(true);
        helpMenu.SetActive(false);
    }

    private void RerollButton() {       //reroll button, cost increases every time it's pressed!
        character.money -= rerollCost;
        characterMoney.text = character.money.ToString();
        rerollCost *= 2;
        rerollButtonText.text = "Re-roll | " + rerollCost + "g";
        if (character.money >= rerollCost) {
            rerollButton.interactable = true;
        } else {
            rerollButton.interactable = false;
        }
        Setup(upgradeType);
    }

    public void Finish() { 
        /*
        Communicate with appropriate slot, and add an upgrade based on the slot and upgrade chosen in this interface.

        This method will grab the rarity from the rarity assigned to each upgrade shown, from upgradeRarities[0-2]. 
        Then, populates an appropriate string based on the rarity.
        Then, grabs the upgrade ID assigned to each upgrade shown, from upgradeSelection[0-2].
        Then, using the slot selected from slotSelected, it calls the respective slot's ApplySlotUpgrade() and lets it assign itself the upgrade using the rarity strng and upgrade selected previously.

        As such, the slot upgrade pool is universal throughout the game.
        */
        
        string rarity = "";
        //apply to slot selected
        switch(upgradeRarities[upgradePositionSelected]) {
            case 0: rarity = "common"; break;
            case 1: rarity = "rare"; break;
            case 2: rarity = "legendary"; break;
        }

        if (slots[slotSelected].GetLegendaryUpgrade(2) > 0) {       //legendary 2
            if (slotSelected >= 1 ) {
                slots[slotSelected - 1].ApplySlotUpgrade(rarity, upgradeSelection[upgradePositionSelected]);
                notify.AdjustSlotUpgradeCounter(slots[slotSelected - 1].Identity);
            }

            if (slotSelected <= 3) {
                if (slots[slotSelected + 1].isActiveAndEnabled) {
                    slots[slotSelected + 1].ApplySlotUpgrade(rarity, upgradeSelection[upgradePositionSelected]);
                    notify.AdjustSlotUpgradeCounter(slots[slotSelected + 1].Identity);
                }
            }

            slots[slotSelected].ApplySlotUpgrade(rarity, upgradeSelection[upgradePositionSelected]);
            notify.AdjustSlotUpgradeCounter(slots[slotSelected].Identity);
        } else {
            slots[slotSelected].ApplySlotUpgrade(rarity, upgradeSelection[upgradePositionSelected]);
            notify.AdjustSlotUpgradeCounter(slots[slotSelected].Identity);
        }

        //subtract from the available upgrades
        switch(upgradeRarities[upgradePositionSelected]) {
            case 0: commonUpgradePool[upgradePositionSelected] -= 1; break;
            case 1: rareUpgradePool[upgradePositionSelected] -= 1; break;
            case 2: legendaryUpgradePool[upgradePositionSelected] -= 1; break;
        }

        //reset all the buttons
        upgradeButtons[0].interactable = true;
        upgradeButtons[1].interactable = true;
        upgradeButtons[2].interactable = true;

        for (int i = 0; i < maxSlots; i++) {
            slotButtons[i].interactable = true;
        }

        //reset both the slots and the upgrades to be unselected
        upgradePositionSelected = -1;
        slotSelected = -1;

        //go back to the map, or shop
        if (fromShop) {
            fromShop = false;
            notify.ReceiveCommand("shop");
        }  else if (fromBoss) {
            fromBoss = false;
            notify.ReceiveCommand("dialogue");
        } else {
            notify.ReceiveCommand("map");
        }
    }

    private void InitializeButtons() {
        upgradeButtons[0].onClick.AddListener(() => ClickedUpgrade(0));
        upgradeButtons[1].onClick.AddListener(() => ClickedUpgrade(1));
        upgradeButtons[2].onClick.AddListener(() => ClickedUpgrade(2));

        slotButtons[2].gameObject.SetActive(true);
        slotButtons[3].gameObject.SetActive(true);
        slotButtons[4].gameObject.SetActive(true);

        slotButtons[0].onClick.AddListener(() => ClickedSlot(0));
        slotButtons[1].onClick.AddListener(() => ClickedSlot(1));
        slotButtons[2].onClick.AddListener(() => ClickedSlot(2));
        slotButtons[3].onClick.AddListener(() => ClickedSlot(3));
        slotButtons[4].onClick.AddListener(() => ClickedSlot(4));

        slotButtons[2].gameObject.SetActive(false);
        slotButtons[3].gameObject.SetActive(false);
        slotButtons[4].gameObject.SetActive(false);

        confirmationButton.onClick.AddListener(() => Finish());
        helpButton.onClick.AddListener(() => OpenHelpMenu());
        closeHelpMenu.onClick.AddListener(() => CloseHelpMenu());
        advanceHelpMenu.onClick.AddListener(() => UpdateHelpMenu("advance"));
        backHelpMenu.onClick.AddListener(() => UpdateHelpMenu("back"));
        rerollButton.onClick.AddListener(() => RerollButton());
    }

    public void IncreaseMaxSlots() {
        maxSlots++;
        slotButtons[maxSlots-1].gameObject.SetActive(true);

        switch (maxSlots) {
            //temp method
            case 3:
                slotButtons[0].gameObject.transform.Translate(-1.5f, 0, 0);
                slotButtons[1].gameObject.transform.Translate(-1.5f, 0, 0);
                break;
            case 4:
                slotButtons[0].gameObject.transform.Translate(-1.5f, 0, 0);
                slotButtons[1].gameObject.transform.Translate(-1.5f, 0, 0);
                slotButtons[2].gameObject.transform.Translate(-1.5f, 0, 0); 
                break;
            case 5:
                slotButtons[0].gameObject.transform.Translate(-1.5f, 0, 0);
                slotButtons[1].gameObject.transform.Translate(-1.5f, 0, 0);
                slotButtons[2].gameObject.transform.Translate(-1.5f, 0, 0);
                slotButtons[3].gameObject.transform.Translate(-1.5f, 0, 0);
                break;
        }
        notify.CalculateWeight();
        DisplayWeight();
        Debug.Log("Upgrade Manager: Maximum slots increased to " + maxSlots + ". Process successful.");
    }
}
