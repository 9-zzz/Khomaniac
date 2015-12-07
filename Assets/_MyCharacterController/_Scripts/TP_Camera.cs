using UnityEngine;
using System.Collections;

public class TP_Camera : MonoBehaviour
{
    public static TP_Camera S;

    public Transform TargetLookAt;

    public float deadZone = 0.01f; // Must be 0.01f, not 0.1f
    public float Distance = 5f;
    public float DistanceMin = 3f;
    public float DistanceMax = 10f;
    public float DistanceSmooth = 0.05f;
    public float DistanceResumeSmooth = 1f;
    public float X_MouseSensitivity = 5f;
    public float Y_MouseSensitivity = 5f;

    // Haven't tested or planned to use a joystick controller yet... Want to...
    //public float X_StickSensitivity = 2.5f;
    //public float Y_StickSensitivity = 2.5f;

    public float MouseWheelSensitivity = 5f;
    public float X_Smooth = 0.05f;
    public float Y_Smooth = 0.1f;
    public float Y_MinLimit = -40;
    public float Y_MaxLimit = 80;
    public float OcclusionDistanceStep = 0.5f;
    public int MaxOcclusionChecks = 10;

    private float mouseX = 0f;
    private float mouseY = 0f;
    private float velocityX = 0f;
    private float velocityY = 0f;
    private float velocityZ = 0f;
    private float velocityDistance = 0f;
    private float startDistance = 0f;
    private float desiredDistance = 0f;
    private Vector3 position = Vector3.zero; // Part of the smoothing
    private Vector3 desiredPosition = Vector3.zero;
    private float distanceSmooth = 0f;
    private float preOccludedDistance = 0f;

    // CAMERA SHAKE ###################
    public Transform cameraTransform;      // Transform of the camera to shake. Grabs the gameObject's transform if null.
    public float shake = 0f;            // How long the object should shake for.
    public float shakeAmount = 0.7f;    // Amplitude of the shake. A larger value shakes the camera harder.
    public float decreaseFactor = 1.0f;
    Vector3 originalPosition;

    void Awake()
    {
        S = this;

        // CAMERA SHAKE ###################
        //if (camTransform == null) camTransform = GetComponent(typeof(Transform)) as Transform;
        cameraTransform = this.transform;
    }

    // Use this for initialization
    void Start()
    {
        Distance = Mathf.Clamp(Distance, DistanceMin, DistanceMax); // Verify
        startDistance = Distance;                                   // Set
        Reset();                                                    // Establish initial values
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (TargetLookAt == null) return;

        // Takes mouseX mouesY gets desired Dist
        HandlePlayerInput();

        var count = 0;
        do
        {
            CalculateDesiredPosition();
            count++;
        } while (CheckIfOccluded(count));

        //CalculateDesiredPosition();//used to be out

        //CheckCameraPoints(TargetLookAt.position, desiredPosition);

        UpdatePosition();

    }

    void HandlePlayerInput()
    {
        mouseX += Input.GetAxis("Mouse X") * X_MouseSensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * Y_MouseSensitivity;

        //mouseX += (Input.GetAxis("RightStickHorizontal") * X_StickSensitivity);
        //mouseY += (Input.GetAxis("RightStickVertical") * Y_StickSensitivity);

        // This is where we're going to clamp mouse rotation limit, normalize y
        mouseY = Helper.ClampAngle(mouseY, Y_MinLimit, Y_MaxLimit);

        if (Input.GetAxis("Mouse ScrollWheel") < -deadZone || Input.GetAxis("Mouse ScrollWheel") > deadZone)
        {
            desiredDistance = Mathf.Clamp(Distance - Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity, DistanceMin, DistanceMax);

            preOccludedDistance = desiredDistance;
            distanceSmooth = DistanceSmooth;//normal distance smooth
        }
    }

    void CalculateDesiredPosition()
    {
        ResetDesiredDistance();
        // Evaluate distance
        Distance = Mathf.SmoothDamp(Distance, desiredDistance, ref velocityDistance, distanceSmooth);

        // Calculate desired position       becomes rotx, becomes roty. Inverting
        desiredPosition = CalculatePosition(mouseY, mouseX, Distance); // THE LINE y,x,z, not xyz
    }

    // Taking in 2D mouse input and transforming it to affect Camera's 3D space
    Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0); // No roll camera
        return TargetLookAt.position + rotation * direction;             // The magic, makes it orbital
    }

    bool CheckIfOccluded(int count)
    {
        var isOccluded = false;

        var nearestDistance = CheckCameraPoints(TargetLookAt.position, desiredPosition);

        if (nearestDistance != -1)//we are occluded
        {
            if (count < MaxOcclusionChecks)
            {
                isOccluded = true;
                Distance -= OcclusionDistanceStep;

                if (Distance < 0.25f)//must never be closer than zero :(, hard coded, test for self
                {
                    Distance = 0.25f;
                }
            }
            else
            {
                Distance = nearestDistance - Camera.main.nearClipPlane;
            }
            desiredDistance = Distance;
            distanceSmooth = DistanceResumeSmooth;
        }
        return isOccluded;
    }

    float CheckCameraPoints(Vector3 from, Vector3 to)
    {
        var nearestDistance = -1f;

        RaycastHit hitInfo;

        Helper.ClipPlanePoints clipPlanePoints = Helper.ClipPlaneAtNear(to);

        // Draw Lines in the editor to make it easier to visualize
        Debug.DrawLine(from, to + transform.forward * -GetComponent<Camera>().nearClipPlane, Color.red);
        Debug.DrawLine(from, clipPlanePoints.UpperLeft);
        Debug.DrawLine(from, clipPlanePoints.LowerLeft);
        Debug.DrawLine(from, clipPlanePoints.UpperRight);
        Debug.DrawLine(from, clipPlanePoints.LowerRight);


        Debug.DrawLine(clipPlanePoints.UpperLeft, clipPlanePoints.UpperRight);
        Debug.DrawLine(clipPlanePoints.UpperRight, clipPlanePoints.LowerRight);
        Debug.DrawLine(clipPlanePoints.LowerRight, clipPlanePoints.LowerLeft);
        Debug.DrawLine(clipPlanePoints.LowerLeft, clipPlanePoints.UpperLeft);

        // ### ADDED && to ignore bullets sucksssss #######################################################################
        if (Physics.Linecast(from, clipPlanePoints.UpperLeft, out hitInfo) && hitInfo.collider.tag != "Player"
            && hitInfo.collider.gameObject.tag != "bullet")
            nearestDistance = hitInfo.distance;

        if (Physics.Linecast(from, clipPlanePoints.LowerLeft, out hitInfo) && hitInfo.collider.tag != "Player"
            && hitInfo.collider.gameObject.tag != "bullet")
            if (hitInfo.distance < nearestDistance || nearestDistance == -1)
                nearestDistance = hitInfo.distance;

        if (Physics.Linecast(from, clipPlanePoints.UpperRight, out hitInfo) && hitInfo.collider.tag != "Player"
            && hitInfo.collider.gameObject.tag != "bullet")
            if (hitInfo.distance < nearestDistance || nearestDistance == -1)
                nearestDistance = hitInfo.distance;

        if (Physics.Linecast(from, clipPlanePoints.LowerRight, out hitInfo) && hitInfo.collider.tag != "Player"
            && hitInfo.collider.gameObject.tag != "bullet")
            if (hitInfo.distance < nearestDistance || nearestDistance == -1)
                nearestDistance = hitInfo.distance;

        if (Physics.Linecast(from, (to + transform.forward * -GetComponent<Camera>().nearClipPlane), out hitInfo) && hitInfo.collider.tag != "Player"
            && hitInfo.collider.gameObject.tag != "bullet")
            if (hitInfo.distance < nearestDistance || nearestDistance == -1)
                nearestDistance = hitInfo.distance;

        return nearestDistance;
    }

    void ResetDesiredDistance()
    {
        if (desiredDistance < preOccludedDistance)
        {
            var pos = CalculatePosition(mouseY, mouseX, preOccludedDistance);

            var nearestDistance = CheckCameraPoints(TargetLookAt.position, pos);

            if (nearestDistance == -1 || nearestDistance > preOccludedDistance)
            {
                desiredDistance = preOccludedDistance;
            }
        }
    }

    void UpdatePosition()
    {
        // Smooths each individually, not a whole vector3 smooth
        var posX = Mathf.SmoothDamp(position.x, desiredPosition.x, ref velocityX, X_Smooth);
        var posY = Mathf.SmoothDamp(position.y, desiredPosition.y, ref velocityY, Y_Smooth);
        var posZ = Mathf.SmoothDamp(position.z, desiredPosition.z, ref velocityZ, X_Smooth);

        // MainCamera's position in world, and lookAt point set to these
        position = new Vector3(posX, posY, posZ);
        transform.position = position; // Above position calculated

        transform.LookAt(TargetLookAt);

        // Shake shake shake!!!
        if (shake > 0)
        {
            cameraTransform.localPosition += Random.insideUnitSphere * shakeAmount; // So I can still have the camera follow the player.
            shake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shake = 0f; //camTransform.localPosition = originalPos;	// Again, to enable camera player following.
        }
    }

    public void Reset()
    {
        mouseX = 0;
        mouseY = 10;
        Distance = startDistance;
        desiredDistance = Distance; // Reset this too so we have no motion after previous resets
        preOccludedDistance = Distance;
    }

    // Didn't make create camera function.

}
