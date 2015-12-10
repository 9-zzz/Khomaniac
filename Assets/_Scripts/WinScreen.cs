using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinScreen : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(0);
    }
}
