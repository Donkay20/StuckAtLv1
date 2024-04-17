using UnityEngine;

public class SandSlash : MonoBehaviour
{
    private readonly int SLASH_DAMAGE = 5;
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Character>(out var player)) {
            BuffManager b = FindAnyObjectByType<BuffManager>();
            b.AddDebuff("slow", 0.5f, 2f);

            if (player.currentHp > 10) {
                player.TakeDamage(player.currentHp / 2);
            } else {
                player.TakeDamage(SLASH_DAMAGE);
            }
        }
    }
}