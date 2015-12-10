using UnityEngine;
using System.Collections;

public class P_Controller : MonoBehaviour
{
    public static CharacterController CharacterController;
    public static P_Controller S;

    public float deadZone = 0.01f; // Can't remember correct default.

    public Vector3 input;

    void Awake()
    {
        CharacterController = this.GetComponent<CharacterController>();
        S = this;
    }

    void Update()
    {
        // Do I need this?
        if (Camera.main == null) return;

        GetLocomotionInput();
        HandlePlayerInput();

        P_Motor.S.UpdateMotor();
    }

    void GetLocomotionInput()
    {
        // We have to save our verical compoenent before we zero it out ???
        // Otherwise we'll just be repeating the same part of gravity, no accelleration, slow gravity ???
        P_Motor.S.verticalVelocity = P_Motor.S.MoveVector.y;

        // Maybe interpolate to zero instead of having abrupt stop
        // Keeps motion from becoming additive. Every frame is recalculated
        P_Motor.S.MoveVector = Vector3.zero;
        //P_Motor.S.MoveVector = Vector3.Lerp(P_Motor.S.MoveVector, Vector3.zero, Time.deltaTime * 30.0f);

        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if ((input.x > 0.0f) || (input.z > 0.0f)) input = Vector3.MoveTowards(input, Vector3.zero, Time.deltaTime * 0.05f);

        if ((input.z > deadZone) || (input.z < -deadZone))
            P_Motor.S.MoveVector += new Vector3(0, 0, input.z);

        if ((input.x > deadZone) || (input.x < -deadZone))
            P_Motor.S.MoveVector += new Vector3(input.x, 0, 0);

        P_Animator.S.DetermineCurrentMoveDirection();//specifically at the end of this function since we have movevector
    }

    void HandlePlayerInput()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        if (Input.GetButtonDown("Jump"))
            Jump();
    }

    void Jump()
    {
        P_Motor.S.Jump();
    }

}
