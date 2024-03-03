using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberParent : MonoBehaviour
{
    public void BattleOver() {
        Destroy(gameObject);
    }
}
