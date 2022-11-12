using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserRenderer : MonoBehaviour
{
    private LineRenderer laserLine;

    public void SetLaserPoint(Vector3 start, Vector3 direction, int layerMask, float maxDistance = 100)
    {
        laserLine.SetPosition(0, start);
        Physics.Raycast(new Ray(start, direction), out RaycastHit hit, maxDistance, layerMask);
        laserLine.SetPosition(1, hit.collider ? hit.point : start + direction * 100);
    }

    private void Start() 
	{
		laserLine = GetComponent<LineRenderer>();
    }

    private void OnEnable()
	{
		if (!laserLine) return;
		laserLine.enabled = true;
	}
	private void OnDisable()
	{
        if (!laserLine) return;
        laserLine.enabled = false;
	}
}
