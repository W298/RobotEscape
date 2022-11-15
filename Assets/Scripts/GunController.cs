using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum FIREMODE
{
    SEMI,
    BURST,
    FULL
}

public class AmmoSystem
{
    public int magAmmo;
    public int magCapacity;
    public int remainAmmo;

    public bool canReload { get => this.remainAmmo > 0 && this.magAmmo < this.magCapacity; }

    public AmmoSystem(int magCapacity, int totalAmmo)
    {
        this.magAmmo = magCapacity;
        this.magCapacity = magCapacity;
        this.remainAmmo = totalAmmo - magAmmo;
    }

    public void Reload()
    {
        int addAmmo = this.magCapacity - this.magAmmo;

        if (this.remainAmmo < addAmmo)
        {
            addAmmo = this.remainAmmo;
            this.remainAmmo = 0;
        }
        else
        {
            this.remainAmmo -= addAmmo;
        }

        this.magAmmo += addAmmo;
    }
}

public class GunController : MonoBehaviour
{
    [Header("Muzzle Flash")]
    public GameObject muzzleFlash;
    public Transform muzzleFireStart;

    [Header("Audio")]
    public AudioClip fireAudioClip;
    public AudioClip reloadAudioClip;
    private AudioSource audioSource;
    public float fireSoundRange = 20f;

    [Header("Effects")]
    public GameObject bulletTrail;
    public ParticleSystem hitParticle;

    private PlayerCameraController camController;

    [Header("Accuracy")] 
    public float circleRadius = 0.5f;
    public float circleDistance = 10f;

    [Header("Mode")]
    public FIREMODE fireMode = FIREMODE.FULL;
    public float fireDelay = 0.1f;
    private float timeLastFired = 0;
    private bool semiFired = false;

    public int magCapacity = 30;
    public int totalAmmo = 30;
    public int gunDamage = 5;
    public AmmoSystem ammoSystem;

    private void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        camController = transform.root.GetComponent<PlayerCameraController>();
        ammoSystem = new AmmoSystem(magCapacity, totalAmmo);
    }

    private void FixedUpdate()
    {
        DebugExtension.DebugCircle(muzzleFireStart.transform.position + muzzleFireStart.forward * circleDistance, muzzleFireStart.forward, Color.yellow, circleRadius);
    }

    private Vector3 ApplyAccuracy()
    {
        Vector2 randomOffset = Random.insideUnitCircle;
        randomOffset *= circleRadius;

        Vector3 destination = muzzleFireStart.transform.position;
        destination += muzzleFireStart.forward * circleDistance;
        destination += muzzleFireStart.up * randomOffset.y;
        destination += muzzleFireStart.right * randomOffset.x;

        DebugExtension.DebugPoint(destination, Color.yellow, 0.5f, 1f);

        return destination;
    }

    private RaycastHit CheckHit(Vector3 destination)
    {
        Physics.Raycast(muzzleFireStart.position, destination - muzzleFireStart.position, out RaycastHit hit, 100);

        return hit;
    }

    private IEnumerator SpawnTrail(RaycastHit hit)
    {
        GameObject trail = Instantiate(bulletTrail, muzzleFireStart.position, muzzleFireStart.rotation);

        Vector3 hitPoint = hit.collider ? hit.point : muzzleFireStart.position + muzzleFireStart.forward * 100;
        Vector3 startPoint = muzzleFireStart.position;
        float distance = Vector3.Distance(startPoint, hitPoint);
        Vector3 direction = (hitPoint - startPoint).normalized;

        while (Vector3.Distance(startPoint, trail.transform.position) < distance)
        {
            trail.transform.position += direction * 3f;
            yield return null;
        }

        trail.transform.position = hitPoint;

        Destroy(trail);
    }

    private void SpawnHitParticle(Vector3 hitPoint, Vector3 hitNormal)
    {
        Instantiate(hitParticle, hitPoint, Quaternion.LookRotation(hitNormal));
    }

    private void FireWeapon()
    {
        if (ammoSystem.magAmmo <= 0) return;
        ammoSystem.magAmmo--;

        GameObject muzzleFlashGameObject = Instantiate(muzzleFlash, muzzleFireStart);
        muzzleFlashGameObject.transform.Rotate(Vector3.up, -90);

        RaycastHit hit = CheckHit(ApplyAccuracy());
        if (hit.collider)
        {
            SpawnHitParticle(hit.point, hit.normal);

            var statusController = hit.collider.gameObject.GetComponent<RobotStatusController>();
            statusController?.HitBullet(gunDamage, transform.root.gameObject);
        }

        StartCoroutine(SpawnTrail(hit));

        audioSource.clip = fireAudioClip;
        audioSource.Play();
        if (camController) camController.ShakeCamera(0.7f, 0.07f);

        timeLastFired = Time.time;

        foreach (var robot in GameObject.FindGameObjectsWithTag("Robot"))
        {
            if (robot == transform.root.gameObject) continue;

            var soundSensor = robot.GetComponentInChildren<AISoundSensor>();
            if (soundSensor) soundSensor.OnSoundHear(fireSoundRange, transform.position, transform.root.gameObject);
        }
    }

    public void PlayReloadSound()
    {
        audioSource.clip = reloadAudioClip;
        audioSource.Play();
    }

    public void OnFire()
    {
        if (fireMode != FIREMODE.FULL) return;

        if (timeLastFired + fireDelay <= Time.time)
        {
            FireWeapon();
        }
    }

    public void OnFireStart()
    {
        if (fireMode != FIREMODE.SEMI || semiFired) return;

        if (timeLastFired + fireDelay <= Time.time)
        {
            FireWeapon();
            semiFired = true;
        }
    }

    public void OnFireEnd()
    {
        semiFired = false;
    }

    public void OnReload()
    {
        ammoSystem.Reload();
    }
}
