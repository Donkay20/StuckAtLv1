using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private int identity;
    private bool containsSkill = false;
    private bool absorbBulletAvailable = true;
    private int skillID = 0, skillUses = 0;
    [SerializeField] private Image skillImage;
    [SerializeField] private TextMeshProUGUI uIText;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject[] attack = new GameObject[2];
    [SerializeField] private Transform bulletTransform;
    

    private void Start() {
        absorbBulletAvailable = true;
        containsSkill = false;
    }

    public int Identity { get => identity; set => identity = value; }
    public bool ContainsSkill { get => containsSkill; set => containsSkill = value; }
    public bool AbsorbBulletAvailable { get => absorbBulletAvailable; set => absorbBulletAvailable = value; }

    public void Engage() {  //handles slot; whether to use skill or to absorb skill
        if (!containsSkill && absorbBulletAvailable) {
            Instantiate(bullet, bulletTransform.position, Quaternion.identity, transform);
            this.absorbBulletAvailable = false;
        } else {
            Instantiate(attack[skillID], bulletTransform.position, Quaternion.identity, transform);
            if (skillUses > 0) {
                skillUses--;
            }
            uIText.text = skillUses.ToString();
            if (skillUses <= 0) {
                //empty out the skill, remove image
                containsSkill = false;
                //skillImage.sprite = null;
                skillImage.sprite = attack[0].GetComponent<SpriteRenderer>().sprite;
                skillID = 0; skillUses = 0;
            }
        }
    }
    public void AcquireSkill(int ID, int uses) {
        if(ID != 0) {
            skillID = ID;
            skillUses = uses;
            skillImage.sprite = attack[ID].GetComponent<SpriteRenderer>().sprite;
            uIText.text = skillUses.ToString();
            containsSkill = true;
        }
    }
}
