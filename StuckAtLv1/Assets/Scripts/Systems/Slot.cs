using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    /*
    This class is responsible for handling behavior for each slot individually.
    As such, each attack needs to be placed in each slot respectively. Current plans = 5 slots.

    The two public functions are Engage() and AcquireSkill(); more may need to be added in the future.

    Engage: decides whether or not a skill is cast or the absorption bullet is fired. Absorption bullet should only be fire if there is no skill currently in the slot.
    AcquireSkill: method for handling when the absorption bullet hits an enemy and steals their power. Needed paramters: the skill's ID & how many uses the skill gets.

    To add later:
        > variables to alter values of skill dependent on bonuses acquired
        > more conditions for Engage(); not every skill will be a projectile attack and Engage() currently doesn't cover that
        > if necessary, pooling will need to be used over instantiation if performance suffers because of it

    Current Skill IDs and their correspondences:
    0 - Empty
    1 - Bone Toss (from Skeleton 1)
    2 - Boulder Toss (from Golem 1) [ideal position: 4]
    */

    private int identity;
    private bool containsSkill = false;
    private bool absorbBulletAvailable = true;
    private int skillID = 0, skillUses = 0;
    private int[] commonUpgrades = new int[13];
    private int[] rareUpgrades = new int[13];
    private int[] legendaryUpgrades = new int[13];
    [SerializeField] private Character character;
    [SerializeField] private Image skillImage;                          //display for the skill image on the UI
    [SerializeField] private TextMeshProUGUI uIText;                    //display for the skill usages on the UI
    [SerializeField] private GameObject bullet;                         //exclusively for the absorption bullet
    [SerializeField] private GameObject[] attack = new GameObject[2];   //this will expand in accordance to the # of attacks we have
    [SerializeField] private Transform bulletTransform;
    

    private void Start() {
        absorbBulletAvailable = true;
        containsSkill = false;
    }

    public int Identity { get => identity; set => identity = value; }                                           //slot number, assigned by SlotManager class
    public bool ContainsSkill { get => containsSkill; set => containsSkill = value; }                           //determines whether absorption bullet is shot or not
    public bool AbsorbBulletAvailable { get => absorbBulletAvailable; set => absorbBulletAvailable = value; }   //variable that prevents absorption bullet from being shot until the current shot one dissipates

    public void Engage() {                                                                                      //handles slot; whether to use skill or to absorb skill
        if (!containsSkill && absorbBulletAvailable) {
            Instantiate(bullet, bulletTransform.position, Quaternion.identity, transform);
            this.absorbBulletAvailable = false;                                                     //the absoption bullet class will set this value back to true when it dissipates
        } else {
            Instantiate(attack[skillID], bulletTransform.position, Quaternion.identity, transform); //launches the skill, positioned from the player. more checks will need to be added as the player gets more types of skills.
            //-beginning of slot effects-
            //Apply overheal on cast (Upgrade 3)
            character.Heal((5 * commonUpgrades[3]) + (7 * rareUpgrades[3]) + (10 * legendaryUpgrades[3]));
            Debug.Log("heal applied:" + ((5 * commonUpgrades[3]) + (7 * rareUpgrades[3]) + (10 * legendaryUpgrades[3])));
            //-end of slot effects-
            if (skillUses > 0) {
                skillUses--;
            }
            uIText.text = skillUses.ToString();
            if (skillUses <= 0) {
                containsSkill = false;
                skillImage.sprite = attack[0].GetComponent<SpriteRenderer>().sprite;
                skillID = 0; skillUses = 0;
            }
        }
    }
    public void AcquireSkill(int ID, int uses) {    //calls to this method require the ID of the skill and the amt of base uses the skill has.
        if(ID != 0) {
            skillID = ID;
            skillUses = uses;           //for slot buffs that add more uses, a modifier would be applied here
            skillImage.sprite = attack[ID].GetComponent<SpriteRenderer>().sprite;
            uIText.text = skillUses.ToString();
            containsSkill = true;
        }
    }

    public void ApplySlotUpgrade(string rarity, int upgrade) {
        switch(rarity) {
            case "common":
                commonUpgrades[upgrade]++;
                Debug.Log("common upgrade applied." );
                Debug.Log("common damage: " + commonUpgrades[0]);
                Debug.Log("common size: " + commonUpgrades[1]);
                Debug.Log("common duration: " + commonUpgrades[2]);
                Debug.Log("common overheal: " + commonUpgrades[3]);
                break;
            case "rare":
                rareUpgrades[upgrade]++;
                Debug.Log("rare upgrade applied");
                Debug.Log("rare damage: " + rareUpgrades[0]);
                Debug.Log("rare size: " + rareUpgrades[1]);
                Debug.Log("rare duration: " + rareUpgrades[2]);
                Debug.Log("rare overheal: " + rareUpgrades[3]);
                break;
            case "legendary":
                legendaryUpgrades[upgrade]++;
                Debug.Log("legendary upgrade applied");
                Debug.Log("legendary damage: " + legendaryUpgrades[0]);
                Debug.Log("legendary size: " + legendaryUpgrades[1]);
                Debug.Log("legendary duration: " + legendaryUpgrades[2]);
                Debug.Log("legendary overheal: " + legendaryUpgrades[3]);
                break;
        }
    }

    public int GetCommonUpgrade(int upgrade) {
        return commonUpgrades[upgrade];
    }

    public int GetRareUpgrade(int upgrade) {
        return rareUpgrades[upgrade];
    }

    public int GetLegendaryUpgrade(int upgrade) {
        return legendaryUpgrades[upgrade];
    }

    /*
    List of upgrades (demo):
    Common: 
    0. Damage +20%
    1. Size +20%
    2. Duration +20%
    3. Overheal +5

    Rare:
    0. Damage +40%
    1. Size +30%
    2. Duration +40%
    3. Overheal +7

    Legendary:
    0. Damage +60%
    1. Size +40%
    2. Duration +60%
    3. Overheal +10
    */
}
