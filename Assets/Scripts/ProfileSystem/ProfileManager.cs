using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ProfileManager : MonoBehaviour
{
    public static ProfileManager instance;

    public Transform profileContent;

    public GameObject profileBodyContent;

    public ProfileBodyText profileBodyText;

    public Form currentlyVisibleProfile;

    public List<GameObject> profileForms = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public void StartUp()
    {
        //This is a Test. Generally, each day we'll need to load in the relevant task objects
        LoadInProfiles();
        print("dlfkjvndlkjvb");
    }


    // method to load in form objects -- right now just testing with LoadInTask() method
    public void LoadInProfileMain(string taskName)
    {
        print(taskName);

        foreach (GameObject taskObj in profileForms)
        {
            if (taskObj.name == taskName)
            {
                GameObject profileObject = Instantiate(taskObj, transform.position, Quaternion.identity);
                profileObject.transform.SetParent(profileContent, false);
                Form profileData = profileObject.GetComponent<Form>();
                profileData.profileManager = this;
            }
            else
            {

            }
        }
    }

    public void LoadInProfiles()
    {
        switch (DayManager.instance.dayNumber)
        {
            case 0:
                PopulateProfiles(profileForms);
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;

        }

        print("does something happen here?");
        profileBodyContent.SetActive(false);
    }

    public void PopulateProfiles(List<GameObject> profileForms)
    {
        foreach (GameObject profileForm in profileForms)
        {
            GameObject profileObject = Instantiate(profileForm, transform.position, Quaternion.identity);
            profileObject.transform.SetParent(profileContent, false);
            Form profileData = profileObject.GetComponent<Form>();
            
        }

    }

}