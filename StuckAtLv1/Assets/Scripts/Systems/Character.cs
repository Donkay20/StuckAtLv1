using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    private readonly float iframe = 0.3f;
    private bool invincible; //iframe check
    private bool healthDraining; //overheal drain check
    [SerializeField] Animator playerAnim;
    [SerializeField] Animator healthBarAnim;
    [SerializeField] StatusBar hpBar;
    [SerializeField] private TextMeshProUGUI healthText;

    public void TakeDamage(int damage) { //todo; edit for buffs/dmg reduction values
        if (!invincible) {
            playerAnim.SetTrigger("Hit");
            healthBarAnim.SetTrigger("Hit");
            invincible = true;
            currentHp -= damage; 
            healthText.text = currentHp.ToString();
            if (currentHp <= maxHp) {
                healthText.color = new Color32(255, 0, 0, 255);
                healthDraining = false;
            }
            StartCoroutine(InvincibilityFrame());
        }
        
        if (currentHp <= 0) {
            //Debug.Log("Jamp is dead.");
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
                yield return new WaitForSeconds(1); //if there's a buff/debuff that would increase/reduce drain time, add it here.
            }
        }
    }

    public void Interrupt() { //needed when we stop combat and move to the next scene
        StopAllCoroutines();
        invincible = false;
        healthDraining = false;
    }

    public void Heal(int amount) {  //needs to be changed to account for overhealing / temp HP
        if (currentHp <= 0) {
            return;
        }
        Debug.Log("Healed " + amount + " HP. Health: " + currentHp);
        currentHp += amount;
        healthText.text = currentHp.ToString();

        if (currentHp > maxHp && !healthDraining) {
            healthText.color = new Color32(166, 254, 0, 255);
            StartCoroutine(DrainHealth());
        }
    }

    public void DashingIFrames()
    {
        invincible = true;
    }
    public void StopDashingIFrames()
    {
        invincible = false;
    }
}
