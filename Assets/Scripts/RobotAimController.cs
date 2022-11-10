using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAimController : MonoBehaviour
{
    private LaserRenderer laserRenderer;
    private RobotInputHandler inputHandler;

    private EnemyRobotAI enemyRobotAI;
    private PlayerCameraController cam;

    public Transform laserStartPoint;

    private void OnDisable()
    {
        laserRenderer.gameObject.SetActive(false);
    }

    private void Start()
    {
        laserRenderer = transform.root.gameObject.GetComponentInChildren<LaserRenderer>();
        inputHandler = GetComponentInParent<RobotInputHandler>();

        enemyRobotAI = GetComponentInParent<EnemyRobotAI>();
        cam = GetComponentInParent<PlayerCameraController>();
    }

    private void FixedUpdate()
    {
        int groundLayer = 1 << LayerMask.NameToLayer("Ground");
        int obstacleLayer = 1 << LayerMask.NameToLayer("Obstacle");

        Vector3 targetPos = transform.root.position;
        if (enemyRobotAI && enemyRobotAI.navAgent.remainingDistance > 1.5f)
        {
            targetPos += enemyRobotAI.navAgent.velocity.normalized * 3;
        }
        else
        {
            targetPos += transform.root.forward * 3;
        }

        targetPos.y = transform.root.position.y + (inputHandler.isCrouch ? 0.7f : 1.25f);

        if (cam)
        {
            Ray ray = cam.mainCam.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit groundHit, 1000, groundLayer);
            Physics.Raycast(ray, out RaycastHit obstacleHit, 1000, obstacleLayer);

            if (obstacleHit.collider)
            {
                targetPos = obstacleHit.point;
            }
            else if (groundHit.collider)
            {
                targetPos = groundHit.point;
                targetPos.y = transform.root.position.y + (inputHandler.isCrouch ? 0.7f : 1.25f);
            }
            else
            {
                targetPos = transform.root.position + transform.root.forward * 3;
                targetPos.y = transform.root.position.y + (inputHandler.isCrouch ? 0.7f : 1.25f);
            }
        }
        else if (enemyRobotAI.enemyObject)
        {
            targetPos = enemyRobotAI.enemyObject.transform.position;
        }

        gameObject.transform.position = targetPos;

        laserRenderer.enabled = inputHandler.isAim;
        laserRenderer.start = laserStartPoint.position;
        laserRenderer.end = laserRenderer.start + laserStartPoint.forward * 100;

        Ray laserRay = new Ray(laserRenderer.start, laserStartPoint.forward);
        Physics.Raycast(laserRay, out RaycastHit laserHit, 100);
        if (laserHit.collider)
        {
            laserRenderer.end = laserHit.point;
        }
    }
}
