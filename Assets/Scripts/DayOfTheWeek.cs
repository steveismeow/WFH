using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayOfTheWeek : MonoBehaviour
{
    public TMP_Text text;

    public enum Day
    {
        Mon,
        Tues,
        Wed,
        Thur,
        Fri,
        Sat,
        Sun
    }

    public Day day;

    public int dayNumber;

    public void SetDayOfTheWeek()
    {

        day = (Day)dayNumber;

        text.text = day.ToString();

    }
}
