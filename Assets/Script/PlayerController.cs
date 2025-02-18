using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    private PlayerInput playerInput;
    private float moveInput;
    public float moveSpeed = 5f;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Movement.Move.performed += ctx => moveInput = ctx.ReadValue<float>();
        playerInput.Movement.Move.canceled += ctx => moveInput = 0;
    }

    private void OnDisable()
    {
        playerInput.Movement.Move.performed -= ctx => moveInput = ctx.ReadValue<float>();
        playerInput.Movement.Move.canceled -= ctx => moveInput = 0;
        playerInput.Disable();
    }

    private void Update()
    {
        transform.Translate(Vector3.right * moveInput * moveSpeed * Time.deltaTime);
    }

}
