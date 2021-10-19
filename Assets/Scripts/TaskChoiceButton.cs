using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskChoiceButton : MonoBehaviour
{
    public WorkManager workManager;

    [SerializeField]
    private TMP_Text taskChoicetext;

    public void ChooseTaskChoice()
    {
        workManager.ChooseTaskChoice(taskChoicetext.text);

    }

}
