using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ConductionManager : MonoBehaviour
{
    [SerializeField] private Conductor[] conductors;
    [SerializeField] private ConductorBolt[] conductorBolts;
    [SerializeField] private TextMeshProUGUI conductorCountUI;
    [SerializeField] private GameObject additionalInfo;
    private int[] conductorsActivated = new int[3];
    private ConductorBolt[] chosenBolts = new ConductorBolt[3];
    private int conductorsActive;
    private bool boltBarrage;

    /*
    This class is responsible for the lightning conductor mechanic for the catacombs miniboss, the Goop (bigslime).
    An additional info field was added to the UI to inform the user of how many conductors are still needed to activate the lightning attack that'll damage the boss.

    Grabbing the boss' ability and firing it at a conductor will charge it, and charging three of them will launch an electric attack in a triangle that'll damage the boss.
    This attack shrinks the boss, and can only hurt the Goop.
    */

    void OnEnable() {
        additionalInfo.SetActive(true);
        conductorCountUI.text = "Conductors: "+ conductorsActive + "/3";
    }

    public void ActivateConductor(int id) {
        conductorsActivated[conductorsActive] = id;
        conductorsActive++;
        conductorCountUI.text = "Conductors: "+ conductorsActive + "/3";
        if (conductorsActive == 3) {
            conductorCountUI.text = "Shock imminent!";
            BoltBarrageSetup();
        }
    }

    private void BoltBarrageSetup() {
        boltBarrage = true;
        for (int i = 0; i < 3; i++) {
            switch(i) {
                case 0:
                    switch (conductorsActivated[0], conductorsActivated[1]) {
                        case (1,2):
                        case (2,1):
                            chosenBolts[0] = conductorBolts[0];
                            break;
                        case (1,3):
                        case (3,1):
                            chosenBolts[0] = conductorBolts[1];
                            break;
                        case (1,4):
                        case (4,1):
                            chosenBolts[0] = conductorBolts[2];
                            break;
                        case (2,3):
                        case (3,2):
                            chosenBolts[0] = conductorBolts[3];
                            break;
                        case (2,4):
                        case (4,2):
                            chosenBolts[0] = conductorBolts[4];
                            break;
                        case (3,4):
                        case (4,3):
                            chosenBolts[0] = conductorBolts[5];
                            break;
                    }
                    break;
                case 1:
                    switch (conductorsActivated[1], conductorsActivated[2]) {
                        case (1,2):
                        case (2,1):
                            chosenBolts[1] = conductorBolts[0];
                            break;
                        case (1,3):
                        case (3,1):
                            chosenBolts[1] = conductorBolts[1];
                            break;
                        case (1,4):
                        case (4,1):
                            chosenBolts[1] = conductorBolts[2];
                            break;
                        case (2,3):
                        case (3,2):
                            chosenBolts[1] = conductorBolts[3];
                            break;
                        case (2,4):
                        case (4,2):
                            chosenBolts[1] = conductorBolts[4];
                            break;
                        case (3,4):
                        case (4,3):
                            chosenBolts[1] = conductorBolts[5];
                            break;
                    }
                    break;
                case 2:
                    switch (conductorsActivated[2], conductorsActivated[0]) {
                        case (1,2):
                        case (2,1):
                            chosenBolts[2] = conductorBolts[0];
                            break;
                        case (1,3):
                        case (3,1):
                            chosenBolts[2] = conductorBolts[1];
                            break;
                        case (1,4):
                        case (4,1):
                            chosenBolts[2] = conductorBolts[2];
                            break;
                        case (2,3):
                        case (3,2):
                            chosenBolts[2] = conductorBolts[3];
                            break;
                        case (2,4):
                        case (4,2):
                            chosenBolts[2] = conductorBolts[4];
                            break;
                        case (3,4):
                        case (4,3):
                            chosenBolts[2] = conductorBolts[5];
                            break;
                    }
                    break;
            }
        }
        StartCoroutine(BoltBarrage());
    }

    private IEnumerator BoltBarrage() {
        chosenBolts[0].gameObject.SetActive(true);
        chosenBolts[1].gameObject.SetActive(true);
        chosenBolts[2].gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        DeactivateAllConductors();
    }

    private void DeactivateAllConductors() {
        conductors[conductorsActivated[0] - 1].Deactivate();
        conductors[conductorsActivated[1] - 1].Deactivate();
        conductors[conductorsActivated[2] - 1].Deactivate();

        chosenBolts[0].gameObject.SetActive(false);
        chosenBolts[1].gameObject.SetActive(false);
        chosenBolts[2].gameObject.SetActive(false);

        conductorsActivated[0] = 0; conductorsActivated[1] = 0; conductorsActivated[2] = 0;
        chosenBolts[0] = null; chosenBolts[1] = null; chosenBolts[2] = null;

        conductorsActive = 0; 
        conductorCountUI.text = "Conductors: "+ conductorsActive + "/3";
        boltBarrage = false;
    }

    public bool BarrageActive() {
        return boltBarrage;
    }
}