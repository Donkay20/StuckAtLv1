using UnityEngine;

public class VineAttack : MonoBehaviour
{
    [SerializeField] private bool vertical;
    [SerializeField] private bool horizontal;
    private readonly int VINE_DAMAGE = 3;
    private BuffManager buffManager; 
    void Start() {
        buffManager = FindAnyObjectByType<BuffManager>();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Character>(out var player)) {
            player.TakeDamage(VINE_DAMAGE);
            buffManager.AddDebuff("slow", 0.5f, 1);

            if (vertical) {
                buffManager.AddDebuff("bleed", 0.5f, 3);
            }
            
            if (horizontal) {
                buffManager.AddDebuff("anemia", 0.1f, 3);
            }
        }
    }
}
