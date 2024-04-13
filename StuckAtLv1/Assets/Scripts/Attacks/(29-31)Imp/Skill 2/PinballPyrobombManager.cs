using TMPro;
using UnityEngine;

public class PinballPyrobombManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro counterText;
    [SerializeField] private PinballPyrobomb fireball;
    private int hitCounter = 5;
    void Start() {
        counterText.text = hitCounter.ToString();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.TryGetComponent<Enemy>(out var enemy)) {
            enemy.ApplySlow(0.1f, 0.1f);
            hitCounter--; counterText.text = hitCounter.ToString();
            if (hitCounter <= 0) {
                fireball.ActivateExplosion();
            }
        }
    }
}