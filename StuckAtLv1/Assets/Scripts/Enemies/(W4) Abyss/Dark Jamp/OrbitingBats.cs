using UnityEngine;

public class OrbitingBats : MonoBehaviour
{
    private readonly int BAT_BASE_DAMAGE = 3;
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Character>(out var player)) {
            BuffManager b = FindAnyObjectByType<BuffManager>();
            b.AddDebuff("bleed", 0.3f, 3f);
            if (player.currentHp > 10) {
                player.TakeDamage(player.currentHp / 5);
            } else {
                player.TakeDamage(BAT_BASE_DAMAGE);
            }
        }
    }
}