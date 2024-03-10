using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BuffManager : MonoBehaviour
{
    [SerializeField] private Character character;
    //Debugging purposes.
    [SerializeField] private GameObject buffPrefab, debuffPrefab;    
    [SerializeField] private GameObject[] buffPlacement = new GameObject[5];
    [SerializeField] private GameObject[] debuffPlacement = new GameObject[5];
    private Buff[] buffs = new Buff[5];
    private Debuff[] debuffs = new Debuff[5];
    //Back-end
    private int buffPosition, debuffPosition;
    private bool bloodsuckerActive, bulwarkActive, penetrationActive, avariceActive;
    //Variables

    private void Awake() {
        buffPosition = 0;
        debuffPosition = 0;
    }

    private void OnEnable() {
        buffPosition = 0;
        debuffPosition = 0;
    }

    public void AddBuff(string buff, float efficacy, float duration) {
        //Add a buff to the list. Instantiate a new buff object and populate it.
        if (buffPosition < 5) {
            GameObject newBuff = Instantiate(buffPrefab, transform);
            Buff b = newBuff.GetComponent<Buff>(); 
            b.Identity = buffPosition;
            buffs[buffPosition] = b;
            b.gameObject.transform.position = buffPlacement[buffPosition].transform.position;
            b.Initialize(buff, efficacy, duration);
            buffPosition++;
        }
    }

    public void AddDebuff(string debuff, float severity, float duration) {
        //Add a debuff to the list. Instantiate a new debuff object and populate it.
        if (debuffPosition < 5) {
            GameObject newDebuff = Instantiate(debuffPrefab, transform);
            Debuff d = newDebuff.GetComponent<Debuff>();
            d.Identity = debuffPosition;
            debuffs[debuffPosition] = d;
            d.gameObject.transform.position = debuffPlacement[debuffPosition].transform.position;
            d.Initialize(debuff, severity, duration);
            debuffPosition++;
        }
    }

    public void BuffExpired(int id) {
        //Remove a buff from the list. Rearranges the positions of the existing buffs and changes their identities accordingly.
        buffs[id] = null;
        for (int i = id; i < buffPosition - 1; i++) {
            buffs[i + 1].transform.position = buffPlacement[i].transform.position;
            buffs[i] = buffs[i + 1];
            buffs[i].Identity--; 
        }
        buffs[buffPosition - 1] = null;
        buffPosition--;
        //Debug.Log("Buff Position: " + buffPosition);
    }

    public void DebuffExpired(int id) {
        //Remove a debuff from the list. Rearranges the positions of the existing debuffs and changes their identities accordingly.
        debuffs[id] = null;
        for (int i = id; i < debuffPosition - 1; i++) {
            debuffs[i + 1].transform.position = debuffPlacement[i].transform.position;
            debuffs[i] = debuffs[i + 1];
            debuffs[i].Identity--;
        }
        debuffs[debuffPosition - 1] = null;
        debuffPosition--;
        //Debug.Log("Debuff Position: " + debuffPosition);
    }

    public void DispelDebuff() {
        if (debuffPosition > 0) {
            debuffs[0].EndDebuff();
        }
    }

    public bool IsBloodsuckerActive() {
        return bloodsuckerActive;
    }

    public void SetBloodsuckerStatus(bool x) {
        bloodsuckerActive = x;
    }

    public bool IsBulwarkActive() {
        return bulwarkActive;
    }

    public void SetBulwarkStatus(bool x) {
        bulwarkActive = x;
    }

    public bool IsPenetrationActive() {
        return penetrationActive;
    }

    public void SetPenetrationStatus(bool x) {
        penetrationActive = x;
    }

    public bool IsAvariceActive() {
        return avariceActive;
    }

    public void SetAvariceStatus(bool x) {
        avariceActive = x;
    }

    public void BattleEnd() {
        //Clear all buffs and debuffs.
        Buff[] buffsToDelete = FindObjectsOfType<Buff>();
        foreach(Buff b in buffsToDelete) {
            b.EndBuff();
        }
        Debuff[] debuffsToDelete = FindObjectsOfType<Debuff>();
        foreach (Debuff d in debuffsToDelete) {
            d.EndDebuff();
        }
    }
}