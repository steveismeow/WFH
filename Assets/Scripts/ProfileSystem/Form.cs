using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Form : MonoBehaviour
{

    // Sets previews -- these are in the preview container -- these are good
    public TMP_Text previewName;
    public TMP_Text previewTitle;


    // This section needs to have each of the parameters
    public string characterName;
    public string title;
    public string age;
    public string medicalHistory;
    public string workPerformance;
    public string futureGoals;
    public string additionalNotes;

    public Sprite portrait;

    //public Sprite profileImage;


    private void Start()
    {

        previewName.SetText(characterName);
        previewTitle.SetText(title);
    }

    public void SelectProfileFromProfileContainer()
    {
        if (ProfileManager.instance.profileBodyContent.activeSelf)
        {
        }
        else
        {
            ProfileManager.instance.profileBodyContent.SetActive(true);
        }

        ProfileManager.instance.profileBodyText.characterName.text = characterName;
        ProfileManager.instance.profileBodyText.jobTitle.text = title;
        ProfileManager.instance.profileBodyText.age.text = age;
        ProfileManager.instance.profileBodyText.medicalHistory.text = medicalHistory;
        ProfileManager.instance.profileBodyText.workPerformance.text = workPerformance;
        ProfileManager.instance.profileBodyText.futureGoals.text = futureGoals;
        ProfileManager.instance.profileBodyText.additionalNotes.text = additionalNotes;
        ProfileManager.instance.portrait.sprite = portrait;
        //ProfileManager.instance.profileBodyText.profileImage.sprite = profileImage;

        ProfileManager.instance.currentlyVisibleProfile = this;

    }
}
