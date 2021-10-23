using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetingManager : MonoBehaviour
{
    public static MeetingManager instance;

    public Transform videoFeed;
    public SpriteRenderer background;
    public SpriteRenderer bloodyBackground;
    public GameObject startMeetingButton;
    public Character currentCharacter;

    private Animator currentAnimator;

    [SerializeField]
    private Sprite defaultBackground;
    [SerializeField]
    private Sprite whiteNoise;

    public AudioClip startCall, endCall;

    [SerializeField]
    private GameObject notificationTag;

    public string queuedMeeting;

    public List<GameObject> characters = new List<GameObject>();

    public GameObject characterPool;

    public bool hasBeenInitialized = false;


    private void Awake()
    {
        instance = this;
    }

    //put on the screen canvas
    public void OnScreenOpen()
    {
        if (queuedMeeting != "")
        {
            //if a meeting is queued, set the button to active
            print("Queued meeting is not null");

            startMeetingButton.SetActive(true);
        }

    }

    public void StartUp()
    {
        hasBeenInitialized = true;

        foreach (Transform child in characterPool.transform)
        {
            characters.Add(child.gameObject);
        }

        startMeetingButton.SetActive(false);
        notificationTag.SetActive(false);

    }


    public void SetMeetingDetails(GameObject character, string meetingName)
    {
        //get the Character script
        //Character characterData = character.GetComponent<Character>();
        if (currentCharacter != null)
        {
            currentCharacter.gameObject.SetActive(false);
        }

        //set the character
        currentCharacter = character.GetComponent<Character>();


        //set the queued meeting
        queuedMeeting = meetingName;

  
        notificationTag.SetActive(true);
    }

    bool isstartingmeeting { get { return startingMeeting != null; } }
    Coroutine startingMeeting;
    public void StartMeeting()
    {
        startMeetingButton.SetActive(false);

        notificationTag.SetActive(false);

        AudioManager.instance.PlayMusic(null);

        startingMeeting = StartCoroutine(StartingMeeting());

    }

    IEnumerator StartingMeeting()
    {
        AudioManager.instance.PlaySFX(startCall);

        yield return new WaitForSeconds(2f);

        //Need to debug this line
        background.sprite = currentCharacter.background;

        //GameObject characterToMeet = Instantiate(currentCharacter.gameObject, videoFeed);

        currentCharacter.gameObject.SetActive(true);

        currentAnimator = currentCharacter.GetComponent<Animator>();


        NovelController.instance.LoadChapterFile(queuedMeeting);

    }

    bool isendingmeeting { get { return endingMeeting != null; } }
    Coroutine endingMeeting;

    public void EndMeeting()
    {
        //set the background back to default
        background.sprite = defaultBackground;

        currentCharacter.gameObject.SetActive(false);

        queuedMeeting = "";

        endingMeeting = StartCoroutine(EndingMeeting());
    }

    IEnumerator EndingMeeting()
    {
        AudioManager.instance.PlaySFX(endCall);

        yield return new WaitForSeconds(2f);
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

    Coroutine glitching;
    public void ReplacementGlitch()
    {
        glitching = StartCoroutine(Glitching());
    }

    IEnumerator Glitching()
    {
        currentCharacter.gameObject.SetActive(false);

        background.sprite = whiteNoise;
        bloodyBackground.sprite = currentCharacter.bloodyBackground;

        float fadeOutDuration = 1.2f;
        float elapsedTime = 0;
        float startValue = background.color.a;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, 0, elapsedTime/fadeOutDuration);

            background.color = new Color(background.color.r, background.color.r, background.color.b, newAlpha);
            yield return null;
        }

        bloodyBackground.sprite = null;

        background.color = new Color(background.color.r, background.color.r, background.color.b, 255);
        background.sprite = defaultBackground;

        yield return new WaitForSeconds(0.4f);

        background.sprite = currentCharacter.background;
        currentCharacter.gameObject.SetActive(true);
    }

}
