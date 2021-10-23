using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

/// <summary>
/// Pulls realtime clock information and sets the in-game clock accordingly.
/// </summary>
public class Clock : MonoBehaviour
{

    public static Clock instance;

    public TMP_Text clockText;

    // Give it an awaken function
    private void Awake()
    {
        instance = this;
    }

    public void SetTimeTo(string time_data)
    {
        // Set to 9 AM
        if (time_data == "09:00")
        {
            clockText.text = "09::00";
        }
        // Set to 12 PM
        if (time_data == "12:00")
        {
            clockText.text = "12::00";
        }
        // Set to 3 PM
        if (time_data == "15:00")
        {
            clockText.text = "15::00";
        }
        // Set to 5 PM
        if (time_data == "17:00")
        {
            clockText.text = "17::00";
        }


    }

}
