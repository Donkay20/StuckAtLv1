using UnityEngine;

public class SquirrelBite : MonoBehaviour
{
    [SerializeField] private bool leftSide, rightSide;
    private readonly int SQUIRREL_BITE_BASE_DMG = 15;
    private readonly float SQUIRREL_BITE_BASE_DURATION = 0.3f;
    private float timer, maxTime;
    private int damage;
    private float size;
    Slot slot;
    
    Vector2 startingPos;
    Vector2 basePos;
    void Start() {
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        size = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(size, size);

        maxTime = asb.GetDurationBonus(slot, SQUIRREL_BITE_BASE_DURATION);
        damage = asb.GetDamageBonus(slot, SQUIRREL_BITE_BASE_DMG);

        transform.Translate(transform.localPosition.x * timer, transform.localPosition.y, 0);   //move the teeth further away based on duration
        startingPos = transform.localPosition;
        basePos = new Vector2(0, transform.localPosition.y);
    }

    void Update() {
        timer += Time.deltaTime;
        if (timer >= maxTime) {
            if (leftSide) {
                GetComponentInParent<SquirrelBite_Parent>().DestroyParent();
            } else {
                Destroy(gameObject);
            }
        }

        if (timer <= maxTime) {
            float t = timer / maxTime;
            transform.localPosition = Vector2.Lerp(startingPos, basePos, t);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        Enemy enemy = col.collider.GetComponent<Enemy>();
        FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
    }
}
