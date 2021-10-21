using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Form : MonoBehaviour
{
    public TMP_Text previewName;
    public TMP_Text previewTitle;
    public TMP_Text previewAge;


    // Needs to match the parameters in ProfiuleBodyText
    public string characterName;
    public string title;
    public string age;

    public ProfileManager profileManager;

    private void Start()
    {

        previewName.SetText(characterName);
        previewTitle.SetText(title);
        previewAge.SetText(age);
    }

    public void SelectProfileFromProfileContainer()
    {
        if (ProfileManager.instance.profileBodyContent.activeSelf)
        {
        }
        else
        {
            ProfileManager.instance.profileBodyContent.SetActive(true);
            print("activated Profile Body");
        }

        // Each parameter in ProfileBodyText is referenced here, and the values declared above are set to those parameters
        ProfileManager.instance.profileBodyText.characterName.text = characterName;
        ProfileManager.instance.profileBodyText.jobTitle.text = title;
        ProfileManager.instance.profileBodyText.age.text = age;

        ProfileManager.instance.currentlyVisibleProfile = this;

    }
}
