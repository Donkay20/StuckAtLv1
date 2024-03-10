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

    [SerializeField] private int identity; //slot number, assigned by SlotManager class
    private readonly float BASE_SLOT_COOLDOWN = 3f;
    private bool containsSkill = false;
    private bool atkDmgBoostAvailable = true;   //common 9
    [SerializeField] private int skillID = 0;
    [SerializeField] private int skillUses = 0; 
    private float cooldown, activeCD, cooldownModifier;
    private bool coolingDown = false;
    private int[] commonUpgrades = new int[15];
    private int[] rareUpgrades = new int[15];
    private int[] legendaryUpgrades = new int[15];
    [SerializeField] private Character character;
    [SerializeField] private Movement movement;
    [SerializeField] private Image skillImage;
    //display for the skill image on the UI
    [SerializeField] private TextMeshProUGUI uIText;
    //display for the skill usages on the UI
    //[SerializeField] private GameObject bullet;
    //exclusively for the absorption bullet
    [SerializeField] private GameObject[] attack = new GameObject[2];
    //this will expand in accordance to the # of attacks we have
    [SerializeField] private Image cooldownFill;
    [SerializeField] private TextMeshProUGUI cooldownValueText;
    [SerializeField] private Transform bulletTransform;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SlotManager slotManager;
    [SerializeField] private BuffManager buffManager;
    [SerializeField] private GameObject anemiaScreenBlastPrefab;
    [SerializeField] private GameObject knockbackCirclePrefab;
    public int Identity { get => identity; set => identity = value; }

    private void Start() {
        atkDmgBoostAvailable = true;
        containsSkill = false;
        if (identity == 1) {
            //use this to test for upgrades
        }
    }

    void OnEnable() {
        CalculateWeightPenalty();
    }

    private void Update() {
        //for handling cooldown.
        if (activeCD > 0) {
            coolingDown = true; cooldownValueText.gameObject.SetActive(true);
            activeCD -= Time.deltaTime;
            if (activeCD > 1) {
                cooldownValueText.text = activeCD.ToString("f0");
            } else {
                cooldownValueText.text = activeCD.ToString("f1");
            }
            cooldownFill.fillAmount = activeCD / (BASE_SLOT_COOLDOWN + cooldownModifier);
        }

        if (activeCD <= 0 && coolingDown) {
            coolingDown = false; cooldownValueText.gameObject.SetActive(false);
            atkDmgBoostAvailable = true;                                            //common 9
        }

        if (!containsSkill) {
            skillImage.sprite = attack[0].GetComponent<SpriteRenderer>().sprite;
        }

        if (!coolingDown) {
            cooldownValueText.gameObject.SetActive(false);
        }
    }

    public void Engage() {
        if (!coolingDown) { //slot won't do anything if it's on cooldown.
            if (containsSkill) {
                Instantiate(attack[skillID], bulletTransform.position, Quaternion.identity, transform);
                //-BEGIN slot effects-
                if (commonUpgrades[3] > 0) {                                        //common 3 (rare 7)
                    if (rareUpgrades[7] > 0) {
                        character.Heal(commonUpgrades[3] * 3 * (rareUpgrades[7] * 2));
                    } else {
                        character.Heal(commonUpgrades[3] * 3);
                    }
                    Debug.Log("Common | 3");
                }

                if (commonUpgrades[5] > 0) {                                        //common 5
                    buffManager.AddBuff("power", commonUpgrades[5] * 0.05f, 3f);
                    Debug.Log("Common | 5");
                }

                if (commonUpgrades[8] > 0) {                                        //common 8
                    buffManager.AddBuff("speed", commonUpgrades[8] * 0.1f, 3f);
                    Debug.Log("Common | 8");
                }

                if (commonUpgrades[11] > 0) {                                       //common 11
                    Instantiate(knockbackCirclePrefab, this.transform);
                    Debug.Log("Common | 11");
                }

                if (commonUpgrades[14] > 0) {                                       //common 14
                    slotManager.AddTempAtkSpd(1);
                }

                if (rareUpgrades[0] > 0) {                                          //rare 0
                    movement.ActiveDashCD -= movement.ActiveDashCD * rareUpgrades[0] * 0.1f;
                    Debug.Log("Rare | 0");
                }

                if (rareUpgrades[5] > 0 && character.afterimage > 0) {              //rare 5
                    buffManager.AddBuff("speed", rareUpgrades[5] * (0.05f * character.afterimage), 5f);
                    Debug.Log("Rare | 5");
                }

                if (rareUpgrades[6] > 0) {                                          //rare 6
                    if (character.currentHp > 10) {
                        int atkSpdGain = character.currentHp - 10;
                        character.TakeDamage(atkSpdGain);
                        slotManager.AddTempAtkSpd(atkSpdGain / 10);
                        Debug.Log("Rare | 6, success");
                    } else {
                        Debug.Log("Rare | 6, failure");
                    }
                }

                if (legendaryUpgrades[7] > 0) {                                     //legendary 7
                    Instantiate(anemiaScreenBlastPrefab, this.transform);
                    Debug.Log("Legendary | 7 (on-cast)");
                }

                if (legendaryUpgrades[9] > 0) {                                     //legendary 9
                    if (!buffManager.IsBloodsuckerActive()) {
                        buffManager.AddBuff("bloodsucker", 0, 2);
                        Debug.Log("Legendary | 9, success");
                    } else {
                        Debug.Log("Legendary | 9, fail");
                    }
                }
                //-END slot effects-

                //todo, change this stuff below; skills only have 1 use now
                if (skillUses > 0) {
                    skillUses--;
                }
                uIText.text = skillUses.ToString();
                //drain a skill use and reflect it in the UI

                activeCD = cooldown;
                //set the cooldown when a skill is used

                if (skillUses <= 0) {
                    containsSkill = false;
                    skillImage.sprite = attack[0].GetComponent<SpriteRenderer>().sprite;
                    skillID = 0; skillUses = 0; cooldown = 0;
                    //if skill has run out of uses, reset everything
                }
            }
        }
    }

    public void BattleEnd() {   //When a stage is cleared, reset all cooldowns
        activeCD = 0;
        cooldownFill.fillAmount = 0;
        cooldownValueText.gameObject.SetActive(true);
        coolingDown = false;
    }

    public void AcquireSkill(int ID) {    //calls to this method require the ID of the skill and the amt of base uses the skill has.
        if (ID != 0) {
            skillID = ID;
            skillUses = 1;
            cooldown = BASE_SLOT_COOLDOWN + cooldownModifier;
            skillImage.sprite = attack[ID].GetComponent<SpriteRenderer>().sprite;
            uIText.text = skillUses.ToString();
            containsSkill = true;
        }
    }

    public void DumpSkill() {
        containsSkill = false;
        skillImage.sprite = attack[0].GetComponent<SpriteRenderer>().sprite;
        skillID = 0; skillUses = 0; cooldown = 0;
    }

    public void TempAtkDmgBoost() {                         //common 9
        if (atkDmgBoostAvailable) {
            slotManager.AddTempAtkDmg(commonUpgrades[9]);
            atkDmgBoostAvailable = false;
        }
    }

    public void ApplySlotUpgrade(string rarity, int ID) {
        switch(rarity) {
            case "common":
                commonUpgrades[ID]++;
                Debug.Log("Common upgrade applied." );
                break;
            case "rare":
                rareUpgrades[ID]++;
                Debug.Log("Rare upgrade applied.");;
                break;
            case "legendary":
                legendaryUpgrades[ID]++;
                Debug.Log("Legendary upgrade applied.");
                break;
        }
    }

    private void CalculateWeightPenalty() {
        Debug.Log("Identity: " + identity + ". Identity-1: " + (identity-1));
        Debug.Log(gameManager.GetWeight(identity));
        int weight = gameManager.GetWeight(identity);
        if (weight == 0) {
            cooldownModifier = -0.25f;
        }
        if (weight >= 1 && weight <= 3) {
            cooldownModifier = 0.5f;
        }
        if (weight >= 4 && weight <= 6) {
            cooldownModifier = 1f;
        }
        if (weight >= 7 && weight <= 9) {
            cooldownModifier = 2f;
        }
        if (weight >= 10) {
            cooldownModifier = 4f;
        }
    }

    public bool IsCoolingDown() {
        return coolingDown;
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
        bool isCrit = false; int critChance = 5;        //base crit chance is 5%

        //critical hit chance bonuses start here
        if (commonUpgrades[4] > 0) {                    //common 4
            critChance += commonUpgrades[4] * 10;
        }

        if (rareUpgrades[3] > 0) {                      //rare 3
            if (character.currentHp <= 10) {
                critChance += 30 * rareUpgrades[3];
            }
        }
        //critical hit chance bonuses end here

        if (Random.Range(1,101) <= critChance) {isCrit = true;}     //calculate crit odds

        //bonuses upon crit begin here
        if (rareUpgrades[4] > 0 && isCrit) {            //rare 4
            if (character.currentHp <= 10) {
                character.GainAfterimage(0.5f, false);
            }
        }

        if (legendaryUpgrades[4] > 0 && isCrit) {       //legendary 4
            if (!buffManager.IsAvariceActive()) {
                buffManager.AddBuff("avarice", 0, 3);
                Debug.Log("Legendary | 9, success");
            } else {
                Debug.Log("Legendary | 9, fail");
            }
        }
        //bonuses upon crit end here
        return isCrit;
    }

    public float CriticalDamage() {
        float critDmg = 2;                              //critical strikes do 200% dmg at base
        //crit damage bonuses start here
        if (commonUpgrades[6] > 0) {                    //common 6
            critDmg += commonUpgrades[6] * 0.2f;
        }
        critDmg += character.GetCriticalDamageModifier();    //buffs
        //crit damage bonuses end here
        return critDmg;
    }

    /*
    List of upgrades:
    Common:
    0.  Damage +10%                                     - OK
    1.  Size +5%                                        - OK
    2.  Duration +20%                                   - OK
    3.  Overheal +3                                     - OK
    4.  Critical chance +10%                            - OK
    5.  Damage buff +5%, 3s duration                    - OK
    6.  Critical damage +20%                            - OK
    7.  Inflict slow -20%, 3s duration                  - OK
    8.  Movement speed buff (+10%), 3s duration         - OK
    9.  +1 basic attack damage (room)                   - OK
    10. 50% chance of inflicting Anemia                 - OK
    11. Knockback nearby enemies                        - OK
    12. Treasure Chest spawn chance ON KILL + 5%        - OK
    13. Gold ON KILL +5                                 - OK
    14. Attack speed buff +1% (room)                    - OK

    Rare:
    0.  -10% Dash cooldown                              - OK
    1.  Size +5%, Damage +10%, Duration +20%            - OK
    2.  3 slot weight, +1000g                           - OK
    3.  No overheal = +30% crit chance                  - OK
    4.  No overheal = crit = 0.5s afterimage            - OK
    5.  Movement speed +5% * 1s of afterimage           - OK
    6.  Turn overheal to atk spd (room)                 - OK
    7.  x2 overhealing                                  - OK
    8.  Overheal 1 for each enemy hit                   - OK
    9.  Bonus dmg * 0.5% of HP                          - OK
    10. Overheal = +size%                               - OK
    11. Inflict Anemia on-hit, 10s                      - OK
    12. Hitting anemic enemy = +gold                    - OK
    13. Anemia spread                                   - OK
    14. Anemia inflict = +20% skill dmg boost           - OK

    Legendary:
    0.  Enemy explodes on-kill                          - OK
    1.  kill = +penetration                             - OK
    2.  Skill upgrade spread                            - OK
    3.  no overheal = hit = +afterimage/++crit (no cap) - OK
    4.  Crit = avarice                                  - OK
    5.  crit kill = perma atk spd                       - OK
    6.  Crit = +crit dmg buff                           - OK
    7.  Damage all anemic enemies on use or on kill     - OK
    8.  Anemia on anemic enemy = anemic shock           - OK
    9.  Attack = bloodsucker (basic atks = anemia)      - OK
    10. speeds up anemia tick rate if anemic            - OK
    11. kill = treasure chest spawn chance% * overheal% - OK
    12. dmg = slow (scales with overhealth)             - OK
    13. kill = bulwark buff (drain reversed)            - OK
    14. kill = 1 perma basic atk dmg per 100hp          - OK
    */
}
