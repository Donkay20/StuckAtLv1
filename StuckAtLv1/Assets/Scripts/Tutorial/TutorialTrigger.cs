using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private int trigger;
    [SerializeField] private TutorialManager tutorialManager;
    private bool hasBeenTriggered;

    private void Start() {
        hasBeenTriggered = false;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Character>(out var character) && !hasBeenTriggered) {
            tutorialManager.NextMessage(trigger);
            hasBeenTriggered = true;
        }
    }
}
