using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        onFoot.Jump.started += ctx => motor.Jump();

        // onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Crouch.started += ctx => motor.StartCrouching();
        onFoot.Crouch.canceled += ctx => motor.StopCrouching();

        // onFoot.Sprint.performed += ctx => motor.Sprint();
        onFoot.Sprint.started += ctx => motor.StartSprinting();
        onFoot.Sprint.canceled += ctx => motor.StopSprinting();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Tell the playermotor to move using the value from our movement action
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
