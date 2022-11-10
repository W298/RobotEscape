using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering;

/*
public class CoverPointEditor : EditorWindow
{
    [MenuItem("Window/CoverPointMaker")]
    public static void ShowWindow()
    {
        GetWindow<CoverPointEditor>("Cover Point Editor");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("RUN"))
        {
            foreach (var coverPointMaker in GameObject.FindObjectsOfType<CoverPointMaker>())
            {
                coverPointMaker.Run();
            }
        }
    }
}
*/

[ExecuteInEditMode]
public class CoverPointMaker : MonoBehaviour
{
    public float horizontalDivide = 4;
    public float verticalDivide = 2;

    IEnumerator Destroy(GameObject obj) {
        yield return null;
        DestroyImmediate(obj);
    }

    IEnumerator CreatePoints()
    {
        horizontalDivide += 2;
        verticalDivide += 2;

        for (int i = 0; i < horizontalDivide; i++)
        {
            if (i == 0 || i == horizontalDivide - 1) continue;

            var coverPoint = new GameObject("CT" + i);
            coverPoint.transform.parent = gameObject.transform;
            coverPoint.transform.localScale = Vector3.one;
            coverPoint.transform.localPosition = new Vector3(i / (horizontalDivide - 1) - 0.5f, -0.5f, 0.5f);
            coverPoint.transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 1));

            coverPoint.tag = "CoverPoint";

            bool isValid = NavMesh.SamplePosition(coverPoint.transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas);
            if (!isValid)
            {
                DestroyImmediate(coverPoint);
            }
            else
            {
                coverPoint.transform.position = hit.position;
            }

            yield return null;
        }

        for (int i = 0; i < horizontalDivide; i++)
        {
            if (i == 0 || i == horizontalDivide - 1) continue;

            var coverPoint = new GameObject("CB" + i);
            coverPoint.transform.parent = gameObject.transform;
            coverPoint.transform.localScale = Vector3.one;
            coverPoint.transform.localPosition = new Vector3(i / (horizontalDivide - 1) - 0.5f, -0.5f, -0.5f);
            coverPoint.transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, -1));

            coverPoint.tag = "CoverPoint";

            bool isValid = NavMesh.SamplePosition(coverPoint.transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas);
            if (!isValid)
            {
                DestroyImmediate(coverPoint);
            }
            else
            {
                coverPoint.transform.position = hit.position;
            }

            yield return null;
        }

        for (int i = 0; i < verticalDivide; i++)
        {
            if (i == 0 || i == verticalDivide - 1) continue;

            var coverPoint = new GameObject("CL" + i);
            coverPoint.transform.parent = gameObject.transform;
            coverPoint.transform.localScale = Vector3.one;
            coverPoint.transform.localPosition = new Vector3(-0.5f, -0.5f, i / (verticalDivide - 1) - 0.5f);
            coverPoint.transform.rotation = Quaternion.LookRotation(new Vector3(-1, 0, 0));

            coverPoint.tag = "CoverPoint";

            bool isValid = NavMesh.SamplePosition(coverPoint.transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas);
            if (!isValid)
            {
                DestroyImmediate(coverPoint);
            }
            else
            {
                coverPoint.transform.position = hit.position;
            }

            yield return null;
        }

        for (int i = 0; i < verticalDivide; i++)
        {
            if (i == 0 || i == verticalDivide - 1) continue;

            var coverPoint = new GameObject("CR" + i);
            coverPoint.transform.parent = gameObject.transform;
            coverPoint.transform.localScale = Vector3.one;
            coverPoint.transform.localPosition = new Vector3(0.5f, -0.5f, i / (verticalDivide - 1) - 0.5f);
            coverPoint.transform.rotation = Quaternion.LookRotation(new Vector3(1, 0, 0));

            coverPoint.tag = "CoverPoint";

            bool isValid = NavMesh.SamplePosition(coverPoint.transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas);
            if (!isValid)
            {
                DestroyImmediate(coverPoint);
            }
            else
            {
                coverPoint.transform.position = hit.position;
            }

            yield return null;
        }

        horizontalDivide -= 2;
        verticalDivide -= 2;
    }

    public void Run()
    {
        foreach (Transform child in transform)
        {
            StartCoroutine(Destroy(child.gameObject));
        }

        StartCoroutine(CreatePoints());
    }
}
