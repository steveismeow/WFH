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

    private void Start()
    {
        //This is a Test. Generally, each day we'll need to load in the relevant mail objects
        LoadInMail("TestMail");
    }

    public void LoadInMail(string mailName)
    {

        print(mailName);

        foreach(GameObject mailObj in mailList)
        {
            print(mailObj.name);

            if (mailObj.name == mailName)
            {
                print("mailobj should be instantiated");

                GameObject mailObject = Instantiate(mailObj, transform.position, Quaternion.identity);
                mailObject.transform.SetParent(inbox, false);
                mailObject.GetComponent<Mail>().mailManager = this;
            }
            else
            {
                print("mailobj was not found");

                return;
            }
        }


    }

}
