using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsOfTheDamned : MonoBehaviour
{
    private Slot parent;
    private float duration = 5f;
    private float refreshTimer = 0.05f;
    [SerializeField] private GameObject miniGhost;

    void Start() {
        parent = GetComponentInParent<Slot>();
        duration *= 1 + (parent.GetCommonUpgrade(2)*0.2f + parent.GetRareUpgrade(2)*0.4f + parent.GetLegendaryUpgrade(2)*0.6f);
    }

    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0) {
            Destroy(gameObject);
        }

        refreshTimer -= Time.deltaTime;
        if (refreshTimer <= 0) {
            Instantiate(miniGhost, parent.transform);
            refreshTimer = 0.05f;
        }
    }

    public Slot GetSlot() {
        return parent;
    }
}
