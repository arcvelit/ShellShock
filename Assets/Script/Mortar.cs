using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : MonoBehaviour
{

    private bool fired;
    public GameObject missile;

    public Transform launcher;

    public Transform player;

    public float fireCooldown;

    private bool canFire;
    private float cooldownTimer;

    public float detectionRadius;
    public float projectileSpeed;



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
        Vector3 directionToPlayer = player.transform.position - launcher.position;
        directionToPlayer.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(-directionToPlayer);
        launcher.rotation = Quaternion.Euler(-45f, targetRotation.eulerAngles.y, 0);
    }



void FireAtPlayer(Vector3 targetPosition)
    {

        AntagonistEntity ae = GetComponent<AntagonistEntity>();
        
        GameObject bigMissileYe = Instantiate(missile, transform.position + transform.up * 0.8f, transform.rotation);
        Projectile p = bigMissileYe.GetComponent<Projectile>();
        p.SetBuff(ae.powerUp);
    
        Vector3 direction = targetPosition - transform.position;
        float horizontalDistance = new Vector3(direction.x, 0, direction.z).magnitude;
        float verticalDistance = direction.y;

        float time = horizontalDistance / projectileSpeed;

        Vector3 horizontalVelocity = new Vector3(direction.x, 0, direction.z).normalized * projectileSpeed;
        float verticalVelocity = (verticalDistance / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

        Rigidbody rb = bigMissileYe.GetComponent<Rigidbody>();
        rb.velocity = horizontalVelocity + Vector3.up * verticalVelocity;
    }


}