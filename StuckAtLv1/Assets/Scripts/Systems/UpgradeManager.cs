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
    [Space]
    //the following are for the interactable, visual stuff
    [SerializeField] private Image[] upgradeRarityBG = new Image[3];        //background of the actual clickable upgrade image that you select
    [SerializeField] private Image[] upgradeIcon = new Image[3];            //the image associated with each upgrade (eg. sword for dmg boost)
    [SerializeField] private TextMeshProUGUI[] upgradeText = new TextMeshProUGUI[3]; //the text that describes what the upgrade does
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

    public void Shop() {
        fromShop = true;
    }

    private void InitializeButtons() {
        upgradeButtons[0].onClick.AddListener(() => ClickedUpgrade(0));
        upgradeButtons[1].onClick.AddListener(() => ClickedUpgrade(1));
        upgradeButtons[2].onClick.AddListener(() => ClickedUpgrade(2));
        slotButtons[0].onClick.AddListener(() => ClickedSlot(0));
        slotButtons[1].onClick.AddListener(() => ClickedSlot(1));
        confirmationButton.onClick.AddListener(() => Finish());
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
