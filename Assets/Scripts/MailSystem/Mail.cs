using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mail : MonoBehaviour
{
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
        if (MailManager.instance.bodyContent.activeSelf)
        {
        }
        else
        {
            MailManager.instance.bodyContent.SetActive(true);
        }

        if (!hasOpened)
        {
            hasOpened = true;
            MailManager.instance.UpdateNotifications();
        }

        MailManager.instance.bodyText.sender.text = sender;
        MailManager.instance.bodyText.subject.text = subject;
        MailManager.instance.bodyText.mailText.text = mailTextPrefab.text;

        MailManager.instance.currentlyVisibleMail = this;
        MailManager.instance.ResetScrollBar();


        ClearReplyContent();
        LoadReplyContent();

        CheckForBools();

    }

    public void ClearReplyContent()
    {
        MailManager.instance.ClearReplyContent();
    }

    public void LoadReplyContent()
    {
        if (hasReplied)
        {
            MailManager.instance.LoadInReplyText(this);
        }
        else
        {
            MailManager.instance.LoadInReplyButtons(this);
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
