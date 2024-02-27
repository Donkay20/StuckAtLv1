using System.Collections;
using TMPro;
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
    */

    [SerializeField] private int identity;
    private bool containsSkill = false;
    private bool absorbBulletAvailable = true;
    private int skillID = 0, skillUses = 0;
    private float cooldown, activeCD, cooldownModifier;
    private bool coolingDown = false;
    private int[] commonUpgrades = new int[15];
    private int[] rareUpgrades = new int[15];
    private int[] legendaryUpgrades = new int[15];
    [SerializeField] private Character character;
    [SerializeField] private Image skillImage;                          
    //display for the skill image on the UI
    [SerializeField] private TextMeshProUGUI uIText;                    
    //display for the skill usages on the UI
    [SerializeField] private GameObject bullet;                         
    //exclusively for the absorption bullet
    [SerializeField] private GameObject[] attack = new GameObject[2];   
    //this will expand in accordance to the # of attacks we have
    [SerializeField] private Image cooldownFill;
    [SerializeField] private TextMeshProUGUI cooldownValueText;
    [SerializeField] private Transform bulletTransform;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SlotManager slotManager;

    private void Start() {
        absorbBulletAvailable = true;
        containsSkill = false;
    }

    void OnEnable() {
        CalculateWeightPenalty();
    }
    
    private void Update() {
        //for handling cooldown. precise fill means we can't really use a coroutine..
        if (activeCD > 0) {
            coolingDown = true; cooldownValueText.gameObject.SetActive(true);
            activeCD -= Time.deltaTime;
            if (activeCD > 1) {
                cooldownValueText.text = activeCD.ToString("f0");
            } else {
                cooldownValueText.text = activeCD.ToString("f1");
            }
            cooldownFill.fillAmount = activeCD/cooldown*cooldownModifier;
        } else {
            coolingDown = false; cooldownValueText.gameObject.SetActive(false);
        }
    }

    public int Identity { get => identity; set => identity = value; }                                           
    //slot number, assigned by SlotManager class
    public bool ContainsSkill { get => containsSkill; set => containsSkill = value; }                           
    //determines whether absorption bullet is shot or not
    public bool AbsorbBulletAvailable { get => absorbBulletAvailable; set => absorbBulletAvailable = value; }   
    //variable that prevents absorption bullet from being shot until the current shot one dissipates

    public void Engage() {     
        if (!coolingDown) {
            //slot won't do anything if it's on cooldown.
            if (!containsSkill && absorbBulletAvailable) {
                //handles slot; whether to use skill or to absorb skill
                Instantiate(bullet, bulletTransform.position, Quaternion.identity, transform);
                this.absorbBulletAvailable = false;                                                     
                //the absoption bullet class will set this value back to true when it dissipates
            } else {
                Instantiate(attack[skillID], bulletTransform.position, Quaternion.identity, transform); 
                //launches the skill, positioned from the player. more checks will need to be added as the player gets more types of skills.

                //-beginning of slot effects-
                if (containsSkill) {
                    character.Heal((5 * commonUpgrades[3]) + (7 * rareUpgrades[3]) + (10 * legendaryUpgrades[3]));
                }
                Debug.Log("Heal applied: " + ((5 * commonUpgrades[3]) + (7 * rareUpgrades[3]) + (10 * legendaryUpgrades[3])));
                //Apply overheal on cast (Upgrade 3)

                //-end of slot effects-

                if (skillUses > 0) {
                    skillUses--;
                }
                uIText.text = skillUses.ToString();
                //drain a skill use and reflect it in the UI

                activeCD = cooldown * cooldownModifier;
                //set the cooldown when a skill is used

                if (skillUses <= 0) {
                    containsSkill = false;
                    skillImage.sprite = attack[0].GetComponent<SpriteRenderer>().sprite;
                    skillID = 0; skillUses = 0; cooldown = 0;
                }
                //if skill has run out of uses, reset everything
            }
        }                                                                                 
    }

    public void BattleEnd() {
        //When a stage is cleared, reset all cooldowns
        activeCD = 0;
        cooldownFill.fillAmount = 0; 
        cooldownValueText.gameObject.SetActive(true); 
        coolingDown = false;
    }

    public void AcquireSkill(int ID, int uses, float cd) {    //calls to this method require the ID of the skill and the amt of base uses the skill has.
        if(ID != 0) {
            skillID = ID;
            skillUses = uses;           //for slot buffs that add more uses, a modifier would be applied here
            cooldown = cd;
            skillImage.sprite = attack[ID].GetComponent<SpriteRenderer>().sprite;
            uIText.text = skillUses.ToString();
            containsSkill = true;
        }
    }

    public void ApplySlotUpgrade(string rarity, int upgrade) {
        switch(rarity) {
            case "common":
                commonUpgrades[upgrade]++;
                Debug.Log("Common upgrade applied." );
                break;
            case "rare":
                rareUpgrades[upgrade]++;
                Debug.Log("Rare upgrade applied.");;
                break;
            case "legendary":
                legendaryUpgrades[upgrade]++;
                Debug.Log("Legendary upgrade applied.");
                break;
        }
    }

    private void CalculateWeightPenalty() {
        Debug.Log("Identity: " + identity + ". Identity-1: " + (identity-1));
        Debug.Log(gameManager.GetWeight(identity));
        int weight = gameManager.GetWeight(identity);
        if (weight == 0) {
            cooldownModifier = 0.9f;
        }
        if (weight >= 1 && weight <= 3) {
            cooldownModifier = 1 + 0.1f * weight;
        }
        if (weight >= 4 && weight <= 6) {
            cooldownModifier = 1 + 0.2f * weight;
        }
        if (weight >= 7 && weight <= 9) {
            cooldownModifier = 1 + 0.3f * weight;
        }
        if (weight >= 10) {
            cooldownModifier = 1 + 0.5f * weight;
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

    public bool CriticalHit(Slot slot) {
        bool isCrit = false; int critChance = 5;
        //apply crit bonuses here, todo
        if (Random.Range(1,101) <= critChance) {isCrit = true;}
        return isCrit;
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

    /*
    List of real upgrades:
    0. Attacks [from this slot] increase in size by 5%.
    1. Attacks [from this slot] increase in damage by 10%.
    2. Attacks [from this slot] result in a critical hit 10% more often.
    3. Attacks [from this slot] grant a buff that boosts damage by 5%, lasting for 3 seconds.
    4. Attacks [from this slot] do 20% more critical damage.
    5. Attacks [from this slot] slow enemies by 20% for 5 seconds.
    6. Attacks [from this slot] boost movement speed by 10% for 5 seconds.
    7. Skills acquired [to this slot] have +1 skill usage.
    8. Skills [from this slot] have a 50% chance of inflicting anemia.
    9. Skills [from this slot] last 20% longer.
    10. Skills [from this slot] pushes away all nearby enemies.
    11. Skills [from this slot] grant 3 (over)healing.
    12. Kills [from this slot] grant a 5% increased chance to spawn a Treasure Chest.
    13. Kills from this slot grant increased gold.
    14. Kills from this slot clear the most recent de-buff.
    */
}
