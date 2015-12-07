using UnityEngine;
using System.Collections;

public class KhomaniacGun : MonoBehaviour
{
    public static KhomaniacGun S;

    public GameObject kBullet;
    Transform kSpawnPoint;
    public float waitTime = 0.25f;
    public bool fireNow = false;
    public Color emCol;
    public Color gunHeatUpColor;

    public ParticleSystem kps;

    Renderer rend;

    RaycastHit hit;
    public Vector3 gunRayHit;

    void Awake()
    {
        S = this;
        rend = GetComponent<Renderer>();
        kSpawnPoint = transform.GetChild(0);
    }

    void Start()
    {
        this.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        this.GetComponent<Renderer>().materials[1].EnableKeyword("_EMISSION");
        StartCoroutine(shooterTest());
    }

    void Update()
    {
        rend.materials[1].SetColor("_EmissionColor", emCol);

       

        if (Input.GetMouseButtonDown(0))
        {
            fireNow = true;
            kps.Play();
        }

        if (Input.GetMouseButtonUp(0))
        {
            fireNow = false;
            kps.Stop();
        }

        if (fireNow)
            emCol = Color.Lerp(emCol, gunHeatUpColor, Time.deltaTime * 3.0f);
        else
            emCol = Color.Lerp(emCol, Color.black, Time.deltaTime * 1.0f);

        if (Input.GetMouseButton(0))
            Shoot();

    } // End of Update()

    void Shoot()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 500.0f))
        {
            //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //sphere.transform.position = hit.point;
            gunRayHit = hit.point;
        }
        else
        {
            gunRayHit = Camera.main.transform.forward * 400.0f;
        }
    }

    IEnumerator shooterTest()
    {
        while (true)
        {
            if (fireNow)
            {
                Instantiate(kBullet, kSpawnPoint.position, kSpawnPoint.rotation);
                yield return new WaitForSeconds(waitTime);
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

}
