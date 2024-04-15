using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConductionManager : MonoBehaviour
{
    [SerializeField] private Conductor[] conductors;
    [SerializeField] private ConductorBolt[] conductorBolts;
    [SerializeField] private TextMeshProUGUI conductorCountUI;
    private int[] conductorsActivated = new int[3];
    private ConductorBolt[] chosenBolts = new ConductorBolt[3];
    private int conductorsActive;
    private bool boltBarrage;
    void Start() {
        
    }

    void Update() {
        
    }

    public void ActivateConductor(int id) {
        conductorsActivated[conductorsActive] = id;
        conductorsActive++;
        if (conductorsActive == 3) {
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
        //todo
        yield return new WaitForSeconds(0);
        DeactivateAllConductors();
    }

    private void DeactivateAllConductors() {
        conductors[conductorsActivated[0]].Deactivate();
        conductors[conductorsActivated[1]].Deactivate();
        conductors[conductorsActivated[2]].Deactivate();
        boltBarrage = false;
    }

    public bool BarrageActive() {
        return boltBarrage;
    }
}
