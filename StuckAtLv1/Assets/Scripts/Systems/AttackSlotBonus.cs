using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSlotBonus : MonoBehaviour
{
    public int GetDamageBonus(Slot s, int baseDmg) {
        int damage = baseDmg;
        damage *= (int) (1 + (s.GetCommonUpgrade(0) * 0.1f));                                       //common 0
        damage *= (int) (1 + (s.GetRareUpgrade(1) * 0.1f));                                         //rare 1
        damage += (int) s.GetRareUpgrade(9) * (FindAnyObjectByType<Character>().currentHp / 10);    //rare 9 
        damage *= (int) FindAnyObjectByType<Character>().DamageModifier;                            //buffs
        damage *= (int) FindAnyObjectByType<GameManager>().GetShopDamageBonus();                    //shop bonus is final damage multiplier
        return damage;
    }

    public float GetSizeBonus(Slot s) {
        float size = 1;
        size += s.GetCommonUpgrade(1)*0.05f;                                                    //common 1
        size += s.GetRareUpgrade(1)*0.05f;                                                      //rare 1
        if (s.GetRareUpgrade(10) > 0 && FindAnyObjectByType<Character>().currentHp > 10) {      //rare 10
            size += (float) (FindAnyObjectByType<Character>().currentHp - 10) * 0.01f;
        }                                                    
        return size;
    }

    public float GetDurationBonus(Slot s, float baseDuration) {
        float duration = baseDuration;
        duration *= 1 + s.GetCommonUpgrade(2)*0.2f;                 //common 2
        duration *= 1 + s.GetRareUpgrade(1)*0.2f;                   //rare 1
        return duration;
    }
}
