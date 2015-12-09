using UnityEngine;

public class FPSCameraController : MonoBehaviour
{
    private Vector3 rotation = Vector3.zero;

    [SerializeField]
    private float lookSensitivity = 3f;
    [SerializeField]
    private float cameraRotationLimit = 85f;


    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;

    Rigidbody rb;
    Camera cam;

    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody>(); // ??? only to rotate the player?
        cam = Camera.main;
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    // Gets a rotational vector for the camera
    public void RotateCamera(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }

    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        //if (cam != null)
            // Set our rotation and clamp it
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            //Apply our rotation to the transform of our camera
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
            //cam.transform.localEulerAngles = Vector3.MoveTowards(cam.transform.localEulerAngles, new Vector3(currentCameraRotationX, 0f, 0f), Time.deltaTime * 50.0f);
    }

    // Run every physics iteration
    void FixedUpdate()
    {
        PerformRotation();
    }

    void Update()
    {
        //Calculate rotation as a 3D vector (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        //Apply rotation
        Rotate(_rotation);

        //Calculate camera rotation as a 3D vector (turning around)
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRot * lookSensitivity;

        //Apply camera rotation
        RotateCamera(_cameraRotationX);
    }

}
