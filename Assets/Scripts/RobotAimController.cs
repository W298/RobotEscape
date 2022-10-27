using System.Collections;
using System.Collections.Generic;
using EnemyRobotAIState;
using UnityEngine;

public class RobotAimController : MonoBehaviour
{
    private LaserRenderer laserRenderer;
    private RobotInputHandler inputHandler;

    private EnemyRobotAI enemyRobotAI;
    private PlayerCameraController cam;

    public Transform laserStartPoint;

    private void Start()
    {
        laserRenderer = transform.root.gameObject.GetComponentInChildren<LaserRenderer>();
        inputHandler = GetComponentInParent<RobotInputHandler>();

        enemyRobotAI = GetComponentInParent<EnemyRobotAI>();
        cam = GetComponentInParent<PlayerCameraController>();
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3();

        if (cam)
        {
            Ray ray = cam.mainCam.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << LayerMask.NameToLayer("Ground");
            Physics.Raycast(ray, out RaycastHit hit, 1000, layerMask);
            targetPos = hit.collider ? hit.point : transform.root.position + transform.forward * 3;
        }
        else
        {
            var cur = enemyRobotAI.aStateMachine.currentState;

            if (cur is AttackAimState or AttackFireState)
            {
                targetPos = cur.attackTargetObject ? cur.attackTargetObject.transform.position : cur.attackPosition;
            }
        }

        Vector3 direction = (targetPos - transform.root.position).normalized;
        targetPos = direction * 3 + transform.root.position;

        targetPos.y = inputHandler.isCrouch ? 0.7f : 1.25f;

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
