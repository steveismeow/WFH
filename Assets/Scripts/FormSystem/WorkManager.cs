using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WorkManager : MonoBehaviour
{

    public Transform workTaskContent;

    public GameObject workBodyContent;

    public BodyText workBodyText;


    public List<GameObject> auxiliaryTaskList = new List<GameObject>();

    public Form currentlyVisibleTask;


    // Declarations of task lists based on day
    public List<GameObject> day1TaskList = new List<GameObject>();
    public List<GameObject> day2TaskList = new List<GameObject>();
    public List<GameObject> day3TaskList = new List<GameObject>();
    public List<GameObject> day4TaskList = new List<GameObject>();
    public List<GameObject> day5TaskList = new List<GameObject>();



    private void Start()
    {
        //This is a Test. Generally, each day we'll need to load in the relevant mail objects
        LoadInTask();

    }


    // method to load in form objects -- right now just testing with LoadInTask() method
    public void LoadInTaskMain(string taskName)
    {
        print(taskName);

        foreach (GameObject taskObj in auxiliaryTaskList)
        {
            if (taskObj.name == taskName)
            {
                GameObject taskObject = Instantiate(taskObj, transform.position, Quaternion.identity);
                taskObject.transform.SetParent(workTaskContent, false);
                Form taskData = taskObject.GetComponent<Form>();
                taskData.workManager = this;
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
                PopulateTasks(day1TaskList);
                break;
            case 1:
                PopulateTasks(day2TaskList);
                break;
            case 2:
                PopulateTasks(day3TaskList);
                break;
            case 3:
                PopulateTasks(day4TaskList);
                break;
            case 4:
                PopulateTasks(day5TaskList);
                break;

        }

        bodyContent.SetActive(false);
    }

    public void PopulateTasks(List<GameObject> task)
    {
        foreach (GameObject taskObj in taskList)
        {
            GameObject taskObject = Instantiate(taskObj, transform.position, Quaternion.identity);
            taskObject.transform.SetParent(workTaskContent, false);
            Form taskData = taskObject.GetComponent<Form>();
            taskData.workManager = this;
        }

    }

}