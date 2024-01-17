using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    private Event selectedEvent; //chosen event
    private string[] dialogue, names, options; private int[] skip, outcome; //copy from the chosen event
    private int messageCounter, numberOfButtons; //messagecounter is to track what line we're on, where numberofbuttons is to track how many buttons to display
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

        numberOfButtons = options.Length;

        dialogue = selectedEvent.getDialogue();
        names = selectedEvent.getNames();
        options = selectedEvent.getOptions();
        skip = selectedEvent.getSkipEntry();
        outcome = selectedEvent.getOutcome();
        title.text = selectedEvent.getTitle();

        ProgressDialogue(messageCounter);
    }

    private void ProgressDialogue(int number) { //advances the dialogue.
        if(!selecting) {
            switch(names[number]) {
                case "[CHOOSE]":
                    selecting = true;
                    enableButtons();
                    break;
                case "[END]":
                    //do outcome and end
                    break;
                default:
                    if (names[number] == "Jamp") {
                        //put Jamp's image here
                    }
                    nameText.text = names[number];
                    dialogueText.text = dialogue[number];
                    break;
            }
        }
    }

    private void enableButtons() {
        if(numberOfButtons == 2) {
            button1.gameObject.SetActive(true);
            button2.gameObject.SetActive(true);
        }
        if(numberOfButtons == 3) {
            button3.gameObject.SetActive(true);
        }
    }

    private void disableButtons() {
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
    }

    //fix these later
    private void Exit() {
        manager.ReceiveCommand("map");
    }

    private void InitializeButtons() { 
        confirm.onClick.AddListener(() => {Exit();});
        advanceButton.onClick.AddListener(() => {ProgressDialogue(messageCounter);});
    }
}
