using System.Collections;
using System.Collections.Generic;
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
    private readonly float BASE_BULLET_COOLDOWN = 0.5f;
    private float activeBulletCD;
    [SerializeField] private bool isSkillFiring;
    [SerializeField] public int[] idRegistry = new int[5];
    [SerializeField] private Slot slot1, slot2, slot3, slot4, slot5;
    [SerializeField] private int maxSlots;
    [SerializeField] private GameObject[] slots;
    [SerializeField] private Sprite[] onSpriteList;
    [SerializeField] private Sprite[] offSpriteList;
    [SerializeField] private AbsorbBullet absorbBullet;
    
    private void Awake() {
        activeBulletCD = BASE_BULLET_COOLDOWN;
        slotNum = 0; 
        maxSlots = 2; //# of slots unlocked
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

        //ToggleSlot();
        TurnOffSlots();
    }

    /*
    private void ToggleSlot() {
        if (Input.GetKeyDown(slotKey1)) {
            slotNum = 1;
            AnimationControl(slots[0], 0);
        }

        if (Input.GetKeyDown(slotKey2)) {
            slotNum = 2;
            AnimationControl(slots[1], 1);
        }

        if (maxSlots >= 3) {
            if (Input.GetKeyDown(slotKey3)) {
            slotNum = 3;
            AnimationControl(slots[2], 2);
            }
        }

        if (maxSlots >= 4) {
            if (Input.GetKeyDown(slotKey4)) {
            slotNum = 4;
            AnimationControl(slots[3], 3);
            }
        }

        if (maxSlots == 5) {
            if (Input.GetKeyDown(slotKey5)) {
            slotNum = 5;
            AnimationControl(slots[4], 4);
            }
        }
    }
    */

    public void AcquireSkill(int ID) {
        if(slotNum < maxSlots) {
            if (!idRegistry.Contains(ID) && !isSkillFiring) {
                switch(slotNum) {
                case 0:
                    if (!slot1.IsCoolingDown()) {slot1.AcquireSkill(ID); idRegistry[0] = ID; slotNum++; AnimationControl(slots[0], 0);}
                    
                    break;
                case 1:
                    if (!slot2.IsCoolingDown()) {slot2.AcquireSkill(ID); idRegistry[1] = ID; slotNum++; AnimationControl(slots[1], 1);}
                    
                    break;
                case 2:
                    if (!slot3.IsCoolingDown()) {slot3.AcquireSkill(ID); idRegistry[2] = ID; slotNum++; AnimationControl(slots[2], 2);}
                    
                    break;
                case 3:
                    if (!slot4.IsCoolingDown()) {slot4.AcquireSkill(ID); idRegistry[3] = ID; slotNum++; AnimationControl(slots[3], 3);}
                    
                    break;
                case 4:
                    if (!slot5.IsCoolingDown()) {slot5.AcquireSkill(ID); idRegistry[4] = ID; slotNum++; AnimationControl(slots[4], 4);}
                    
                    break;
                }
            }
        }
    }

    private void AbsorbShots() {
        Instantiate(absorbBullet, transform.position, Quaternion.identity, transform);
        activeBulletCD = BASE_BULLET_COOLDOWN;                                            //set the bullet timer back to the cooldown time. adjust for buffs, nerfs etc
    }
    
    private void FireAllSlots() {
        if (slotNum > 0) {
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

    public void RareTwoCooldownCut(int identity, int intensity) {       //rare 2
        for (int i = 0; i < maxSlots; i++) {
            switch (i) {
                case 0:
                    if (identity != i) {
                        slot1.CutCooldown(intensity);
                    }
                    break;
                case 1:
                    if (identity != i) {
                        slot2.CutCooldown(intensity);
                    }
                    break;
                case 2:
                    if (identity != i) {
                        slot3.CutCooldown(intensity);
                    }
                    break;
                case 3:
                    if (identity != i) {
                        slot4.CutCooldown(intensity);
                    }
                    break;
                case 4:
                    if (identity != i) {
                        slot5.CutCooldown(intensity);
                    }
                    break;
            }
        }
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
}
