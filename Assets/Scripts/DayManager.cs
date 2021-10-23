using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Manages which day it is, determining day of the week, advancing time, and managing actions based on time of day
/// </summary>
public class DayManager : MonoBehaviour
{
    public DayOfTheWeek dayUI;
    public DialogueManager dialogueManager;

    public TMP_Text dayScreenText;
    public Animator dayScreenAnim;

    public static DayManager instance;

    public int dayNumber;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //dayScreenText.enabled = false;

        //TEST
        StartWeek();
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
        dayNumber++;
        UpdateDayUI();

        print("Day Number:" + dayNumber);
        print("UI Day Number:" + dayUI.dayNumber);


        //StartDay();
    }

    public bool isstartingday { get { return startingDay != null; } }
    Coroutine startingDay;
    public void StartDay()
    {
        print("Starting Day!");

        dayScreenText.enabled = false;

        //Normal start day actions prior to any dialogue (i.e. fade in, etc.)
        //startingDay = StartCoroutine(startingDay());
        if (!MeetingManager.instance.hasBeenInitialized)
        {
            MeetingManager.instance.StartUp();
        }

        if (!ProfileManager.instance.hasBeenInitialized)
        {
            ProfileManager.instance.StartUp();
        }

        MailManager.instance.StartUp();

        ScreenManager.instance.ActivateLockScreen();

        Clock.instance.SetTimeTo("9:00AM");


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

    //IEnumerator StartingDay()
    //{

    //}

    public void EndDay()
    {

        AdvanceDay();
        SetDayScreenText();

        endingDay = StartCoroutine(EndingDay());

    }

    public bool isendingday { get { return endingDay != null; } }
    Coroutine endingDay;
    IEnumerator EndingDay()
    {
        ScreenManager.instance.DeactivateAllWindows();
        ScreenManager.instance.DeactivateDock();

        dayScreenText.enabled = true;

        dayScreenAnim.Play("Play");

        yield return new WaitForSeconds(3f);

        StartDay();
        endingDay = null;
    }

    void SetDayScreenText()
    {
        switch (dayUI.dayNumber)
        {
            case 1:
                dayScreenText.text = "Tuesday";

                break;
            case 2:
                dayScreenText.text = "Wednesday";

                break;
            case 3:
                dayScreenText.text = "Thursday";

                break;
            case 4:
                dayScreenText.text = "Friday";

                break;
        }
    }

}
