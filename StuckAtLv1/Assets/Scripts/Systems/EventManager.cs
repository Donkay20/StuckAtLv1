using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EventManager : MonoBehaviour
{
    //temporary, todo
    [SerializeField] private Button confirm;
    [SerializeField] private GameManager manager;
    [SerializeField] private MapManager map;
    //backend stuff
    [SerializeField] private Event[] ruinsEvents, forestEvents, sewerEvents, abyssEvents;
    private Event selectedEvent;
    private int counter;
    void Awake()
    {
        InitializeButtons();
    }

    public void InitializeEvent() {
        
        //reset the stuff from before I guess

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
    }

    private void Exit() {
        manager.ReceiveCommand("map");
    }

    private void InitializeButtons() {
        confirm.onClick.AddListener(() => {Exit();});
    }
}
