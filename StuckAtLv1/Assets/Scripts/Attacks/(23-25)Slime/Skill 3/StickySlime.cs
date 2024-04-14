using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickySlime : MonoBehaviour
{
    private Slot slot;
    private Dictionary<Transform, Vector2> enemyPositions = new Dictionary<Transform, Vector2>();
    private List<Enemy> enemyList = new List<Enemy>();
    private readonly int STICKYSLIME_BASE_DMG = 30;
    private readonly float STICKYSLIME_BASE_DURATION = 3f;
    private int damage;
    private float duration;
    private float size;
    [SerializeField] Sprite properSprite;


    void Start() {
        GetComponent<SpriteRenderer>().sprite = properSprite;
        slot = GetComponentInParent<Slot>();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        damage = asb.GetDamageBonus(slot, STICKYSLIME_BASE_DMG);
        size = asb.GetSizeBonus(slot); transform.localScale = new Vector2(size, size);
        duration = asb.GetDurationBonus(slot, STICKYSLIME_BASE_DURATION);
    }

    void Update() {
        duration -= Time.deltaTime;
        if (duration <= 0) {
            foreach (Enemy enemy in enemyList) {
                if (enemy != null) {
                    enemy.ApplyStun(2f);
                    FindAnyObjectByType<Character>().Heal(1);
                    enemy.transform.SetParent(FindAnyObjectByType<EnemyManager>().transform, true);
                    enemy.transform.localScale = new Vector2(1, 1);
                    FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
                }
            }   
            Destroy(gameObject);
            return; 
        }

        foreach (KeyValuePair<Transform, Vector2> pair in enemyPositions) {
            Transform enemyTransform = pair.Key;
            Vector3 localPosition = pair.Value;
            enemyTransform.localPosition = localPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Enemy>(out var enemy)) {
            if (!enemy.CompareTag("BigSlime") && !enemy.CompareTag("Sandworm") && !enemy.CompareTag("ShadowKnight") 
            && !enemy.CompareTag("DarkJamp") && !enemy.CompareTag("FinalBossPhase1") && !enemy.CompareTag("FinalBossPhase2") && !enemy.CompareTag("FinalBossPhase3")) {
                enemy.ApplySlow(1, duration);
                enemyList.Add(enemy);

                Transform enemyTransform = enemy.transform;
                enemyTransform.SetParent(transform, true);

                Vector2 localPosition = enemyTransform.localPosition;
                enemyPositions[enemyTransform] = localPosition;
            }
        }
    }  
}