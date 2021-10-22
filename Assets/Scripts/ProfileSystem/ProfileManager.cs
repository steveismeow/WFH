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

    public Image portrait;

    public Form currentlyVisibleProfile;

    public List<GameObject> profileForms = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //This is a Test. Generally, each day we'll need to load in the relevant task objects
        LoadInProfiles();

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

        profileBodyContent.SetActive(false);
    }

    public void PopulateProfiles(List<GameObject> profileForms)
    {
        foreach (GameObject profileForm in profileForms)
        {
            GameObject profileObject = Instantiate(profileForm, transform.position, Quaternion.identity);
            profileObject.transform.SetParent(profileContent, false);
            Form taskData = profileObject.GetComponent<Form>();
        }

    }

}