using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneToss : MonoBehaviour
{
    float timer = 2f;   //if a modifier increase skill time duration, it would call back to the parent slot and acquire the modifier for calculation
    private readonly int BONETOSS_BASE_DMG = 5;
    Rigidbody2D rb;
    Slot parent;
    private Vector3 mousePosition;
    private Camera mainCamera;
    public float speed;
    private int damage;
    private bool criticalHit;

    void Start() {  
        //Aim towards the mouse
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        parent = GetComponentInParent<Slot>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;

        //start of dmg bonuses
        damage += BONETOSS_BASE_DMG;                                                //base damage
        damage *= (int) (1 + (parent.GetCommonUpgrade(0) * 0.1f));                  //common 0, 10% damage
        damage *= (int) (1 + (parent.GetRareUpgrade(1) * 0.1f));                    //rare 1, 10% damage
        damage *= (int) FindAnyObjectByType<Character>().DamageModifier;            //buffs
        damage *= (int) FindAnyObjectByType<GameManager>().GetShopDamageBonus();    //should be the last multiplier
        //end of dmg bonuses

        //start of size bonuses
        float scalingFactor = 1 + parent.GetCommonUpgrade(1)*0.05f + parent.GetRareUpgrade(1)*0.05f;                 //common 1, size | rare 1, size
        transform.localScale = new Vector2(.5f*scalingFactor, .5f*scalingFactor);       //for the bone toss, 0.5f is the base size (this shit's too big).
        //end of size bonuses

        //start of duration bonuses
        timer *= 1 + parent.GetCommonUpgrade(2)*0.2f + parent.GetRareUpgrade(1)*0.2f;   //common 2, duration | rare 1, duration
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
            criticalHit = parent.CriticalHit();
            
            //begin on-kill slot bonuses here
            if (damage > enemy.GetHealth()) {
                int bonusDropChance = 0;
                bonusDropChance += 5 * parent.GetCommonUpgrade(12);     //common 12, bonus treasure chest drop chance

                if (parent.GetCommonUpgrade(13) > 0) {                  //common 13, bonus gold
                    enemy.RaiseReward(parent.GetCommonUpgrade(13) * 5);
                }

                if (parent.GetCommonUpgrade(14) > 0) {                  //common 14, dispel debuff
                    BuffManager b = FindAnyObjectByType<BuffManager>();
                    for (int i = 0; i < parent.GetCommonUpgrade(14); i++) {
                        b.DispelDebuff();
                    }
                }
                enemy.DropMoney(bonusDropChance);
            }
            //end on-kill slot bonuses here

            //begin on-hit slot bonuses here
            if (parent.GetCommonUpgrade(7) > 0) {                   //common 7, slow
                enemy.ApplySlow(1 - (parent.GetCommonUpgrade(7) * 0.2f), 3);
            }
            if (parent.GetCommonUpgrade(10) > 0) {                  //common 8, anemia
                if (Random.Range(0, 2) == 1) {
                    enemy.ApplyAnemia(parent.GetCommonUpgrade(10), parent.GetCommonUpgrade(10) * 3);
                }
            }
            //end on-hit slot bonuses here

            if (criticalHit) {
                damage *= (int) parent.CriticalDamage();
                enemy.CriticalHit();
            } 
            enemy.TakeDamage(damage);
        }
    }
}