using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Tambahkan ini untuk mengakses SceneManager

public class PlayerController : MonoBehaviour
{   
    public PlayerController instance;
    public NPC_Interaction npc;
    private Animator playerAnimator;
    private float movement;
    private float verticalMovement; // Tambahkan variabel untuk gerakan vertikal
    public float moveSpeed = 5f;
    public bool canMove = true;
    private SpriteRenderer spriteRenderer; // Tambahkan SpriteRenderer

    private void Awake()
    {   
        instance = this;
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Ambil komponen SpriteRenderer
    }

    private void Update()
    {
        inputPlayer();

        // Animasi berjalan jika ada pergerakan
        if(movement == 0 && verticalMovement == 0)
        {
            playerAnimator.SetBool("isWalking", false);
            playerAnimator.SetBool("WalkingAtas", false);
            playerAnimator.SetBool("WalkingBawah", false);
        }
        // Flip sprite berdasarkan arah horizontal
        if (movement > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movement < 0)
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
        if (!canMove)
        {
            return;
        }

        // Input horizontal (A & D)
        movement = 0;
        if (Input.GetKey(KeyCode.A))
        {   
            playerAnimator.SetBool("isWalking", true);
            movement = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {   
            playerAnimator.SetBool("isWalking", true);
            movement = 1;
        }

        // Input vertikal hanya jika scene bernama "Kamar"
        verticalMovement = 0;
        if (SceneManager.GetActiveScene().name == "Kamar")
        {
            if (Input.GetKey(KeyCode.W))
            {   
                playerAnimator.SetBool("WalkingAtas", true);
                playerAnimator.SetBool("WalkingBawah", false);
                playerAnimator.SetBool("isWalking", false);
                verticalMovement = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {   
                playerAnimator.SetBool("WalkingBawah", true);
                playerAnimator.SetBool("WalkingAtas", false);
                playerAnimator.SetBool("isWalking", false);
                verticalMovement = -1;
            }
        }
    }

    private void Move()
    {   
        transform.position += new Vector3(movement * moveSpeed * Time.fixedDeltaTime, verticalMovement * moveSpeed * Time.fixedDeltaTime, 0);
    }
}