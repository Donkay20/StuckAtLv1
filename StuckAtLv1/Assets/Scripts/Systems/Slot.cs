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
    [SerializeField] private BuffManager buffManager;

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

                //-beginning of slot effects-
                if (containsSkill) {
                    if (commonUpgrades[3] > 0) {
                        character.Heal(commonUpgrades[3] * 3); //common 3
                    }
                    if (commonUpgrades[5] > 0) {
                        buffManager.AddBuff("power", commonUpgrades[5] * 0.05f, 3f);
                    }
                    if (commonUpgrades[8] > 0) {
                        buffManager.AddBuff("speed",commonUpgrades[8] * 0.1f, 3f);
                    }
                }

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
            skillUses = uses;
            cooldown = cd;
            skillImage.sprite = attack[ID].GetComponent<SpriteRenderer>().sprite;
            uIText.text = skillUses.ToString();
            containsSkill = true;

            //start of slot bonuses
            skillUses += commonUpgrades[9];
            //end of slot bonuses
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

    public bool CriticalHit() {
        //base crit chance is 5%
        bool isCrit = false; int critChance = 5; 
        //critical hit bonuses start here
        critChance += commonUpgrades[4] * 10; //common 4
        //critical hit bonuses end here
        if (Random.Range(1,101) <= critChance) {isCrit = true;}
        //calculate crit odds
        return isCrit;
    }

    public float CriticalDamage() {
        //critical strikes do 200% dmg at base
        float critDmg = 2; 
        //crit damage bonuses start here
        critDmg += commonUpgrades[6] * 0.2f; //common 6
        //crit damage bonuses end here
        return critDmg;
    }

    /*
    List of upgrades:
    Common: 
    0.  Damage +10%                                     - atk1 done
    1.  Size +5%                                        - atk1 done
    2.  Duration +20%                                   - atk1 done
    3.  Overheal +3                                     - OK
    4.  Critical chance +10%                            - OK
    5.  Damage buff +5%, 3s duration                    - OK
    6.  Critical damage +20%                            - OK
    7.  Inflict slow -20%, 3s duration                  - atk1 done
    8.  Movement speed buff (+10%), 3s duration         - OK
    9.  +1 max skill usage                              - OK
    10. 50% chance of inflicting Anemia                 - atk1 done
    11. Knockback nearby enemies                        - todo
    12. Treasure Chest spawn chance ON KILL + 5%        - atk1 done
    13. Gold ON KILL +5                                 - atk1 done
    14. Debuff cleanse ON KILL +1                       - atk1 done

    Rare: 
    0.
    1.
    2.
    3.
    4.
    5.
    6.
    7.
    8.
    9.
    10.
    11.
    12.
    13.
    14.

    Legendary:
    0.
    1.
    2.
    3.
    4.
    5.
    6.
    7.
    8.
    9.
    10.
    11.
    12.
    13.
    14.
    */
}
