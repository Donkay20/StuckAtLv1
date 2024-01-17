using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private TextMeshProUGUI title, dialogueText, button1, button2, button3, health, afterimages, money;
    private Event selectedEvent;
    private string[] dialogue, names;
    private int messageCounter;
     void Awake()
    {
        InitializeButtons();
    }

    public void InitializeEvent() {

        messageCounter = 0;
        title.text = "";
        dialogueText.text = "";
        button1.text = ""; button2.text = ""; button3.text = "";
        health.text = player.currentHp.ToString();
        afterimages.text = 0.ToString(); //todo
        money.text = 0.ToString(); //todo
        
        //reset the stuff from before

        switch(map.GetWorld()) {
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

        dialogue = selectedEvent.getDialogue();
        names = selectedEvent.getNames();
    }

    private void Exit() {
        manager.ReceiveCommand("map");
    }

    private void InitializeButtons() {
        confirm.onClick.AddListener(() => {Exit();});
    }
}
