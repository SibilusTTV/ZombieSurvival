using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public float walkingSpeed = 5f;
    public float sprintingSpeed = 8f;
    public float crouchWalkingSpeed = 3f;
    public float crouchSprintingSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool crouching;
    private float crouchTimer;
    private bool lerpCrouch;
    private bool sprinting;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        speed = walkingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
            {
                controller.height = Mathf.Lerp(controller.height, 1, p);
            }
            else
            {
                controller.height = Mathf.Lerp(controller.height, 2, p);
            }

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    // Recieve the inputs for our InputManager.cs and apply them to our character controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
        // Debug.Log(playerVelocity.y);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }
    }
    
    public void StartCrouching()
    {
        crouching = true;
        crouchTimer = 0f;
        lerpCrouch = true;
        if (sprinting)
        {
            speed = crouchSprintingSpeed;
        }
        else
        {
            speed = crouchWalkingSpeed;
        }
    }

    public void StopCrouching()
    {
        crouching = false;
        crouchTimer = 0f;
        lerpCrouch = true;
        if (sprinting)
        {
            speed = sprintingSpeed;
        }
        else
        {
            speed = walkingSpeed;
        }
    }

    public void StartSprinting()
    {
        sprinting = true;
        if (crouching)
        {
            speed = crouchSprintingSpeed;
        }
        else
        {
            speed = sprintingSpeed;
        }
    }

    public void StopSprinting()
    {
        sprinting = false;
        if (crouching)
        {
            speed = crouchWalkingSpeed;
        }
        else
        {
            speed = walkingSpeed;
        }
    }
}
