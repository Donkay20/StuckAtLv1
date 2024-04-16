using UnityEngine;

public class OnHitBonus : MonoBehaviour
{
    [SerializeField] private GameObject anemiaScreenBlastPrefab;
    [SerializeField] private GameObject anemiaSpreadPrefab;
    [SerializeField] private GameObject explosionPrefab;
    public void ApplyDamageBonus(Slot slot, Enemy enemy, int damage) {
        if (enemy != null) {

            bool appliedAnemia = false;
            bool criticalHit = slot.CriticalHit();                                  //calculate critical hit

            if (criticalHit) {
                    damage = (int) (damage * slot.CriticalDamage());
                    Debug.Log("Critical hit. Damage: " + damage);
                    enemy.CriticalHit();
            }

            //ON-KILL slot bonuses BEGIN here
            if (damage >= enemy.GetHealth()) {                                      //if the attack kills:
                int bonusDropChance = 0;

                if (slot.GetCommonUpgrade(9) > 0) {                                 //common 9
                    slot.TempAtkDmgBoost();
                }

                if (slot.GetCommonUpgrade(12) > 0) {                                //common 12
                    bonusDropChance += 5 * slot.GetCommonUpgrade(12);
                    Debug.Log("Common | 12");
                }

                if (slot.GetCommonUpgrade(13) > 0) {                                //common 13
                    enemy.RaiseReward(slot.GetCommonUpgrade(13) * 5);
                    Debug.Log("Common | 13");
                }

                if (slot.GetRareUpgrade(0) > 0) {                                   //rare 0
                    slot.RareZeroBoost();
                }

                if (slot.GetRareUpgrade(8) > 0) {                                   //rare 8 (rare 7) (rare 6)
                    Character c = FindAnyObjectByType<Character>();
                    SlotManager s = FindAnyObjectByType<SlotManager>();
                    if (slot.GetRareUpgrade(7) > 0) {
                        if (slot.GetRareUpgrade(6) > 0) {
                            s.AddTempAtkSpd(slot.GetRareUpgrade(6) * slot.GetRareUpgrade(8) * 2 * slot.GetRareUpgrade(7));
                        } else {
                            c.Heal(1 * slot.GetRareUpgrade(8) * 2 * slot.GetRareUpgrade(7));
                        }
                    } else {
                        if (slot.GetRareUpgrade(6) > 0) {
                            s.AddTempAtkSpd(slot.GetRareUpgrade(6) * slot.GetRareUpgrade(8));
                        } else {
                            c.Heal(1 * slot.GetRareUpgrade(8));
                        }
                    }
                    Debug.Log("Rare | 8 (Rare | 7) (Rare | 6)");
                }

                if (slot.GetRareUpgrade(13) > 0 && enemy.IsAnemic()) {              //rare 13 (on-kill version)
                    GameObject c = Instantiate(anemiaSpreadPrefab, enemy.transform.position, Quaternion.identity);
                    c.transform.localScale *= slot.GetRareUpgrade(13);
                    Debug.Log("Rare | 13");
                }

                if (slot.GetLegendaryUpgrade(0) > 0) {                              //legendary 0
                    GameObject e = Instantiate(explosionPrefab, enemy.transform.position, Quaternion.identity);
                    Explosion g = e.GetComponent<Explosion>();
                    g.Activate(slot);
                    Debug.Log("Legendary | 0");
                }

                if (slot.GetLegendaryUpgrade(1) > 0) {                              //legendary 1
                    if (!FindAnyObjectByType<BuffManager>().IsPenetrationActive()) {
                        FindAnyObjectByType<BuffManager>().AddBuff("penetration", 0, 3);
                        Debug.Log("Legendary | 1");
                    }
                }

                if (slot.GetLegendaryUpgrade(5) > 0 && criticalHit) {               //legendary 5
                    FindAnyObjectByType<SlotManager>().AddPermanentAtkSpd();
                }

                if (slot.GetLegendaryUpgrade(6) > 0 && criticalHit) {               //legendary 6 (on-kill version)
                    FindAnyObjectByType<BuffManager>().AddBuff("critdmg", 0.5f * slot.GetLegendaryUpgrade(6), 3);
                    FindAnyObjectByType<BuffManager>().DispelDebuff();
                    Debug.Log("Legendary | 6 (on-kill)");
                }

                if (slot.GetLegendaryUpgrade(7) > 0) {
                    Instantiate(anemiaScreenBlastPrefab, slot.transform);           //legendary 7
                    Debug.Log("Legendary | 7");
                }

                if (slot.GetLegendaryUpgrade(11) > 0) {                             //legendary 11
                    if (FindAnyObjectByType<Character>().currentHp > 10) {
                        bonusDropChance += (int) ((FindAnyObjectByType<Character>().currentHp - 10) * (0.05f * slot.GetLegendaryUpgrade(11)));
                        Debug.Log("Legendary | 11, success");
                    }
                    Debug.Log("Legendary | 11, failure");
                }
                
                if (slot.GetLegendaryUpgrade(13) > 0) {                             //legendary 13
                    if (!FindAnyObjectByType<BuffManager>().IsBulwarkActive()) {
                        FindAnyObjectByType<BuffManager>().AddBuff("bulwark", 0, 5);
                        Debug.Log("Legendary | 13");
                    }
                }

                if (slot.GetLegendaryUpgrade(14) > 0) {                             //legendary 14
                    Character c = FindAnyObjectByType<Character>();
                    SlotManager s = FindAnyObjectByType<SlotManager>();
                    int amountToAdd = (int) Mathf.Floor((float) c.currentHp / 100);
                    s.AddPermanentAtkDmg(amountToAdd);
                }

                enemy.DropMoney(bonusDropChance);
                //ON-KILL slot bonuses END here
            } else {                                                                //if the attack doesn't kill (do on-hit stuff instead):
                //ON-HIT slot bonuses BEGIN here
                if (slot.GetRareUpgrade(13) > 0 && enemy.IsAnemic()) {              //rare 13 (on-hit version)
                    GameObject c = Instantiate(anemiaSpreadPrefab, enemy.transform.position, Quaternion.identity);
                    c.transform.localScale *= slot.GetRareUpgrade(13);
                    Debug.Log("Rare | 13");
                }

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

                if (slot.GetRareUpgrade(8) > 0) {                                   //rare 8 (rare 7) (rare 6)
                    Character c = FindAnyObjectByType<Character>();
                    SlotManager s = FindAnyObjectByType<SlotManager>();
                    if (slot.GetRareUpgrade(7) > 0) {
                        if (slot.GetRareUpgrade(6) > 0) {
                            s.AddTempAtkSpd(slot.GetRareUpgrade(6) * slot.GetRareUpgrade(8) * 2 * slot.GetRareUpgrade(7));
                        } else {
                            c.Heal(1 * slot.GetRareUpgrade(8) * 2 * slot.GetRareUpgrade(7));
                        }
                    } else {
                        if (slot.GetRareUpgrade(6) > 0) {
                            s.AddTempAtkSpd(slot.GetRareUpgrade(6) * slot.GetRareUpgrade(8));
                        } else {
                            c.Heal(1 * slot.GetRareUpgrade(8));
                        }
                    }
                    Debug.Log("Rare | 8 (Rare | 7) (Rare | 6)");
                }

                if (slot.GetRareUpgrade(11) > 0) {                                  //rare 11
                    if (damage / 10 < 1) {
                        enemy.ApplyAnemia(1 * slot.GetRareUpgrade(11), 10);
                    } else {
                        enemy.ApplyAnemia((damage / 10) * slot.GetRareUpgrade(11), 10);
                    }
                    appliedAnemia = true;
                    Debug.Log("Rare | 11");
                }

                if (slot.GetRareUpgrade(14) > 0 && appliedAnemia) {                 //rare 14
                    FindAnyObjectByType<BuffManager>().AddBuff("power", 0.2f * slot.GetRareUpgrade(14), 5f);
                    Debug.Log("Rare | 14");
                }

                if (slot.GetLegendaryUpgrade(3) > 0) {                              //legendary 3
                    if (criticalHit) {
                        FindAnyObjectByType<Character>().GainAfterimage(3, true);
                        Debug.Log("Legendary | 3, crit");
                    } else {
                        FindAnyObjectByType<Character>().GainAfterimage(1, true);
                        Debug.Log("Legendary | 3, normal");
                    }
                }

                if (slot.GetLegendaryUpgrade(6) > 0 && criticalHit) {               //legendary 6 (on-hit version)
                    FindAnyObjectByType<BuffManager>().AddBuff("critdmg", 0.5f * slot.GetLegendaryUpgrade(6), 3);
                    FindAnyObjectByType<BuffManager>().DispelDebuff();
                    Debug.Log("Legendary | 6 (on-hit)");
                }

                if (slot.GetLegendaryUpgrade(8) > 0 && appliedAnemia) {             //legendary 8
                    enemy.AnemicShock();
                    Debug.Log("Legendary | 8");
                }

                if (slot.GetLegendaryUpgrade(10) > 0 && enemy.IsAnemic()) {         //legendary 10
                    enemy.AnemiaAcceleration();
                }

                if (slot.GetLegendaryUpgrade(12) > 0) {                             //legendary 12
                    if (FindAnyObjectByType<Character>().currentHp > 10) {
                        enemy.ApplySlow((float) ((FindAnyObjectByType<Character>().currentHp - 10) * 0.01f), 3);
                        Debug.Log("Legendary | 12");
                    }
                }
                //ON-HIT slot bonuses END here
            }
            if (enemy != null) {
                enemy.TakeDamage(damage);
            }
        }
    }
}