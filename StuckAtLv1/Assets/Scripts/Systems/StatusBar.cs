using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBar : MonoBehaviour
/*
Handles HP bar under main character during combat. Probably temporary.
*/
{
    [SerializeField] Transform bar;
    [SerializeField] private SpriteRenderer barFill;

    public void SetState(int current, int max) {
        float state = (float)current;
        state /= max;
        if (state < 0f) {state = 0f;}
        if (state > 1f) {state = 1f;}
        bar.transform.localScale = new Vector3(state, 1f, 1f);

        if (state > 1) {
            barFill.color = new Color32(166, 254, 0, 255);
        } else {
            barFill.color = new Color32(255, 240, 240, 255);
        }
    }
}
