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

    public static DayManager instance;

    public int dayNumber;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //TEST
        //StartWeek();
    }

    public void StartWeek()
    {
        dayNumber = 0;
        UpdateDayUI();

        StartDay();
    }

    public void UpdateDayUI()
    {
        dayUI.dayNumber = dayNumber;

        dayUI.SetDayOfTheWeek();
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

        MeetingManager.instance.StartUp();
        //MailManager.instance.StartUp();
        //WorkManager.instance.StartUp();


        switch (dayUI.dayNumber)
        {
            case 0:
                NovelController.instance.LoadChapterFile("Day1_Start");
                break;
            case 1:
                NovelController.instance.LoadChapterFile("Day2_Start");
                break;
            case 2:
                NovelController.instance.LoadChapterFile("Day3_Start");
                break;
            case 3:
                NovelController.instance.LoadChapterFile("Day4_Start");
                break;
            case 4:
                NovelController.instance.LoadChapterFile("Day5_Start");
                break;
            //case 5:
            //    NovelController.instance.LoadChapterFile("Day1_Start");
            //    break;
        }
    }
}
