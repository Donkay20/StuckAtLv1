using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private int identity;
    private bool containsSkill = false;
    private bool absorbBulletAvailable = true;
    private int skillID = -1, skillUses = -1;
    [SerializeField] private Image skillImage;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletTransform;
    

    private void Start() {
        absorbBulletAvailable = true;
        containsSkill = false;
    }

    public int Identity { get => identity; set => identity = value; }
    public bool ContainsSkill { get => containsSkill; set => containsSkill = value; }
    public bool AbsorbBulletAvailable { get => absorbBulletAvailable; set => absorbBulletAvailable = value; }

    public void Engage() {  //handles slot; whether to use skill or to absorb skill
        Instantiate(bullet, bulletTransform.position, Quaternion.identity, transform);
        /*
        if (!containsSkill && absorbBulletAvailable) {
            Debug.Log("conditions met");
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            //newBullet.transform.parent = transform;
            Debug.Log(absorbBulletAvailable);
            this.absorbBulletAvailable = false;
            Debug.Log(absorbBulletAvailable);
        } else {
            //use skill
            if (skillUses <= 0) {
                //empty out the skill, remove image
                containsSkill = false;
                skillID = -1; skillUses = -1;
            }
        }
        */
    }
}
