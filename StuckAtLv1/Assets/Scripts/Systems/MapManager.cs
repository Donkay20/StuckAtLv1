using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Button node13, node22, node24, node31, node33, node35, node42, node44, node53, node62, node64, node71, node73, node75, node82, node84, node91, node93, node95, node102, node104, node113;
    [SerializeField] private Room[] rooms = new Room[19]; //not adding room 1-3 (always combat), room 5-3 (always miniboss), and room 11-3 (always boss)
    [SerializeField] private Room startingRoom, minibossRoom, bossRoom;
    [Space]
    [SerializeField] private Sprite inactiveImage;
    [SerializeField] private Sprite combatImage;
    [SerializeField] private Sprite survivalImage;
    [SerializeField] private Sprite eventImage;
    [SerializeField] private Sprite shopImage;
    [SerializeField] private Sprite miniBossImage;
    [SerializeField] private Sprite bossImage;
    [SerializeField] private GameManager manager;
    private int world; private int level; private int section;
    private String report;

    void Start()
    {
        world = 1;
        level = 0;
        section = 2;

        foreach (Room r in rooms) {
            GiveRoom(r);
        }

        InitializeButtons();

        startingRoom.AssignRoomType("combat"); startingRoom.GetComponent<Image>().sprite = combatImage;
        minibossRoom.AssignRoomType("miniboss"); minibossRoom.GetComponent<Image>().sprite = miniBossImage;
        bossRoom.AssignRoomType("boss"); bossRoom.GetComponent<Image>().sprite = bossImage;
        //hard code the first room to be combat, and hard code the positions of miniboss room & boss room
    }

    public void clickedNode(int clickedLevel, int clickedSection) {
        /*  First, check if the node that was clicked is one level ahead, and one that connects to a path that the user can go to.
            For example, you can't go from a 3-layer bottom room to a 2-layer top room as the paths do not connect.
            Then, 
        */
        if (level + 1 == clickedLevel && (section + 1 == clickedSection || section - 1 == clickedSection)) { //check if the button clicked is a valid one
            level++; //the level should always be advancing
            switch (clickedLevel, clickedSection) { //there's probably a better way to do this too, but whatever
            case (1,3):
                section++;
                node13.interactable = false;
                report = node13.GetComponent<Room>().GetRoomType();
                break;
            case (2,2):
                section--;
                node22.interactable = false; node24.interactable = false;
                report = node22.GetComponent<Room>().GetRoomType();
                break;
            case (2,4):
                section++;
                node22.interactable = false; node24.interactable = false;
                report = node22.GetComponent<Room>().GetRoomType();
                break;
            case (3,1):
                section--;
                node31.interactable = false; node33.interactable = false; node35.interactable = false;
                report = node31.GetComponent<Room>().GetRoomType();
                break;
            case (3,3):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node31.interactable = false; node33.interactable = false; node35.interactable = false;
                report = node33.GetComponent<Room>().GetRoomType();
                break;
            case (3,5):
                section++;
                node31.interactable = false; node33.interactable = false; node35.interactable = false;
                report = node35.GetComponent<Room>().GetRoomType();
                break;
            case (4,2):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node42.interactable = false; node44.interactable = false;
                report = node42.GetComponent<Room>().GetRoomType();
                break;
            case (4,4):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node42.interactable = false; node44.interactable = false;
                report = node44.GetComponent<Room>().GetRoomType();
                break;
            case (5,3):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node53.interactable = false;
                report = node53.GetComponent<Room>().GetRoomType();
                break;
            case (6,2):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node62.interactable = false; node64.interactable = false;
                report = node62.GetComponent<Room>().GetRoomType();
                break;
            case (6,4):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node62.interactable = false; node64.interactable = false;
                report = node64.GetComponent<Room>().GetRoomType();
                break;
            case (7,1):
                section--;
                node71.interactable = false; node73.interactable = false; node75.interactable = false;
                report = node71.GetComponent<Room>().GetRoomType();
                break;
            case (7,3):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node71.interactable = false; node73.interactable = false; node75.interactable = false;
                report = node73.GetComponent<Room>().GetRoomType();
                break;
            case (7,5):
                section++;
                node71.interactable = false; node73.interactable = false; node75.interactable = false;
                report = node75.GetComponent<Room>().GetRoomType();
                break;
            case (8,2): 
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node82.interactable = false; node84.interactable = false;
                report = node82.GetComponent<Room>().GetRoomType();
                break;
            case (8,4):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node82.interactable = false; node84.interactable = false;
                report = node84.GetComponent<Room>().GetRoomType();
                break;
            case (9,1):
                section--;
                node91.interactable = false; node93.interactable = false; node95.interactable = false;
                report = node91.GetComponent<Room>().GetRoomType();
                break;
            case (9,3):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node91.interactable = false; node93.interactable = false; node95.interactable = false;
                report = node93.GetComponent<Room>().GetRoomType();
                break;
            case (9,5):
                section++;
                node91.interactable = false; node93.interactable = false; node95.interactable = false;
                report = node95.GetComponent<Room>().GetRoomType();
                break;
            case (10,2):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node102.interactable = false; node104.interactable = false;
                report = node102.GetComponent<Room>().GetRoomType();
                break;
            case (10,4):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node102.interactable = false; node104.interactable = false;
                report = node104.GetComponent<Room>().GetRoomType();
                break;
            case (11,3):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node113.interactable = false;
                report = node113.GetComponent<Room>().GetRoomType();
                break;
            }
            Debug.Log("level " + level);
            Debug.Log("section " + section);
            manager.ReceiveCommand(report);
        }
    }
    
    private void GiveRoom(Room r) {
        int number = Random.Range(1,5);
        Debug.Log(number);
        switch(number) {
            case 1:
                r.AssignRoomType("combat");
                r.GetComponent<Image>().sprite = combatImage;
                break;
            case 2:
                r.AssignRoomType("survival");
                r.GetComponent<Image>().sprite = survivalImage;
                break;
            case 3:
                r.AssignRoomType("event");
                r.GetComponent<Image>().sprite = eventImage;
                break;
            case 4:
                r.AssignRoomType("shop");
                r.GetComponent<Image>().sprite = shopImage;
                break;
        }
    }

    private void InitializeButtons() {
        node13.onClick.AddListener(() => {clickedNode(1,3);});  //surely there's a better way to do this
        node22.onClick.AddListener(() => {clickedNode(2,2);});
        node24.onClick.AddListener(() => {clickedNode(2,4);});
        node31.onClick.AddListener(() => {clickedNode(3,1);});
        node33.onClick.AddListener(() => {clickedNode(3,3);});
        node35.onClick.AddListener(() => {clickedNode(3,5);});
        node42.onClick.AddListener(() => {clickedNode(4,2);});
        node44.onClick.AddListener(() => {clickedNode(4,4);});
        node53.onClick.AddListener(() => {clickedNode(5,3);});
        node62.onClick.AddListener(() => {clickedNode(6,2);});
        node64.onClick.AddListener(() => {clickedNode(6,4);});
        node71.onClick.AddListener(() => {clickedNode(7,1);});
        node73.onClick.AddListener(() => {clickedNode(7,3);});
        node75.onClick.AddListener(() => {clickedNode(7,5);});
        node82.onClick.AddListener(() => {clickedNode(8,2);});
        node84.onClick.AddListener(() => {clickedNode(8,4);});
        node91.onClick.AddListener(() => {clickedNode(9,1);});
        node91.onClick.AddListener(() => {clickedNode(9,1);});
        node93.onClick.AddListener(() => {clickedNode(9,3);});
        node95.onClick.AddListener(() => {clickedNode(9,5);});
        node102.onClick.AddListener(() => {clickedNode(10,2);});
        node104.onClick.AddListener(() => {clickedNode(10,4);});
        node113.onClick.AddListener(() => {clickedNode(11,3);});
    }
}
