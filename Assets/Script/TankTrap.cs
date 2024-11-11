using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTrap : MonoBehaviour
{
    public float collideDamage;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            // Deal damage to player
            case "Player":
            AntagonistEntity ae = GetComponent<AntagonistEntity>();
            PlayerHealth.Instance.InflictDamage(ae.powerUp * collideDamage);
            Debug.Log("Player [=] Trap" + (ae.powerUp > 1.0f ? " ~~b" : ""));
            break;
        }
    }
}
