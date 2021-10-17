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
    public Transform inbox;

    public GameObject bodyContent;

    public BodyText bodyText;

    public List<GameObject> mailList = new List<GameObject>();

    public Mail currentlyVisibleMail;



    [SerializeField]
    private Transform replyButtonContainer;

    [SerializeField]
    private GameObject replyButtonPrefab, replyTextContainer;

    [SerializeField]
    private TMP_Text replyText;




    private void Start()
    {
        //This is a Test. Generally, each day we'll need to load in the relevant mail objects
        LoadInMail("Day1_Boss");

    }

    // Reply Container will house buttons with reply options before you have replied.
    // Mail Manager will have a reference to this object.
    // MailObject OnClick will load in the replies to the Reply Container in order to generate buttons with each reply string.


    public void LoadInMail(string mailName)
    {
        print(mailName);

        foreach(GameObject mailObj in mailList)
        {
            if (mailObj.name == mailName)
            {
                GameObject mailObject = Instantiate(mailObj, transform.position, Quaternion.identity);
                mailObject.transform.SetParent(inbox, false);
                Mail mailData = mailObject.GetComponent<Mail>();
                mailData.mailManager = this;
            }
            else
            {

            }
        }
    }

    public void LoadInReplyButtons(Mail mailData)
    {
        replyButtonContainer.gameObject.SetActive(true);
        replyTextContainer.SetActive(false);

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
        replyTextContainer.SetActive(true);

        replyText.text = mailData.chosenReply;
    }

    public void ChooseReply(string replyButtonText)
    {
        currentlyVisibleMail.hasReplied = true;
        currentlyVisibleMail.chosenReply = replyButtonText;

        ClearReplyContent();

        replyButtonContainer.gameObject.SetActive(false);
        replyTextContainer.SetActive(true);
        replyText.text = replyButtonText;

    }

    public void ClearReplyContent()
    {
        replyText.text = "";
        print(replyButtonContainer.transform.childCount);

        foreach (Transform child in replyButtonContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
