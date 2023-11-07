using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public KeyCode slotKey1, slotKey2;
    [SerializeField] private Slot slot1, slot2;
    private void Awake() {
        slot1.Identity = 1;
        slot2.Identity = 2;
    }
    void Update()
    {
        if (Input.GetKeyDown(slotKey1)) {
            Debug.Log("Slot 1 pressed.");
            slot1.Engage();
        }

        if (Input.GetKeyDown(slotKey2)) {
            Debug.Log("Slot 2 pressed.");
            slot2.Engage();
        }
    }
}
