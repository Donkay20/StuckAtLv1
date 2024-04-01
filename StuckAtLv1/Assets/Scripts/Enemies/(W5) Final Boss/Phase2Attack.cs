using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2Attack : MonoBehaviour
{
    private readonly int ARROW_DAMAGE = 4;
    private readonly int LASER_DAMAGE = 3;
    private readonly int JAIL_DAMAGE = 10;
    private BuffManager buffManager; 
    [SerializeField] bool arrow, laser, jail;
    void Start() {
        buffManager = FindAnyObjectByType<BuffManager>();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Character>(out var player)) {
            if (arrow) {
                player.TakeDamage(ARROW_DAMAGE);
                buffManager.AddDebuff("slow", 0.5f, 1);
            } 
            if (laser) {
                player.TakeDamage(LASER_DAMAGE);
                buffManager.AddDebuff("bleed", 0.7f, 3);
            }
            if (jail) {
                player.TakeDamage(JAIL_DAMAGE);
                buffManager.AddDebuff("anemia", 0.25f, 2);
                buffManager.AddDebuff("slow", 0.5f, 1);
            }
        }
    }
}
