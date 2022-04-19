using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static src_Models;

public class src_CharacterController : MonoBehaviour
{
    private CharacterController characterController;
    private DefaultInput defaultInput;
    public Vector2 inputMovement;
    public Vector2 inputView;

    private Vector3 newCameraRotation;
    private Vector3 newCharacterRotation;

    [Header("References")]
    public Transform cameraHolder;

    [Header("Settings")]
    public PlayerSettingsModel playerSettings;
    public float viewClampYMin = -70;
    public float viewClampYMax = 80;

    [Header("Gravity")]
    public float gravityAmount;
    public float gravityMin;
    private float playerGravity;

    public Vector3 jumpingForce;
    private Vector3 jumpingForceVelocity;


    [Header("State")]
    public PlayerState playerState;
    public float slowWalkSpeed = 0.7f;
    public float walkSpeed = 0.7f;
    public float runSpeed = 1.3f;
    public bool isNoisy = false;

    private void Awake()
    {

        defaultInput = new DefaultInput();

        defaultInput.Character.Movement.performed += e => inputMovement = e.ReadValue<Vector2>();
        defaultInput.Character.View.performed += e => inputView = e.ReadValue<Vector2>();
        defaultInput.Character.Jump.performed += e => Jump();

        defaultInput.Enable();

        newCameraRotation = cameraHolder.localRotation.eulerAngles;
        newCharacterRotation = transform.localRotation.eulerAngles;

        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        EvaluateSpeed();
        CalculateView();
        CalculateMovement();
        CalculateJump();
    }

    private void EvaluateSpeed()
    {
        //if ()
    }

    private void CalculateMovement()
    {
        var verticalSpeed = playerSettings.walkingForwardSpeed * inputMovement.y * Time.deltaTime;
        var horizontalSpeed = playerSettings.walkingStrafeSpeed * inputMovement.x * Time.deltaTime;

        var newMovementSpeed = new Vector3(horizontalSpeed, 0, verticalSpeed);
        newMovementSpeed = cameraHolder.TransformDirection(newMovementSpeed);

        characterController.Move(newMovementSpeed);

        if (playerGravity > gravityMin)
        {
            playerGravity -= gravityAmount * Time.deltaTime;
        }

        if (playerGravity < -0.1f && characterController.isGrounded)
        {
            playerGravity = -0.1f;
        }

        newMovementSpeed.y += playerGravity;
        newMovementSpeed += jumpingForce * Time.deltaTime;

        characterController.Move(newMovementSpeed);
    }

    private void CalculateView() 
    {
        newCameraRotation.y += playerSettings.viewXSensitivity * inputView.x * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(newCharacterRotation);

        //Ajuste selon la sensibilité & le min + max de hauteur que la caméra peu monter
        newCameraRotation.x += playerSettings.viewYSensitivity * -inputView.y * Time.deltaTime;
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);

        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
    }

    private void CalculateJump()
    {
        jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity, playerSettings.jumpingFalloff);
    }

    private void Jump()
    {
        if (!characterController.isGrounded) 
        {
            return;
        }

        //Jump
        jumpingForce = Vector3.up * playerSettings.jumpingHeight;
        playerGravity = 0;
    }
}
