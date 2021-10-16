using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the computer screens panels
/// </summary>
public class ScreenManager : MonoBehaviour
{
    [SerializeField]
    private GameObject meetingWindow;
    [SerializeField]
    private GameObject mailWindow;
    [SerializeField]
    private GameObject workWindow;

    [SerializeField]
    private List<Button> buttons = new List<Button>();


    private void Start()
    {
        DeactivateAllWindows();
    }

    public void ActivateMeetingWindow()
    {
        mailWindow.SetActive(false);
        workWindow.SetActive(false);

        if (meetingWindow.activeSelf)
        {
            meetingWindow.SetActive(false);
        }
        else
        {
            meetingWindow.SetActive(true);
        }
    }
    public void ActivateMailWindow()
    {
        meetingWindow.SetActive(false);
        workWindow.SetActive(false);

        if (mailWindow.activeSelf)
        {
            mailWindow.SetActive(false);
        }
        else
        {
            mailWindow.SetActive(true);
        }
    }

    public void ActivateWorkWindow()
    {
        meetingWindow.SetActive(false);
        mailWindow.SetActive(false);

        if (workWindow.activeSelf)
        {
            workWindow.SetActive(false);
        }
        else
        {
            workWindow.SetActive(true);
        }
    }

    public void DeactivateAllWindows()
    {
        mailWindow.SetActive(false);
        workWindow.SetActive(false);
        meetingWindow.SetActive(false);
    }

    public void DisableButtonInteraction()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }

    public void EnableButtonInteraction()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }


}