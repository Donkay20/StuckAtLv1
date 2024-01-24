using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Button confirm; //temp button
    [SerializeField] private Button buyHP, buyAfterimage, buyUpgrade, exitButton;
    [SerializeField] private TextMeshProUGUI hp, afterimages, money, shopkeeperText;
    [SerializeField] private GameManager manager;
    [SerializeField] private Character player;
    void Awake()
    {
        InitializeButtons();
    }
    
    void OnEnable() {
        shopkeeperText.text = "buy my shit"; //temp
        hp.text = player.currentHp.ToString();
        money.text = player.money.ToString();
        afterimages.text = player.afterimages.ToString();
        CheckPurchasability();
    }

    private void CheckPurchasability() {
        if(player.money < 5) {
            buyAfterimage.interactable = false;
        } else {
            buyAfterimage.interactable = true;
        }

        if(player.money < 10) {
            buyHP.interactable = false;
        } else {
            buyHP.interactable = true;
        }

        if(player.money < 150) {
            buyUpgrade.interactable = false;
        } else {
            buyUpgrade.interactable = true;
        }
    }

    private void Purchase(string type) {
        switch (type) {
            case "hp":
                player.money -= 10; money.text = player.money.ToString();
                player.currentHp += 10; hp.text = player.currentHp.ToString();
                shopkeeperText.text = "why did you buy health"; //temp
                break;
            case "afterimage":
                player.money -= 5; money.text = player.money.ToString();
                player.afterimages += 1; afterimages.text = player.afterimages.ToString();
                shopkeeperText.text = "why did you buy afterimages"; //temp
                break;
            case "upgrade":
                player.money -= 150; money.text = player.money.ToString();
                shopkeeperText.text = "how did you even buy an upgrade"; //temp
                manager.ReceiveCommand("upgrade");
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
