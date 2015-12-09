using UnityEngine;
using UnityEngine.SceneManagement;
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
