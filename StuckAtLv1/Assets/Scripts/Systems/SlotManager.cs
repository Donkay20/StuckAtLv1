using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    /*
    Manager/parent for every slot. Assigns an identity to each slot for many purposes, including applying bonuses.
    Currently only needs to account for 2, but will eventually be expanded to account for 5. At some point conditions will be added to lock slot usage until it is unlocked via game progress.
    */
    public KeyCode slotKey1, slotKey2;
    [SerializeField] private Slot slot1, slot2;
    private void Awake() {
        slot1.Identity = 1;
        slot2.Identity = 2;
    }
    void Update()
    {
        if (Input.GetKeyDown(slotKey1)) {
            //Debug.Log("Slot 1 pressed.");
            slot1.Engage();
        }

        if (Input.GetKeyDown(slotKey2)) {
            //Debug.Log("Slot 2 pressed.");
            slot2.Engage();
        }
    }
}
