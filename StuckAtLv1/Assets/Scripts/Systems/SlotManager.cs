using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    /*
    Manager/parent for every slot. Assigns an identity to each slot for many purposes, including applying bonuses.
    Currently only needs to account for 2, but will eventually be expanded to account for 5. At some point conditions will be added to lock slot usage until it is unlocked via game progress.
    */

    public KeyCode slotKey1, slotKey2, slotKey3, slotKey4, slotKey5;
    [SerializeField] private Slot slot1, slot2, slot3, slot4, slot5;
    private int slotNum = 1;
    private int maxSlots;

    [SerializeField] private GameObject[] slots;
    [SerializeField] private Sprite[] onSpriteList;
    [SerializeField] private Sprite[] offSpriteList;

    private void Awake() {
        //Sets the first slot as default
        slots[0].GetComponent<Animator>().SetTrigger("Hit");
        slots[0].transform.GetChild(1).GetComponent<Image>().sprite = onSpriteList[0];
        slotNum = 1; 
        maxSlots = 2; //# of slots unlocked
    }

    void Update() {
        ToggleSlot();
        InitiateSlot();
        TurnOffSlots();
    }

    //Select Slot Player wants to use
    private void ToggleSlot() {
        if (Input.mouseScrollDelta.y > 0) {
            if (slotNum - 1 == 0) {
                slotNum = maxSlots;
            } else {
                slotNum--;
            }
            AnimationControl(slots[slotNum - 1], slotNum - 1);
        }

        if (Input.mouseScrollDelta.y < 0) {
            if (slotNum + 1 > maxSlots) {
                slotNum = 1;
            } else {
                slotNum++;
            }
            AnimationControl(slots[slotNum - 1], slotNum - 1);
        }

        if (Input.GetKeyDown(slotKey1)) {
            slotNum = 1;
            AnimationControl(slots[0], 0);
            //Debug.Log("Slot 1 pressed.");
            //slot1.Engage();
        }

        if (Input.GetKeyDown(slotKey2)) {
            slotNum = 2;
            AnimationControl(slots[1], 1);
            //Debug.Log("Slot 2 pressed.");
            //slot2.Engage();
        }

        if (Input.GetKeyDown(slotKey3)) {
            slotNum = 3;
            AnimationControl(slots[2], 2);
            //Debug.Log("Slot 2 pressed.");
            //slot2.Engage();
        }

        if (Input.GetKeyDown(slotKey4)) {
            slotNum = 4;
            AnimationControl(slots[3], 3);
            //Debug.Log("Slot 2 pressed.");
            //slot2.Engage();
        }

        if (Input.GetKeyDown(slotKey5)) {
            slotNum = 5;
            AnimationControl(slots[4], 4);
            //Debug.Log("Slot 2 pressed.");
            //slot2.Engage();
        }
    }
    
    private void InitiateSlot() {
        //Initiates slot selected
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)){
            switch(slotNum){
                case 1:
                    slot1.Engage();
                    break;
                case 2:
                    slot2.Engage();
                    break;
                case 3:
                    slot3.Engage();
                    break;
                case 4:
                    slot4.Engage();
                    break;
                case 5:
                    slot5.Engage();
                    break;
            }
        }
    }

    private void AnimationControl(GameObject slot, int spriteNumOn) {
        slot.GetComponent<Animator>().ResetTrigger("Hit2");
        slot.GetComponent<Animator>().SetTrigger("Hit");
        slot.transform.GetChild(1).GetComponent<Image>().sprite = onSpriteList[spriteNumOn];
    }

    private void TurnOffSlots() {
        if(slotNum != 1) {
            slots[0].GetComponent<Animator>().SetTrigger("Hit2");
            slots[0].transform.Find("Slot 1_Border").GetComponent<Image>().sprite = offSpriteList[0];
        }

        if(slotNum != 2) {
            slots[1].GetComponent<Animator>().SetTrigger("Hit2");
            slots[1].transform.Find("Slot 2_Border").GetComponent<Image>().sprite = offSpriteList[1];
        }

        if(slotNum != 3 && maxSlots >= 3) {
            slots[2].GetComponent<Animator>().SetTrigger("Hit2");
            slots[2].transform.Find("Slot 3_Border").GetComponent<Image>().sprite = offSpriteList[2];
        }

        if(slotNum != 4 && maxSlots >= 4) {
            slots[3].GetComponent<Animator>().SetTrigger("Hit2");
            slots[3].transform.Find("Slot 4_Border").GetComponent<Image>().sprite = offSpriteList[3];
        }

        if(slotNum != 5 && maxSlots >= 5) {
            slots[4].GetComponent<Animator>().SetTrigger("Hit2");
            slots[4].transform.Find("Slot 5_Border").GetComponent<Image>().sprite = offSpriteList[4];
        }
    }

    private void IncreaseMaxSlots() {
        maxSlots++;
        slots[maxSlots].SetActive(true);
    }
}