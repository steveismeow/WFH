using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextHeader))]
public class InputScreen : MonoBehaviour
{
    public static InputScreen instance;

    public TMP_InputField inputField;
    public static string currentInput { get { return instance.inputField.text; } }

    public TextHeader header;

    public GameObject root;


    private void Awake()
    {
        instance = this;
        Hide();
    }
    // Start is called before the first frame update
    public static void Show(string title, bool clearCurrentInput = true)
    {
        instance.root.SetActive(true);

        if (clearCurrentInput)
        {
            instance.inputField.text = "";
        }

        if (title != "") 
        {
            instance.header.Show(title);
        }
        else
        {
            instance.header.Hide();
        }

        if(isRevealing)
        {
            instance.StopCoroutine(revealing);
        }
        revealing = instance.StartCoroutine(Revealing());
    }

    public static void Hide()
    {
        instance.root.SetActive(false);
        instance.header.Hide();
    }

    public static bool isWaitingForUserInput { get { return instance.root.activeInHierarchy; } }
    public static bool isRevealing { get { return revealing != null; } }
    static Coroutine revealing = null;
    static IEnumerator Revealing()
    {
        instance.inputField.gameObject.SetActive(false);

        while(instance.header.isRevealing)
        {
            yield return new WaitForEndOfFrame();
        }

        instance.inputField.gameObject.SetActive(true);

        revealing = null;
    }

    public void Accept()
    {
        Hide();
    }
}
