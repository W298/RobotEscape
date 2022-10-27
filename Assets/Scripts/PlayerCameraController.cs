using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Update()
    {
        if (shake.remainShake > 0)
        {
            shake.remainShake -= Time.deltaTime;
            perlin.m_AmplitudeGain = Mathf.Lerp(shake.startShakeIntensity, 0f, 1 - shake.remainShake / shake.shakeDuration);
        }
    }
}
