using UnityEngine;
using System.Collections;

public class VisualAmmoBar : MonoBehaviour
{
    public static VisualAmmoBar S;

    int ammo;
    public int ammoIndex = 0;
    public int maxAmmoIndex = 0;
    public GameObject[] ammoCubes;

    public Color[] cubeColors;

    public int ammoCubesLength;

    //P_Motor pMotor;

    void Awake()
    {
        S = this;
        //pMotor = P_Motor.S;
    }

    // Use this for initialization
    void Start()
    {
        ammoCubesLength = ammoCubes.Length;
    }

    void setCube(int i)
    {
        //transform.GetChild(i).GetComponent<Mesh
    }

    // Update is called once per frame
    void Update()
    {

    }

}
