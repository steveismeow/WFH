using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReplyButton : MonoBehaviour
{
    public MailManager mailManager;

    [SerializeField]
    private TMP_Text replyText;

    public void ChooseReply()
    {
        mailManager.ChooseReply(replyText.text);

        //mailManager.currentlyVisibleMail.hasReplied = true;
        //mailManager.currentlyVisibleMail.chosenReply = transform.GetChild(0).GetComponent<TMP_Text>().text;
    }

}
