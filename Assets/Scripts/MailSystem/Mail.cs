using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mail : MonoBehaviour
{
    public MailManager mailManager; 

    public TMP_Text previewSender;
    public TMP_Text previewSubject;
    public TMP_Text previewText;

    public string sender;
    public string subject;
    public TMP_Text mailTextPrefab;

    //Replies
    public List<string> replies = new List<string>();

    public bool hasOpened = false;
    public bool hasReplied = false;

    public string chosenReply = "";

    //Bool Checks
    [SerializeField]
    private bool startsMeeting;
    [SerializeField]
    private string meetingChar, meetingName;

    [SerializeField]
    private bool startsDialogue;
    [SerializeField]
    private string dialogueName; 

    private void Start()
    {
        previewSender.SetText(sender);
        previewSubject.SetText(subject);
        previewText.SetText(mailTextPrefab.text);
    }

    public void SelectMailFromInbox()
    {
        if (mailManager.bodyContent.activeSelf)
        {
        }
        else
        {
            mailManager.bodyContent.SetActive(true);
        }

        if (!hasOpened)
        {
            hasOpened = true;
        }

        mailManager.bodyText.sender.text = sender;
        mailManager.bodyText.subject.text = subject;
        mailManager.bodyText.mailText.text = mailTextPrefab.text;

        mailManager.currentlyVisibleMail = this;

        mailManager.UpdateNotifications();

        ClearReplyContent();
        LoadReplyContent();

        CheckForBools();

    }

    public void ClearReplyContent()
    {
        mailManager.ClearReplyContent();
    }

    public void LoadReplyContent()
    {
        if (hasReplied)
        {
            mailManager.LoadInReplyText(this);
        }
        else
        {
            mailManager.LoadInReplyButtons(this);
        }
    }

    public void CheckForBools()
    {
        if (hasOpened && startsMeeting)
        {
            var character = MeetingManager.instance.IdentifyCharacter(meetingChar);
            MeetingManager.instance.SetMeetingDetails(character, meetingName);
        }
        else if (hasOpened && startsDialogue)
        {
            NovelController.instance.LoadChapterFile(dialogueName);
        }
        else
        {
            return;
        }
    }
}
