using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Button confirm; //temp button
    [SerializeField] private Button buyHP, buyAfterimage, buyUpgrade, buyDamageBoost, exitButton;
    [SerializeField] private TextMeshProUGUI hp, afterimages, money, damage, shopkeeperText;
    [SerializeField] private TextMeshProUGUI hpButtonText, afterimagesButtonText, damageButtonText, upgradeButtonText;
    [SerializeField] private GameManager manager;
    [SerializeField] private Character player;
    private int healthCost, afterimageCost, upgradeCost, damageCost;
    private int damageCounter;
    private bool doNotReset;
    private Animator anim;
    void Awake()
    {
        InitializeButtons();
        anim = GetComponent<Animator>();
        doNotReset = false;
        healthCost = 20;
        afterimageCost = 50;
        upgradeCost = 200;
        damageCost = 1;
        damageCounter = 0;
        anim.SetTrigger("Intro");
    }
    
    void OnEnable() {
        if (!doNotReset) {
            shopkeeperText.text = "Welcome."; //temp
            hp.text = player.currentHp.ToString();
            money.text = player.money.ToString();
            afterimages.text = player.afterimage.ToString("f1");
            healthCost = 20; hpButtonText.text = "10 HP | $" + healthCost;
            afterimageCost = 50; afterimagesButtonText.text = "Afterimage-time | $" + afterimageCost;
            upgradeCost = 150; upgradeButtonText.text = "Upgrade | $" + upgradeCost;
            damageCost = 1; damageButtonText.text = "Damage | $" + damageCost;
        }
        CheckPurchasability();
        anim.SetTrigger("Intro");
    }

    private void CheckPurchasability() {
        if(player.money < afterimageCost || player.afterimage >= 10.0f) {
            buyAfterimage.interactable = false;
        } else {
            buyAfterimage.interactable = true;
        }

        if(player.money < healthCost || player.currentHp >= 999) {
            buyHP.interactable = false;
        } else {
            buyHP.interactable = true;
        }

        if(player.money < upgradeCost) {
            buyUpgrade.interactable = false;
        } else {
            buyUpgrade.interactable = true;
        }

        if(player.money < damageCost) {
            buyDamageBoost.interactable = false;
        } else {
            buyDamageBoost.interactable = true;
        }
    }

    private void Purchase(string type) {
        switch (type) {
            case "hp":
                player.money -= healthCost; 
                money.text = player.money.ToString();
                if (player.currentHp + 10 > 999) {
                    player.currentHp = 999;
                } else {
                    player.currentHp += 10;
                }
                hp.text = player.currentHp.ToString();
                if (healthCost < 100) {
                    healthCost += 20; hpButtonText.text = "10 HP | $" + healthCost;
                }
                shopkeeperText.text = "Health purchased."; //temp
                break;
            case "afterimage":
                player.money -= afterimageCost; 
                money.text = player.money.ToString();
                if (player.afterimage + 1 > 10) {
                    player.afterimage = 10;
                } else {
                    player.afterimage += 1.0f;
                }
                afterimages.text = player.afterimage.ToString("f1");
                if (afterimageCost < 250) {
                    afterimageCost += 50; afterimagesButtonText.text = "Afterimage-time | $" + afterimageCost;
                }
                shopkeeperText.text = "Afterimage-time purchased."; //temp
                break;
            case "upgrade":
                player.money -= upgradeCost; 
                money.text = player.money.ToString();
                upgradeCost *= 2;
                upgradeButtonText.text = "Upgrade | $" + upgradeCost;
                shopkeeperText.text = "Upgrade purchased."; //temp
                doNotReset = true;
                manager.ReceiveCommand("upgrade");
                break;
            case "damage":
                player.money -= damageCost; money.text = player.money.ToString();
                manager.UpgradeShopDamageBonus();
                damageCost *= 2; 
                damageButtonText.text = "Damage | $" + damageCost;
                damageCounter++; damage.text = damageCounter.ToString() + "%";
                shopkeeperText.text = "Damage boost purchased."; //temp
                break;
        }
        CheckPurchasability();
    }

    private void Exit() {
        doNotReset = false;
        anim.SetTrigger("Outro");
    }

    public void ExitAfterOutroFinished() {
        manager.ReceiveCommand("map");
    }

    private void InitializeButtons() {
        confirm.onClick.AddListener(() => {Exit();});
        buyHP.onClick.AddListener(() => {Purchase("hp");});
        buyAfterimage.onClick.AddListener(() => {Purchase("afterimage");});
        buyUpgrade.onClick.AddListener(() => {Purchase("upgrade");});
        buyDamageBoost.onClick.AddListener(() => {Purchase("damage");});
        exitButton.onClick.AddListener(() => {Exit();});
    }
}
