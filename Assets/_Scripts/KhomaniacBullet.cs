using UnityEngine;
using System.Collections;

public class KhomaniacBullet : MonoBehaviour
{
    public float kBulletForce = 80.0f;
    public float kBulletLifetime = 6.0f;
    Rigidbody rb;
    public bool hitSomething = false;
    Light kBulletLight;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        kBulletLight = GetComponent<Light>();
    }

    void Start()
    {
        transform.LookAt(KhomaniacGun.S.gunRayHit);
        Destroy(this.gameObject, kBulletLifetime);
        rb.AddRelativeForce(0, 0, kBulletForce, ForceMode.Impulse);

        //rb.velocity += new Vector3(0, P_Motor.S.MoveVector.y, 0);
        //rb.velocity = new Vector3(rb.velocity.x, P_Motor.S.MoveVector.y, rb.velocity.z);
    }

    void OnCollisionEnter(Collision col)
    {
        //Destroy(gameObject);
        //rb.velocity = Vector3.zero;
        hitSomething = true;
        Destroy(GetComponent<Rigidbody>());
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        if (hitSomething)
            kBulletLight.intensity = Mathf.MoveTowards(kBulletLight.intensity, 0.0f, Time.deltaTime * 20.0f);

        if (kBulletLight.intensity == 0.0f)
            Destroy(gameObject);
    }

}
