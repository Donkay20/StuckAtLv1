using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneToss : MonoBehaviour
{
    float BASE_TIMER = 2f;   //if a modifier increase skill time duration, it would call back to the parent slot and acquire the modifier for calculation
    private readonly int BONETOSS_BASE_DMG = 5;
    Rigidbody2D rb;
    Slot parent;
    private Vector3 mousePosition;
    private Camera mainCamera;
    public float timer;
    public float speed;
    private int damage;
    private float size;
    private bool criticalHit;

    void Start() {  
        //Aim towards the mouse
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        parent = GetComponentInParent<Slot>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        damage = asb.GetDamageBonus(parent, BONETOSS_BASE_DMG);

        size = asb.GetSizeBonus(parent);
        transform.localScale = new Vector2(.5f * size, .5f * size);       //for the bone toss, 0.5f is the base size.

        timer = asb.GetDurationBonus(parent, BASE_TIMER);
        //end of duration bonuses
    }

    void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            bool appliedAnemia = false;
            criticalHit = parent.CriticalHit();                                     //calculate critical hit
            if (criticalHit) {
                    damage *= (int) parent.CriticalDamage();
                    enemy.CriticalHit();
            }

            //begin on-kill slot bonuses here
            if (damage > enemy.GetHealth()) {                                       //if the attack kills
                int bonusDropChance = 0;

                if (parent.GetCommonUpgrade(12) > 0) {                              //common 12
                    bonusDropChance += 5 * parent.GetCommonUpgrade(12);
                    Debug.Log("Common | 12");
                }

                if (parent.GetCommonUpgrade(13) > 0) {                              //common 13
                    enemy.RaiseReward(parent.GetCommonUpgrade(13) * 5);
                    Debug.Log("Common | 13");
                }

                if (parent.GetCommonUpgrade(14) > 0) {                              //common 14
                    for (int i = 0; i < parent.GetCommonUpgrade(14); i++) {
                        FindAnyObjectByType<BuffManager>().DispelDebuff();
                    }
                    Debug.Log("Common | 14");
                }

                if (parent.GetLegendaryUpgrade(1) > 0 && parent.SkillUses() > 0) {  //legendary 1
                    parent.GetSkillUses();
                }

                if (parent.GetLegendaryUpgrade(4) > 0 && criticalHit) {             //legendary 4 (on-kill version)
                    parent.RefundCooldown();
                }

                if (parent.GetLegendaryUpgrade(5) > 0 && criticalHit) {             //legendary 5 (on-kill version)
                    FindAnyObjectByType<Character>().GainMoney(damage / 10);
                }

                if (parent.GetLegendaryUpgrade(11) > 0) {                           //legendary 11
                    if (FindAnyObjectByType<Character>().currentHp > 10) {
                        bonusDropChance += (int) ((FindAnyObjectByType<Character>().currentHp - 10) * (0.05f * parent.GetLegendaryUpgrade(11)));
                    }
                }
                
                if (parent.GetLegendaryUpgrade(13) > 0) {                           //legendary 13
                    if (!FindAnyObjectByType<BuffManager>().IsBulwarkActive()) {
                        FindAnyObjectByType<BuffManager>().AddBuff("bulwark", 0, 5);
                    }
                }

                enemy.DropMoney(bonusDropChance);
                //end on-kill slot bonuses here
            } else {                                                                //if the attack doesn't kill (do on-hit stuff instead)
                //begin on-hit slot bonuses here
                if (parent.GetRareUpgrade(12) > 0 && enemy.IsAnemic()) {            //rare 12
                    FindAnyObjectByType<Character>().GainMoney(enemy.AnemiaSeverity());
                    Debug.Log("Rare | 12");
                }

                if (parent.GetCommonUpgrade(7) > 0) {                               //common 7
                    enemy.ApplySlow(1 - (parent.GetCommonUpgrade(7) * 0.2f), 3);
                    Debug.Log("Common | 7");
                }

                if (parent.GetCommonUpgrade(10) > 0) {                              //common 8
                    if (Random.Range(0, 2) == 1) {
                        enemy.ApplyAnemia(parent.GetCommonUpgrade(10), parent.GetCommonUpgrade(10) * 3);
                        appliedAnemia = true;
                        Debug.Log("Common | 8");
                    }
                }

                if (parent.GetRareUpgrade(8) > 0) {                                 //rare 8
                    Character c = FindAnyObjectByType<Character>();
                    if (criticalHit) {
                        c.Heal(damage *= (int) parent.CriticalDamage() / 10);
                    } else {
                        c.Heal(damage / 10);
                    }
                    Debug.Log("Rare | 8");
                }

                if (parent.GetRareUpgrade(11) > 0) {                                //rare 11
                    enemy.ApplyAnemia((damage / 10) * parent.GetRareUpgrade(11), 10);
                    appliedAnemia = true;
                    Debug.Log("Rare | 11");
                }

                if (parent.GetRareUpgrade(14) > 0 && appliedAnemia) {               //rare 14
                    FindAnyObjectByType<BuffManager>().AddBuff("power", 0.2f * parent.GetRareUpgrade(14), 5f);
                    Debug.Log("Rare | 14");
                }

                if (parent.GetLegendaryUpgrade(3) > 0) {                            //legendary 3
                    if (criticalHit) {
                        FindAnyObjectByType<Character>().GainMoney(FindAnyObjectByType<Character>().afterimages * 2);
                    } else {
                        FindAnyObjectByType<Character>().GainMoney(FindAnyObjectByType<Character>().afterimages);
                    }
                }
                
                if (parent.GetLegendaryUpgrade(4) > 0 && criticalHit) {             //legendary 4 (on-hit version)
                    parent.RefundCooldown();
                }

                if (parent.GetLegendaryUpgrade(5) > 0 && criticalHit) {             //legendary 5 (on-hit version)
                    FindAnyObjectByType<Character>().GainMoney(damage / 20);
                }

                if (parent.GetLegendaryUpgrade(6) > 0 && criticalHit) {             //legendary 6 (on-hit version)
                    FindAnyObjectByType<BuffManager>().AddBuff("critdmg", 0.5f * parent.GetLegendaryUpgrade(6), 3);
                }

                if (parent.GetLegendaryUpgrade(8) > 0 && appliedAnemia) {           //legendary 8
                    enemy.AnemicShock();
                }

                if (parent.GetLegendaryUpgrade(10) > 0 && enemy.IsAnemic()) {       //legendary 10
                    if (enemy.GetHealth() <= (enemy.maxHP / 2)) {
                        enemy.AnemicTorture();
                    }
                }

                if (parent.GetLegendaryUpgrade(12) > 0) {                           //legendary 12
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