using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the txt file and interprets actions and inputs
/// </summary>
public class NovelController : MonoBehaviour
{
    public static NovelController instance;

    List<string> data = new List<string>();

    //int progress = 0;

    void Awake()
    {
        instance = this;
    }

    //int activeGameFileNumber = 0;
    //GameFile activeGameFile = null;
    string activeChapterFile = "";

    // Start is called before the first frame update
    void Start()
    {
        //LoadGameFile(0);

        //InitializeDialogue(0);

        //LoadChapterFile("Scene0");

    }

    //public void InitializeDialogue(int sceneNumber)
    //{
    //    data = FileManager.LoadFile(FileManager.savePath + "Resources/Story/Scene" + sceneNumber);// + ".txt");

    //    print(data[0]);


    //    if (handlingChapterFile != null)
    //    {
    //        StopCoroutine(handlingChapterFile);
    //    }

    //    handlingChapterFile = StartCoroutine(HandlingChapterFile());



    //}

    //public void LoadGameFile(int gameFileNumber)
    //{
    //    activeGameFileNumber = gameFileNumber;

    //    string filePath = FileManager.savePath + "userData/gameFiles/" + gameFileNumber.ToString() + ".txt";

    //    if (!System.IO.File.Exists(filePath))
    //    {
    //        FileManager.SaveJSON(filePath, new GameFile());
    //    }

    //    activeGameFile = FileManager.LoadJSON<GameFile>(filePath);

    //    data = FileManager.LoadFile(FileManager.savePath + "Resources/Story/" + activeGameFile.chapterName);
    //    activeChapterFile = activeGameFile.chapterName;
    //    cachedLastSpeaker = activeGameFile.cachedLastSpeaker;

    //    DialogueManager.instance.Open(activeGameFile.currentTextSystemSpeakerDisplayText, activeGameFile.currentTextSystemDisplayText);
    //    //Load the characters
    //    //for (int i = 0; i < activeGameFile.charactersInScene.Count; i++)
    //    //{
    //    //    GameFile.CharacterData data = activeGameFile.charactersInScene[i];
    //    //    Character character = CharacterManager.instance.CreateCharacter(data.characterName, data.enabled);
    //    //    character.ChangeExpression(data.emotion);
    //    //    if (data.facingLeft)
    //    //    {
    //    //        character.FaceLeft();
    //    //    }
    //    //    else
    //    //    {
    //    //        character.FaceRight();
    //    //    }
    //    //    character.SetPosition(data.position);
    //    //}

    //    //Load the layr images
    //    if (activeGameFile.background != null)
    //    {
    //        LayerController.instance.background.SetTexture(activeGameFile.background as Texture2D);
    //    }
    //    if (activeGameFile.foreground != null)
    //    {
    //        LayerController.instance.foreground.SetTexture(activeGameFile.foreground as Texture2D);
    //    }
    //    if (activeGameFile.cinematic != null)
    //    {
    //        LayerController.instance.cinematic.SetTexture(activeGameFile.cinematic as Texture2D);
    //    }

    //    //Load music
    //    if (activeGameFile.music != null)
    //    {
    //        AudioManager.instance.PlayMusic(activeGameFile.music);
    //    }


    //    if (handlingChapterFile != null)
    //    {
    //        StopCoroutine(handlingChapterFile);
    //    }

    //    handlingChapterFile = StartCoroutine(HandlingChapterFile());

    //    chapterProgress = activeGameFile.chapterProgress;
    //}

    //public void SaveGameFile()
    //{
    //    string filePath = FileManager.savePath + "userData/gameFiles/" + activeGameFileNumber.ToString() + ".txt";

    //    activeGameFile.chapterName = activeChapterFile;
    //    activeGameFile.chapterProgress = chapterProgress;
    //    activeGameFile.cachedLastSpeaker = cachedLastSpeaker;

    //    activeGameFile.currentTextSystemSpeakerDisplayText = DialogueManager.instance.speakerNameText.text;
    //    activeGameFile.currentTextSystemDisplayText = DialogueManager.instance.dialogueText.text;

    //    //Save characters
    //    //activeGameFile.charactersInScene.Clear();
    //    //for (int i = 0; i < CharacterManager.instance.characters.Count; i++)
    //    //{
    //    //    Character character = CharacterManager.instance.characters[i];
    //    //    GameFile.CharacterData data = new GameFile.CharacterData(character);
    //    //    activeGameFile.charactersInScene.Add(data);
    //    //}

    //    //Save layer images
    //    LayerController l = LayerController.instance;
    //    activeGameFile.background = l.background.currentGraphic != null ? l.background.currentGraphic.renderer.texture : null;
    //    activeGameFile.foreground = l.foreground.currentGraphic != null ? l.foreground.currentGraphic.renderer.texture : null;
    //    activeGameFile.cinematic = l.cinematic.currentGraphic != null ? l.cinematic.currentGraphic.renderer.texture : null;


    //    //Save music
    //    activeGameFile.music = AudioManager.activeMusic != null ? AudioManager.activeMusic.clip : null;

    //    FileManager.SaveJSON(filePath, activeGameFile);
    //}



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Next();
        }
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    SaveGameFile();
        //}
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

            //case "enter":
            //    Command_Enter(data[1]);
            //    break;
            //case "exit":
            //    Command_Exit(data[1]);
            //    break;
            case "setBackground":
                Command_SetLayerImage(data[1], LayerController.instance.background);
                break;
            case "setForeground":
                Command_SetLayerImage(data[1], LayerController.instance.foreground);
                break;
            case "setCinematic":
                Command_SetLayerImage(data[1], LayerController.instance.cinematic);
                break;
            case "clearFG":
                Command_ClearFGLayer(data[1], LayerController.instance.foreground);
                break;
            case "plusPerformance":
                Command_PlusPerformance(data[1]);
                break;
            case "setTestBool":
                Command_SetTestBool(data[1]);
                break;

            //case "setCharPosition":
            //    Command_SetCharPosition(data[1]);//(name, position)
            //    break;
            //case "move":
            //    Command_MoveCharacter(data[1]);//(name, position, speed, smooth)
            //    break;
            //case "flip":
            //    Command_FlipSprite(data[1]);
            //    break;
            //case "faceLeft":
            //    Command_FaceLeft(data[1]);
            //    break;
            //case "faceRight":
            //    Command_FaceRight(data[1]);
            //    break;
            //case "setExpression":
            //    Command_SetExpression(data[1]);
            //    break;
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


        }
    }

    //void Command_Enter(string data)
    //{
    //    string[] parameters = data.Split(',');
    //    string characterName = parameters[0];
    //    string emotion = parameters[1];
    //    float duration = parameters.Length == 4 ? float.Parse(parameters[3]) : 2f;

    //    //Character character = CharacterManager.instance.GetCharacter(characterName, true, false);
    //    //character.enabled = true;
    //    //character.cg.alpha = 0f;

    //    //Vector2 target = new Vector2(0, 0);
    //    //if (parameters.Length >= 3)
    //    //{
    //    //    switch (parameters[2])
    //    //    {
    //    //        case "left":
    //    //            target = CharacterManager.characterPositions.left;
    //    //            break;
    //    //        case "slightleft":
    //    //            target = CharacterManager.characterPositions.slightleft;
    //    //            break;
    //    //        case "center":
    //    //            target = CharacterManager.characterPositions.center;
    //    //            break;
    //    //        case "slightright":
    //    //            target = CharacterManager.characterPositions.slightright;
    //    //            break;
    //    //        case "right":
    //    //            target = CharacterManager.characterPositions.right;
    //    //            break;
    //    //    }
    //    //}
    //    //else
    //    //{
    //    //    target = CharacterManager.characterPositions.center;
    //    //}
    //    //character.SetPosition(target);


    //    //character.FadeIn(emotion, duration);


    //}
    //void Command_Exit(string data)
    //{
    //    string[] parameters = data.Split(',');
    //    string[] characters = parameters[0].Split(';');
    //    float duration = parameters.Length == 2 ? float.Parse(parameters[1]) : 2f;

    //    //foreach (string s in characters)
    //    //{
    //    //    Character character = CharacterManager.instance.GetCharacter(s);
    //    //    character.FadeOut(duration);
    //    //    character.cg.alpha = 0f;
    //    //    character.enabled = true;

    //    //}


    //}
    //void Command_SetCharPosition(string data)
    //{
    //    if (data.Contains(","))
    //    {
    //        string[] parameters = data.Split(',');

    //        Character character = CharacterManager.instance.GetCharacter(parameters[0]);

    //        Vector2 target = new Vector2(0, 0);

    //        if (parameters[1] == "left")
    //        {
    //            target = CharacterManager.characterPositions.left;
    //        }
    //        if (parameters[1] == "slightleft")
    //        {
    //            target = CharacterManager.characterPositions.slightleft;
    //        }
    //        if (parameters[1] == "center")
    //        {
    //            target = CharacterManager.characterPositions.center;
    //        }
    //        if (parameters[1] == "slightright")
    //        {
    //            target = CharacterManager.characterPositions.slightright;
    //        }
    //        if (parameters[1] == "right")
    //        {
    //            target = CharacterManager.characterPositions.right;
    //        }
    //        print(target);

    //        character.SetPosition(target);
    //    }


    //}

    //void Command_MoveCharacter(string data)
    //{
    //    if (data.Contains(","))
    //    {
    //        string[] parameters = data.Split(',');

    //        Character character = CharacterManager.instance.GetCharacter(parameters[0]);
    //        Vector2 target = new Vector2(0, 0);

    //        if (parameters[1] == "left")
    //        {
    //            target = CharacterManager.characterPositions.left;
    //        }
    //        if (parameters[1] == "slightleft")
    //        {
    //            target = CharacterManager.characterPositions.slightleft;
    //        }
    //        if (parameters[1] == "center")
    //        {
    //            target = CharacterManager.characterPositions.center;
    //        }
    //        if (parameters[1] == "slightright")
    //        {
    //            target = CharacterManager.characterPositions.slightright;
    //        }
    //        if (parameters[1] == "right")
    //        {
    //            target = CharacterManager.characterPositions.right;
    //        }

    //        float speed = parameters.Length >= 3 ? float.Parse(parameters[2]) : 1f;
    //        bool smooth = parameters.Length == 4 ? bool.Parse(parameters[3]) : true;

    //        character.Move(target, speed, smooth);
    //    }

    //}

    //void Command_SetExpression(string data)
    //{
    //    string[] parameters = data.Split(',');
    //    string characterName = parameters[0];
    //    string expression = parameters[1];
    //    float speed = parameters.Length >= 3 ? float.Parse(parameters[3]) : 10f;
    //    bool smooth = parameters.Length == 4 ? bool.Parse(parameters[3]) : true;

    //    Character character = CharacterManager.instance.GetCharacter(characterName);
    //    Sprite sprite = character.GetSprite(expression);
    //    character.TransitionExpression(sprite, speed, smooth);
    //}

    //void Command_FlipSprite(string data)
    //{
    //    string[] characters = data.Split(',');

    //    foreach (string s in characters)
    //    {
    //        Character character = CharacterManager.instance.GetCharacter(s);

    //        character.FlipSprite();

    //    }
    //}

    //void Command_FaceLeft(string data)
    //{
    //    string[] characters = data.Split(',');

    //    foreach (string s in characters)
    //    {
    //        Character character = CharacterManager.instance.GetCharacter(s);

    //        character.FaceLeft();

    //    }
    //}

    //void Command_FaceRight(string data)
    //{
    //    string[] characters = data.Split(',');

    //    foreach (string s in characters)
    //    {
    //        Character character = CharacterManager.instance.GetCharacter(s);

    //        character.FaceRight();

    //    }
    //}

    //void Command_ShakeSprite(string data)
    //{
    //    string[] parameters = data.Split(',');
    //    string characterName = parameters[0];
    //    float time = parameters[1];
    //    Character character = CharacterManager.instance.GetCharacter(characterName);
    //    character.Shake(time);
    //}

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

    void Command_PlusPerformance(string data)
    {
        int i =int.Parse(data);
        PlayerData.instance.performance += i;
    }

    void Command_SetTestBool(string data)
    {
        PlayerData.instance.testBool = bool.Parse(data);    }

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
}
