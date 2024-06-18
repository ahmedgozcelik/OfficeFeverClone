using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private CharacterController characterController;
    private Animator playerAnimator;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;

    private Vector2 moveValue;
    private Vector3 velocity;

    public Transform paperPoint;

    public bool isCarrying;


    private void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Update()
    {
        moveValue = playerInput.Player.Move.ReadValue<Vector2>();
        HandleMovement();
        UpdateAnimations();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(moveValue.x, 0, moveValue.y);

        if (moveDirection != Vector3.zero)
        {
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
            RotateCharacter(moveDirection);
        }

        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0;
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    private void RotateCharacter(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.deltaTime);
    }

    private void UpdateAnimations()
    {
        bool isRunning = moveValue != Vector2.zero;

        //if (isCarrying)
        //{
        //    playerAnimator.SetBool("IsRunning", false);
        //    playerAnimator.SetBool("IsCarrying", isRunning);
        //}
        //else
        //{
        //    playerAnimator.SetBool("IsRunning", isRunning);
        //    playerAnimator.SetBool("IsCarrying", false);
        //}

        if (isCarrying)
        {
            if (moveValue != Vector2.zero)
            {
                playerAnimator.SetBool("IsRunning", false);
                playerAnimator.SetBool("IsCarryingIdle", false);
                playerAnimator.SetBool("IsCarrying", true);
            }
            else
            {
                playerAnimator.SetBool("IsRunning", false);
                playerAnimator.SetBool("IsCarrying", false);
                playerAnimator.SetBool("IsCarryingIdle", true);
            }
        }
        else
        {
            playerAnimator.SetBool("IsRunning", isRunning);
            playerAnimator.SetBool("IsCarrying", false);
        }
    }
}
