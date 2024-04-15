using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    [SerializeField] private ConductionManager conductionManager;
    [SerializeField] private int identity;
    [SerializeField] private Sprite inactive, active;
    private bool isActive;

    public void Activate() {
        if (!isActive && !conductionManager.BarrageActive()) {
            GetComponent<SpriteRenderer>().sprite = active;
            conductionManager.ActivateConductor(identity);
            isActive = true;
        }
    }

    public void Deactivate() {
        GetComponent<SpriteRenderer>().sprite = inactive;
        isActive = false;
    }
}
