using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
/*
Handles main character's active stats in combat, their buffs, and their damage handling.

todo: 
> implement variables and conditions for buffs and debuffs
> implement temp hp
*/
{
    public readonly int maxHp = 10;
    public int currentHp = 10;
    public int money = 0;
    public int afterimages = 0;
    private readonly float iframe = 0.3f;
    private bool invincible; //iframe check
    private bool healthDraining; //overheal drain check
    private float damageModifier; public float DamageModifier { get => damageModifier; set => damageModifier = value; }
    private float drainTimer = 1; public float DrainTimer { get => drainTimer; set => drainTimer = value; }

    [SerializeField] Animator playerAnim;
    [SerializeField] Animator healthBarAnim;
    [SerializeField] StatusBar hpBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI afterimageText;
    [SerializeField] private TextMeshProUGUI moneyText;

    void OnEnable() {
        afterimageText.text = afterimages.ToString();
        moneyText.text = money.ToString();
        damageModifier = 0;
        drainTimer = 1;
    }

    public void TakeDamage(int damage) { //todo; edit for buffs/dmg reduction values
        if (!invincible) {
            playerAnim.SetTrigger("Hit");
            healthBarAnim.SetTrigger("Hit");
            invincible = true;
            if (afterimages > 0) {
                afterimages --;
                afterimageText.text = afterimages.ToString();
            } else {
                currentHp -= damage; 
                healthText.text = currentHp.ToString();
                if (currentHp <= maxHp) {
                    healthText.color = new Color32(255, 0, 0, 255);
                    healthDraining = false;
                }
            }
            
            StartCoroutine(InvincibilityFrame());
        }
        
        if (currentHp <= 0) {
            SceneManager.LoadScene("TitleScreen");
        }
        hpBar.SetState(currentHp, maxHp);
    }

    IEnumerator InvincibilityFrame() {              //elapse this amt of time before the character can be hit again
        yield return new WaitForSeconds(iframe);    //anything that would increase the amt of iframes would prob go here, although idk if this will be a bonus
        invincible = false;
    }

    IEnumerator DrainHealth() {
        healthDraining = true;
        while (currentHp > maxHp) {
            currentHp--; healthText.text = currentHp.ToString();
            if (currentHp <= maxHp) {
                healthText.color = new Color32(255, 0, 0, 255);
                healthDraining = false;
                yield break;
            } else {
                yield return new WaitForSeconds(drainTimer); //Buffs or debuffs that affect drain time take effect here.
            }
        }
    }

    public void Interrupt() { //needed when we stop combat and move to the next scene
        StopAllCoroutines();
        invincible = false;
        healthDraining = false;
    }

    public void Heal(int amount) { 
        if (currentHp <= 0) {
            return;
        }
        if (currentHp + amount > 999) {
            currentHp = 999;
        } else {
            currentHp += amount;
        }
        healthText.text = currentHp.ToString();
        Debug.Log("Healed " + amount + " HP. Health: " + currentHp);
        if (currentHp > maxHp && !healthDraining) {
            healthText.color = new Color32(166, 254, 0, 255);
            StartCoroutine(DrainHealth());
        }
    }

    public void GainAfterimage(int amount) {
        if (afterimages + amount > 99) {
            afterimages = 99;
        } else {
            afterimages += amount;
        }
        afterimageText.text = afterimages.ToString();
    }

    public void GainMoney(int amount) {
        money += amount;
        moneyText.text = money.ToString();
    }

    public void DashingIFrames() {
        invincible = true;
    }

    public void StopDashingIFrames() {
        invincible = false;
    }

    public bool CriticalHit(Slot slot) {
        bool isCrit = false; int critChance = 5;
        //apply crit bonuses here, todo
        if (Random.Range(1,101) <= critChance) {isCrit = true;}
        return isCrit;
    }
}
