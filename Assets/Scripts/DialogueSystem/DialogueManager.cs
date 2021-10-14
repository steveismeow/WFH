using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the dialogue UI elements and the TextArchitect
/// </summary>
public class DialogueManager : MonoBehaviour
{

    public static DialogueManager instance;
    public ELEMENTS elements;

    [SerializeField]
    private ScreenManager screenManager;

    void Awake()
    {
        instance = this;
    }

    public void Speak(string dialogue, string speaker = "", bool additive = false)
    {
        StopSpeaking();

        if (additive)
        {
            dialogueText.text = targetDialogue;
        }

        speaking = StartCoroutine(Speaking(dialogue, additive, speaker));


    }

    public void StopSpeaking()
    {
        if (isSpeaking)
        {
            StopCoroutine(speaking);
        }
        if (textArchitect != null && textArchitect.isConstructing)
        {
            textArchitect.Stop();
        }
        speaking = null;

        screenManager.EnableButtonInteraction();

    }

    public bool isSpeaking { get { return speaking != null; } }
    /*[HideInInspector] */public bool isWaitingForUserInput = false;

    public string targetDialogue = "";
    Coroutine speaking = null;
    public TextArchitect textArchitect = null;
    public TextArchitect currentArchitect { get { return textArchitect; } }

    IEnumerator Speaking(string dialogue, bool additive, string speaker = "")
    {
        dialoguePanel.SetActive(true);

        screenManager.DisableButtonInteraction();

        string additiveDialogue = additive ? dialogueText.text : "";
        targetDialogue = additiveDialogue + dialogue;

        if (textArchitect == null)
        {
            textArchitect = new TextArchitect(dialogueText, dialogue, additiveDialogue);
        }
        else
        {
            textArchitect.Renew(dialogue, additiveDialogue);
        }
        speakerNameText.text = DetermineSpeaker(speaker);
        speakerPanel.SetActive(speakerNameText.text != "");

        isWaitingForUserInput = false;

        if (isClosed)
        {
            OpenAllRequirementsForDialogueSystemVisibility(true);
        }

        while (textArchitect.isConstructing)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                textArchitect.skip = true;
            }

            yield return new WaitForEndOfFrame();
        }

        //text has finished
        isWaitingForUserInput = true;
        while (isWaitingForUserInput)

            yield return new WaitForEndOfFrame();

        StopSpeaking();
    }

    string DetermineSpeaker(string s)
    {
        string returnValue = speakerNameText.text;//default is the current name
        if (s != speakerNameText.text && s != "")
        {
            returnValue = (s.ToLower().Contains("player")) ? "" : s;
        }

        return returnValue;

    }

    public void Close()
    {
        StopSpeaking();
        for (int i = 0; i < DialoguePanelRequirements.Length; i++)
        {
            DialoguePanelRequirements[i].SetActive(false);
        }
    }

    public void OpenAllRequirementsForDialogueSystemVisibility(bool v)
    {
        for (int i = 0; i < DialoguePanelRequirements.Length; i++)
        {
            DialoguePanelRequirements[i].SetActive(v);
        }
    }

    public void Open(string speakerName = "", string dialogue = "")
    {
        if (speakerName == "" && dialogue == "")
        {
            OpenAllRequirementsForDialogueSystemVisibility(false);
            return;
        }

        OpenAllRequirementsForDialogueSystemVisibility(true);

        speakerNameText.text = speakerName;
        speakerPanel.SetActive(speakerName != "");
        dialogueText.text = dialogue;
    }

    public bool isClosed { get { return !dialogueBox.activeInHierarchy; } }

    [System.Serializable]
    public class ELEMENTS
    {
        public GameObject dialoguePanel;
        public GameObject speakerPanel;
        public TextMeshProUGUI dialogueText;
        public TextMeshProUGUI speakerNameText;
    }
    public GameObject dialoguePanel { get { return elements.dialoguePanel; } }
    public GameObject speakerPanel { get { return elements.speakerPanel; } }
    public TextMeshProUGUI speakerNameText { get { return elements.speakerNameText; } }
    public TextMeshProUGUI dialogueText { get { return elements.dialogueText; } }

    public GameObject[] DialoguePanelRequirements;
    public GameObject dialogueBox;

}
