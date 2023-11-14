using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
/*
Handles main character's active stats in combat, their buffs, and their damage handling.

todo: 
> implement variables and conditions for buffs and debuffs
> implement temp hp
*/
{
    public int maxHp = 10;
    public int currentHp = 10;
    private readonly float iframe = 0.3f;
    private bool invincible;
    [SerializeField] Animator playerAnim;
    [SerializeField] Animator healthBarAnim;
    [SerializeField] StatusBar hpBar;

    public void TakeDamage(int damage) { //todo; edit for buffs/dmg reduction values
        if (!invincible) {
            playerAnim.SetTrigger("Hit");
            healthBarAnim.SetTrigger("Hit");
            invincible = true;
            currentHp -= damage;
            StartCoroutine(InvincibilityFrame());
        }
        
        if (currentHp <= 0) {
            Debug.Log("Jamp is dead.");
        }
        hpBar.SetState(currentHp, maxHp);
    }

    IEnumerator InvincibilityFrame() {              //elapse this amt of time before the character can be hit again
        yield return new WaitForSeconds(iframe);    //anything that would increase the amt of iframes would prob go here, although idk if this will be a bonus
        invincible = false;
    }

    public void Heal(int amount) {  //needs to be changed to account for overhealing / temp HP
        if (currentHp <= 0) {
            return;
        }
        currentHp += amount;
        if (currentHp > maxHp) {
            currentHp = maxHp;
        }
    }
}
