using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BuffManager : MonoBehaviour
{
    [SerializeField] private GameObject buffPrefab, debuffPrefab;    
    [SerializeField]private GameObject[] buffPlacement = new GameObject[5];
    private Buff[] buffs = new Buff[5];
    //Back-end
    private int buffPosition, debuffPosition;
    //Variables

    private void Awake() {
        buffPosition = 0;
        debuffPosition = 0;
    }

    private void OnEnable() {
        buffPosition = 0;
        debuffPosition = 0;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            AddBuff("speed", 0.3f, 5f);
        }
        if (Input.GetKeyDown(KeyCode.Y)) {
            AddBuff("power", 0.3f, 4f);
        }
        if (Input.GetKeyDown(KeyCode.U)) {
            AddBuff("bloodsucker", 0.5f, 6f);
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            AddBuff("bulwark", 0.5f, 30f);
        }
        //Manual buffs for testing purposes
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

    public void AddDebuff(string condition, int duration) {
        //todo
    }

    public void BuffExpired(int id) {
        buffs[id] = null;
        for (int i = id; i < buffPosition - 1; i++) {
            buffs[i+1].transform.position = buffPlacement[i].transform.position;
            buffs[i] = buffs[i+1]; 
        }
        buffs[buffPosition - 1] = null;
        buffPosition--;
        Debug.Log(buffPosition);
    }

    public void DebuffExpired(int id) {
        //todo
    }

    private void EndBattle() {
        Buff[] buffsToDelete = FindObjectsOfType<Buff>();
        foreach(Buff b in buffsToDelete) {
            b.EndBuff();
        }
    }
}

/*
Buff Icon directory:
0 - nothing/blank
1 - speed
2 - damage
*/
