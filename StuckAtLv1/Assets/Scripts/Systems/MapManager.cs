using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Button node13, node22, node24, node31, node33, node35, node42, node44, node53, node62, node64, node71, node73, node75, node82, node84, node91, node93, node95, node102, node104, node113;
    [Space]
    [SerializeField] private Sprite inactiveImage;
    [SerializeField] private Sprite combatImage;
    [SerializeField] private Sprite survivalImage;
    [SerializeField] private Sprite eventImage;
    [SerializeField] private Sprite shopImage;
    [SerializeField] private Sprite miniBossImage;
    [SerializeField] private Sprite bossImage;
    [SerializeField] private GameManager manager;
    private int level; private int section;
    private String report;

    void Start()
    {
        level = 0;
        section = 2;
        node13.onClick.AddListener(() => {clickedNode(1,3);}); //add the rest of the buttons later
        node22.onClick.AddListener(() => {clickedNode(2,2);});
        node24.onClick.AddListener(() => {clickedNode(2,4);});

        //todo, assign nodes here. how? idk
        node13.GetComponent<Image>().sprite = combatImage;
        node53.GetComponent<Image>().sprite = miniBossImage;
        node113.GetComponent<Image>().sprite = bossImage;
        //the first node is always a combat node, and set the miniboss and boss nodes automatically
    }

    public void clickedNode(int clickedLevel, int clickedSection) {
        if (level + 1 == clickedLevel && (section + 1 == clickedSection || section -1 == clickedSection)) { //check if the button clicked is a valid one
            switch (clickedLevel, clickedSection) {
            case (1,3):
                node13.interactable = false;
                    report = "combat";
                    break;
            }
            manager.ReceiveCommand(report);
        }
    }
}
