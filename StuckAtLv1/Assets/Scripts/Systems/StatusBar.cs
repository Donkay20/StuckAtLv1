using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBar : MonoBehaviour //Handles HP bar under main character during combat. Probably temporary.
{
    [SerializeField] Transform bar;
    [SerializeField] Transform overhealBar;
    [SerializeField] private SpriteRenderer barFill;
    [SerializeField] private SpriteRenderer overhealBarFill;
    private readonly int MAX_HP = 10;
    private readonly int OVERHEAL_MAX_HP = 999;

    public void SetState(int hp) {
        int overhealHP = 0;
        if (hp > MAX_HP) {
            overhealHP = hp - MAX_HP;
        }
        float hpState = (float) hp / MAX_HP;
        float overhealState = (float) overhealHP / OVERHEAL_MAX_HP;
        
        if (hpState < 0f) {hpState = 0f;}
        if (hpState > 1f) {hpState = 1f;}
        if (overhealState < 0f) {overhealState = 0f;}
        if (overhealState > 1f) {overhealState = 1f;}

        overhealBar.transform.localScale = new Vector3(overhealState, .7f, 1f);
        bar.transform.localScale = new Vector3(hpState, 1f, 1f);
    }
}
