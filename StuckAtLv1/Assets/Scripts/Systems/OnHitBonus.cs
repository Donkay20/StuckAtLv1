using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitBonus : MonoBehaviour
{
    public void ApplyDamageBonus(Slot slot, Enemy enemy, int damage) {
        if (enemy != null) {

            bool appliedAnemia = false;
            bool criticalHit = slot.CriticalHit();                                  //calculate critical hit

            if (criticalHit) {
                    damage *= (int) slot.CriticalDamage();
                    enemy.CriticalHit();
            }

            //begin on-kill slot bonuses here
            if (damage > enemy.GetHealth()) {                                       //if the attack kills
                int bonusDropChance = 0;

                if (slot.GetCommonUpgrade(12) > 0) {                                //common 12
                    bonusDropChance += 5 * slot.GetCommonUpgrade(12);
                    Debug.Log("Common | 12");
                }

                if (slot.GetCommonUpgrade(13) > 0) {                                //common 13
                    enemy.RaiseReward(slot.GetCommonUpgrade(13) * 5);
                    Debug.Log("Common | 13");
                }

                if (slot.GetCommonUpgrade(14) > 0) {                                //common 14
                    for (int i = 0; i < slot.GetCommonUpgrade(14); i++) {
                        FindAnyObjectByType<BuffManager>().DispelDebuff();
                    }
                    Debug.Log("Common | 14");
                }

                if (slot.GetLegendaryUpgrade(1) > 0 && slot.SkillUses() > 0) {      //legendary 1
                    slot.GetSkillUses();
                }

                if (slot.GetLegendaryUpgrade(4) > 0 && criticalHit) {               //legendary 4 (on-kill version)
                    slot.RefundCooldown();
                }

                if (slot.GetLegendaryUpgrade(5) > 0 && criticalHit) {               //legendary 5 (on-kill version)
                    FindAnyObjectByType<Character>().GainMoney(damage / 10);
                }

                if (slot.GetLegendaryUpgrade(11) > 0) {                             //legendary 11
                    if (FindAnyObjectByType<Character>().currentHp > 10) {
                        bonusDropChance += (int) ((FindAnyObjectByType<Character>().currentHp - 10) * (0.05f * slot.GetLegendaryUpgrade(11)));
                    }
                }
                
                if (slot.GetLegendaryUpgrade(13) > 0) {                             //legendary 13
                    if (!FindAnyObjectByType<BuffManager>().IsBulwarkActive()) {
                        FindAnyObjectByType<BuffManager>().AddBuff("bulwark", 0, 5);
                    }
                }
                enemy.DropMoney(bonusDropChance);
                //end on-kill slot bonuses here
            } else {                                                                //if the attack doesn't kill (do on-hit stuff instead)
                //begin on-hit slot bonuses here
                if (slot.GetRareUpgrade(12) > 0 && enemy.IsAnemic()) {              //rare 12
                    FindAnyObjectByType<Character>().GainMoney(enemy.AnemiaSeverity());
                    Debug.Log("Rare | 12");
                }

                if (slot.GetCommonUpgrade(7) > 0) {                                 //common 7
                    enemy.ApplySlow(1 - (slot.GetCommonUpgrade(7) * 0.2f), 3);
                    Debug.Log("Common | 7");
                }

                if (slot.GetCommonUpgrade(10) > 0) {                                //common 8
                    if (Random.Range(0, 2) == 1) {
                        enemy.ApplyAnemia(slot.GetCommonUpgrade(10), slot.GetCommonUpgrade(10) * 3);
                        appliedAnemia = true;
                        Debug.Log("Common | 8");
                    }
                }

                if (slot.GetRareUpgrade(8) > 0) {                                   //rare 8
                    Character c = FindAnyObjectByType<Character>();
                    if (criticalHit) {
                        c.Heal(damage *= (int) slot.CriticalDamage() / 10);
                    } else {
                        c.Heal(damage / 10);
                    }
                    Debug.Log("Rare | 8");
                }

                if (slot.GetRareUpgrade(11) > 0) {                                  //rare 11
                    enemy.ApplyAnemia((damage / 10) * slot.GetRareUpgrade(11), 10);
                    appliedAnemia = true;
                    Debug.Log("Rare | 11");
                }

                if (slot.GetRareUpgrade(14) > 0 && appliedAnemia) {                 //rare 14
                    FindAnyObjectByType<BuffManager>().AddBuff("power", 0.2f * slot.GetRareUpgrade(14), 5f);
                    Debug.Log("Rare | 14");
                }

                if (slot.GetLegendaryUpgrade(3) > 0) {                              //legendary 3
                    if (criticalHit) {
                        FindAnyObjectByType<Character>().GainMoney(FindAnyObjectByType<Character>().afterimages * 2);
                    } else {
                        FindAnyObjectByType<Character>().GainMoney(FindAnyObjectByType<Character>().afterimages);
                    }
                }
                
                if (slot.GetLegendaryUpgrade(4) > 0 && criticalHit) {               //legendary 4 (on-hit version)
                    slot.RefundCooldown();
                }

                if (slot.GetLegendaryUpgrade(5) > 0 && criticalHit) {               //legendary 5 (on-hit version)
                    FindAnyObjectByType<Character>().GainMoney(damage / 20);
                }

                if (slot.GetLegendaryUpgrade(6) > 0 && criticalHit) {               //legendary 6 (on-hit version)
                    FindAnyObjectByType<BuffManager>().AddBuff("critdmg", 0.5f * slot.GetLegendaryUpgrade(6), 3);
                }

                if (slot.GetLegendaryUpgrade(8) > 0 && appliedAnemia) {             //legendary 8
                    enemy.AnemicShock();
                }

                if (slot.GetLegendaryUpgrade(10) > 0 && enemy.IsAnemic()) {         //legendary 10
                    if (enemy.GetHealth() <= (enemy.maxHP / 2)) {
                        enemy.AnemicTorture();
                    }
                }

                if (slot.GetLegendaryUpgrade(12) > 0) {                             //legendary 12
                    if (FindAnyObjectByType<Character>().currentHp > 10) {
                        enemy.ApplySlow((float) ((FindAnyObjectByType<Character>().currentHp - 10) * 0.01f), 3);
                    }
                }
                //end on-hit slot bonuses here
            }
            enemy.TakeDamage(damage);
        }
    }
}
