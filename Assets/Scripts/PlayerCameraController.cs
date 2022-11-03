using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public struct ShakeParameter
{
    public ShakeParameter(float inten = 0, float dur = 0, float remain = 0)
    {
        startShakeIntensity = inten;
        shakeDuration = dur;
        remainShake = remain;
    }

    readonly public float startShakeIntensity;
    readonly public float shakeDuration;
    public float remainShake;
}

public class PlayerCameraController : MonoBehaviour
{
    public Camera mainCam;
    public CinemachineVirtualCamera vcam;

    private CinemachineBasicMultiChannelPerlin perlin;
    private ShakeParameter shake;

    private void Awake()
    {
        perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float duration)
    {
        perlin.m_AmplitudeGain = intensity;

        shake = new ShakeParameter(intensity, duration, duration);
    }

    private void MoveCamera()
    {
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;

        x -= Screen.width / 2;
        y -= Screen.height / 2;

        x /= Screen.width / 2;
        y /= Screen.height / 2;

        x *= 4;
        y *= 4;

        var xx = x * Mathf.Cos(Mathf.PI / 4) - y * Mathf.Sin(Mathf.PI / 4);
        var yy = x * Mathf.Sin(Mathf.PI / 4) + y * Mathf.Cos(Mathf.PI / 4);

        Vector3 rotatedVector = Quaternion.Inverse(transform.rotation) * new Vector3(xx, 0, yy);

        vcam.GetComponentInChildren<CinemachineFramingTransposer>().m_TrackedObjectOffset = rotatedVector;
    }

    private void ObstacleCheck()
    {
        Physics.Linecast(mainCam.transform.position, transform.position, out RaycastHit hit,
            1 << LayerMask.NameToLayer("Obstacle"));
    }

    private void FixedUpdate()
    {
        if (shake.remainShake > 0)
        {
            shake.remainShake -= Time.deltaTime;
            perlin.m_AmplitudeGain = Mathf.Lerp(shake.startShakeIntensity, 0f, 1 - shake.remainShake / shake.shakeDuration);
        }

        MoveCamera();
        ObstacleCheck();
    }
}
