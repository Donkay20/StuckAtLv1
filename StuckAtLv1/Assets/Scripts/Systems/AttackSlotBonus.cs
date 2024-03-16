using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSlotBonus : MonoBehaviour
{
    public int GetDamageBonus(Slot s, int baseDmg) {
        int damage = baseDmg;
        if (s.GetCommonUpgrade(0) > 0) {                                                                //common 0
            damage *= (int) (1 + (s.GetCommonUpgrade(0) * 0.1f));
            Debug.Log("Common | 0, Damage:" + damage);
        }

        if (s.GetRareUpgrade(1) > 0) {                                                                  //rare 1
            damage *= (int) (1 + (s.GetRareUpgrade(1) * 0.1f));
            Debug.Log("Rare | 1, Damage:" + damage);                                         
        }

        if (s.GetRareUpgrade(9) > 0) {                                                                  //rare 9
            damage *= (int) ((float) s.GetRareUpgrade(9) * (1 + (FindAnyObjectByType<Character>().currentHp / 200)));
            Debug.Log("Rare | 9, Damage:" + damage);    
        }  

        damage = (int) (damage * FindAnyObjectByType<Character>().GetDamageModifier());                 //buffs
        Debug.Log("Damage modifier: " + FindAnyObjectByType<Character>().GetDamageModifier());
        Debug.Log("After buffs: " + damage);
        damage *= (int) FindAnyObjectByType<GameManager>().GetShopDamageBonus();                        //shop bonus is final damage multiplier
        Debug.Log("After shop multiplier: " + damage);
        return damage;
    }

    public float GetSizeBonus(Slot s) {
        float size = 1;
        if (s.GetCommonUpgrade(1) > 0) {                                                        //common 1
            size += s.GetCommonUpgrade(1) * 0.05f;
            Debug.Log("Common | 1");
        }
        
        if (s.GetRareUpgrade(1) > 0) {                                                          //rare 1
            size += s.GetRareUpgrade(1) * 0.05f;
            Debug.Log("Rare | 1");
        } 
        
        if (s.GetRareUpgrade(10) > 0 && FindAnyObjectByType<Character>().currentHp > 10) {      //rare 10
            size += (float) (FindAnyObjectByType<Character>().currentHp - 10) * 0.01f;
            Debug.Log("Rare | 10");
        }                                                    
        return size;
    }

    public float GetDurationBonus(Slot s, float baseDuration) {
        float duration = baseDuration;
        if (s.GetCommonUpgrade(2) > 0) {                    //common 2
            duration *= 1 + s.GetCommonUpgrade(2) * 0.2f;
            Debug.Log("Common | 2");
        }

        if (s.GetRareUpgrade(1) > 0) {                      //rare 1
            duration *= 1 + s.GetRareUpgrade(1) * 0.2f;    
            Debug.Log("Rare | 11");               
        }
        return duration;
    }

    public (float, float, float) GetUpgradeCalculation(Slot s) {
        float damage = 1;
        if (s.GetCommonUpgrade(0) > 0) {damage *= 1 + (s.GetCommonUpgrade(0) * 0.1f);}
        if (s.GetRareUpgrade(1) > 0) {damage *= 1 + (s.GetRareUpgrade(1) * 0.1f);}
        if (s.GetRareUpgrade(9) > 0) {damage *= s.GetRareUpgrade(9) * (1 + (FindAnyObjectByType<Character>(FindObjectsInactive.Include).currentHp / 200));}  
        damage *= FindAnyObjectByType<Character>(FindObjectsInactive.Include).GetDamageModifier();
        damage *= FindAnyObjectByType<GameManager>().GetShopDamageBonus();

        float size = 1;
        if (s.GetCommonUpgrade(1) > 0) {size += s.GetCommonUpgrade(1) * 0.05f;}
        if (s.GetRareUpgrade(1) > 0) {size += s.GetRareUpgrade(1) * 0.05f;} 
        if (s.GetRareUpgrade(10) > 0 && FindAnyObjectByType<Character>(FindObjectsInactive.Include).currentHp > 10) {size += (FindAnyObjectByType<Character>(FindObjectsInactive.Include).currentHp - 10) * 0.01f;}    

        float duration = 1;
        if (s.GetCommonUpgrade(2) > 0) {duration *= 1 + s.GetCommonUpgrade(2) * 0.2f;}
        if (s.GetRareUpgrade(1) > 0) {duration *= 1 + s.GetRareUpgrade(1) * 0.2f;}

        return (damage, size, duration);
    }
}
