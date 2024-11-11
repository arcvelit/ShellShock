using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Covid19 : MonoBehaviour
{
    private Transform player;
    public float moveSpeed;  
    private float lastDamageTime;
    private float damageCooldown = 1.0f;
    public float damage;

    void Start()
    {
        player = GlobalPlayerPosition.Instance.GetPlayerTransform(); 
    }

    void Update()
    {
        float multiplier = 1.0f + 4.0f * GlobalPlayerPosition.Instance.GetSpeedRatio() ;

        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * multiplier * Time.deltaTime);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time >= damageCooldown + lastDamageTime)
            {
                PlayerHealth.Instance.InflictDamage(damage);
                lastDamageTime = Time.time;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth.Instance.InflictDamage(damage);
            lastDamageTime = Time.time;
        }
    }

}
