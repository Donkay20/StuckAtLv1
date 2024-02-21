using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] Character target;

    // Update is called once per frame
    void Update()
    {
        transform.right = target.gameObject.transform.position - transform.position;
    }
}
