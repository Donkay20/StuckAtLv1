using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TextMeshPro additionalText;
    [SerializeField] private TextMeshPro damageNumber;
    private float lifetime = 2f;
    void Start() {
        Destroy(gameObject, lifetime);
        damageNumber = GetComponent<TextMeshPro>();
    }

    public void Setup(int damage, string additionalStatus, bool crit, bool anemic) {
        damageNumber.text = damage.ToString();
        if (crit) {damageNumber.text += "!";}
        additionalText.text = additionalStatus;
        if (anemic) {damageNumber.color = Color.green;}
    }
}
