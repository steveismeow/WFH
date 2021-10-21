using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Form : MonoBehaviour
{
    public TMP_Text previewName;
    public TMP_Text previewTitle;


    public string characterName;
    public string title;
    public string age;

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

        ProfileManager.instance.currentlyVisibleProfile = this;

    }
}
