using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadNextLevelTrigger : MonoBehaviour
{
    public int indexToLoad;

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
        ScreenFader.S.thisImage.CrossFadeAlpha(1.0f, 2.0f, true);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(indexToLoad);
    }
}
