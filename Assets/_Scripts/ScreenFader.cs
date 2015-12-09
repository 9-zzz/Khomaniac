using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader S;
    public Image thisImage;
    public Color initialColor;

    void Awake()
    {
        S = this;
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
