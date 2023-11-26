using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UpgradeManager : MonoBehaviour
{
    //back-end stuff
    private int[] commonUpgradePool = new int[13];
    private int[] rareUpgradePool = new int[13];
    private int[] legendaryUpgradePool = new int[13];
    private int[] upgradeRarities = new int[3]; //displays 3 upgrades, this is for choosing their rarities. 0 = common // 1 = rare // 2 = legendary
    private int[] upgradeSelection = new int[3]; //upgrade ID chosen for each upgrade displayed up above.
    private bool commonUpgradesAvailable, rareUpgradesAvailable, legendaryUpgradeAvailable, allUpgradesTaken; //for later
    private int upgradeSelected, slotSelected; //determined which one that is clicked on in the game menu (goes from 0-2 for upgradeselected and 0-4 for slot selected)
    [SerializeField] private Slot[] slots = new Slot[2];
    //clickables
    [SerializeField] private GameObject upgrade1, upgrade2, upgrade3;
    [Space]
    [SerializeField] private Button[] upgradeButtons = new Button[3]; //buttons to click to select desired upgrade
    [SerializeField] private Button[] slotButtons = new Button[2]; //buttons to click to select desired slot (add 3-5 later)
    [SerializeField] private Button confirmationButton; //click this after upgrade and slot to apply are selected
    [Space]
    //the following are for the interactable, visual stuff
    [SerializeField] private Image[] upgradeRarityBG = new Image[3]; //background of the actual clickable upgrade image that you select
    [SerializeField] private Image[] upgradeIcon = new Image[3];    //the image associated with each upgrade (eg. sword for dmg boost)
    [SerializeField] private TextMeshProUGUI[] upgradeText = new TextMeshProUGUI[3]; //the text that describes what the upgrade does
    [Space]
    //the following are to be held for later use, not necessarily visible
    [SerializeField] private Sprite[] upgradeRarityImage = new Sprite[3]; //background image for different rarities
    [SerializeField] private Sprite[] commonIconPool = new Sprite[3]; //image associated w/ upgrades
    [SerializeField] private Sprite[] rareIconPool = new Sprite[3];
    [SerializeField] private Sprite[] legendaryIconPool = new Sprite[3];
    [SerializeField] private string[] commonUpgradeText = new string[3]; //text that describes what the upgrade does
    [SerializeField] private string[] rareUpgradeText = new string[3];
    [SerializeField] private string[] legendaryUpgradeText = new string[3];

    void Start()    //initialize the buttons, and capacity for each upgrade at the beginning of the game.
    {
        InitializeButtons();

        for (int i = 0; i < 13; i++) {
            commonUpgradePool[i] = 5;
            rareUpgradePool[i] = 3;
            legendaryUpgradePool[i] = 1;
        }
        commonUpgradesAvailable = true;
        rareUpgradesAvailable = true;
        legendaryUpgradeAvailable = true;
        allUpgradesTaken = false;
        upgradeSelected = -1;
        slotSelected = -1;

        Setup("legendary");
    }

    private void Update() {
        if (upgradeSelected != -1 && slotSelected != -1) {
            confirmationButton.interactable = true;
        } else {
            confirmationButton.interactable = false;
        }
    }

    public void Setup(string type) {
        for (int i = 0; i < 3; i++) {       //this logic will need to be re-written for if the upgrades run out
            if (type == "normal") {
                int rarity = Random.Range(0,101);
                if (rarity > 60) {
                    upgradeRarities[i] = 1; //give rare upgrade
                    upgradeRarityBG[i].sprite = upgradeRarityImage[1];
                } else {
                    upgradeRarities[i] = 0; //give common upgrade
                    upgradeRarityBG[i].sprite = upgradeRarityImage[0];
                }
            }

            if (type == "legendary") {
            upgradeRarities[0] = 2; upgradeRarities[1] = 2; upgradeRarities[2] = 2;
            upgradeRarityBG[i].sprite = upgradeRarityImage[2];
            }
        }

        for (int i = 0; i < 3; i++) {   //checks to see if the specific upgrade is empty. if so, reroll until you get one that isn't
            int roll = Random.Range(0,3); //for now, only 3 possible upgrades. should update to 13 later on
            switch (upgradeRarities[i]) {
                case 0: //common
                    while (commonUpgradePool[roll] == 0) {
                        roll = Random.Range(0,3);
                    }
                    upgradeSelection[i] = roll;
                    upgradeIcon[i].sprite = commonIconPool[roll];
                    upgradeText[i].text = commonUpgradeText[roll];
                    break;
                case 1: //rare
                    while (rareUpgradePool[roll] == 0) {
                        roll = Random.Range(0,3);
                    }
                    upgradeSelection[i] = roll;
                    upgradeIcon[i].sprite = rareIconPool[roll];
                    upgradeText[i].text = rareUpgradeText[roll];
                    break;
                case 2: //legendary
                    while (legendaryUpgradePool[roll] == 0) {
                        roll = Random.Range(0,3);
                    }
                    upgradeSelection[i] = roll;
                    upgradeIcon[i].sprite = legendaryIconPool[roll];
                    upgradeText[i].text = legendaryUpgradeText[roll];
                    break;
            }
        }
    }

    public void ClickedUpgrade(int position) { //select an upgrade
        Debug.Log("Clicked upgrade " + position);
        upgradeSelected = position;
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

    public void ClickedSlot(int slot) { //select a slot
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

    public void Finish() { //communicate with slot and add an upgrade based on the slot and upgrade chosen
        string rarity = "";
        //apply to slot selected
        switch(upgradeRarities[upgradeSelected]) {
            case 0: rarity = "common"; break;
            case 1: rarity = "rare"; break;
            case 2: rarity = "legendary"; break;
        }
        slots[slotSelected].ApplySlotUpgrade(rarity, upgradeSelection[upgradeSelected]);

        //subtract from the available upgrades
        switch(upgradeRarities[upgradeSelected]) {
            case 0: commonUpgradePool[upgradeSelected] -= 1; break;
            case 1: rareUpgradePool[upgradeSelected] -= 1; break;
            case 2: legendaryUpgradePool[upgradeSelected] -= 1; break;
        }

        //reset all the buttons
        upgradeButtons[0].interactable = true;
        upgradeButtons[1].interactable = true;
        upgradeButtons[2].interactable = true;
        slotButtons[0].interactable = true;
        slotButtons[1].interactable = true;

        //reset both the slots and the upgrades to be unselected
        upgradeSelected = -1;
        slotSelected = -1;
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
    0. Damage +10%
    1. Size +10%
    2. Duration +20%

    Rare:
    0. Damage +20%
    1. Size +15%
    2. Duration +30%

    Legendary:
    0. Damage +30%
    1. Size +20%
    2. Duration +50%
    */
}
