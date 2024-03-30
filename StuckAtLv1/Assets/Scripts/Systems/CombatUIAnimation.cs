using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUIAnimation : MonoBehaviour
{
    [SerializeField] private CombatManager combatManager;

    public void OutroAnimationFinished() {
        combatManager.OutroTransitionAnimationComplete();
    }
}
