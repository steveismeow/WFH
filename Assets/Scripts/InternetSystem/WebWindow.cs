using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class WebWindow : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField searchField;

    [SerializeField]
    private GameObject warningWindow;


    public void OpenWarningWindow()
    {
        if (searchField.text != "")
        {
            warningWindow.SetActive(true);
        }
        else
        {
            return;
        }
    }

    public void CloseWarningWindow()
    {
        warningWindow.SetActive(false);
    }
}
