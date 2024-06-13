using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    public float moveSpeed = 5f;
    private Vector2 moveValue;
    public bool isCarrying; // Taþýma durumunu temsil eden bir bayrak

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        moveValue = playerInput.Player.Move.ReadValue<Vector2>();
        UpdateAnimation(); // Animasyonlarý güncelle
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void FixedUpdate()
    {
        MoveCharacter(moveValue);
    }

    private void MoveCharacter(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
            Vector3 newPosition = playerRigidbody.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
            playerRigidbody.MovePosition(newPosition);

            // Karakterin yönünü hareket yönüne doðru çevir
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            playerRigidbody.rotation = Quaternion.RotateTowards(playerRigidbody.rotation, toRotation, 360 * Time.fixedDeltaTime);
        }
    }

    private void UpdateAnimation()
    {
        bool isRunning = moveValue != Vector2.zero;

        if (isCarrying)
        {
            playerAnimator.SetBool("IsRunning", false);
            playerAnimator.SetBool("IsCarrying", isRunning); // Hareket ederken taþýma animasyonu oynat
        }
        else
        {
            playerAnimator.SetBool("IsRunning", isRunning);
            playerAnimator.SetBool("IsCarrying", false);
        }

    }
}
