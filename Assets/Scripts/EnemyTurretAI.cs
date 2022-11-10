using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyTurretState
{
    SEARCH,
    FIRE,
    OVERHEAT,
    DEACTIVATED
}

public class EnemyTurretAI : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    private AIVisionSensor visionSensor;
    private Transform xAxisTransform;
    private LaserRenderer laserRenderer;
    private Transform laserStartTransform;
    private Transform gunFireTransform;
    
    private Canvas canvas;
    private RectTransform overheatRect;
    private RectTransform overheatGageRect;
    private Text overheatText;

    private float gunFireDelay = 1f / 5f;
    private float gunFireTimer;
    private bool gunFireReady = true;

    [Header("Prefabs")]
    public GameObject muzzleFlash;
    public AudioClip fireAudioClip;
    public GameObject bulletTrail;
    public ParticleSystem hitParticle;

    [Header("Property")]
    public int gunDamage = 8;
    public float circleRadius = 0.5f;
    public float circleDistance = 10f;
    public float rotateSpeed = 50;

    [Header("Search")] 
    public float searchSpeed = 0.2f;

    [Header("Overheat")]
    public int overheatThreshold = 10;
    public float overheatDelay = 5f;
    [SerializeField] private float overheatGage = 0;

    [Header("FSM")] public EnemyTurretState currentState;

    private void DrawLaser()
    {
        laserRenderer.start = laserStartTransform.position;
        laserRenderer.end = laserStartTransform.position + laserStartTransform.forward * 100;

        Physics.Raycast(laserRenderer.start, laserStartTransform.forward, out RaycastHit hit, 100);
        if (hit.collider) laserRenderer.end = hit.point;
    }

    private void Aim(Vector3 targetPosition, float rotateSpeed)
    {
        var lookRotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, lookRotation.eulerAngles.y, 0), Time.deltaTime * rotateSpeed);

        xAxisTransform.localRotation = Quaternion.RotateTowards(xAxisTransform.localRotation, Quaternion.Euler(lookRotation.eulerAngles.x, 0, 0), Time.deltaTime * rotateSpeed);
    }

    private GameObject GetClosestTarget()
    {
        if (visionSensor.redZoneObjectList.Count == 0) return null;

        GameObject closestObject = null;
        float minDistance = 1000;
        foreach (var g in visionSensor.redZoneObjectList)
        {
            float distance = Vector3.Distance(transform.position, g.transform.position);
            if (distance < minDistance)
            {
                closestObject = g;
                minDistance = distance;
            }
        }

        return closestObject;
    }

    private Vector3 ApplyAccuracy()
    {
        Vector2 randomOffset = Random.insideUnitCircle;
        randomOffset *= circleRadius;

        Vector3 destination = gunFireTransform.transform.position;
        destination += gunFireTransform.forward * circleDistance;
        destination += gunFireTransform.up * randomOffset.y;
        destination += gunFireTransform.right * randomOffset.x;

        DebugExtension.DebugPoint(destination, Color.yellow, 0.5f, 1f);

        return destination;
    }

    private RaycastHit CheckHit(Vector3 destination)
    {
        Physics.Raycast(gunFireTransform.position, destination - gunFireTransform.position, out RaycastHit hit, 100);
        return hit;
    }

    private void SpawnMuzzleFlash()
    {
        GameObject muzzleFlashGameObject = Instantiate(muzzleFlash, gunFireTransform);
        muzzleFlashGameObject.transform.Rotate(Vector3.up, -90);
    }

    private IEnumerator SpawnTrail(RaycastHit hit)
    {
        GameObject trail = Instantiate(bulletTrail, gunFireTransform.position, gunFireTransform.rotation);

        Vector3 hitPoint = hit.collider ? hit.point : gunFireTransform.position + gunFireTransform.forward * 100;
        Vector3 startPoint = gunFireTransform.position;
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
        SpawnMuzzleFlash();

        RaycastHit hit = CheckHit(ApplyAccuracy());
        if (hit.collider)
        {
            SpawnHitParticle(hit.point, hit.normal);

            var statusController = hit.collider.gameObject.GetComponent<RobotStatusController>();
            statusController?.HitBullet(gunDamage, transform.root.gameObject);
        }

        StartCoroutine(SpawnTrail(hit));
        audioSource.Play();

        gunFireReady = false;
        overheatGage += 2;

        if (overheatGage >= overheatThreshold)
        {
            currentState = EnemyTurretState.OVERHEAT;
            overheatGage = 0;
            StartCoroutine(OverheatTimer());
        }
    }

    private void UpdateGunFireTimer()
    {
        if (!gunFireReady)
        {
            gunFireTimer -= Time.deltaTime;
            if (gunFireTimer < 0)
            {
                gunFireTimer = gunFireDelay;
                gunFireReady = true;
            }
        }
    }

    private IEnumerator OverheatTimer()
    {
        yield return new WaitForSeconds(overheatDelay);
        currentState = EnemyTurretState.SEARCH;
    }

    public void Deactivate()
    {
        animator.SetBool("isShoot", false);
        canvas.gameObject.SetActive(false);
        currentState = EnemyTurretState.DEACTIVATED;
        laserRenderer.enabled = false;
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponentInChildren<AudioSource>();
        visionSensor = GetComponentInChildren<AIVisionSensor>();
        xAxisTransform = transform.GetChild(0).GetChild(0);
        laserRenderer = xAxisTransform.GetComponentInChildren<LaserRenderer>();
        laserStartTransform = xAxisTransform.Find("LaserStart").transform;
        gunFireTransform = xAxisTransform.Find("GunFirePoint").transform;
        
        canvas = GetComponentInChildren<Canvas>(true);
        overheatRect = canvas.transform.GetChild(0).GetComponent<RectTransform>();
        overheatGageRect = overheatRect.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        overheatText = overheatRect.GetChild(1).GetComponent<Text>();

        laserRenderer.enabled = true;
        audioSource.clip = fireAudioClip;
        gunFireTimer = gunFireDelay;
    }

    private void FixedUpdate()
    {
        if (currentState == EnemyTurretState.DEACTIVATED)
        {
            animator.SetBool("isShoot", false);
            Aim(transform.position + transform.forward - transform.up, 50);
            return;
        }

        DrawLaser();

        if (currentState != EnemyTurretState.OVERHEAT)
        {
            GameObject closestObject = GetClosestTarget();
            if (closestObject != null)
            {
                Aim(closestObject.transform.position, rotateSpeed);
                animator.SetBool("isShoot", true);
                currentState = EnemyTurretState.FIRE;
                if (gunFireReady) FireWeapon();
            }
            else
            {
                animator.SetBool("isShoot", false);
                currentState = EnemyTurretState.SEARCH;
                DebugExtension.DebugPoint(transform.position + transform.forward + transform.right * 2, Color.yellow);
                Aim(transform.position + transform.forward + transform.right * 2, rotateSpeed * searchSpeed);
                overheatGage = Mathf.Clamp(overheatGage - Time.deltaTime * 5, 0, 100);
            }

            canvas.gameObject.SetActive(overheatGage > 0);
            overheatGageRect.sizeDelta = new Vector2(overheatGage * (100f / overheatThreshold), overheatGageRect.sizeDelta.y);
            overheatText.color = new Color(1f, 1f, 1f, 0.1f);
        }
        else
        {
            animator.SetBool("isShoot", false);
            Aim(transform.position + transform.forward - transform.up, 50);
            
            canvas.gameObject.SetActive(true);
            overheatGageRect.sizeDelta = new Vector2(overheatGage * (100f / overheatThreshold), overheatGageRect.sizeDelta.y);
            overheatText.color = new Color(1, 0.1647f, 0.1647f, 1);
        }

        var screenPos = Camera.main.WorldToScreenPoint(transform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos,
            canvas.worldCamera, out Vector2 movePos);

        overheatRect.position = canvas.transform.TransformPoint(movePos + new Vector2(50, 50));

        UpdateGunFireTimer();
    }
}
