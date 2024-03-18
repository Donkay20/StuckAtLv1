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
    [SerializeField] private Image background;
    [SerializeField] private Sprite forestBackground, sewerBackground, abyssBackground;
    [SerializeField] private GameManager manager;
    [SerializeField] private int world; 
    [SerializeField] private int level; 
    [SerializeField] private int section;
    private string report;
    Animator mapAnimation;

    //Map Highlights
    [SerializeField] private Sprite combatImageHighlight, survivalImageHighlight, eventImageHighlight, shopImageHighlight, miniBossImageHighlight, bossImageHighlight;
    [SerializeField] private GameObject[] lines;
    private List<Vector2> nodeList = new List<Vector2>();

    void Awake() {
        world = 1;
        level = 0;
        section = 2;

        AssignRoomProcess();
        InitializeButtons();

        startingRoom.AssignRoomType("combat"); startingRoom.GetComponent<Image>().sprite = combatImage;
        minibossRoom.AssignRoomType("dialogue"); minibossRoom.GetComponent<Image>().sprite = miniBossImage;
        bossRoom.AssignRoomType("dialogue"); bossRoom.GetComponent<Image>().sprite = bossImage;
        mapAnimation = GetComponent<Animator>();
        //hard code the first room to be combat, and hard code the positions of miniboss room & boss room

        HighlightRoom(startingRoom);
        mapAnimation.SetTrigger("Intro");
    }

    private void OnEnable() {
        mapAnimation.SetTrigger("Intro");
    }

    public void ClickedNode(int clickedLevel, int clickedSection) {
        
        /*  First, check if the node that was clicked is one level ahead, and one that connects to a path that the user can go to.
            For example, you can't go from a 3-layer bottom room to a 2-layer top room as the paths do not connect.

            If it's an invalid move, do nothing. 
            If it's a valid move, disable the button and all other buttons in that section.

            Then, mark down the type of room that was clicked by getting the info from the room script assigned to the button that was clicked.

            Then, do the outro animation and tell the game manager what type of room was clicked so it can act accordingly.
        */
        if (level + 1 == clickedLevel && (section + 1 == clickedSection || section - 1 == clickedSection)) { //check if the button clicked is a valid one
            Vector2 currentNode = new Vector2(clickedLevel, clickedSection);
            nodeList.Add(currentNode);
            AssignLine(currentNode);

            level++; //the level should always be advancing
            switch (clickedLevel, clickedSection) { //there's probably a better way to do this too, but whatever
            case (1,3):
                section++;
                node13.interactable = false;
                report = node13.GetComponent<Room>().GetRoomType();
                HighlightRoom(node13.GetComponent<Room>());
                break;
            case (2,2):
                section--;
                node22.interactable = false; node24.interactable = false;
                report = node22.GetComponent<Room>().GetRoomType();
                HighlightRoom(node22.GetComponent<Room>());
                break;
            case (2,4):
                section++;
                node22.interactable = false; node24.interactable = false;
                report = node24.GetComponent<Room>().GetRoomType();
                HighlightRoom(node24.GetComponent<Room>());
                break;
            case (3,1):
                section--;
                node31.interactable = false; node33.interactable = false; node35.interactable = false;
                report = node31.GetComponent<Room>().GetRoomType();
                HighlightRoom(node31.GetComponent<Room>());
                break;
            case (3,3):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node31.interactable = false; node33.interactable = false; node35.interactable = false;
                report = node33.GetComponent<Room>().GetRoomType();
                HighlightRoom(node33.GetComponent<Room>());
                break;
            case (3,5):
                section++;
                node31.interactable = false; node33.interactable = false; node35.interactable = false;
                report = node35.GetComponent<Room>().GetRoomType();
                HighlightRoom(node35.GetComponent<Room>());
                break;
            case (4,2):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node42.interactable = false; node44.interactable = false;
                report = node42.GetComponent<Room>().GetRoomType();
                HighlightRoom(node42.GetComponent<Room>());
                break;
            case (4,4):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node42.interactable = false; node44.interactable = false;
                report = node44.GetComponent<Room>().GetRoomType();
                HighlightRoom(node44.GetComponent<Room>());
                break;
            case (5,3):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node53.interactable = false;
                report = node53.GetComponent<Room>().GetRoomType();
                HighlightRoom(node53.GetComponent<Room>());
                break;
            case (6,2):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node62.interactable = false; node64.interactable = false;
                report = node62.GetComponent<Room>().GetRoomType();
                HighlightRoom(node62.GetComponent<Room>());
                break;
            case (6,4):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node62.interactable = false; node64.interactable = false;
                report = node64.GetComponent<Room>().GetRoomType();
                HighlightRoom(node64.GetComponent<Room>());
                break;
            case (7,1):
                section--;
                node71.interactable = false; node73.interactable = false; node75.interactable = false;
                report = node71.GetComponent<Room>().GetRoomType();
                HighlightRoom(node71.GetComponent<Room>());
                break;
            case (7,3):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node71.interactable = false; node73.interactable = false; node75.interactable = false;
                report = node73.GetComponent<Room>().GetRoomType();
                HighlightRoom(node73.GetComponent<Room>());
                break;
            case (7,5):
                section++;
                node71.interactable = false; node73.interactable = false; node75.interactable = false;
                report = node75.GetComponent<Room>().GetRoomType();
                HighlightRoom(node75.GetComponent<Room>());
                break;
            case (8,2): 
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node82.interactable = false; node84.interactable = false;
                report = node82.GetComponent<Room>().GetRoomType();
                HighlightRoom(node82.GetComponent<Room>());
                break;
            case (8,4):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node82.interactable = false; node84.interactable = false;
                report = node84.GetComponent<Room>().GetRoomType();
                HighlightRoom(node84.GetComponent<Room>());
                break;
            case (9,1):
                section--;
                node91.interactable = false; node93.interactable = false; node95.interactable = false;
                report = node91.GetComponent<Room>().GetRoomType();
                HighlightRoom(node91.GetComponent<Room>());
                break;
            case (9,3):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node91.interactable = false; node93.interactable = false; node95.interactable = false;
                report = node93.GetComponent<Room>().GetRoomType();
                HighlightRoom(node93.GetComponent<Room>());
                break;
            case (9,5):
                section++;
                node91.interactable = false; node93.interactable = false; node95.interactable = false;
                report = node95.GetComponent<Room>().GetRoomType();
                HighlightRoom(node95.GetComponent<Room>());
                break;
            case (10,2):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node102.interactable = false; node104.interactable = false;
                report = node102.GetComponent<Room>().GetRoomType();
                HighlightRoom(node102.GetComponent<Room>());
                break;
            case (10,4):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node102.interactable = false; node104.interactable = false;
                report = node104.GetComponent<Room>().GetRoomType();
                HighlightRoom(node104.GetComponent<Room>());
                break;
            case (11,3):
                if (clickedSection > section) {
                    section++;
                } else {
                    section--;
                }
                node113.interactable = false;
                report = node113.GetComponent<Room>().GetRoomType();
                HighlightRoom(node113.GetComponent<Room>());
                break;
            }
            manager.AdjustScaling();
            mapAnimation.SetTrigger("Outro");
        }
    }

    public void TransitionAnimationComplete() {    //called from the end of the "Outro" animation's via an event
        manager.ReceiveCommand(report);
    }

    private void AssignRoomProcess() {      //only have a set amount of shops in the map at once.
        int firstShopBeforeMiniboss, secondShopBeforeMiniboss, firstShopAfterMiniboss, secondShopAfterMiniboss;
        firstShopBeforeMiniboss = Random.Range(2,5);
        secondShopBeforeMiniboss = Random.Range(5,7);
        firstShopAfterMiniboss = Random.Range(14,17);
        secondShopAfterMiniboss = Random.Range(17,19);

        if (world == 1) {   //In the first world, only put one shop before the miniboss. After, put two before every miniboss.
            for (int i = 0; i < 19; i++) {
                if (i == secondShopBeforeMiniboss || i == firstShopAfterMiniboss || i == secondShopAfterMiniboss) {
                    GiveRoom(rooms[i], true);
                } else {
                    GiveRoom(rooms[i], false);
                }
            }
        } else {
            for (int i = 0; i < 19; i++) {
                if (i == firstShopBeforeMiniboss || i == secondShopBeforeMiniboss || i == firstShopAfterMiniboss || i == secondShopAfterMiniboss) {
                    GiveRoom(rooms[i], true);
                } else {
                    GiveRoom(rooms[i], false);
                }
            }
        }
    }
    
    private void GiveRoom(Room r, bool isShop) {
        r.GetComponent<Button>().interactable = true;
        if (isShop) {
            r.AssignRoomType("shop");
            r.GetComponent<Image>().sprite = shopImage;
        } else {
            int number = Random.Range(1,4);
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
            }
        }
    }

    //Highlights given room
    private void HighlightRoom(Room r) {
        if(r.GetRoomType() == "combat")
        {
            r.GetComponent<Image>().sprite = combatImageHighlight;
        }
        if(r.GetRoomType() == "survival")
        {
            r.GetComponent<Image>().sprite = survivalImageHighlight;
        }
        if(r.GetRoomType() == "event")
        {
            r.GetComponent<Image>().sprite = eventImageHighlight;
        }
        if(r.GetRoomType() == "shop")
        {
            r.GetComponent<Image>().sprite = shopImageHighlight;
        }
        if(r.GetRoomType() == "miniboss")
        {
            r.GetComponent<Image>().sprite = miniBossImageHighlight;
        }
        if(r.GetRoomType() == "boss")
        {
            r.GetComponent<Image>().sprite = bossImageHighlight;
        }
    }

    //Adds Line to see where traveled
    private void AssignLine(Vector2 newNode) {
        Vector2 previousNode;
        for(int i = 0; i < nodeList.Count; i++)
        {
            if(nodeList[i] == newNode && nodeList.Count > 1)
            {
                Debug.Log("find previous Node");
                previousNode = nodeList[i - 1];
                Debug.Log("find newNode");
                Debug.Log(newNode);
                
                //Checks previous node and selected node to determine where to place line
                SetLinesActive(previousNode, newNode, 0, new Vector2(1,3), new Vector2(2,2));
                SetLinesActive(previousNode, newNode, 1, new Vector2(2,2), new Vector2(3,1));
                SetLinesActive(previousNode, newNode, 2, new Vector2(3,1), new Vector2(4,2));
                SetLinesActive(previousNode, newNode, 3, new Vector2(4,2), new Vector2(5,3));
                SetLinesActive(previousNode, newNode, 4, new Vector2(5,3), new Vector2(6,2));
                SetLinesActive(previousNode, newNode, 5, new Vector2(6,2), new Vector2(7,1));
                SetLinesActive(previousNode, newNode, 6, new Vector2(7,1), new Vector2(8,2));
                SetLinesActive(previousNode, newNode, 7, new Vector2(8,2), new Vector2(9,1));
                SetLinesActive(previousNode, newNode, 8, new Vector2(9,1), new Vector2(10,2));
                SetLinesActive(previousNode, newNode, 9, new Vector2(10,2), new Vector2(11,3));
                
                SetLinesActive(previousNode, newNode, 10, new Vector2(1,3), new Vector2(2,4));
                SetLinesActive(previousNode, newNode, 11, new Vector2(2,4), new Vector2(3,5));
                SetLinesActive(previousNode, newNode, 12, new Vector2(3,5), new Vector2(4,4));
                SetLinesActive(previousNode, newNode, 13, new Vector2(4,4), new Vector2(5,3));
                SetLinesActive(previousNode, newNode, 14, new Vector2(5,3), new Vector2(6,4));
                SetLinesActive(previousNode, newNode, 15, new Vector2(6,4), new Vector2(7,5));
                SetLinesActive(previousNode, newNode, 16, new Vector2(7,5), new Vector2(8,4));
                SetLinesActive(previousNode, newNode, 17, new Vector2(8,4), new Vector2(9,5));
                SetLinesActive(previousNode, newNode, 18, new Vector2(9,5), new Vector2(10,4));
                SetLinesActive(previousNode, newNode, 19, new Vector2(10,4), new Vector2(11,3));

                SetLinesActive(previousNode, newNode, 20, new Vector2(2,2), new Vector2(3,3));
                SetLinesActive(previousNode, newNode, 21, new Vector2(3,3), new Vector2(4,2));
                SetLinesActive(previousNode, newNode, 22, new Vector2(6,2), new Vector2(7,3));
                SetLinesActive(previousNode, newNode, 23, new Vector2(7,3), new Vector2(8,2));
                SetLinesActive(previousNode, newNode, 24, new Vector2(8,2), new Vector2(9,3));
                SetLinesActive(previousNode, newNode, 25, new Vector2(9,3), new Vector2(10,2));

                SetLinesActive(previousNode, newNode, 26, new Vector2(2,4), new Vector2(3,3));
                SetLinesActive(previousNode, newNode, 27, new Vector2(3,3), new Vector2(4,4));
                SetLinesActive(previousNode, newNode, 28, new Vector2(6,4), new Vector2(7,3));
                SetLinesActive(previousNode, newNode, 29, new Vector2(7,3), new Vector2(8,4));
                SetLinesActive(previousNode, newNode, 30, new Vector2(8,4), new Vector2(9,3));
                SetLinesActive(previousNode, newNode, 31, new Vector2(9,3), new Vector2(10,4));
            }   
        }
    }

    private void SetLinesActive(Vector2 prevNode, Vector2 currentNode, int lineNum, Vector2 checkPrev, Vector2 CheckCurrent) {
        if(prevNode == checkPrev && currentNode == CheckCurrent)
                {
                    lines[lineNum].SetActive(true);
                }
    }

    public void SetLinesOff()
    {
        nodeList.Clear();
        for(int i = 0; i < lines.Length; i++)
        {
            lines[i].SetActive(false);
        }
    }

    private void InitializeButtons() {
        //initializes the connection between the rooms and their buttons
        node13.onClick.AddListener(() => {ClickedNode(1,3);});  //surely there's a better way to do this
        node22.onClick.AddListener(() => {ClickedNode(2,2);});
        node24.onClick.AddListener(() => {ClickedNode(2,4);});
        node31.onClick.AddListener(() => {ClickedNode(3,1);});
        node33.onClick.AddListener(() => {ClickedNode(3,3);});
        node35.onClick.AddListener(() => {ClickedNode(3,5);});
        node42.onClick.AddListener(() => {ClickedNode(4,2);});
        node44.onClick.AddListener(() => {ClickedNode(4,4);});
        node53.onClick.AddListener(() => {ClickedNode(5,3);});
        node62.onClick.AddListener(() => {ClickedNode(6,2);});
        node64.onClick.AddListener(() => {ClickedNode(6,4);});
        node71.onClick.AddListener(() => {ClickedNode(7,1);});
        node73.onClick.AddListener(() => {ClickedNode(7,3);});
        node75.onClick.AddListener(() => {ClickedNode(7,5);});
        node82.onClick.AddListener(() => {ClickedNode(8,2);});
        node84.onClick.AddListener(() => {ClickedNode(8,4);});
        node91.onClick.AddListener(() => {ClickedNode(9,1);});
        node93.onClick.AddListener(() => {ClickedNode(9,3);});
        node95.onClick.AddListener(() => {ClickedNode(9,5);});
        node102.onClick.AddListener(() => {ClickedNode(10,2);});
        node104.onClick.AddListener(() => {ClickedNode(10,4);});
        node113.onClick.AddListener(() => {ClickedNode(11,3);});
    }

    public int GetWorld() {
        return world;
    }

    public int GetLevel() {
        return level;
    }

    public void NewWorld() {
        world++;
        level = 0;
        section = 2;

        switch(world) {
            case 2:
                background.sprite = forestBackground;
                break;
            case 3: 
                background.sprite = sewerBackground;
                break;
            case 4:
                background.sprite = abyssBackground;
                break;
        }

        //foreach (Room r in rooms) {GiveRoom(r);}
        AssignRoomProcess();

        startingRoom.AssignRoomType("combat"); startingRoom.GetComponent<Image>().sprite = combatImage;
        startingRoom.GetComponent<Button>().interactable = true;
        minibossRoom.AssignRoomType("dialogue"); minibossRoom.GetComponent<Image>().sprite = miniBossImage;
        minibossRoom.GetComponent<Button>().interactable = true;
        bossRoom.AssignRoomType("dialogue"); bossRoom.GetComponent<Image>().sprite = bossImage;
        bossRoom.GetComponent<Button>().interactable = true;

        HighlightRoom(startingRoom);

        //Philip: add more code here to reset the highlights and lines.
    }
}
