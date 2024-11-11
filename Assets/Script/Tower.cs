using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float affectedRange;

    private Collider[] colliders;

    void Start()
    {
        
    }

    void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, affectedRange);
        foreach (Collider collider in colliders)
        {
            AntagonistEntity antagonist = collider.GetComponent<AntagonistEntity>();
            if (antagonist != null)
            {
                antagonist.ApplyBuff();
            }
        }
    }

    void OnDestroy() => Die();

    public void Die()
    {
        colliders = Physics.OverlapSphere(transform.position, affectedRange);
        foreach(Collider collider in colliders)
        {
            AntagonistEntity antagonist = collider.GetComponent<AntagonistEntity>();
            if (antagonist != null)
            {
                antagonist.RemoveBuff();
            }
        }
    }
}
