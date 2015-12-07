using UnityEngine;
using System.Collections;

public class KhomaniacBullet : MonoBehaviour
{
    public float kBulletForce = 10.0f;
    Rigidbody rb;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Start()
    {
        transform.LookAt(KhomaniacGun.S.gunRayHit);
        Destroy(this.gameObject, 6.0f);
        rb.AddRelativeForce(0, 0, kBulletForce, ForceMode.Impulse);

        //rb.velocity += new Vector3(0, P_Motor.S.MoveVector.y, 0);
        //rb.velocity = new Vector3(rb.velocity.x, P_Motor.S.MoveVector.y, rb.velocity.z);
    }

    void Update()
    {

    }

}
