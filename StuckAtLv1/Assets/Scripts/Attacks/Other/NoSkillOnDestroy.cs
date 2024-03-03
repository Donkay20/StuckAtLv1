using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSkillOnDestroy : MonoBehaviour
{
    //We don't need these, so get rid of them if they ever do spawn.
    void Start() {
        Debug.Log("NoSkill has spawned.");
        Destroy(gameObject);
    }
}
