using UnityEngine;
using System.Collections;

public class FallDeathRestarter : MonoBehaviour
{
    public GameObject playerRef;
    public float fallDeathDistance = -40.0f;

    // Use this for initialization
    void Start()
    {
        playerRef = GameObject.Find("Khomaniac");
    }

    // Update is called once per frame
    void Update()
    {

        if (playerRef.transform.position.y < fallDeathDistance)
            Application.LoadLevel(Application.loadedLevel);

        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(Application.loadedLevel);
    }

}
