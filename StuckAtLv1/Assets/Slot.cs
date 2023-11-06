using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private int identity;
    private bool containsSkill;
    private int skillID, skillUses;
    [SerializeField] private Image skillImage;

    private void Start() {
        //Debug.Log(identity);
        //Debug.Log(containsSkill);
        skillID = -1;
        skillUses = -1;
    }

    public int Identity { get => identity; set => identity = value; }
    public bool ContainsSkill { get => containsSkill; set => containsSkill = value; }

    public void Engage() {  //handles slot; whether to use skill or to absorb skill
        if (!containsSkill) {
            //absorb bullet
            containsSkill = true;
        } else {
            //use skill
            if (skillUses <= 0) {
                //empty out the skill, remove image
                containsSkill = false;
                skillID = -1; skillUses = -1;
            }
        }
    }
}
