using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Button confirm; //temp button
    [SerializeField] private Button buyHP, buyAfterimage, buyUpgrade, exitButton;
    [SerializeField] private TextMeshProUGUI hp, afterimages, money;
    [SerializeField] private GameManager manager;
    void Awake()
    {
        InitializeButtons();
    }
    
    void OnEnable() {
        CheckPurchasability();
    }

    private void CheckPurchasability() {
        
    }

    private void Purchase(string type) {
        switch (type) {
            case "hp":
                break;
            case "afterimage":
                break;
            case "upgrade":
                break;
        }
        CheckPurchasability();
    }

    private void Exit() {
        manager.ReceiveCommand("map");
    }

    private void InitializeButtons() {
        confirm.onClick.AddListener(() => {Exit();});
        //temp button
        buyHP.onClick.AddListener(() => {Purchase("hp");});
        buyAfterimage.onClick.AddListener(() => {Purchase("afterimage");});
        buyUpgrade.onClick.AddListener(() => {Purchase("upgrade");});
        exitButton.onClick.AddListener(() => {Exit();});
    }
}
