using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetingManager : MonoBehaviour
{
    public static MeetingManager instance;

    public Transform videoFeed;
    public SpriteRenderer background;
    public GameObject startMeetingButton;
    public Character currentCharacter;

    private Animator currentAnimator;

    [SerializeField]
    private Sprite defaultBackground;

    private string queuedMeeting;


    public List<GameObject> characters = new List<GameObject>();

    public GameObject characterPool;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach (Transform child in characterPool.transform)
        {
            characters.Add(child.gameObject);
        }
    }


    public void SetMeetingDetails(GameObject character, string meetingName)
    {
        //get the Character script
        //Character characterData = character.GetComponent<Character>();

        //set the character
        currentCharacter = character.GetComponent<Character>();


        //set the queued meeting
        queuedMeeting = meetingName;

        //activate StartMeetingButton
        startMeetingButton.SetActive(true);
    }

    public void StartMeeting()
    {
        startMeetingButton.SetActive(false);

        //Need to debug this line
        background.sprite = currentCharacter.background;

        //GameObject characterToMeet = Instantiate(currentCharacter.gameObject, videoFeed);

        currentCharacter.gameObject.SetActive(true);

        currentAnimator = currentCharacter.GetComponent<Animator>();


        NovelController.instance.LoadChapterFile(queuedMeeting);
    }

    public void EndMeeting()
    {
        //set the background back to default
        background.sprite = defaultBackground;

        currentCharacter.gameObject.SetActive(false);
    }

    public GameObject IdentifyCharacter(string name)
    {

        foreach (GameObject character in characters)
        {
            string charName = character.GetComponent<Character>().characterName;

            if (charName == name)
            {
                return character;
            }
     
 
        }

        return null;
    }

    public void PlayAnimation(string animStateName)
    {
        currentAnimator.Play(animStateName);
    }

}
