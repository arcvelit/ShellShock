using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float damage;
    private float powerUp = 1.0f;
    private Rigidbody rb;


    public GameObject explosion;

    public float missileRadius;
    public float missileAreaDamage;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb.velocity == Vector3.zero)
            rb.velocity = transform.up * speed;
    }

    public void SetBuff(float buff)
    {
        powerUp = buff;
    }

    public float ComputeDamage() { return powerUp*damage; }
    public float ComputeAreaDamage() { return powerUp*missileAreaDamage; }

    void Update()
    {
        if (rb.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(-rb.velocity) * PewpewManager.shellRotation;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        string tag = collider.gameObject.tag;
        switch (tag)
        {
            case "Covid": return;
            case "Ground":
            break;

            case "EnemyTank": case "TankTrap": case "Mortar": case "Tower":
            AntagonistEntity ae = collider.GetComponent<AntagonistEntity>();
            ae.InflictDamage(ComputeDamage());
            Debug.Log("Projectile [X] " + tag  + (powerUp > 1.0f ? " ~~b" : ""));
            break;

            case "Player":
            PlayerHealth.Instance.InflictDamage(ComputeDamage());
            Debug.Log("Projectile [X] Player"  + (powerUp > 1.0f ? " ~~b" : ""));

            break;
        }

        if (gameObject.tag == "Missile") 
        {


            // Apply damage to all entities within the radius of missile
            Collider[] colliders = Physics.OverlapSphere(transform.position, missileRadius);
            foreach (Collider nearby in colliders)
            {
                AntagonistEntity antagonist = nearby.GetComponent<AntagonistEntity>();
                if (antagonist != null)
                {
                    antagonist.InflictDamage(ComputeAreaDamage());
                }
                PlayerHealth playerHealth = nearby.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    PlayerHealth.Instance.InflictDamage(ComputeAreaDamage());
                }
            }

            // Make a blast radius ball for missile
            Destroy(Instantiate(explosion, transform.position, Quaternion.identity), 0.5f);
        }

        Destroy(gameObject);

    }

}
