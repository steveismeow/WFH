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

    private void Start()
    {
        previewSender.SetText(sender);
        previewSubject.SetText(subject);
        previewText.SetText(mailTextPrefab.text);
    }

    public void SelectMailFromInbox()
    {
        if (mailManager.bodyText.gameObject.activeSelf)
        {
            mailManager.bodyText.sender.text = sender;
            mailManager.bodyText.subject.text = subject;
            mailManager.bodyText.mailText.text = mailTextPrefab.text;
        }
        else
        {
            mailManager.bodyText.gameObject.SetActive(true);

            mailManager.bodyText.sender.text = sender;
            mailManager.bodyText.subject.text = subject;
            mailManager.bodyText.mailText.text = mailTextPrefab.text;

        }



    }
}
