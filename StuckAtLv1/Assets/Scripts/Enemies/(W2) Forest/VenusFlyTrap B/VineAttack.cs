using UnityEngine;

public class VineAttack : MonoBehaviour
{
    private readonly int VINE_DAMAGE = 3;
    private BuffManager buffManager; 
    void Start() {
        buffManager = FindAnyObjectByType<BuffManager>();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Character>(out var player)) {
            player.TakeDamage(VINE_DAMAGE);
            buffManager.AddDebuff("slow", 0.5f, 1);
            buffManager.AddDebuff("bleed", 0.5f, 3);
        }
    }
}
