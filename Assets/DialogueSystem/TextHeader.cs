using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextHeader : MonoBehaviour
{

    public Image banner;
    public TextMeshProUGUI titleText;
    public string title { get { return titleText.text; } set{titleText.text = value;}}

    public enum Display_Method
    {
        instant,
        slowFade,
        typeWriter,
        floatingFade,
    }
    public Display_Method displayMethod = Display_Method.instant;
    public float fadeSpeed = 1;

    public void Show(string displayTitle)
    {
        title = displayTitle;

        if (isRevealing)
        {
            StopCoroutine(revealing);
        }

        if(!cachedBannerPos)
        {
            cachedBannerOriginalPosition = banner.transform.position;
            cachedBannerPos = true;
        }

        revealing = StartCoroutine(Revealing());

    }
    public void Hide()
    {
        if (isRevealing)
        {
            StopCoroutine(revealing);
        }
        revealing = null;

        banner.enabled = false;
        titleText.enabled = false;

        if(cachedBannerPos)
        {
            banner.transform.position = cachedBannerOriginalPosition;
        }
    }
        
    public bool isRevealing { get { return revealing != null; } }
    Coroutine revealing = null;
    IEnumerator Revealing()
    {
        banner.enabled = true;
        titleText.enabled = true;

        switch(displayMethod)
        {
            case Display_Method.instant:
                banner.color = GlobalFunctions.SetAlpha(banner.color, 1);
                titleText.color = GlobalFunctions.SetAlpha(titleText.color, 1);
                break;
            case Display_Method.slowFade:
                yield return SlowFade();
                break;
            case Display_Method.typeWriter:
                yield return TypeWriter();
                break;
            case Display_Method.floatingFade:
                yield return FloatingFade();
                break;

        }

        revealing = null;
    }

    IEnumerator SlowFade()
    {
        banner.color = GlobalFunctions.SetAlpha(banner.color, 0);
        titleText.color = GlobalFunctions.SetAlpha(titleText.color, 0);
        while (banner.color.a < 1)
        {
            banner.color = GlobalFunctions.SetAlpha(banner.color, Mathf.MoveTowards(banner.color.a, 1, fadeSpeed * Time.unscaledDeltaTime));
            titleText.color = GlobalFunctions.SetAlpha(titleText.color, banner.color.a);
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator TypeWriter()
    {
        banner.color = GlobalFunctions.SetAlpha(banner.color, 1);
        titleText.color = GlobalFunctions.SetAlpha(titleText.color, 1);
        TextArchitect architect = new TextArchitect(titleText, title);
        while (architect.isConstructing)
        {
            yield return new WaitForEndOfFrame();
        }
    }
    bool cachedBannerPos = false;
    Vector3 cachedBannerOriginalPosition = Vector3.zero;
    IEnumerator FloatingFade()
    {
        banner.color = GlobalFunctions.SetAlpha(banner.color, 0);
        titleText.color = GlobalFunctions.SetAlpha(titleText.color, 0);

        float amount = 25f * ((float)Screen.height / 720f);
        Vector3 downPos = new Vector3(0, amount, 0);
        banner.transform.position = cachedBannerOriginalPosition - downPos;

        while (banner.color.a < 1 || banner.transform.position != cachedBannerOriginalPosition)
        {
            banner.color = GlobalFunctions.SetAlpha(banner.color, Mathf.MoveTowards(banner.color.a, 1, fadeSpeed * Time.unscaledDeltaTime));
            titleText.color = GlobalFunctions.SetAlpha(titleText.color, banner.color.a);

            banner.transform.position = Vector3.MoveTowards(banner.transform.position, cachedBannerOriginalPosition, 11 * fadeSpeed * Time.unscaledDeltaTime);

            yield return new WaitForEndOfFrame();
        }

    }
}
