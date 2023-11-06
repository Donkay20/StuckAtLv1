using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private int identity;
    private bool containsSkill;

    private void Start() {
        Debug.Log(identity);
        Debug.Log(containsSkill);
    }

    public int Identity { get => identity; set => identity = value; }
    public bool ContainsSkill { get => containsSkill; set => containsSkill = value; }
}
