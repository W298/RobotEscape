using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
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

    private void SetEnemyRobotOutline()
    {
        foreach (var enemyRobot in enemyAry)
        {
            if (enemyRobot == null) continue;
            Physics.Linecast(camera.transform.position, enemyRobot.transform.position, out RaycastHit hit,
                1 << LayerMask.NameToLayer("Obstacle") | 1 << LayerMask.NameToLayer("Ground"));
            
            for (int i = 4; i < enemyRobot.transform.childCount; i++)
            {
                enemyRobot.transform.GetChild(i).gameObject.layer = hit.collider ? outlineLayer : objectLayer;
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
        Physics.Linecast(camera.transform.position, transform.position, out RaycastHit obstacleHit, 1 << LayerMask.NameToLayer("Obstacle"));

        Physics.Linecast(camera.transform.position, transform.position, out RaycastHit groundHit, 1 << LayerMask.NameToLayer("Ground"));

        HidePlayerOutline();
        HideObstacleOutline();

        if (groundHit.collider)
        {
            ShowPlayerOutline();
            ShowObstacleOutline();
        }
        if (obstacleHit.collider)
        {
            ShowPlayerOutline();
        }

        SetEnemyRobotOutline();
    }
}
