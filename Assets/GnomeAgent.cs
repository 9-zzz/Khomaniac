using UnityEngine;
using System.Collections;

public class GnomeAgent : MonoBehaviour
{
    Renderer rend;
    Color[] colors = new Color[6];
    public Transform goal;
    NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rend = GetComponent<Renderer>();
        colors[0] = Color.cyan;
        colors[1] = Color.red;
        colors[2] = Color.green;
        colors[3] = new Color(255, 165, 0);
        colors[4] = Color.yellow;
        colors[5] = Color.magenta;
    }

    // Use this for initialization
    void Start()
    {
        this.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        StartCoroutine(waitColorChange());
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = goal.position;
    }

    IEnumerator waitColorChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            rend.materials[3].SetColor("_EmissionColor", colors[Random.Range(0, colors.Length)]);
        }
    }

}
