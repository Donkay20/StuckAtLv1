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
    public readonly int MAX_HP = 10;
    public int currentHp = 10;
    public int money = 0;
    public float afterimage = 0;
    private readonly float iframe = 0.3f;
    private bool invincible; //iframe check
    private bool healthDraining; //overheal drain check
    private float damageModifier; //from buffs/debuffs
    private float criticalDamageModifier; //from buffs
    private float drainTimer;
    private float drainTimerModifier = 1; public float DrainTimerModifier { get => drainTimerModifier; set => drainTimerModifier = value; } //from buffs/debuffs
    private int drainValue;

    [SerializeField] Animator playerAnim;
    [SerializeField] Animator healthBarAnim;
    [SerializeField] StatusBar hpBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI afterimageText;
    [SerializeField] private TextMeshProUGUI moneyText;

    void OnEnable() {
        afterimageText.text = afterimage.ToString();
        moneyText.text = money.ToString();
        damageModifier = 1;
        drainValue = 1;
        criticalDamageModifier = 0;
        CalculateDrainTimer();
    }

    private void Update() {
        CalculateDrainTimer();
        if (currentHp > 999) {
            currentHp = 999;
            healthText.text = currentHp.ToString();
        }

        if (afterimage > 0) {
            afterimage -= Time.deltaTime;
            afterimageText.text = afterimage.ToString("f1");
        }

        if (afterimage < 0) {
            afterimage = 0;
            afterimageText.text = afterimage.ToString("f1");
        }
    }

    public void TakeDamage(int damage) {
        if (!invincible) {
            playerAnim.SetTrigger("Hit");
            healthBarAnim.SetTrigger("Hit");
            invincible = true;
            if (afterimage <= 0) {
                currentHp -= damage; 
                healthText.text = currentHp.ToString();
                if (currentHp <= MAX_HP) {
                    healthText.color = new Color32(255, 240, 240, 255);
                    healthDraining = false;
                }
            }
            StartCoroutine(InvincibilityFrame());
        }
        
        if (currentHp <= 0) {
            //die
            SceneManager.LoadScene("TitleScreen");
        }
        hpBar.SetState(currentHp, MAX_HP);
    }

    IEnumerator InvincibilityFrame() {              //elapse this amt of time before the character can be hit again
        yield return new WaitForSeconds(iframe);    //anything that would increase the amt of iframes would prob go here, although idk if this will be a bonus
        invincible = false;
    }

    IEnumerator DrainHealth() {
        healthDraining = true;

        while (currentHp > MAX_HP) {
            yield return new WaitForSeconds(drainTimer); //Buffs or debuffs that affect drain time take effect here.
            currentHp -= drainValue; 
            healthText.text = currentHp.ToString();

            if (currentHp <= MAX_HP) {
                healthText.color = new Color32(255, 240, 240, 255);
                healthDraining = false;
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

        if (currentHp > MAX_HP && !healthDraining) {
            healthText.color = new Color32(166, 254, 0, 255);
            StartCoroutine(DrainHealth());
        }
    }

    public void GainAfterimage(float amount, bool exceedCap) {
        if (afterimage + amount > 10 && !exceedCap) {
            afterimage = 10;
        } else {
            afterimage += amount;
        }
    }

    public void GainMoney(int amount) {
        if (amount + money > 999999) {
            money = 999999;
        } else {
            money += amount;
        }
        moneyText.text = money.ToString();
    }

    private void CalculateDrainTimer() {
        switch(currentHp) {         //overheal will drain faster the more health the player has.
            case int n when n > 10 && n <= 100:
                drainTimer = 1;
                break;
            case int n when n > 100 && n <= 200:
                drainTimer = 0.9f;
                break;
            case int n when n > 200 && n <= 300:
                drainTimer = 0.8f;
                break;
            case int n when n > 300 && n <= 400:
                drainTimer = 0.7f;
                break;
            case int n when n > 400 && n <= 500:
                drainTimer = 0.6f;
                break;
            case int n when n > 500 && n <= 600:
                drainTimer = 0.5f;
                break;
            case int n when n > 600 && n <= 700:
                drainTimer = 0.4f;
                break;
            case int n when n > 700 && n <= 800:
                drainTimer = 0.3f;
                break;
            case int n when n > 800 && n <= 900:
                drainTimer = 0.2f;
                break;
            case int n when n > 900:
                drainTimer = 0.1f;
                break;
        }

        drainTimer *= drainTimerModifier;   //apply modifiers

        if (drainValue < 0) {               //if bulwark is active, don't accelerate anything
            drainTimer = 1 * drainTimerModifier;
        }
    }

    public void ActivateBloodsucker(int hpToRestore) {
        Heal(hpToRestore);
    }

    public void AdjustDamageModifier(float value) {
        damageModifier += value;
        damageModifier =  Mathf.Round(damageModifier * 100) / 100f;               //floats can sometimes lose precision; this should restore the precision after each instance
    }

    public float GetDamageModifier() {
        return damageModifier;
    }

    public void AdjustCriticalDamageModifier(float value) {
        criticalDamageModifier += value;
        criticalDamageModifier = Mathf.Round(criticalDamageModifier * 100) / 100f;       //floats can sometimes lose precision; this should restore the precision after each instance
    }

    public float GetCriticalDamageModifier() {
        return criticalDamageModifier;
    }

    public void ActivateBulwark() {
        drainValue = -1;
        drainTimerModifier /= 2;
    }

    public void DeactivateBulwark() {
        drainValue = 1;
        drainTimerModifier *= 2;
    }

    public void DashingIFrames() {
        invincible = true;
    }

    public void StopDashingIFrames() {
        invincible = false;
    }
}