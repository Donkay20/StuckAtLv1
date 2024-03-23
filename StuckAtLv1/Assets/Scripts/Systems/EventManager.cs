using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EventManager : MonoBehaviour
{
    [SerializeField] private GameManager manager;
    [SerializeField] private MapManager map;
    [SerializeField] private CombatManager combat;
    [SerializeField] private Character player;
    [SerializeField] private Movement playerMove;
    //backend stuff
    [SerializeField] private Event[] ruinsEvents, forestEvents, sewerEvents, abyssEvents;
    [SerializeField] private TextMeshProUGUI title, nameText, dialogueText, button1Text, button2Text, button3Text, health, afterimages, money;
    [SerializeField] private Button button1, button2, button3, advanceButton;
    [SerializeField] private Image speaker, background;
    [SerializeField] private Sprite def, jamp, oldMan, lonelyGhost, angryGhost, oldShadow, ghostShadow, ash, fairy, wolf, tree;
    [SerializeField] private Sprite ruinsBG, forestBG, sewerBG, abyssBG;
    private Event selectedEvent; //chosen event
    private string[] dialogue, names, options; private int[] skip, outcome; //copy from the chosen event
    private int messageCounter, numberOfButtons, outcomeDecided; //messagecounter is to track what line we're on, where numberofbuttons is to track how many buttons to display
    private bool selecting; //if this is enabled, do not allow dialogue progression, as the user needs to make a button choice
    private string resolve; //this is for deciding what screen to go to after the event is done.
    private Animator eventAnimation;

    void Awake() {
        InitializeButtons();
        DisableButtons();
        eventAnimation = GetComponent<Animator>();
        eventAnimation.SetTrigger("Intro");
    }

    private void OnEnable() {
        eventAnimation.SetTrigger("Intro");
    }

    public void InitializeEvent() { //to reset the event and determine what event to do
        messageCounter = 0;
        title.text = "";
        dialogueText.text = "";
        button1Text.text = ""; button2Text.text = ""; button3Text.text = "";
        health.text = player.currentHp.ToString();
        selecting = false;
        afterimages.text = player.afterimage.ToString("f1"); //todo
        money.text = player.money.ToString(); //todo
        
        //reset the stuff from before

        switch(map.GetWorld()) { //Events change depending on the world the player is in. Select the event to do randomly.
            case 1: //ruins
                selectedEvent = ruinsEvents[Random.Range(0, ruinsEvents.Length)];
                background.sprite = ruinsBG;
                break;
            case 2: //forest
                //todo
                break;
            case 3: //sewer
                //todo
                break;
            case 4: //abyss
                //todo
                break;
        }

        dialogue = selectedEvent.GetDialogue();
        names = selectedEvent.GetNames();
        options = selectedEvent.GetOptions();
        skip = selectedEvent.GetSkipEntry();
        outcome = selectedEvent.GetOutcome();
        title.text = selectedEvent.GetTitle();

        numberOfButtons = options.Length;
        ProgressDialogue(messageCounter);
    }

    private void ProgressDialogue(int number) { //advances the dialogue.
        if(!selecting) {
            switch(names[number]) {
                case "[CHOOSE]": //the dialogue will have a choose option. when this appears, the buttons appear to make the player decide what action to take
                    selecting = true;
                    EnableButtons();
                    break;
                case "[END]":   //resolve the outcome and end the event.
                    ResolveOutcome();
                    break;
                default:
                    nameText.text = names[number];
                    dialogueText.text = dialogue[number];
                    switch(names[number]) {
                        case "NARRATION":
                            nameText.text = "";
                            speaker.sprite = def;
                            break;
                        case "Jamp":
                            speaker.sprite = jamp;
                            break;
                        case "Old Man":
                            speaker.sprite = oldMan;
                            break;
                        case "Lonely Ghost":
                            speaker.sprite = lonelyGhost;
                            break;
                        case "Angry Ghost":
                            speaker.sprite = angryGhost;
                            break;
                        case "Old Man Shadow":
                            speaker.sprite = oldShadow;
                            break;
                        case "Ghost Shadow":
                            speaker.sprite = ghostShadow;
                            break;
                        case "Ash":
                            speaker.sprite = ash;
                            break;
                        case "Fairy":
                            speaker.sprite = fairy;
                            break;
                        case "Wolf":
                            speaker.sprite = wolf;
                            break;
                        case "Tree":
                            speaker.sprite = tree;
                            break;
                    }
                    messageCounter++;
                    break;
            }
        }
    }

    private void DialogueOption(int number) {
        selecting = false;                  //re-enables the dialogue options
        DisableButtons();                   //gets rid of the buttons

        if (selectedEvent.GetTitle() == "The Injury" && number == 0) {
            //This event one button with a 50/50 chance of two different outcomes. 
            //I didn't say no before realizing this would be a pain in the ass to implement, so we'll just hard code it
            int randomSelection = Random.Range(0,2);  
            messageCounter = skip[number + randomSelection];       
            ProgressDialogue(messageCounter);
            outcomeDecided = outcome[number + randomSelection];
        } else {
            messageCounter = skip[number];       
            ProgressDialogue(messageCounter);   //skips to the appropriate dialogue line determined by the event
            outcomeDecided = outcome[number];   //sets the action to take; the result of the event
        }
        
    }

    private void EnableButtons() { //enables the buttons for each dialogue option, then sets them to the text that they need to be
        if (numberOfButtons >= 1) {
            button1.gameObject.SetActive(true);
            button1Text.text = options[0];
        }
        if (numberOfButtons >= 2) {
            button2.gameObject.SetActive(true);
            button2Text.text = options[1];
        }
        if (numberOfButtons == 3) {
            button3.gameObject.SetActive(true);
            button3Text.text = options[2];
        }

        if (selectedEvent.GetTitle() == "The Fairy Circle") {   //If you don't have 500 gold, you can't pay the 500 gold.
            if (player.money < 500) {
                button1.interactable = false;
            }
        }
    }

    private void DisableButtons() { //gets rid of all the buttons
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
    }

    private void InitializeButtons() {
        advanceButton.onClick.AddListener(() => {ProgressDialogue(messageCounter);});
        button1.onClick.AddListener(()=> {DialogueOption(0);});
        button2.onClick.AddListener(()=> {DialogueOption(1);});
        button3.onClick.AddListener(()=> {DialogueOption(2);});
    }

    private void ResolveOutcome() { //all of the outcomes for the events.
        /*  
            Progress:
                Ruins:
                    1. done
                    2. done
                    3. done
                    4. done
                    5. done
                Forest:
                    1. done
                    2. todo
                    3. done
                    4. todo
                    5. todo
                Sewer:
                    1. todo
                    2. todo
                    3. todo
                    4. todo
                    5. todo
        */

        switch(outcomeDecided) {
            case 0:     //+10 HP (event 1, ruins)
                player.currentHp += 10;
                resolve = "normal";
                break;
            case 1:     //-10 HP (cannot kill) (event 1, ruins)
                if (player.currentHp <= 10) {
                    player.currentHp -= player.currentHp- 1;
                } else {
                    player.currentHp -= 10;
                }
                resolve = "normal";
                break;
            case 2:     //skeleton swarm (event 2, ruins)
                combat.ReceiveCondition(outcomeDecided);
                resolve = "combat";
                break;
            case 3:     //-100g, +1 buff (event 3, ruins)
                player.money += 100;
                resolve = "upgrade";
                break;
            case 4:     //+100g, increased DashCD for one battle (event 3, ruins)
                if (player.money < 100) {
                    player.money = 0;
                } else {
                    player.money -= 100;
                }
                playerMove.SetExternalModifier(3f);
                resolve = "normal";
                break;
            case 5:     //reduced DashCD for one battle (event 4, ruins)
                player.money -= 1;
                playerMove.SetExternalModifier(-2f);
                resolve = "normal";
                break;
            case 6:     //-50 HP (cannot kill) (event 4, ruins)
                if (player.currentHp <= 50) {
                    player.currentHp -= player.currentHp - 1;
                } else {
                    player.currentHp -= 50;
                }
                resolve = "normal";
                break;
            case 7:     //+500 gold (event 4, ruins)
                player.money += 500;
                resolve = "normal";
                break;
            case 8:     //vs fight against golem swarm (event 5, ruins)
                combat.ReceiveCondition(outcomeDecided);
                resolve = "combat";
                break;
            case 9:     //nothing happens (event 5, ruins)
                resolve = "normal";
                break;
            case 10:    //-500g if 500+g (event 1, forest)
                player.money -= 500;
                resolve = "normal";
                break;
            case 11:    //fight vs wolf swarm (event 1, forest)
                combat.ReceiveCondition(outcomeDecided);
                resolve = "combat";
                break;
            case 12:    //fight vs tree swarm (event 3, forest)
                combat.ReceiveCondition(outcomeDecided);
                resolve = "combat";
                break;
            case 13:    //Ash wrestles the wolf off of Jamp, nothing happens(event 2, forest)
                resolve = "normal";
                break;
            case 14:    //Jamp gets bit by the wolf (-10 HP) (event 2, forest)
                if (player.currentHp <= 10) {
                    player.currentHp = 1;
                } else {
                    player.currentHp -= 10;
                }
                resolve = "normal";
                break;
            case 15:    //Jamp and Ash leave the wolf alone, nothing happens (event 2, forest)
                resolve = "normal";
                break;
            case 16:    //Jamp cuts the tree down and eats the golden apples. +20HP (event 4, forest)
                player.currentHp += 20;
                resolve = "normal";
                break;
            case 17:    //Jamp leaves the tree alone. Nothing happens. (event 4, forest)
                resolve = "normal";
                break;
            case 18:    //Jamp knocks out all the squirrels. Nothing happens. (event 5, forest)
                resolve = "normal";
                break;
            case 19:    //fight vs squirrel swarm (event 5, forest)
                combat.ReceiveCondition(outcomeDecided);
                resolve = "combat";
                break;
        }
        eventAnimation.SetTrigger("Outro");
    }

    private void OutroAnimationFinished() {
        Exit();
    }
    
    private void Exit() {
        switch(resolve) {
            case "combat":
                manager.ReceiveCommand("combat");
                break;
            case "upgrade":
                manager.ReceiveCommand("upgrade");
                break;
            case "normal":
                manager.ReceiveCommand("map");
                break;
        }
    }
}