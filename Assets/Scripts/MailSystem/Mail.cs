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

    public MailManager mailManager;

    public List<string> replies = new List<string>();

    public bool hasReplied = false;

    public string chosenReply = "";

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

        mailManager.bodyText.sender.text = sender;
        mailManager.bodyText.subject.text = subject;
        mailManager.bodyText.mailText.text = mailTextPrefab.text;

        mailManager.currentlyVisibleMail = this;

        ClearReplyContent();
        LoadReplyContent();



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
}
