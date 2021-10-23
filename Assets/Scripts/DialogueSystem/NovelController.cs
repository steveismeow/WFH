using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Manages the txt file and interprets actions and inputs
/// </summary>
public class NovelController : MonoBehaviour
{
    public static NovelController instance;

    List<string> data = new List<string>();

    void Awake()
    {
        instance = this;
    }


    string activeChapterFile = "";


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Next();
        }

    }

    public void LoadChapterFile(string fileName)
    {
        activeChapterFile = fileName;
        data = FileManager.LoadFile(FileManager.savePath + "Resources/Story/" + fileName);
        cachedLastSpeaker = "";

        if (handlingChapterFile != null)
        {
            StopCoroutine(handlingChapterFile);
        }

        handlingChapterFile = StartCoroutine(HandlingChapterFile());

        Next();
    }

    bool _next = false;
    public void Next()
    {
        _next = true;
    }

    public bool isHandlingChapterFile { get { return handlingChapterFile != null; } }
    Coroutine handlingChapterFile = null;
    private int chapterProgress = 0;
    IEnumerator HandlingChapterFile()
    {
        print("Handling text file!");
        chapterProgress = 0;

        while (chapterProgress < data.Count)
        {
            if (_next)
            {
                string line = data[chapterProgress];

                if (line.StartsWith("choice"))
                {
                    yield return HandlingChoiceLine(line);
                    chapterProgress++;
                }
                else
                {
                    HandleLine(data[chapterProgress]);
                    chapterProgress++;
                    while (isHandlingLine)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }

            }

            yield return new WaitForEndOfFrame();

        }

        handlingChapterFile = null;
    }
    IEnumerator HandlingChoiceLine(string line)
    {
        string title = line.Split('"')[1];
        List<string> choices = new List<string>();
        List<string> actions = new List<string>();
        print("Handle Choice line:" + title);

        while (true)
        {
            chapterProgress++;
            line = data[chapterProgress];
            if (line == "{")
            {
                continue;
            }

            if (line != "}")
            {
                choices.Add(line.Split('"')[1]);
                actions.Add(data[chapterProgress + 1]);
                chapterProgress++;

            }
            else
            {
                break;
            }

        }

        if (choices.Count > 0)
        {
            ChoiceScreen.Show(title, choices.ToArray());
            yield return new WaitForEndOfFrame();

            while (ChoiceScreen.isWaitingForChoiceToBeMade)
            {
                yield return new WaitForEndOfFrame();
            }

            string action = actions[ChoiceScreen.lastChoiceMade.index];
            HandleLine(action);

            while (isHandlingLine)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            Debug.Log("List[Choices] is empty. No choices were found");
        }

    }

    void HandleLine(string rawLine)
    {
        ChapterLineManager.Line line = ChapterLineManager.Interpret(rawLine);

        StopHandlingLine();
        handlingLine = StartCoroutine(HandlingLine(line));
    }

    void StopHandlingLine()
    {
        if (isHandlingLine)
        {
            StopCoroutine(handlingLine);
        }
        handlingLine = null;
    }

    public string cachedLastSpeaker = "";

    public bool isHandlingLine { get { return handlingLine != null; } }
    Coroutine handlingLine = null;
    IEnumerator HandlingLine(ChapterLineManager.Line line)
    {
        _next = false;
        int lineProgress = 0;

        while (lineProgress < line.segments.Count)
        {
            _next = false;
            ChapterLineManager.Line.Segment segment = line.segments[lineProgress];

            if (lineProgress > 0)
            {
                if (segment.trigger == ChapterLineManager.Line.Segment.Trigger.autoDelay)
                {
                    for (float timer = segment.autoDelay; timer >= 0; timer -= Time.deltaTime)
                    {
                        yield return new WaitForEndOfFrame();
                        if (_next)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    while (!_next)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
            }
            _next = false;

            segment.Run();

            while (segment.isRunning)
            {
                yield return new WaitForEndOfFrame();

                if (_next)
                {
                    if (!segment.architect.skip)
                    {
                        segment.architect.skip = true;
                    }
                    else
                    {
                        segment.ForceFinish();
                    }

                    _next = false;
                }
            }

            lineProgress++;

            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < line.actions.Count; i++)
        {
            HandleAction(line.actions[i]);
        }
        handlingLine = null;
    }

    //ACTIONS
    //////////////////////////////////////////////////////////////////////
    public void HandleAction(string action)
    {
        print("Handle action [" + action + "]");
        string[] data = action.Split('(', ')');
        switch (data[0])
        {
            case "wait":
                Command_Wait(data[1]);
                break;
            case "setBackground":
                Command_SetLayerImage(data[1], LayerController.instance.background);
                break;
            case "setForeground":
                Command_SetLayerImage(data[1], LayerController.instance.foreground);
                break;
            case "setCinematic":
                //Command_SetLayerImage(data[1], LayerController.instance.cinematic);
                Command_SetVideo(data[1], LayerController.instance.cinematic);
                break;
            case "clearFG":
                Command_ClearFGLayer(data[1], LayerController.instance.foreground);
                break;
            case "replacementGlitch":
                Command_ReplacementGlitch();
                break;
            case "plusPerformance":
                Command_PlusPerformance(data[1]);
                break;
            case "setTestBool":
                Command_SetTestBool(data[1]);
                break;
            case "setMeetingUp":
                Command_SetMeetingUp(data[1]);
                break;
            case "endMeeting":
                Command_EndMeeting();
                break;
            case "playAnim":
                Command_PlayAnimation(data[1]);
                break;
            case "playMusic":
                Command_PlayMusic(data[1]);
                break;
            case "playSound":
                Command_PlaySound(data[1]);
                break;
            case "Load":
                Command_Load(data[1]);
                break;
            case "next":
                Next();
                break;
            case "exitDialogue":
                ExitDialogue();
                break;
            case "setTimeTo":
                Command_SetTimeTo(data[1]);
                break;
            case "endDay":
                Command_EndDay();
                break;



        }
    }

    void Command_EndDay()
    {
        //Tell DayManager to play EndDay()
        DayManager.instance.EndDay();

        Command_SetLayerImage("black", LayerController.instance.foreground);
    }


    void Command_Wait(string data)
    {
        float waitTime = float.Parse(data);
        Coroutine waiting = StartCoroutine(Waiting(waitTime));
    }

    IEnumerator Waiting(float waitTIme)
    {
        DialogueManager.instance.Close();

        yield return new WaitForSeconds(waitTIme);

        Next();
    }

    void ExitDialogue()
    {
        DialogueManager.instance.Close();
    }

    void Command_Load(string chapterName)
    {
        //DialogueManager.instance.Close();
        NovelController.instance.LoadChapterFile(chapterName);

    }

    void Command_SetMeetingUp(string data)
    {
        print(data);

        string[] parameters = data.Split(',');

        print(parameters[1]);

        //This is doing a weird thing somehow
        GameObject character = MeetingManager.instance.IdentifyCharacter(parameters[0]);

        MeetingManager.instance.SetMeetingDetails(character, parameters[1]);

    }

    void Command_EndMeeting()
    {
        MeetingManager.instance.EndMeeting();
    }

    void Command_ReplacementGlitch()
    {
        MeetingManager.instance.ReplacementGlitch();
    }

    void Command_PlayAnimation(string animStateName)
    {
        MeetingManager.instance.PlayAnimation(animStateName);
    }

    void Command_PlusPerformance(string data)
    {
        int i =int.Parse(data);
        PlayerData.instance.performance += i;
    }

    void Command_SetTestBool(string data)
    {
        PlayerData.instance.testBool = bool.Parse(data);    
    }

    void Command_SetVideo(string data, LayerController.Layer layer)
    {
        string videoName = data.Contains(",") ? data.Split(',')[0] : data;
        VideoClip video = videoName == "null" ? null : Resources.Load("Video/" + videoName) as VideoClip;

        //May want to come back here and edit in the ability to modify transition speed
        layer.SetVideo(video, 1, false);

    }

    void Command_SetLayerImage(string data, LayerController.Layer layer)
    {
        string textureName = data.Contains(",") ? data.Split(',')[0] : data;
        Texture2D texture = textureName == "null" ? null : Resources.Load("Images/" + textureName) as Texture2D;
        float speed = 2f;
        bool smooth = true;

        //CURRENT CHECK IS ONLY FOR TRANSITION SPEED, ADDITIONAL BOOLS AND VARIABLES MAY BE INTEGRATED AS NECESSARY
        if (data.Contains(","))
        {
            //PARSING MECHANISM SHOWN BELOW, HOLD ONTO IN CASE OF EMERGENCY
            string[] parameters = data.Split(',');
            foreach (string p in parameters)
            {
                float fVal = 0;
                bool bVal = true;

                if (float.TryParse(p, out fVal))
                {
                    speed = fVal;
                    continue;
                }
                if (bool.TryParse(p, out bVal))
                {
                    smooth = bVal;
                    continue;
                }
            }
        }
        layer.SetTexture(texture, speed);
    }

    void Command_ClearFGLayer(string data, LayerController.Layer layer)
    {
        float transitionSpeed = float.Parse(data);
        layer.ClearLayer(transitionSpeed);
    }

    void Command_PlayMusic(string data)
    {
        AudioClip clip = Resources.Load("Audio/Music/" + data) as AudioClip;

        AudioManager.instance.PlayMusic(clip);
    }

    void Command_PlaySound(string data)
    {
        AudioClip clip = Resources.Load("Audio/Sounds/" + data) as AudioClip;

        AudioManager.instance.PlaySFX(clip);

    }

    void Command_SetTimeTo(string time_data)
    {
        // include time switch statements here -- check to see if strings match specific time values
        // make a call to the Clock class's SetTimeTo function
        Clock.instance.SetTimeTo(time_data);
    }


}
