using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{

    PlayerInput playerInput;
    CharacterController characterController;
    Animator anim;
    public float movementSpeedValue;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;
    float rotationFactorPerFrame = 15.0f;

    public Health health;

    private void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;

    }

    void handleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }

    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * movementSpeedValue;
        currentMovement.z = currentMovementInput.y * movementSpeedValue;
        isMovementPressed = currentMovementInput.x != 0 | currentMovementInput.y != 0;
    }

    void handleAnimation()
    {
        //bool isWaling;
        bool isRunning = anim.GetBool("isRunning");

        if(isMovementPressed && !isRunning)
        {
            anim.SetBool("isRunning", true);
        }

        else if (!isMovementPressed && isRunning)
        {
            anim.SetBool("isRunning", false);

        }
    }

    void handleGravity()
    {
        if (characterController.isGrounded)
        {
            float groundGravity = -.05f;
            currentMovement.y = groundGravity;
        }

        else
        {
            float gravity = -9.8f;
            currentMovement.y += gravity;

        }
    }


    void Update()
    {
        if (!health.isDead)
        {
            handleAnimation();
            handleRotation();
            handleGravity();
            characterController.Move(currentMovement * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
