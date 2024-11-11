using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private Transform player;

    public int bonusPointScore;
    public float timeStopSec;

    void Start()
    {
        player = GlobalPlayerPosition.Instance.GetPlayerTransform();

    }

    // Update is called once per frame
    void Update()
    {
        SeekPlayerDirection();
    }

void SeekPlayerDirection()
{
    Vector3 directionToPlayer = player.position - transform.position;
    directionToPlayer.y = 0; 

    Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer); 

    transform.rotation = targetRotation * Quaternion.Euler(90, 0, 0);
}


    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (gameObject.tag == "BonusPoint")
            {
                GameplayHelper.Instance.AddScore(bonusPointScore);
            }
            else if (gameObject.tag == "HealthPack")
            {
                PlayerHealth.Instance.HealFully();
            }
            Destroy(gameObject);
        }
    }


}
