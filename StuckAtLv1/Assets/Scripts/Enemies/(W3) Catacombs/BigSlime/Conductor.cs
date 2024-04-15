using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    [SerializeField] private ConductionManager conductionManager;
    [SerializeField] private int identity;
    [SerializeField] private Sprite inactive, active;
    private bool isActive;
    private readonly float ACTIVITY_TIMER_RESET = 15f;
    private float activityTimer;
    void Start() {
        
    }

    void Update() {
        if (isActive) {
            activityTimer -= Time.deltaTime;
        }

        if (isActive && activityTimer <= 0) {
            Deactivate();
        }
    }

    public void Activate() {
        if (!isActive && !conductionManager.BarrageActive()) {
            activityTimer = ACTIVITY_TIMER_RESET;
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
