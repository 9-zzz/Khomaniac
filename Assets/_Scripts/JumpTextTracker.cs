using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JumpTextTracker : MonoBehaviour
{
    public static JumpTextTracker S;
    Text thisText;

    public float fadeTime = 0.15f;
    public float fadeAlpha = 0.5f;

    void Awake()
    {
        S = this;
        thisText = GetComponent<Text>();
    }

    // Use this for initialization
    void Start()
    {
        thisText.CrossFadeAlpha(0.0f, 0.0f, true);
    }

    // Update is called once per frame
    void Update()
    {
        thisText.text = "JUMPS=" + P_Motor.S.numberOfJumps;
    }
    public void flashJumpTextMethod()
    {
        StartCoroutine(flashJumpText());
    }

    IEnumerator flashJumpText()
    {
        thisText.CrossFadeAlpha(fadeAlpha, fadeTime, true);
        yield return new WaitForSeconds(fadeTime);
        thisText.CrossFadeAlpha(0.0f, fadeTime, true);
    }

}
