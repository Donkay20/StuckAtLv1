using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamImp_Laser : MonoBehaviour
{
    private readonly int LASER_BEAM_BASE_DMG = 4;
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Character>(out var player)) {
            BuffManager b = FindAnyObjectByType<BuffManager>();
            player.TakeDamage(LASER_BEAM_BASE_DMG);
            b.AddDebuff("slow", 0.5f, 0.5f);
        }
    }
}
