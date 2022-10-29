using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UIElements;

public class RobotAnimationController : MonoBehaviour
{
    private RobotInputHandler inputHandler;
    private Animator animator;

    public void OnReload()
    {
        animator.SetBool("isReload", true);
    }

    public void OnDeath()
    {
        animator.SetBool("isDeath", true);
    }

    private void Start()
    {
        inputHandler = GetComponent<RobotInputHandler>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float animationDirectionX = inputHandler.GetVelocity().x / inputHandler.maxSpeed;
        float animationDirectionZ = inputHandler.GetVelocity().z / inputHandler.maxSpeed;
        
        if (inputHandler.isAim)
        {
            Vector2 forwardV2 = new(transform.forward.x, transform.forward.z);
            
            float deg = Vector2.SignedAngle(forwardV2, new Vector2(0, 1));
            float rad = deg * Mathf.Deg2Rad;

            float a = inputHandler.GetVelocity().x;
            float b = inputHandler.GetVelocity().z;

            animationDirectionX = a * Mathf.Cos(rad) - b * Mathf.Sin(rad);
            animationDirectionZ = a * Mathf.Sin(rad) + b * Mathf.Cos(rad);
        }

        animator.SetFloat("ZAxis", animationDirectionZ);
        animator.SetFloat("XAxis", animationDirectionX);
        animator.SetFloat("Speed", inputHandler.GetVelocity().magnitude / inputHandler.maxSpeed);

        animator.SetBool("isMoving", inputHandler.isMoving);
        animator.SetBool("isCrouch", inputHandler.isCrouch);
        animator.SetBool("isAim", inputHandler.isAim);
        animator.SetBool("isWalk", inputHandler.isWalk);
    }
}
