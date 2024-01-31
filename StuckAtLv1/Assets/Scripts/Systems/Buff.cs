using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Buff : MonoBehaviour
{
    private string buffType;    public string BuffType { get => buffType; set => buffType = value; }
    private float efficacy;     public float Efficacy { get => efficacy; set => efficacy = value; }
    private int identity;       public int Identity { get => identity; set => identity = value; }
    private float duration;     public float Duration { get => duration; set => duration = value; }
    private bool buffActive;

    void Update() {
        if(buffActive) {
            if (duration > 0) {
                duration -= Time.deltaTime;
            } else {
                EndBuff();
            }
        }
    }

    public void Initialize(string b, float e, float d) {
        buffType = b;
        efficacy = e;
        duration = d;
        buffActive = true;
    }

    public void EndBuff() {
        //Communicate to the buff manager that this buff should be deleted, and reorganize the buffs.
        FindAnyObjectByType<BuffManager>().BuffExpired(identity);
    }
}
