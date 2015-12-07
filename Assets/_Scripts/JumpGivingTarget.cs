using UnityEngine;
using System.Collections;

public class JumpGivingTarget : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "kBullet")
        {
            P_Motor.S.numberOfJumps++;
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            transform.GetChild(0).transform.parent = null;
            Destroy(gameObject);
        }
    }
}
