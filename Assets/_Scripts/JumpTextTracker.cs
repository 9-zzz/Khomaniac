using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JumpTextTracker : MonoBehaviour
{
    Text thisText;

    void Awake()
    {
        thisText = GetComponent<Text>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        thisText.text = "JUMPS: " + P_Motor.S.numberOfJumps;
    }

}
