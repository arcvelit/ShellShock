using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class AntagonistEntity : MonoBehaviour
{

    // Enemy damage is buffed by this value
    private float BUFFER = 1.5f;

    //
    public float maxHealth;
    private float currentHealth;

    private float percentHealth => currentHealth / maxHealth;
    public float powerUp = 1.0f;

    public Slider healthSlider;
    
    private Transform player;

    void Start()
    {
        player = GlobalPlayerPosition.Instance.GetPlayerTransform();
        currentHealth = maxHealth;
    }
    void Update()
    {
        healthSlider.value = 1 - percentHealth;
        SeekPlayerDirection();
    }

    public void RemoveBuff()
    {
        powerUp = 1.0f;
    }

    public void ApplyBuff()
    {
        powerUp = BUFFER;
    }

    public void InflictDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        GameplayHelper.Instance.KilledEnemy(gameObject.tag);
        Destroy(gameObject);
    }

    void SeekPlayerDirection()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(-directionToPlayer);
        healthSlider.transform.rotation = targetRotation;
    }

}
