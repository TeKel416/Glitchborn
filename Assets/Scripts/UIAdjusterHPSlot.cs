using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIAdjusterHPSlot : MonoBehaviour
{
    public HorizontalLayoutGroup group;
    public UIExtensions.SkewedImage[] images;

    void Start()
    {
        StartCoroutine(Adjust());
    }

    IEnumerator Adjust()
    {
        yield return new WaitForSeconds(0f);

        for(int i = 0; i<images.Length; i++)
        {
            images[i].SkewVector.x = 29.1f * Screen.width/1920f;
        }
        
        group.padding.left = 10 * Screen.width/1920;
        group.padding.right = 50 * Screen.width/1920;

        for(int i = 0; i<images.Length; i++)
        {
            images[i].enabled = false;
        }

        yield return new WaitForSeconds(0f);

        for(int i = 0; i<images.Length; i++)
        {
            images[i].enabled = true;
        }

        yield return new WaitForSeconds(0f);

        group.enabled = false;
    }
}