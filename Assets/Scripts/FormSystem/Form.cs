using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// Class meant to set structure for Profiles -- formerly established structure for "Forms", a series of tasks
public class Form : MonoBehaviour
{


    public TMP_Text previewName;
    public TMP_Text previewAge;
    public TMP_Text previewWeight;
    public TMP_Text previewCity;

    public TMP_Text previewCompanyPosition;

    public TMP_Text previewCompanyYears;

    public TMP_Text previewCompanyAwards;

    public TMP_Text previewDisciplinaryRecord;

    public TMP_Text previewText;





    public string name;
    public string age;
    public string weight;
    public string city;
    public string companyPosition;
    public string companyYears;
    public string companyAwards;
    public string disciplinaryRecord;
    

    public TMP_Text formTextPrefab;

    public WorkManager workManager;



    public List<string> choices = new List<string>();

    public bool hasChosenTask = false;

    public string chosenTaskChoice = "";

    private void Start()
    {

        previewName.SetText(name);
        previewAge.SetText(age);
        previewWeight.SetText(weight);
        previewCity.SetText(city);
        previewCompanyPosition.SetText(companyPosition);
        previewCompanyYears.SetText(companyYears);
        previewCompanyAwards.SetText(companyAwards);
        previewDisciplinaryRecord.SetText(disciplinaryRecord);

        previewText.SetText(formTextPrefab.text);
    }


    // These tasks are actually profiles
    // Implement hidden field -- forbidden information
    // if (creepy_condition > 5)
    //  {
    //   call the forbidden field parameter inside workBodytext that exists as part of workManager (??)
    //   
    //   }
    public void SelectTaskFromWorkTaskContainer()
    {
        if (workManager.workBodyContent.activeSelf)
        {
        }
        else
        {
            workManager.workBodyContent.SetActive(true);
            
        }

        workManager.workBodyText.name.text = name;
        workManager.workBodyText.age.text = age;
        workManager.workBodyText.weight.text = weight;
        workManager.workBodyText.city.text = city;
        workManager.workBodyText.companyPosition.text = companyPosition;
        workManager.workBodyText.companyYears.text = companyYears;
        workManager.workBodyText.companyAwards.text = companyAwards;
        workManager.workBodyText.disciplinaryRecord.text = disciplinaryRecord;

        workManager.workBodyText.formText.text = formTextPrefab.text;

        workManager.currentlyVisibleProfile = this;

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
