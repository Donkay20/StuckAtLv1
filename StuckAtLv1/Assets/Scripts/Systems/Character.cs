using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
/*
Handles main character's active stats in combat, their buffs, and their damage handling.
*/
{
    public readonly int maxHp = 10;
    public int currentHp = 10;
    public int money = 0;
    public int afterimages = 0;
    private readonly float iframe = 0.3f;
    private bool invincible; //iframe check
    private bool healthDraining; //overheal drain check
    private float damageModifier; //from buffs/debuffs
    private float criticalDamageModifier; //from buffs
    private float drainTimer = 1; public float DrainTimer { get => drainTimer; set => drainTimer = value; } //from buffs/debuffs
    private int drainValue;

    [SerializeField] Animator playerAnim;
    [SerializeField] Animator healthBarAnim;
    [SerializeField] StatusBar hpBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI afterimageText;
    [SerializeField] private TextMeshProUGUI moneyText;

    void OnEnable() {
        afterimageText.text = afterimages.ToString();
        moneyText.text = money.ToString();
        damageModifier = 1;
        drainTimer = 1;
        drainValue = 1;
        criticalDamageModifier = 0;
        afterimages /= 2;
    }

    public void TakeDamage(int damage) {
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
                    healthText.color = new Color32(255, 240, 240, 255);
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
            currentHp -= drainValue; 
            healthText.text = currentHp.ToString();
            if (currentHp <= maxHp) {
                healthText.color = new Color32(255, 240, 240, 255);
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
            Debug.Log("Health capped!");
        } else {
            currentHp += amount;
        }
        healthText.text = currentHp.ToString();
        //Debug.Log("Healed " + amount + " HP. Health: " + currentHp);
        if (currentHp > maxHp && !healthDraining) {
            healthText.color = new Color32(166, 254, 0, 255);
            StartCoroutine(DrainHealth());
        }
    }

    public void GainAfterimage(int amount) {
        if (afterimages + amount > 99) {
            afterimages = 99;
            Debug.Log("Afterimages capped!");
        } else {
            afterimages += amount;
        }
        afterimageText.text = afterimages.ToString();
    }

    public void GainMoney(int amount) {
        if (amount + money > 999999) {
            money = 999999;
            Debug.Log("Money capped!");
        } else {
            money += amount;
        }
        moneyText.text = money.ToString();
    }

    public void ActivateBloodsucker(int hpToRestore) {
        Heal(hpToRestore);
    }

    public void AdjustDamageModifier(float value) {
        damageModifier += value;
        damageModifier = Mathf.Round(value * 100) / 100f;               //floats can sometimes lose precision; this should restore the precision after each instance
    }

    public float GetDamageModifier() {
        return damageModifier;
    }

    public void AdjustCriticalDamageModifier(float value) {
        criticalDamageModifier += value;
        criticalDamageModifier = Mathf.Round(value * 100) / 100f;       //floats can sometimes lose precision; this should restore the precision after each instance
    }

    public float GetCriticalDamageModifier() {
        return criticalDamageModifier;
    }

    public void ActivateBulwark() {
        drainValue = -1;
    }

    public void DeactivateBulwark() {
        drainValue = 1;
    }

    public void DashingIFrames() {
        invincible = true;
    }

    public void StopDashingIFrames() {
        invincible = false;
    }
}
