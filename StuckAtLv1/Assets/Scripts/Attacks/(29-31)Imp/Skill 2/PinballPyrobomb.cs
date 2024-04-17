using UnityEngine;

public class PinballPyrobomb : MonoBehaviour
{
    [SerializeField] private GameObject fireballBase;
    [SerializeField] private Sprite explosionSprite;
    private CircleCollider2D explosionCollider;
    private Slot slot;
    Rigidbody2D rb;
    private Vector3 mousePosition;
    private Camera mainCamera;
    private readonly float FIREBALL_SPEED = 6f;
    private readonly float FIREBALL_BASE_DURATION = 15f;
    private readonly int FIREBALL_BASE_DAMAGE = 50;
    private int damage;
    private float duration;
    private float size;
    void Start() {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        explosionCollider = GetComponent<CircleCollider2D>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * FIREBALL_SPEED;

        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        damage = asb.GetDamageBonus(slot, FIREBALL_BASE_DAMAGE);
        size = asb.GetSizeBonus(slot); transform.localScale = new Vector2(size + 1f, size + 1f);
        duration = asb.GetDurationBonus(slot, FIREBALL_BASE_DURATION);
    }

    void Update() {
        duration -= Time.deltaTime;
        if (duration <= 0) {
            Destroy(gameObject);
        }
    }

    public void ActivateExplosion() {
        rb.velocity = Vector2.zero;
        explosionCollider.enabled = true;
        GetComponent<SpriteRenderer>().sprite = explosionSprite;
        Destroy(fireballBase);
        duration = 1f;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent<Enemy>(out var enemy)) {
            FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
        }
    }
}