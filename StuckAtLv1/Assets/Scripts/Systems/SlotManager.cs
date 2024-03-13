using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    /*
    Manager/parent for every slot. Assigns an identity to each slot for many purposes, including applying bonuses.
    Currently only needs to account for 2, but will eventually be expanded to account for 5. At some point conditions will be added to lock slot usage until it is unlocked via game progress.
    */

    //public KeyCode slotKey1, slotKey2, slotKey3, slotKey4, slotKey5;
    [SerializeField] private int slotNum = 0;
    [SerializeField] private bool isSkillFiring;
    [SerializeField] public int[] idRegistry = new int[5];
    [SerializeField] private Slot slot1, slot2, slot3, slot4, slot5;
    [SerializeField] private int maxSlots;
    [SerializeField] private GameObject[] slots;
    [SerializeField] private Sprite[] onSpriteList;
    [SerializeField] private Sprite[] offSpriteList;
    [SerializeField] private AbsorbBullet absorbBullet;
    private readonly float BASE_BULLET_COOLDOWN = 0.5f;
    private float tempBonusAtkSpd, permanentBonusAtkSpd;
    private int tempBonusAtkDmg, permanentBonusAtkDmg;
    private float activeBulletCD;
    private bool penetration, avarice, bloodsucker;
    
    private void Awake() {
        activeBulletCD = BASE_BULLET_COOLDOWN;
        slotNum = 0; 
        maxSlots = 2; //# of slots unlocked
        permanentBonusAtkSpd = 1;
        tempBonusAtkSpd = 1;
    }

    void Update() {
        if (activeBulletCD > 0) {
            activeBulletCD -= Time.deltaTime;
        }

        if (Input.GetMouseButton(0) && activeBulletCD <= 0) {
            AbsorbShots();
        }

        if (Input.GetMouseButtonDown(1)) {
            FireAllSlots();
        }
        TurnOffSlots();
    }

    private void OnEnable() {
        tempBonusAtkDmg = 0;
        tempBonusAtkSpd = 1;
    }

    public void AcquireSkill(int ID) {
        if(slotNum < maxSlots) {
            if (!idRegistry.Contains(ID) && !isSkillFiring) {
                switch(slotNum) {
                case 0:
                    if (!slot1.IsCoolingDown()) {
                        slot1.AcquireSkill(ID); 
                        idRegistry[0] = ID; 
                        slotNum++; 
                        AnimationControl(slots[0], 0);
                    }
                    break;
                case 1:
                    if (!slot2.IsCoolingDown()) {
                        slot2.AcquireSkill(ID); 
                        idRegistry[1] = ID; 
                        slotNum++; 
                        AnimationControl(slots[1], 1);
                    }
                    break;
                case 2:
                    if (!slot3.IsCoolingDown()) {
                        slot3.AcquireSkill(ID); 
                        idRegistry[2] = ID; 
                        slotNum++; 
                        AnimationControl(slots[2], 2);
                    }
                    break;
                case 3:
                    if (!slot4.IsCoolingDown()) {
                        slot4.AcquireSkill(ID); 
                        idRegistry[3] = ID; 
                        slotNum++; 
                        AnimationControl(slots[3], 3);
                    }
                    break;
                case 4:
                    if (!slot5.IsCoolingDown()) {
                        slot5.AcquireSkill(ID); 
                        idRegistry[4] = ID; 
                        slotNum++; 
                        AnimationControl(slots[4], 4);
                    }
                    break;
                }
            }
        }
    }

    private void AbsorbShots() {
        BulletPool.Instance.GetBullet();
        activeBulletCD = BASE_BULLET_COOLDOWN * permanentBonusAtkSpd * tempBonusAtkSpd;     //set the bullet timer back to the cooldown time. adjust for buffs/debuffs
    }
    
    private void FireAllSlots() {
        if (slotNum > 0 && !isSkillFiring) {
            SlotBlast();
        }
    }

    private void SlotBlast() {
        isSkillFiring = true;
        while (slotNum > 0) {
            switch (slotNum) {
                case 1:
                    slot1.Engage(); idRegistry[0] = 0;
                    break;
                case 2:
                    slot2.Engage(); idRegistry[1] = 0;
                    break;
                case 3:
                    slot3.Engage(); idRegistry[2] = 0;
                    break;
                case 4:
                    slot4.Engage(); idRegistry[3] = 0;
                    break;
                case 5:
                    slot5.Engage(); idRegistry[4] = 0;
                    break;
            }
            slotNum--;
        }
        isSkillFiring = false;
    }

    public void BattleEnd() {
        if (isSkillFiring) {
            StopAllCoroutines();
            slotNum = 0;
            for (int i = 0; i < maxSlots; i++) {
                switch (i) {
                    case 0:
                        slot1.DumpSkill();
                        idRegistry[0] = 0;
                        break;
                    case 1:
                        slot2.DumpSkill();
                        idRegistry[1] = 0;
                        break;
                    case 2:
                        slot3.DumpSkill();
                        idRegistry[2] = 0;
                        break;
                    case 3:
                        slot4.DumpSkill();
                        idRegistry[3] = 0;
                        break;
                    case 4:
                        slot5.DumpSkill();
                        idRegistry[4] = 0;
                        break;
                }
            } 
        }
    }

    private void AnimationControl(GameObject slot, int spriteNumOn) {
        slot.GetComponent<Animator>().ResetTrigger("Hit2");
        slot.GetComponent<Animator>().SetTrigger("Hit");
        slot.transform.GetChild(1).GetComponent<Image>().sprite = onSpriteList[spriteNumOn];
    }

    private void TurnOffSlots() {
        if(slotNum < 1) {
            slots[0].GetComponent<Animator>().SetTrigger("Hit2");
            slots[0].transform.Find("Slot 1_Border").GetComponent<Image>().sprite = offSpriteList[0];
        }

        if(slotNum < 2) {
            slots[1].GetComponent<Animator>().SetTrigger("Hit2");
            slots[1].transform.Find("Slot 2_Border").GetComponent<Image>().sprite = offSpriteList[1];
        }

        if(slotNum < 3 && maxSlots >= 3) {
            slots[2].GetComponent<Animator>().SetTrigger("Hit2");
            slots[2].transform.Find("Slot 3_Border").GetComponent<Image>().sprite = offSpriteList[2];
        }

        if(slotNum < 4 && maxSlots >= 4) {
            slots[3].GetComponent<Animator>().SetTrigger("Hit2");
            slots[3].transform.Find("Slot 4_Border").GetComponent<Image>().sprite = offSpriteList[3];
        }

        if(slotNum < 5 && maxSlots >= 5) {
            slots[4].GetComponent<Animator>().SetTrigger("Hit2");
            slots[4].transform.Find("Slot 5_Border").GetComponent<Image>().sprite = offSpriteList[4];
        }
    }

    public void AddPermanentAtkDmg(int bonus) {
        permanentBonusAtkDmg += bonus;
    }

    public void AddTempAtkDmg(int bonus) {
        tempBonusAtkDmg += bonus;
    }

    public int GetTempAtkDmg() {
        return tempBonusAtkDmg;
    }

    public int GetPermanentAtkDmg() {
        return permanentBonusAtkDmg;
    }

    public void AddPermanentAtkSpd() {
        permanentBonusAtkSpd *= 0.99f;
    }

    public void AddTempAtkSpd(int times) {
        for (int i = 0; i < times; i++) {
            tempBonusAtkSpd *= 0.99f;
        }
    }

    public float GetTempAtkSpd() {
        return tempBonusAtkSpd;
    }

    public float GetPermanentAtkSpd() {
        return permanentBonusAtkSpd;
    }
    
    public void ActivatePenetration() {
        penetration = true;
    }
    
    public void DeactivatePenetration() {
        penetration = false;
    }

    public bool IsPenetrationActive() {
        return penetration;
    }

    public void ActivateAvarice() {
        avarice = true;
    }
    
    public void DeactivateAvarice() {
        avarice = false;
    }

    public bool IsAvariceActive() {
        return avarice;
    }

    public void ActivateBloodsucker() {
        bloodsucker = true;
    }

    public void DeactivateBloodsucker() {
        bloodsucker = false;
    }

    public bool IsBloodsuckerActive() {
        return bloodsucker;
    }

    public void IncreaseMaxSlots() {
        if (maxSlots < 5) {
            maxSlots++;
            slots[maxSlots-1].SetActive(true);
            switch (maxSlots) {
                case 3:
                    slot3.gameObject.SetActive(true);
                    slots[0].transform.Translate(-1.3f, 0, 0);
                    slots[1].transform.Translate(-1.3f, 0, 0);
                    break;
                case 4:
                    slot4.gameObject.SetActive(true);
                    slots[0].transform.Translate(-1.3f, 0, 0);
                    slots[1].transform.Translate(-1.3f, 0, 0);
                    slots[2].transform.Translate(-1.3f, 0, 0);
                    break;
                case 5:
                    slot5.gameObject.SetActive(true);
                    slots[0].transform.Translate(-1.3f, 0, 0);
                    slots[1].transform.Translate(-1.3f, 0, 0);
                    slots[2].transform.Translate(-1.3f, 0, 0);
                    slots[3].transform.Translate(-1.3f, 0, 0);
                    break;
            }
        }
        Debug.Log("Slot Manager: Maximum slots increased to " + maxSlots + ". Process successful.");
    }

    /* //todo
    public class AttackSpeedManager : MonoBehaviour
{
    private float baseAttackSpeed = 0.5f; // Base attack speed in seconds
    private float currentAttackSpeed; // Current attack speed after applying boosts

    private void Awake()
    {
        currentAttackSpeed = baseAttackSpeed;
    }

    // Method to apply attack speed boost
    public void ApplyAttackSpeedBoost(float percentageBoost)
    {
        float boostMultiplier = 1f + (percentageBoost / 100f); // Convert percentage to multiplier
        currentAttackSpeed = baseAttackSpeed / boostMultiplier; // Apply the boost
    }

    // Method to reset attack speed to base value
    public void ResetAttackSpeed()
    {
        currentAttackSpeed = baseAttackSpeed;
    }

    // Getter method to access current attack speed
    public float GetCurrentAttackSpeed()
    {
        return currentAttackSpeed;
    }
}
    */
}
