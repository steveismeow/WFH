using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages which day it is, determining day of the week, advancing time, and managing actions based on time of day
/// </summary>
public class DayManager : MonoBehaviour
{
    public DayOfTheWeek dayUI;
    public DialogueManager dialogueManager;

    private void Start()
    {
        //TEST
        StartWeek();
    }

    public void StartWeek()
    {
        dayUI.dayNumber = 0;
        dayUI.SetDayOfTheWeek();

        StartDay();
    }

    public void AdvanceDay()
    {
        dayUI.dayNumber++;
        dayUI.SetDayOfTheWeek();

        StartDay();
    }

    public void StartDay()
    {
        //Normal start day actions prior to any dialogue (i.e. fade in, etc.)

        switch (dayUI.dayNumber)
        {
            case 0:
                NovelController.instance.LoadChapterFile("Scene0");
                break;
            case 1:
                NovelController.instance.LoadChapterFile("Scene0");
                break;
            case 2:
                NovelController.instance.LoadChapterFile("Scene0");
                break;
            case 3:
                NovelController.instance.LoadChapterFile("Scene0");
                break;
            case 4:
                NovelController.instance.LoadChapterFile("Scene0");
                break;
            case 5:
                NovelController.instance.LoadChapterFile("Scene0");
                break;
            case 6:
                NovelController.instance.LoadChapterFile("Scene0");
                break;
            case 7:
                NovelController.instance.LoadChapterFile("Scene0");
                break;
        }
    }
}
