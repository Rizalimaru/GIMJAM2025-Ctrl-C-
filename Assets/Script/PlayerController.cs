using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    private PlayerInput playerInput;
    private float movement;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void Update()
    {
        inputPlayer();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void inputPlayer()
    {
        movement = playerInput.Movement.Move.ReadValue<float>();
    }

    private void Move()
    {
        rb.MovePosition(rb.position + new Vector2(movement, 0) * (moveSpeed * Time.fixedDeltaTime));
    }
}
