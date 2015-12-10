using UnityEngine;
using System.Collections;

public class P_Motor : MonoBehaviour
{
    public static P_Motor S;

    public int baseNumberOfJumps = 2;
    public int numberOfJumps;
    public float jumpSpeed;

    public float gravity = 10f;
    public float terminalVelocity = 10f;

    public float forwardSpeed = 10f;
    public float backwardSpeed = 2f;
    public float strafingSpeed = 5f;

    public float slideSpeed = 10f;
    public float slideFallSmoothing = 10.0f;
    public float slideThreshold = 0.6f;
    public float maxControllableSlideMagnitude = 0.4f;
    public Vector3 slideDirection;

    public Vector3 MoveVector { get; set; }
    public float verticalVelocity { get; set; }

    ParticleSystem jumpPS;

    public float moveSpeed;

    void Awake()
    {
        S = this;
        jumpPS = this.transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    public void UpdateMotor()
    {
        SnapAlignCharacterWithCamera();
        ProcessMotion();
    }

    void ProcessMotion()
    {
        // Transform MoveVector into WorldSpace relative to our character's rotation
        MoveVector = transform.TransformDirection(MoveVector);

        // Then normalize our movevector if our magnitude is greater than 0, Fixes DIAGONAL speed problem
        if (MoveVector.magnitude > 1)
            MoveVector = Vector3.Normalize(MoveVector);

        // Apply sliding important placement
        ApplySlide();

        // Multiply movevector by movespeed
        MoveVector *= MoveSpeed();// Now a method, transmission
        // ^### could apply dash methodology here, save lots of code?

        // multiply  movevector by delta.time for value per second than per frame
        //MoveVector *= Time.deltaTime;//moved down, was for clarification

        // Reapply VerticalVelocity MoveVector.y
        MoveVector = new Vector3(MoveVector.x, verticalVelocity, MoveVector.z);

        ApplyGravity();

        // Move the character in worldspace
        P_Controller.CharacterController.Move(MoveVector * Time.deltaTime); // Meters per frame update to meters per second <- Time.deltaTime
    }

    void ApplyGravity()
    {
        if (MoveVector.y > -terminalVelocity)
            MoveVector = new Vector3(MoveVector.x, MoveVector.y - gravity * Time.deltaTime, MoveVector.z); // Time.deltaTime makes gravity a downwards meters per second calculation.

        if (P_Controller.CharacterController.isGrounded && MoveVector.y < -1)
        {
            MoveVector = new Vector3(MoveVector.x, -1, MoveVector.z);
            numberOfJumps = baseNumberOfJumps; // My addition for double jumping and resetting jumps.

            for (int i = 0; i < VisualAmmoBar.S.ammoCubes.Length; i++)
                VisualAmmoBar.S.ammoCubes[i].GetComponent<MeshRenderer>().enabled = false;

            VisualAmmoBar.S.ammoCubes[0].GetComponent<MeshRenderer>().enabled = true;
            VisualAmmoBar.S.ammoCubes[1].GetComponent<MeshRenderer>().enabled = true;
            VisualAmmoBar.S.ammoIndex = 1;
        }
    }

    public void Jump()
    {
        if (numberOfJumps > 0)
        {
            verticalVelocity = jumpSpeed;
            jumpPS.Play();
            numberOfJumps--;
            VisualAmmoBar.S.ammoCubes[VisualAmmoBar.S.ammoIndex].GetComponent<MeshRenderer>().enabled = false;
            VisualAmmoBar.S.ammoIndex--;
            //TP_Camera.S.shake = 0.07f;
            //FP_Camera.S.shake = 0.07f;
        }
    }


    void ApplySlide()
    {
        if (!P_Controller.CharacterController.isGrounded)
            return;

        slideDirection = Vector3.zero;

        RaycastHit hitInfo;

        //cast from 0,1,0 + trans pos, to 0,-1,0 and put out into hitInfo
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hitInfo))
        {
            Debug.DrawLine(transform.position + Vector3.up, hitInfo.point);

            if (hitInfo.normal.y < slideThreshold)
                slideDirection = new Vector3(hitInfo.normal.x, -hitInfo.normal.y, hitInfo.normal.z);
        }

        if (slideDirection.magnitude < maxControllableSlideMagnitude)
        {
            MoveVector += slideDirection;
        }
        else
        {
            //MoveVector = slideDirection;
            MoveVector = Vector3.Lerp(MoveVector, slideDirection, Time.deltaTime * slideFallSmoothing);
        }
    }

    // If we are we moving, rotate this object to match MainCamera's Y-rotation.
    // Possibly remove if and lerp the rotation to always math MainCamera rotation.
    void SnapAlignCharacterWithCamera()
    {
        if (MoveVector.x != 0 || MoveVector.z != 0)
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);
    }

    float MoveSpeed()
    {
        //var moveSpeed = 0f; // Local var, need, zeroes out every time???

        switch (P_Animator.S.MoveDirection)
        {
            case P_Animator.Direction.Stationary:
                moveSpeed = 20.0f;
                //moveSpeed = Mathf.Lerp(moveSpeed, 10.0f, Time.deltaTime*1.0f);
                break;

            case P_Animator.Direction.Forward:
                moveSpeed = forwardSpeed;
                break;

            case P_Animator.Direction.Backward:
                moveSpeed = backwardSpeed;
                break;

            case P_Animator.Direction.Left:
                moveSpeed = strafingSpeed;
                break;

            case P_Animator.Direction.Right:
                moveSpeed = strafingSpeed;
                break;

            case P_Animator.Direction.LeftForward:
                moveSpeed = forwardSpeed;
                break;

            case P_Animator.Direction.RightForward:
                moveSpeed = forwardSpeed;
                break;


            case P_Animator.Direction.LeftBackward:
                moveSpeed = forwardSpeed;
                break;

            case P_Animator.Direction.RightBackward:
                moveSpeed = forwardSpeed;
                break;
        }

        if (slideDirection.magnitude > 0)
            moveSpeed = slideSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed *= 2.0f;

        return moveSpeed;

    } // End of MoveSpeed()

}
