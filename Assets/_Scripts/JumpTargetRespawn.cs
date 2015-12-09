using UnityEngine;
using System.Collections;

public class JumpTargetRespawn : MonoBehaviour
{

    public static JumpTargetRespawn S;
    public GameObject kTarget;
    public GameObject specialKtarget;

    void Awake()
    {
        S = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void respawn(Vector3 pos, float respawnTime)
    {
        StartCoroutine(respawnWait(pos, respawnTime));
    }

    IEnumerator respawnWait(Vector3 pos, float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);
        Instantiate(kTarget, pos, Quaternion.identity);
    }

    //#######################################################

    public void srespawn(Vector3 pos, float respawnTime)
    {
        StartCoroutine(srespawnWait(pos, respawnTime));
    }

    IEnumerator srespawnWait(Vector3 pos, float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);
        Instantiate(specialKtarget, pos, Quaternion.identity);
    }

}
