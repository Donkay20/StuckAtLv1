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
    [SerializeField] private Movement movement;
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
    [SerializeField] private GameObject anemiaScreenBlastPrefab;
    [SerializeField] private GameObject knockbackCirclePrefab;

    private void Start() {
        absorbBulletAvailable = true;
        containsSkill = false;
        if (identity == 1) {
            //use this to test for upgrades
        }
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
            cooldownFill.fillAmount = activeCD / cooldown * cooldownModifier;
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

                //-BEGIN slot effects-
                if (containsSkill) {
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

                    if (rareUpgrades[0] > 0) {                                          //rare 0
                        movement.ActiveDashCD -= movement.ActiveDashCD * rareUpgrades[0] * 0.1f;
                        Debug.Log("Rare | 0");
                    }

                    if (rareUpgrades[2] > 0) {                                          //rare 2
                        FindAnyObjectByType<SlotManager>().RareTwoCooldownCut(identity, rareUpgrades[2]);
                        Debug.Log("Rare | 2");
                    }

                    if (rareUpgrades[5] > 0 && character.afterimages > 0) {             //rare 5
                        buffManager.AddBuff("speed", rareUpgrades[5] * (0.05f * character.afterimages), 5f);
                        Debug.Log("Rare | 5");
                    }

                    if (rareUpgrades[6] > 0) {                                          //rare 6
                        if (character.currentHp > 10) {
                            int goldGain = character.currentHp - 10;
                            character.TakeDamage(goldGain);
                            character.GainMoney(goldGain * rareUpgrades[6]);
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
                            buffManager.AddBuff("bloodsucker", 0, 3);
                            Debug.Log("Legendary | 9, success");
                        } else {
                            Debug.Log("Legendary | 9, fail");
                        }
                    }

                    if (legendaryUpgrades[14] > 0) {                                    //legendary 14
                        if (character.currentHp - 10 > character.money) {
                            int goldGain = character.currentHp - 10 - character.money;
                            character.TakeDamage(goldGain);
                            character.GainMoney((goldGain * 10) * legendaryUpgrades[14]);
                            Debug.Log("Legendary | 14, success");
                        } else {
                            Debug.Log("Legendary | 14, fail");
                        }
                    }
                }
                //-END slot effects-

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
                    //if skill has run out of uses, reset everything
                }
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

    public int SkillUses() {
        return skillUses;
    }

    public void GetSkillUses() {
        skillUses++;
    }

    public void RefundCooldown() {                      //legendary 4
        activeCD = 0.01f;
    }

    public void CutCooldown(int intensity) {            //rare 2
        activeCD -= cooldown * (0.1f * intensity);
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
        bool isCrit = false; int critChance = 5;        //base crit chance is 5%

        //critical hit chance bonuses start here
        if (commonUpgrades[4] > 0) {                    //common 4
            critChance += commonUpgrades[4] * 10;
        }

        if (rareUpgrades[3] > 0) {                      //rare 3
            if (character.currentHp <= 10) {
                critChance += 50 * rareUpgrades[3];
            }
        }
        //critical hit chance bonuses end here

        if (Random.Range(1,101) <= critChance) {isCrit = true;}     //calculate crit odds

        //bonuses upon crit begin here
        if (rareUpgrades[4] > 0) {                      //rare 4
            if (character.currentHp <= 10) {
                character.GainAfterimage(1);
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
        critDmg += character.CriticalDamageModifier;    //buffs
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
    9.  +1 max skill usage                              - OK
    10. 50% chance of inflicting Anemia                 - OK
    11. Knockback nearby enemies                        - OK
    12. Treasure Chest spawn chance ON KILL + 5%        - OK
    13. Gold ON KILL +5                                 - OK
    14. Debuff cleanse ON KILL +1                       - OK

    Rare:
    0.  -10% Dash cooldown                              - OK
    1.  Size +5%, Damage +10%, Duration +20%            - OK
    2.  Cooldown of other slots -10%                    - OK
    3.  No overheal = +50% crit chance                  - OK
    4.  No overheal = crit = +1 afterimage              - OK
    5.  Movement speed +5% * afterimages                - OK
    6.  Turn overheal to gold                           - OK
    7.  x2 overhealing                                  - OK
    8.  Overheal for 10% of dmg dealt                   - OK
    9.  Bonus dmg = 10% of HP                           - OK
    10. Overheal = +size%                               - OK
    11. Inflict Anemia on-hit, 10s                      - OK
    12. Hitting anemic enemy = +gold                    - OK
    13. Anemia spread                                   - OK
    14. Anemia inflict = +20% dmg boost                 - OK

    Legendary:
    0.  Enemy explodes on-kill                          - OK
    1.  +1 skill usage on-kill                          - OK
    2.  Skill upgrade spread                            - OK
    3.  +Gold = afterimages on-hit (2x for crit)        - OK
    4.  Crit = cd refund                                - OK
    5.  Crit = +gold, kill = ++gold                     - OK
    6.  Crit = +crit dmg buff                           - OK
    7.  Damage all anemic enemies on use or on kill     - OK
    8.  Anemia on anemic enemy = anemic shock           - OK
    9.  Attack = bloodsucker (anemia dmg = overheal)    - OK
    10. Doubles anemic duration if anemic               - OK
    11. kill = treasure chest spawn chance% * overheal% - OK
    12. dmg = slow (scales with overhealth)             - OK
    13. kill = bulwark buff (drain reversed)            - OK
    14. if hp>gold, hp=gold and gold += hplost*10       - OK
    */
}
