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

    private void Start()
    {
        ai = GetComponent<EnemyRobotAI>();
        health = maxHealth;
    }

    public void HitBullet(int damage, GameObject shooter)
    {
        health -= damage;
        if (health <= 0 && !isDeath) Death();
        StartCoroutine(ai.HitReaction(shooter));
    }

    private void Death()
    {
        isDeath = true;
        deathEvent.Invoke();
        Destroy(gameObject, 5f);
    }
}
