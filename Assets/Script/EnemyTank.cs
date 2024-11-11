using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : MonoBehaviour
{

    private bool fired;
    public GameObject cannonBall;

    public Transform turret;
    public Transform cannon;

    public Transform player;

    public float fireCooldown;

    private bool canFire;
    private float cooldownTimer;

    public float detectionRadius;

    void Start()
    {        
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= detectionRadius) SeekPlayerDirection();
        
        if (!canFire)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
                canFire = true;
            return;
        }
        if (distanceToPlayer <= detectionRadius && canFire)
        {
            Vector3 targetPosition = player.transform.position; 
            FireAtPlayer(targetPosition);
            canFire = false;
            cooldownTimer = fireCooldown; 
        }
    }

    void SeekPlayerDirection()
    {
        Vector3 directionToPlayer = player.transform.position - turret.position;
        directionToPlayer.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(-directionToPlayer);
        turret.rotation = targetRotation;
    }

    void FireAtPlayer(Vector3 targetPosition)
    {
        Vector3 spawn = cannon.position - cannon.forward * 1.2f;
        AntagonistEntity ae = GetComponent<AntagonistEntity>();
        Projectile p = Instantiate(cannonBall, spawn, cannon.rotation * PewpewManager.shellRotation).GetComponent<Projectile>();
        p.SetBuff(ae.powerUp);
    }

}
