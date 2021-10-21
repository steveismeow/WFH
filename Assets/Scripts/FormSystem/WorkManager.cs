using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WorkManager : MonoBehaviour
{

    public Transform workTaskContent;

    public GameObject workBodyContent;

    public WorkBodyText workBodyText;


    public List<GameObject> auxiliaryProfileList = new List<GameObject>();

    public Form currentlyVisibleProfile;

    // Declaration of Profile list
    public List<GameObject> profileList = new List<GameObject>();


    // Declarations of task lists based on day
    public List<GameObject> day1TaskList = new List<GameObject>();
    public List<GameObject> day2TaskList = new List<GameObject>();
    public List<GameObject> day3TaskList = new List<GameObject>();
    public List<GameObject> day4TaskList = new List<GameObject>();
    public List<GameObject> day5TaskList = new List<GameObject>();



    // Format in MailManager was "repky__" -- ie replyButtonContaineer, etc.
    // For the WorkManager, idea is you are choosing from a list of options as to how you complete your task, so I'm trying "taskChoice_" for the formatting.
    [SerializeField]
    private Transform taskChoiceButtonContainer;

    [SerializeField]
    private GameObject taskChoiceButtonPrefab;
    //emailSignature;

    [SerializeField]
    private TMP_Text taskChoiceText;


    private void Start()
    {
        //This is a Test. Generally, each day we'll need to load in the relevant task objects
        LoadInTask();

    }


    // method to load in form objects -- right now just testing with LoadInTask() method
    public void LoadInTaskMain(string taskName)
    {
        print(taskName);

        foreach (GameObject profileObj in auxiliaryProfileList)
        {
            if (profileObj.name == taskName)
            {
                GameObject profileObject = Instantiate(profileObj, transform.position, Quaternion.identity);
                profileObject.transform.SetParent(workTaskContent, false);
                Form profileData = profileObject.GetComponent<Form>();
                profileData.workManager = this;
            }
            else
            {

            }
        }
    }

    public void LoadInTask()
    {
        switch (DayManager.instance.dayNumber)
        {
            case 0:
                PopulateTasks(profileList);
                break;
            case 1:
                PopulateTasks(day1TaskList);
                break;
            case 2:
                PopulateTasks(day2TaskList);
                break;
            case 3:
                PopulateTasks(day3TaskList);
                break;
            case 4:
                PopulateTasks(day4TaskList);
                break;
            case 5:
                PopulateTasks(day5TaskList);
                break;

        }

        workBodyContent.SetActive(false);
    }

    public void PopulateTasks(List<GameObject> profileListInput)
    {
        foreach (GameObject profileObj in profileListInput)
        {
            GameObject profileObject = Instantiate(profileObj, transform.position, Quaternion.identity);
            profileObject.transform.SetParent(workTaskContent, false);
            Form profileData = profileObject.GetComponent<Form>();
            profileData.workManager = this;
        }

    }


    // Creates the buttons the player can choose from when working on a task
    public void LoadInTaskChoiceButtons(Form taskData)
    {
        taskChoiceButtonContainer.gameObject.SetActive(true);
        taskChoiceText.gameObject.SetActive(false);
        //emailSignature.SetActive(false);


        foreach (string taskChoice in taskData.choices)
        {
            // taskChoiceButtonPrefab uses the TestReplyButton prefab
            GameObject taskChoiceButton = Instantiate(taskChoiceButtonPrefab, transform.position, Quaternion.identity);
            taskChoiceButton.transform.SetParent(taskChoiceButtonContainer, false);
            taskChoiceButton.GetComponent<TaskChoiceButton>().workManager = this;

            taskChoiceButton.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = taskChoice;
        }
    }

    public void LoadInTaskChoiceText(Form taskData)
    {
        taskChoiceButtonContainer.gameObject.SetActive(false);
        taskChoiceText.gameObject.SetActive(true);
        //emailSignature.SetActive(true);

        taskChoiceText.text = taskData.chosenTaskChoice;
    }

    public void ChooseTaskChoice(string taskChoiceButtonText)
    {
        // Boolean for checking wether or not you've chosen a task uses ChosenTask format instead of TaskChoice format, not sure if it needs correction
        currentlyVisibleProfile.hasChosenTask = true;
        currentlyVisibleProfile.chosenTaskChoice = taskChoiceButtonText;

        ClearTaskChoiceContent();

        taskChoiceButtonContainer.gameObject.SetActive(false);
        taskChoiceText.gameObject.SetActive(true);
        //emailSignature.SetActive(true);
        taskChoiceText.text = taskChoiceButtonText;

    }

    public void ClearTaskChoiceContent()
    {
        taskChoiceText.text = "";
        print(taskChoiceButtonContainer.transform.childCount);

        foreach (Transform child in taskChoiceButtonContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

}