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

    private float lastPositionX; // Simpan posisi X sebelumnya



    private bool isMoving = false; // Menyimpan status gerakan
    private void Start()
    {
        lastPositionX = transform.position.x; // Inisialisasi posisi awal

        if (SceneManager.GetActiveScene().name == "GamePlay")
        {
            int selectedSlot = PlayerPrefs.GetInt("SelectedSaveSlot", -1);

            if (selectedSlot != -1 && PlayerPrefs.HasKey("SaveSlot" + selectedSlot + "_playerPosition")) 
            {
                float playerPosX = PlayerPrefs.GetFloat("SaveSlot" + selectedSlot + "_playerPosition");
                transform.position = new Vector2(playerPosX, -2.02f);

                Debug.Log("ada di slot " + selectedSlot);
            }
            else 
            {
                Debug.Log("tidak ada");
                transform.position = new Vector2(-91.3f, -2.02f); // Posisi default jika tidak ada data tersimpan
            }
        }
    }

    private void Awake()
    {   
        instance = this;
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Ambil komponen SpriteRenderer
    }

private void Update()
{
    if (Time.timeScale == 0)
    {
        if (isMoving)
        {
            AudioManager.Instance.SetPlayOnAwakeAndPlay("Gameplay", false);
            isMoving = false;
        }
        return;
    }  
    

    inputPlayer();

    bool hasMovement = (movement != 0 || verticalMovement != 0);

    if (!canMove || !hasMovement) // Jika tidak bisa bergerak atau tidak ada input gerakan
    {
        if (isMoving) // Hanya stop audio jika sebelumnya bergerak
        {
            AudioManager.Instance.SetPlayOnAwakeAndPlay("Gameplay", false);
            isMoving = false;
        }

        // Reset animasi
        playerAnimator.SetBool("isWalking", false);
        playerAnimator.SetBool("WalkingAtas", false);
        playerAnimator.SetBool("WalkingBawah", false);
    }
    else // Jika bergerak
    {
        float currentPositionX = transform.position.x;
        
        if (Mathf.Abs(currentPositionX - lastPositionX) > 0.01f) // Jika posisi X berubah signifikan
        {
            if (!isMoving) // Hanya play audio jika sebelumnya diam
            {
                AudioManager.Instance.SetPlayOnAwakeAndPlay("Gameplay", true);
                isMoving = true;
            }
            lastPositionX = currentPositionX; // Update posisi terakhir
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
}


    private void FixedUpdate()
    {
        Move();
    }

  private void inputPlayer()
    {
        if (!canMove)
        {   
            playerAnimator.SetBool("isWalking", false);
            playerAnimator.SetBool("WalkingAtas", false);
            playerAnimator.SetBool("WalkingBawah", false);
            return;
        } 
        movement = 0;
        verticalMovement = 0;

        // Input horizontal (A & D)
        if (Input.GetKey(KeyCode.A))
        {
            playerAnimator.SetBool("isWalking", true);
            movement = -1;
            spriteRenderer.flipX = false; // Tambahkan ini!
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerAnimator.SetBool("isWalking", true);
            movement = 1;
            spriteRenderer.flipX = true; // Tambahkan ini!
        }

        // Input vertikal hanya jika di scene "Kamar"
        if (SceneManager.GetActiveScene().name == "Kamar")
        {
            if (Input.GetKey(KeyCode.W))
            {
                playerAnimator.SetBool("WalkingAtas", true);
                playerAnimator.SetBool("WalkingBawah", false);
                verticalMovement = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                playerAnimator.SetBool("WalkingBawah", true);
                playerAnimator.SetBool("WalkingAtas", false);
                verticalMovement = -1;
            }
        }
    }


    private void Move()
    {   
        if (!canMove)
        {   
            playerAnimator.SetBool("isWalking", false);
            playerAnimator.SetBool("WalkingAtas", false);
            playerAnimator.SetBool("WalkingBawah", false);
            return;
        } 
        transform.position += new Vector3(movement * moveSpeed * Time.fixedDeltaTime, verticalMovement * moveSpeed * Time.fixedDeltaTime, 0);
    }
}