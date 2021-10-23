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
            clockText.text = time_data;
    }

}
