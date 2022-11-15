using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    private bool outlineShown = false;
    private int outlineLayer;
    private int objectLayer;
    private int obstacleLayer;
    
    private Camera camera;
    private GameObject[] obstacleAry;
    private GameObject[] enemyAry;

    private void ShowPlayerOutline()
    {
        for (int i = 2; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = outlineLayer;
        }
    }

    private void ShowObstacleOutline()
    {
        foreach (GameObject o in obstacleAry)
        {
            SetLayerRecursive(o, outlineLayer);
        }
    }

    private void ShowEnemyOutline()
    {
        foreach (var enemyRobot in enemyAry)
        {
            Physics.Linecast(camera.transform.position, enemyRobot.transform.position, out RaycastHit hit,
                1 << LayerMask.NameToLayer("Obstacle") | 1 << LayerMask.NameToLayer("Ground"));
            if (!hit.collider) continue;

            for (int i = 4; i < enemyRobot.transform.childCount; i++)
            {
                enemyRobot.transform.GetChild(i).gameObject.layer = outlineLayer;
            }
        }
    }

    private void HidePlayerOutline()
    {
        for (int i = 2; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = objectLayer;
        }
    }

    private void HideObstacleOutline()
    {
        foreach (GameObject o in obstacleAry)
        {
            SetLayerRecursive(o, obstacleLayer);
        }
    }

    private void HideEnemyOutline()
    {
        foreach (var enemyRobot in enemyAry)
        {
            for (int i = 4; i < enemyRobot.transform.childCount; i++)
            {
                enemyRobot.transform.GetChild(i).gameObject.layer = objectLayer;
            }
        }
    }

    private void SetLayerRecursive(GameObject o, int layer)
    {
        o.layer = layer;
        foreach (Transform child in o.transform)
        {
            child.gameObject.layer = layer;

            Transform hasChildren = child.GetComponentInChildren<Transform>();
            if (hasChildren != null) SetLayerRecursive(child.gameObject, layer);
        }
    }

    private void Start()
    {
        outlineLayer = LayerMask.NameToLayer("Outlined");
        objectLayer = LayerMask.NameToLayer("Object");
        obstacleLayer = LayerMask.NameToLayer("Obstacle");

        camera = GetComponent<PlayerCameraController>().mainCam;
        obstacleAry = GameObject.FindGameObjectsWithTag("OutlineObstacle");
        enemyAry = FindObjectsOfType<EnemyRobotAI>().Select(ai => ai.gameObject).ToArray();
    }

    private void Update()
    {
        Physics.Linecast(camera.transform.position, transform.position, out RaycastHit hit, 1 << LayerMask.NameToLayer("Obstacle") | 1 << LayerMask.NameToLayer("Ground"));
        
        if (hit.collider && !outlineShown)
        {
            ShowPlayerOutline();
            ShowObstacleOutline();
            ShowEnemyOutline();
            outlineShown = true;
        }
        
        if (!hit.collider && outlineShown)
        {
            HidePlayerOutline();
            HideObstacleOutline();
            HideEnemyOutline();
            outlineShown = false;
        }
    }
}
