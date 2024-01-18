using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EventManager : MonoBehaviour
{
    [SerializeField] private Button confirm;
    //temporary buttons
    [SerializeField] private GameManager manager;
    [SerializeField] private MapManager map;
    [SerializeField] private Character player;
    //backend stuff
    [SerializeField] private Event[] ruinsEvents, forestEvents, sewerEvents, abyssEvents;
    [SerializeField] private TextMeshProUGUI title, nameText, dialogueText, button1Text, button2Text, button3Text, health, afterimages, money;
    [SerializeField] private Button button1, button2, button3, advanceButton;
    [SerializeField] private Image speaker, background;
    [SerializeField] private Sprite def, jamp, oldMan, lonelyGhost;
    [SerializeField] private Sprite ruinsBG, forestBG, sewerBG, abyssBG;

    private Event selectedEvent; //chosen event
    private string[] dialogue, names, options; private int[] skip, outcome; //copy from the chosen event
    private int messageCounter, numberOfButtons, outcomeDecided; //messagecounter is to track what line we're on, where numberofbuttons is to track how many buttons to display
    private bool selecting;
     void Awake()
    {
        InitializeButtons();
        disableButtons();
    }

    public void InitializeEvent() { //to reset the event and determine what event to do

        messageCounter = 0;
        title.text = "";
        dialogueText.text = "";
        button1Text.text = ""; button2Text.text = ""; button3Text.text = "";
        health.text = player.currentHp.ToString();
        selecting = false;
        afterimages.text = 0.ToString(); //todo
        money.text = 0.ToString(); //todo
        
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
                    enableButtons();
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
                    }
                    messageCounter++;
                    break;
            }
        }
    }

    private void DialogueOption(int number) {
        selecting = false;                  //re-enables the dialogue options
        messageCounter = skip[number];       
        ProgressDialogue(messageCounter);   //skips to the appropriate dialogue line determined by the event
        disableButtons();                   //gets rid of the buttons
        outcomeDecided = outcome[number];   //sets the action to take; the result of the event
    }

    private void enableButtons() { //enables the buttons for each dialogue option, then sets them to the text that they need to be
        if(numberOfButtons == 2) {
            button1.gameObject.SetActive(true);
            button1Text.text = options[0];
            button2.gameObject.SetActive(true);
            button2Text.text = options[1];
        }
        if(numberOfButtons == 3) {
            button3.gameObject.SetActive(true);
            button3Text.text = options[2];
        }
    }

    private void disableButtons() { //gets rid of all the buttons
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
    }

    private void InitializeButtons() { 
        confirm.onClick.AddListener(() => {Exit();});
        advanceButton.onClick.AddListener(() => {ProgressDialogue(messageCounter);});
        button1.onClick.AddListener(()=> {DialogueOption(0);});
        button2.onClick.AddListener(()=> {DialogueOption(1);});
        button3.onClick.AddListener(()=> {DialogueOption(2);});
    }

    private void ResolveOutcome() { //all of the outcomes for the events. 
        switch(outcomeDecided) {
            case 0: //+10 HP to Jamp (event 1, ruins)
                player.currentHp +=10;
                break;
            case 1: //-10 HP to Jamp (cannot kill) (event 1, ruins)
                if (player.currentHp <=10) {
                    player.currentHp -= player.currentHp-1;
                } else {
                    player.currentHp -=10;
                }
                break;
        }
        Exit();
    }
    
    private void Exit() {
        manager.ReceiveCommand("map");
    }
}