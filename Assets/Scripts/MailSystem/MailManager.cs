using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages Inbox and Body MailWindow objects to transmit info from Mail to BodyText
/// </summary>
public class MailManager : MonoBehaviour
{
    public static MailManager instance;

    public Transform inbox;

    public GameObject bodyContent;

    public BodyText bodyText;

    public List<GameObject> auxMailList = new List<GameObject>();

    public Mail currentlyVisibleMail;

    public List<GameObject> day1MorningMailList = new List<GameObject>();
    public List<GameObject> day2MorningMailList = new List<GameObject>();
    public List<GameObject> day3MorningMailList = new List<GameObject>();
    public List<GameObject> day4MorningMailList = new List<GameObject>();
    public List<GameObject> day5MorningMailList = new List<GameObject>();

    [SerializeField]
    private GameObject notificationTag;

    [SerializeField]
    private Scrollbar scrollBar;


    [SerializeField]
    private Transform replyButtonContainer;

    [SerializeField]
    private GameObject replyButtonPrefab, emailSignature;

    [SerializeField]
    private TMP_Text replyText;


    private void Awake()
    {
        instance = this;
    }

    // Name as Start() or StartUp()?
    public void StartUp()
    {
        LoadInMorningMail();
    }


    public void LoadInMorningMail()
    {
        switch(DayManager.instance.dayNumber)
        {
            case 0:
                PopulateInbox(day1MorningMailList);
                break;
            case 1:
                PopulateInbox(day2MorningMailList);
                break;
            case 2:
                PopulateInbox(day3MorningMailList);
                break;
            case 3:
                PopulateInbox(day4MorningMailList);
                break;
            case 4:
                PopulateInbox(day5MorningMailList);
                break;

        }

        bodyContent.SetActive(false);
    }

    public void LoadInMail(string mailName)
    {

        foreach(GameObject mailObj in auxMailList)
        {
            if (mailObj.name == mailName)
            {
                GameObject mailObject = Instantiate(mailObj, transform.position, Quaternion.identity);
                mailObject.transform.SetParent(inbox, false);
                Mail mailData = mailObject.GetComponent<Mail>();
            }
            else
            {

            }
        }

        UpdateNotifications();
    }

    public void PopulateInbox(List<GameObject> mail)
    {
        foreach (GameObject mailObj in mail)
        {
            GameObject mailObject = Instantiate(mailObj, transform.position, Quaternion.identity);
            mailObject.transform.SetParent(inbox, false);
            Mail mailData = mailObject.GetComponent<Mail>();
        }

        UpdateNotifications();
    }

    public void UpdateNotifications()
    {
        int notifications = 0;

        foreach (Transform child in inbox)
        {

            if (!child.gameObject.GetComponent<Mail>().hasOpened)
            {
                notifications += 1;
            }
        }

        if (notifications > 0)
        {
            notificationTag.SetActive(true);
            notificationTag.GetComponent<TMP_Text>().text = notifications.ToString();
        }
        else
        {
            notificationTag.SetActive(false);
        }
    }

    public void LoadInReplyButtons(Mail mailData)
    {
        replyButtonContainer.gameObject.SetActive(true);
        replyText.gameObject.SetActive(false);
        emailSignature.SetActive(false);


        foreach(string reply in mailData.replies)
        {
            GameObject replyButton = Instantiate(replyButtonPrefab, transform.position, Quaternion.identity);
            replyButton.transform.SetParent(replyButtonContainer, false);
            replyButton.GetComponent<ReplyButton>().mailManager = this;

            replyButton.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = reply;
        }
    }

    public void LoadInReplyText(Mail mailData)
    {
        replyButtonContainer.gameObject.SetActive(false);
        replyText.gameObject.SetActive(true);
        emailSignature.SetActive(true);

        replyText.text = mailData.chosenReply;
    }

    public void ChooseReply(string replyButtonText)
    {
        currentlyVisibleMail.hasReplied = true;
        currentlyVisibleMail.chosenReply = replyButtonText;

        ClearReplyContent();

        replyButtonContainer.gameObject.SetActive(false);
        replyText.gameObject.SetActive(true);
        emailSignature.SetActive(true);
        replyText.text = replyButtonText;

    }

    public void ClearReplyContent()
    {
        replyText.text = "";

        foreach (Transform child in replyButtonContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ResetScrollBar()
    {
        scrollBar.value = 1;
    }

}
