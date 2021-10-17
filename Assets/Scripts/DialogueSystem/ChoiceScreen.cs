using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceScreen : MonoBehaviour
{

    public static ChoiceScreen instance;

    public GameObject root;
    public Transform buttonContainer;

    public TextHeader _header;
    public static TextHeader header { get { return instance._header; } }

    public ChoiceButton choiceprefab;

    static List<ChoiceButton> choices = new List<ChoiceButton>();

    public VerticalLayoutGroup layoutGroup;

    void Awake()
    {
        instance = this;
        Hide();
    }

    public static void Show(string title, params string[] choices)
    {
        instance.root.SetActive(true);

        if (title != "")
        {
            header.Show(title);
        }
        else
        {
            header.Hide();
        }

        if (isShowingChoices)
        {
            instance.StopCoroutine(showingChoices);
        }

        ClearAllCurrentChoices();

        showingChoices = instance.StartCoroutine(ShowingChoices(choices));
    }

    public static void Hide()
    {
        if (isShowingChoices)
        {
            instance.StopCoroutine(showingChoices);
        }
        showingChoices = null;

        header.Hide();

        ClearAllCurrentChoices();

        instance.root.SetActive(false);
    }

    static void ClearAllCurrentChoices()
    {
        foreach(ChoiceButton b in choices)
        {
            DestroyImmediate(b.gameObject);
        }
        choices.Clear();
    }

    public static bool isWaitingForChoiceToBeMade { get { return isShowingChoices && !lastChoiceMade.hasBeenMade; } }
    public static bool isShowingChoices { get { return showingChoices != null; } }
    static Coroutine showingChoices = null;
    public static IEnumerator ShowingChoices(string[] choices)
    {
        yield return new WaitForEndOfFrame();
        lastChoiceMade.Reset();

        while(header.isRevealing)
        {
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < choices.Length; i++)
        {
            CreateChoice(choices[i]);
        }

        SetLayoutSpacing();

        while(isWaitingForChoiceToBeMade)
        {
            yield return new WaitForEndOfFrame();
        }

        Hide();
    }

    static void SetLayoutSpacing()
    {
        int i = choices.Count;
        if (i <= 3)
        {
            instance.layoutGroup.spacing = 20;
        }
        else if (i >= 6)
        {
            instance.layoutGroup.spacing = 1;
        }
        else
        {
            switch(i)
            {
                case 4:
                    instance.layoutGroup.spacing = 15;
                    break;
                case 5:
                    instance.layoutGroup.spacing = 10;
                    break;

            }
        }
    }

    static void CreateChoice(string choice)
    {
        GameObject ob = Instantiate(instance.choiceprefab.gameObject, instance.buttonContainer);
        ob.SetActive(true);

        ChoiceButton b = ob.GetComponent<ChoiceButton>();

        b.text = choice;
        b.choiceIndex = choices.Count;

        //Button button = ob.GetComponent<Button>();

        ////This currently sets the OnClick event during runtime (which the inspector does not reflect). Currently, the system utilizes a prefab in the scene.
        ////But we can utilize this system if we start getting weird artificating or other issues. 
        //button.onClick.AddListener(delegate { instance.MakeChoice(b); });

        choices.Add(b);
    }

    [System.Serializable]
    public class Choice
    {
        public bool hasBeenMade { get { return title != "" && index != -1; } }
        public string title = "";
        public int index = -1;

        public void Reset()
        {
            title = "";
            index = -1;
        }

    }
    public Choice choice = new Choice();
    public static Choice lastChoiceMade { get { return instance.choice; } }

    public void MakeChoice(ChoiceButton button)
    {
        choice.index = button.choiceIndex;
        choice.title = button.text;
    }

}
