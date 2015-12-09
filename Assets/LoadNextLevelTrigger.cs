using UnityEngine;
using System.Collections;

public class LoadNextLevelTrigger : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            StartCoroutine(StartFadeLoadLevel());
    }


    IEnumerator StartFadeLoadLevel()
    {
        ScreenFader.S.thisImage.CrossFadeAlpha(1.0f, 4.0f, true);
        yield return new WaitForSeconds(4.0f);
        Application.LoadLevel(1);
    }
}
