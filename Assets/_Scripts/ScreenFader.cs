using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    Image thisImage;
    public Color initialColor;

    void Awake()
    {
        thisImage = GetComponent<Image>();
        thisImage.color = initialColor;
    }

    void Start()
    {
        thisImage.CrossFadeAlpha(0, 1.0f, true);

    }

    void Update()
    {

    }
}
