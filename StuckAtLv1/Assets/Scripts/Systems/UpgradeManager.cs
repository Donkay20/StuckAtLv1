using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UpgradeManager : MonoBehaviour
{
    //back-end stuff
    private int[] commonUpgradePool = new int[13];
    private int[] rareUpgradePool = new int[13];
    private int[] legendaryUpgradePool = new int[13];
    private int[] upgradeRarities = new int[3];         //rarity storage for the 4 upgrades that are available. (rarity: 0-common, 1-rare, 2-legendary)
    private int[] upgradeSelection = new int[3];        //upgrade ID storage for the 4 upgrades that are available. (goes from 0-2 now but should be updated for 0-12 later)
    //private bool commonUpgradesAvailable, rareUpgradesAvailable, legendaryUpgradeAvailable, allUpgradesTaken;   //for later use
    private int upgradePositionSelected, slotSelected;                              //determined which one that is clicked on in the game menu (goes from 0-2 for upgradeselected and 0-4 for slot selected)
    [SerializeField] private Slot[] slots = new Slot[2];                    //UPDATE TO 5 LATER ON
    //in-game buttons
    [SerializeField] private GameObject upgrade1, upgrade2, upgrade3;       //unsure if using this later or not, just grabs the whole gameobject
    [Space]
    [SerializeField] private Button[] upgradeButtons = new Button[3];       //buttons to click to select desired upgrade
    [SerializeField] private Button[] slotButtons = new Button[2];          //buttons to click to select desired slot (UPDATE TO 5 LATER ON)
    [SerializeField] private Button confirmationButton;                     //click this after upgrade and slot to apply are selected
    //the following are for the interactable, visual stuff
    [SerializeField] private Button helpButton;                             //guide button to explain upgrade menu
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
    [SerializeField] private TextMeshProUGUI[] weightText = new TextMeshProUGUI[5]; //text that shows slot weight (UPDATE TO 5 LATER ON)
    [SerializeField] private TextMeshProUGUI equilibriumText;
    [Space]
    //the following are to be held for storage
    [SerializeField] private Sprite[] upgradeRarityImage = new Sprite[3];   //background image for different rarities
    [SerializeField] private Sprite[] commonIconPool = new Sprite[4];       //image associated w/ upgrades(UPDATE TO 13 LATER ON)
    [SerializeField] private Sprite[] rareIconPool = new Sprite[4];         //(UPDATE TO 13 LATER ON)
    [SerializeField] private Sprite[] legendaryIconPool = new Sprite[4];    //(UPDATE TO 13 LATER ON)
    [SerializeField] private string[] commonUpgradeText = new string[4];    //text that describes what the upgrade does(UPDATE TO 13 LATER ON)
    [SerializeField] private string[] rareUpgradeText = new string[4];      //(UPDATE TO 13 LATER ON)
    [SerializeField] private string[] legendaryUpgradeText = new string[4]; //(UPDATE TO 13 LATER ON)
    [Space]
    //other
    [SerializeField] private GameManager notify;
    private bool fromShop;

    void Awake()    //initialize the buttons, and capacity for each upgrade at the beginning of the game.
    {
        InitializeButtons();

        for (int i = 0; i < 13; i++) {
            commonUpgradePool[i] = 5;
            rareUpgradePool[i] = 3;
            legendaryUpgradePool[i] = 1;
        }
        //commonUpgradesAvailable = true;
        //rareUpgradesAvailable = true;
        //legendaryUpgradeAvailable = true;
        //allUpgradesTaken = false;

        upgradePositionSelected = -1;
        slotSelected = -1;
    }

    private void Update() { //todo: move this out of the update method (later)
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
    }

    public void Setup(string type) {    
        //Called from the gamemanager with a type of reward, dependent on type of battle fought. Should be called every time we go into this menu.
        //Sets up the upgrades that are displayed in-game. This logic will need to be re-written for if the upgrades run out entirely, although idk if that'll be possible.
        for (int i = 0; i < 3; i++) {       
            if (type == "normal") {
                int rarity = Random.Range(0,101);
                if (rarity > 70) {          //for now, 30% chance to get a rare upgrade, can be tweaked
                    upgradeRarities[i] = 1; //give rare upgrade
                    upgradeRarityBG[i].sprite = upgradeRarityImage[1];  //Update the background for the appropriate rarity
                } else {
                    upgradeRarities[i] = 0; //give common upgrade
                    upgradeRarityBG[i].sprite = upgradeRarityImage[0];
                }
            }

            if (type == "legendary") {      //legendary upgrades are given after minibosses and bosses, sets all upgrades to legendary
            upgradeRarities[0] = 2; upgradeRarities[1] = 2; upgradeRarities[2] = 2;
            upgradeRarityBG[i].sprite = upgradeRarityImage[2];
            }
        }

        for (int i = 0; i < 3; i++) {       
            //checks to see if the specific upgrade is empty. if so, reroll until you get one that isn't
            int roll = Random.Range(0,4);   
            //for now, only 4 possible upgrades. (UPDATE TO 13 LATER ON)
            switch (upgradeRarities[i]) {
                case 0: //common
                    while (commonUpgradePool[roll] == 0) {
                        roll = Random.Range(0,4);   //UPDATE TO 13 LATER ON
                    }
                    upgradeSelection[i] = roll;
                    upgradeIcon[i].sprite = commonIconPool[roll];
                    upgradeText[i].text = commonUpgradeText[roll];
                    break;
                case 1: //rare
                    while (rareUpgradePool[roll] == 0) {
                        roll = Random.Range(0,4);   //UPDATE TO 13 LATER ON
                    }
                    upgradeSelection[i] = roll;
                    upgradeIcon[i].sprite = rareIconPool[roll];
                    upgradeText[i].text = rareUpgradeText[roll];
                    break;
                case 2: //legendary
                    while (legendaryUpgradePool[roll] == 0) {
                        roll = Random.Range(0,4);   //UPDATE TO 13 LATER ON
                    }
                    upgradeSelection[i] = roll;
                    upgradeIcon[i].sprite = legendaryIconPool[roll];
                    upgradeText[i].text = legendaryUpgradeText[roll];
                    break;
            }
        }
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
        //select a slot, updates the slotSelected variable (UPDATE TO 5 LATER ON)
        Debug.Log("Clicked slot " + slot);
        slotSelected = slot;
        switch (slot) {
            case 0:
                slotButtons[0].interactable = false;
                slotButtons[1].interactable = true;
                break;
            case 1:
                slotButtons[0].interactable = true;
                slotButtons[1].interactable = false;
                break;
        }
    }

    public void DisplayWeight() {
        for (int i = 0; i < notify.GetMaxSlots(); i++) {
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

    public void Finish() { 
        //Communicate with appropriate slot, and add an upgrade based on the slot and upgrade chosen in this interface.

        /*
        What's happening is that it's grabbing the rarity from the rarity assigned to each upgrade shown, from upgradeRarities[0-2]. Then, populates an appropriate string based on the rarity.
        It then grabs the upgrade ID assigned to each upgrade shown, from upgradeSelection[0-3 (UPDATE TO 0-12 LATER)].
        Then, using the slot selected from slotSelected, it calls the respective slot's ApplySlotUpgrade() and lets it assign itself the upgrade using the rarity strng and upgrade selected previously.

        Because of this, the slot upgrade pool must be universal throughout the game.
        */
        
        string rarity = "";
        //apply to slot selected
        switch(upgradeRarities[upgradePositionSelected]) {
            case 0: rarity = "common"; break;
            case 1: rarity = "rare"; break;
            case 2: rarity = "legendary"; break;
        }

        slots[slotSelected].ApplySlotUpgrade(rarity, upgradeSelection[upgradePositionSelected]);
        notify.AdjustSlotUpgradeCounter(slots[slotSelected].Identity);

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
        slotButtons[0].interactable = true;
        slotButtons[1].interactable = true;

        //reset both the slots and the upgrades to be unselected
        upgradePositionSelected = -1;
        slotSelected = -1;

        //go back to the map, or shop
        if (fromShop) {
            fromShop = false;
            notify.ReceiveCommand("shop");
        } else {
            notify.ReceiveCommand("map");
        }
    }

    private void InitializeButtons() {
        upgradeButtons[0].onClick.AddListener(() => ClickedUpgrade(0));
        upgradeButtons[1].onClick.AddListener(() => ClickedUpgrade(1));
        upgradeButtons[2].onClick.AddListener(() => ClickedUpgrade(2));
        slotButtons[0].onClick.AddListener(() => ClickedSlot(0));
        slotButtons[1].onClick.AddListener(() => ClickedSlot(1));
        confirmationButton.onClick.AddListener(() => Finish());
        helpButton.onClick.AddListener(() => OpenHelpMenu());
        closeHelpMenu.onClick.AddListener(() => CloseHelpMenu());
        advanceHelpMenu.onClick.AddListener(() => UpdateHelpMenu("advance"));
        backHelpMenu.onClick.AddListener(() => UpdateHelpMenu("back"));
    }

    /*
    List of upgrades (demo):
    Common: 
    0. Damage +20%
    1. Size +20%
    2. Duration +20%
    3. Overheal +5

    Rare:
    0. Damage +40%
    1. Size +30%
    2. Duration +40%
    3. Overheal +7

    Legendary:
    0. Damage +60%
    1. Size +40%
    2. Duration +60%
    3. Overheal +10
    */
}
