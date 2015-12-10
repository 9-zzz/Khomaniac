using UnityEngine;
using System.Collections;

public class SpecialJumpGivingTarget : MonoBehaviour {

    public float specialRespawnTime = 1.0f;

	void Start () {
	
	}
	
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "kBullet")
        {
            P_Motor.S.numberOfJumps++;

            Debug.Log(VisualAmmoBar.S.ammoIndex + "  " + VisualAmmoBar.S.ammoCubesLength);

            VisualAmmoBar.S.ammoIndex++;

            if (VisualAmmoBar.S.ammoIndex > (VisualAmmoBar.S.ammoCubesLength - 1))
            {
                VisualAmmoBar.S.maxAmmoIndex++;
                VisualAmmoBar.S.ammoIndex = 0;
            }

            VisualAmmoBar.S.ammoCubes[VisualAmmoBar.S.ammoIndex].GetComponent<MeshRenderer>().enabled = true;

            JumpTargetRespawn.S.srespawn(this.gameObject.transform.position, specialRespawnTime);

            //Destroy(transform.GetChild(0).gameObject, 5.0f);

            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            transform.GetChild(0).transform.parent = null;
            Destroy(gameObject);
        }
    }

}
