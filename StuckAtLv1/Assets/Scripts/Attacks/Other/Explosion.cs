using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Slot slot;
    private bool activation;
    private int damage;
    void Start() {
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null && activation) {
            if (enemy.maxHP / 10 < 1) {
                damage = 1;
            } else {
                damage = enemy.maxHP / 10;
            }
            FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
        }
    }

    public void Activate(Slot parent) {
        slot = parent;
        activation = true;
    }
}
