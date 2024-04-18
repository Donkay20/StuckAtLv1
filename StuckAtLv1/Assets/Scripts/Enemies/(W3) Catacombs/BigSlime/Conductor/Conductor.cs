using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    [SerializeField] private ConductionManager conductionManager;
    [SerializeField] private int identity;
    [SerializeField] private Sprite inactive, active;
    private bool isActive;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Activate() { //conductor only activates if it isn't already activated and if a bolt barrage isn't already ongoing.
        if (!isActive && !conductionManager.BarrageActive()) {
            //GetComponent<SpriteRenderer>().sprite = active;
            conductionManager.ActivateConductor(identity);
            anim.SetBool("Active", true);
            isActive = true;
        }
    }

    public void Deactivate() {
        GetComponent<SpriteRenderer>().sprite = inactive;
        anim.SetBool("Active", false);
        isActive = false;
    }
}
