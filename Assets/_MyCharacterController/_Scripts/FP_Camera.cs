using UnityEngine;
using System.Collections;

public class FP_Camera : MonoBehaviour
{
    public static FP_Camera S;

    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;

    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;

    // CAMERA SHAKE ###################
    Transform cameraTransform;      // Transform of the camera to shake. Grabs the gameObject's transform if null.
    public float shake = 0f;            // How long the object should shake for.
    public float shakeAmount = 0.7f;    // Amplitude of the shake. A larger value shakes the camera harder.
    public float decreaseFactor = 1.0f;
    Vector3 originalPosition;

    void Awake()
    {
        S = this;
        cameraTransform = this.transform;
    }

    void Start()
    {
        originalPosition = transform.localPosition;
        m_CharacterTargetRot = this.transform.parent.transform.localRotation;
        m_CameraTargetRot = this.transform.localRotation;
    }


    public void LookRotation(Transform character, Transform camera)
    {
        float yRot = Input.GetAxis("Mouse X") * XSensitivity;
        float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

        m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
        m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

        if (clampVerticalRotation)
            m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

        if (smooth)
        {
            character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                smoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                smoothTime * Time.deltaTime);
        }
        else
        {
            character.localRotation = m_CharacterTargetRot;
            camera.localRotation = m_CameraTargetRot;
        }
    }


    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    void Update()
    {
        LookRotation(this.transform.parent.transform, this.transform);

        // Shake shake shake!!!
        if (shake > 0)
        {
            cameraTransform.localPosition += Random.insideUnitSphere * shakeAmount; // So I can still have the camera follow the player.
            shake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shake = 0f; 
            cameraTransform.localPosition = originalPosition;	// Again, to enable camera player following.
        }
    }

}
