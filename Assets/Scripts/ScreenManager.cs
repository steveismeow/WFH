using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the computer screens panels
/// </summary>
public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;

    [SerializeField]
    private GameObject lockScreen;
    [SerializeField]
    private CanvasGroup lockScreenCanvasGroup;
    [SerializeField]
    private GameObject meetingWindow;
    [SerializeField]
    private GameObject mailWindow;
    [SerializeField]
    private GameObject workWindow;
    [SerializeField]
    private GameObject webWindow;
    [SerializeField]
    private GameObject musicWindow;
    [SerializeField]
    private GameObject notesWindow;

    [SerializeField]
    private GameObject dock;

    public AudioClip unlock;



    [SerializeField]
    private List<Button> buttons = new List<Button>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DeactivateAllWindows();
        DeactivateDock();
    }

    private void Update()
    {
        if (lockScreen.activeSelf && !DialogueManager.instance.dialogueBox.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isdeactivatinglockscreen) 
            {
                DeactivateLockScreen();
            }
        }
    }

    public void ActivateLockScreen()
    {
        lockScreen.SetActive(true);
        lockScreenCanvasGroup.alpha = 1;
    }

    bool isdeactivatinglockscreen { get { return deactivatingLockScreen != null; } }
    Coroutine deactivatingLockScreen;
    public void DeactivateLockScreen()
    {
        deactivatingLockScreen = StartCoroutine(DeactivatingLockScreen());
    }

    IEnumerator DeactivatingLockScreen()
    {
        //play sfx
        AudioManager.instance.PlaySFX(unlock);

        float fadeInDuration = 1.5f;
        float elapsedTime = 0f;

        ActivateDock();

        bool isTransitioning = true;
        while (isTransitioning)
        {

            while (elapsedTime < fadeInDuration)
            {
                elapsedTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(1, 0, elapsedTime / fadeInDuration);
                lockScreenCanvasGroup.alpha = newAlpha;

                if (newAlpha <= 0)
                {
                    isTransitioning = false;
                }

                yield return null;
            }
        }
        


        lockScreen.SetActive(false);

        if (DayManager.instance.dayUI.dayNumber == 0)
        {
            NovelController.instance.LoadChapterFile("Day1_EmailPrompt");
        }

        deactivatingLockScreen = null;
    }

    public void DeactivateDock()
    {
        dock.SetActive(false);
    }

    public void ActivateDock()
    {
        dock.SetActive(true);
    }



    public void ActivateMeetingWindow()
    {
        mailWindow.SetActive(false);
        workWindow.SetActive(false);
        webWindow.SetActive(false);
        musicWindow.SetActive(false);
        notesWindow.SetActive(false);


        if (meetingWindow.activeSelf)
        {
            meetingWindow.SetActive(false);
        }
        else
        {
            meetingWindow.SetActive(true);
            MeetingManager.instance.OnScreenOpen();
        }
    }
    public void ActivateMailWindow()
    {
        meetingWindow.SetActive(false);
        workWindow.SetActive(false);
        webWindow.SetActive(false);
        musicWindow.SetActive(false);
        notesWindow.SetActive(false);

        if (mailWindow.activeSelf)
        {
            mailWindow.SetActive(false);
        }
        else
        {
            mailWindow.SetActive(true);
        }
    }

    public void ActivateWorkWindow()
    {
        meetingWindow.SetActive(false);
        mailWindow.SetActive(false);
        webWindow.SetActive(false);
        musicWindow.SetActive(false);
        notesWindow.SetActive(false);

        if (workWindow.activeSelf)
        {
            workWindow.SetActive(false);
        }
        else
        {
            workWindow.SetActive(true);
        }
    }

    public void ActivateNotesWindow()
    {
        meetingWindow.SetActive(false);
        workWindow.SetActive(false);
        mailWindow.SetActive(false);
        webWindow.SetActive(false);
        musicWindow.SetActive(false);


        if (notesWindow.activeSelf)
        {
            notesWindow.SetActive(false);
        }
        else
        {
            notesWindow.SetActive(true);
        }
    }

    public void ActivateWebWindow()
    {
        meetingWindow.SetActive(false);
        workWindow.SetActive(false);
        mailWindow.SetActive(false);
        notesWindow.SetActive(false);
        musicWindow.SetActive(false);


        if (webWindow.activeSelf)
        {
            webWindow.SetActive(false);
        }
        else
        {
            webWindow.SetActive(true);
        }
    }

    public void ActivateMusicWindow()
    {
        meetingWindow.SetActive(false);
        workWindow.SetActive(false);
        mailWindow.SetActive(false);
        webWindow.SetActive(false);
        notesWindow.SetActive(false);


        if (musicWindow.activeSelf)
        {
            musicWindow.SetActive(false);
        }
        else
        {
            musicWindow.SetActive(true);
        }
    }




    public void DeactivateAllWindows()
    {
        print("test");

        mailWindow.SetActive(false);
        workWindow.SetActive(false);
        meetingWindow.SetActive(false);
        notesWindow.SetActive(false);
        webWindow.SetActive(false);
        musicWindow.SetActive(false);
    }

    public void DisableButtonInteraction()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }

    public void EnableButtonInteraction()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }


}
