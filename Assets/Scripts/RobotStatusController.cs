using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RobotStatusController : MonoBehaviour
{
    private EnemyRobotAI ai;
    public UnityEvent deathEvent;

    [Header("Status")] 
    public float maxHealth = 100;
    public float health;
    public bool isDeath = false;

    public float healthHealAmount = 2f;
    public float healthHealInterval = 2f;
    private float healthHealTimer;

    private void Start()
    {
        ai = GetComponent<EnemyRobotAI>();
        health = maxHealth;
        healthHealTimer = healthHealInterval;
    }

    private void FixedUpdate()
    {
        healthHealTimer -= Time.deltaTime;
        if (healthHealTimer < 0)
        {
            healthHealTimer += healthHealInterval;
            Heal(healthHealAmount);
        }
    }

    public bool Heal(float amount)
    {
        if (health + amount > maxHealth)
        {
            health = maxHealth;
            return true;
        }

        health += amount;
        return false;
    }

    public void HitBullet(int damage, GameObject shooter)
    {
        health -= damage;
        if (health <= 0 && !isDeath)
        {
            health = 0;
            Death();
        }
        
        if (ai) StartCoroutine(ai.HitReaction(shooter));
    }

    private void Death()
    {
        isDeath = true;
        deathEvent.Invoke();
        Destroy(gameObject, 5f);
    }
}
