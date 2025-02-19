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
    private SpriteRenderer spriteRenderer; // Tambahkan SpriteRenderer

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Ambil komponen SpriteRenderer
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void Update()
    {
        inputPlayer();
        
        // Animasi berjalan
        playerAnimator.SetBool("isWalking", movement != 0);
        
        // Flip sprite berdasarkan movement
        if (movement == 1)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
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
