using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages Inbox and Body MailWindow objects to transmit info from Mail to BodyText
/// </summary>
public class MailManager : MonoBehaviour
{
    public Transform inbox;

    public BodyText bodyText;

    public List<GameObject> mailList = new List<GameObject>();


    public Transform replyContent;

    public GameObject replyPrefab;


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
                mailObject.GetComponent<Mail>().mailManager = this;

                // Loading replies (TestReplyButton)
                GameObject reply = Instantiate(replyPrefab, transform.position, Quaternion.identity);
                reply.transform.SetParent(replyContent, false);

            }
            else
            {
                return;
            }
        }


    }

}
