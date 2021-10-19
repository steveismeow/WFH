using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorkBodyText : MonoBehaviour
{

    // day represents day of the week
    public TMP_Text day;
    // character represents the character that the task is related to
    public TMP_Text character;

    // title is the title of the task -- "Evaluation", "Form Submission", etc.
    public TMP_Text title;

    // formText is the text of the task
    public TMP_Text formText;
}
