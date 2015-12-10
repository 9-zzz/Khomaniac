using UnityEngine;
using System.Collections;

public class JumpGivingTarget : MonoBehaviour
{

    public float respawnTime = 5.0f;
    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "kBullet")
        {
            P_Motor.S.numberOfJumps++;

            if (VisualAmmoBar.S.ammoIndex < VisualAmmoBar.S.ammoCubesLength)
            {
                VisualAmmoBar.S.ammoIndex++;
                VisualAmmoBar.S.ammoCubes[VisualAmmoBar.S.ammoIndex].GetComponent<MeshRenderer>().enabled = true;
            }

            JumpTargetRespawn.S.respawn(this.gameObject.transform.position, respawnTime);

            //Destroy(transform.GetChild(0).gameObject, 5.0f);

            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            transform.GetChild(0).transform.parent = null;
            Destroy(gameObject);
        }
    }

}
