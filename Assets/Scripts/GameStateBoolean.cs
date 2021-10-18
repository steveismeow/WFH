using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameStateBoolean : MonoBehaviour
{

    public GameObject mailManager;
    //mailManager.GetComponent<GameObject>();
    MailManager MailScript;


    public GameObject bodyContent;

    // Morning email trigger booleans
    private bool day1MorningMail = false;
    private bool day2MorningMail = false;
    private bool day3MorningMail = false;
    private bool day4MorningMail = false;
    private bool day5MorningMail = false;


    // Start is called before the first frame update
    void Start()
    {
        processMorningMail();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void processMorningMail()
    {

        MailScript = mailManager.GetComponent<MailManager>();

        // if has opened Day1_Start mail = true
        if (MailScript.day1MorningMailList.Count == true)
        {
            day1MorningMail = true;
        }

        // if has opened Day2_Start mail = true
        if (MailScript.day2MorningMailList.Count == true)
        {
            day2MorningMail = true;
        }

        // if has opened Day3_Start mail = true
        if (MailScript.day3MorningMailList.Count == true)
        {
            day3MorningMail = true;
        }

        // if has opened Day4_Start mail = true
        if (MailScript.day4MorningMailList.Count == true)
        {
            day4MorningMail = true;
        }

        // if has opened Day5_Start mail = true
        if (MailScript.day5MorningMailList.Count == true)
        {
            day5MorningMail = true;
        }

    }


}