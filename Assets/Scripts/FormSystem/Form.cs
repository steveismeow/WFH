using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Form : MonoBehaviour
{


    public TMP_Text previewDay;
    public TMP_Text previewCharacter;
    public TMP_Text previewTitle;
    public TMP_Text previewText;


    public string day;
    public string character;
    public string title;
    public TMP_Text formTextPrefab;

    public WorkManager workManager;



    public List<string> choices = new List<string>();

    public bool hasChosenTask = false;

    public string chosenTaskChoice = "";

    private void Start()
    {

        previewDay.SetText(day);
        previewCharacter.SetText(character);
        previewTitle.SetText(title);
        previewText.SetText(formTextPrefab.text);
    }

    public void SelectTaskFromWorkTaskContainer()
    {
        if (workManager.workBodyContent.activeSelf)
        {
        }
        else
        {
            workManager.workBodyContent.SetActive(true);
        }

        workManager.workBodyText.day.text = day;
        workManager.workBodyText.character.text = character;
        workManager.workBodyText.title.text = title;
        workManager.workBodyText.formText.text = formTextPrefab.text;

        workManager.currentlyVisibleTask = this;

        ClearTaskChoiceContent();
        LoadTaskChoiceContent();



    }
    // Uses workManager's ClearTaskChoicecontent() method
    public void ClearTaskChoiceContent()
    {
        workManager.ClearTaskChoiceContent();
    }

    public void LoadTaskChoiceContent()
    {
        
        if (hasChosenTask)
        {
            workManager.LoadInTaskChoiceText(this);
        }
        else
        {
            workManager.LoadInTaskChoiceButtons(this);
        }
    }
}
