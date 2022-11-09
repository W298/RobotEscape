using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RobotInputHandler : MonoBehaviour
{
    private RobotAimController aimController;
    private GunController gunController;
    private PlayerUI playerUI;

    public InputAction movementAction;
    public InputAction fireAction;
    public InputAction aimAction;
    public InputAction crouchAction;
    public InputAction reloadAction;
    public InputAction interactAction;

    private Rigidbody rigidbody;
    private NavMeshAgent navAgent;

    private Vector2 movementAxis;

    private float _maxSpeed = 5.0f;
    public float maxSpeed
    {
        get => _maxSpeed;
        set
        {
            _maxSpeed = value;
            if (isAI) navAgent.speed = value;
        }
    }

    [Header("Status Values")]
    public Vector2 movementVector = new(0, 0);
    public bool isMoving = false;
    public bool isFire = false;
    public bool isAim = false;
    public bool isCrouch = false;
    public bool isWalk = false;

    [Header("Event Binding")]
    public UnityEvent fireEvent;
    public UnityEvent fireStartEvent;
    public UnityEvent fireEndEvent;
    public UnityEvent reloadEvent;

    [Header("CrossHair")] 
    public Texture2D crossHairInactiveTexture;
    public Texture2D crossHairTexture;

    private bool isAI = false;

    public Vector3 GetVelocity()
    {
        return isAI ? navAgent.velocity : rigidbody.velocity;
    }

    public void SetVelocity(Vector3 newVel)
    {
        if (isAI || !rigidbody) return;
        rigidbody.velocity = newVel;
    }

    public void Reload(InputAction.CallbackContext context = new InputAction.CallbackContext())
    {
        if (!gunController.ammoSystem.canReload) return;
        reloadEvent.Invoke();
    }

    public void OnDeath()
    {
        aimController.enabled = false;

        if (isAI) return;

        movementAction.Disable();
        crouchAction.Disable();
        aimAction.Disable();
        fireAction.Disable();
        reloadAction.Disable();
        interactAction.Disable();
    }

    private void SetCrossHair(bool active)
    {
        Cursor.SetCursor(active ? crossHairTexture : crossHairInactiveTexture, Vector2.zero, CursorMode.Auto);
    }

    private void Awake()
    {
        aimController = GetComponentInChildren<RobotAimController>();
        gunController = GetComponentInChildren<GunController>();
        playerUI = GameObject.Find("PlayerUICanvas").GetComponent<PlayerUI>();

        rigidbody = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();

        isAI = !rigidbody || navAgent;
    }

    private void Start()
    {
        if (isAI)
        {
            maxSpeed = maxSpeed;
            return;
        }

        movementAction.Enable();
        crouchAction.Enable();
        aimAction.Enable();
        fireAction.Enable();
        reloadAction.Enable();
        interactAction.Enable();

        fireAction.started += context => fireStartEvent.Invoke();
        fireAction.canceled += context => fireEndEvent.Invoke();

        crouchAction.performed += context =>
        {
            isCrouch = !isCrouch;
        };

        reloadAction.performed += Reload;
        interactAction.performed += context =>
        {
            playerUI?.interactObject?.GetComponentInChildren<BoxInteract>().Interact(gameObject);
        };
    }

    private void FixedUpdate()
    {
        movementAxis = movementAction.ReadValue<Vector2>();
        var a = movementAxis.x * Mathf.Cos(Mathf.PI / 4) - movementAxis.y * Mathf.Sin(Mathf.PI / 4);
        var b = movementAxis.x * Mathf.Sin(Mathf.PI / 4) + movementAxis.y * Mathf.Cos(Mathf.PI / 4);
        movementAxis = new Vector2(a, b);

        if (aimAction.enabled) isAim = aimAction.ReadValue<float>() == 1;
        if (fireAction.enabled) isFire = isAim && fireAction.ReadValue<float>() == 1;
        isMoving = movementAxis != Vector2.zero || GetVelocity().magnitude > 0.05f;
        if (isFire) fireEvent.Invoke();

        movementVector = isMoving ? Vector2.Lerp(movementVector, movementAxis, Time.deltaTime * 7f) : new Vector2(0, 0);
        SetVelocity(new Vector3(movementVector.x, 0, movementVector.y) * maxSpeed / (isCrouch || isAim ? 2 : 1) + new Vector3(0, GetVelocity().y, 0));

        if (isAim)
        {
            Vector3 lookDirection = (aimController.transform.position - transform.position);
            lookDirection.y = 0;
            lookDirection.Normalize();

            Quaternion toRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime * 500);
        }
        else if (isMoving && GetVelocity().magnitude != 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(GetVelocity().x, 0, GetVelocity().z), Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime * 500);
        }

        SetCrossHair(isAim);
    }
}
