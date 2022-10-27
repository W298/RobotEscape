using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserRenderer : MonoBehaviour 
{
	public Vector3 start;
	public Vector3 end;
	
	private LineRenderer laserLine;

	void Start() 
	{
		laserLine = GetComponent<LineRenderer>();
	}
	
	void Update()
	{
		laserLine.SetPosition(0, start);
		laserLine.SetPosition(1, end);
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
