using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public MouthSettings currentMouthSettings;

    private MouseCommand currentMouseCommand;
    private KeyboardCommand currentKeyboardCommand;
    private VowelCommand currentVowelCommand;

    private Queue<KeyCode> keyCache;

    [SerializeField, Range(0.0f, 1.0f)]
    private static float playspaceClampTop = 0.8f;
    [SerializeField, Range(0.0f, 1.0f)]
    private static float playspaceClampBottom = 0.2f;
    [SerializeField, Range(0.0f, 1.0f)]
    private static float playspaceClampLeft = 0.1f;
    [SerializeField, Range(0.0f, 1.0f)]
    private static float playspaceClampRight = 0.9f;

    private bool syllableRecorded = false;

    // Start is called before the first frame update
    void Start()
    {
        this.keyCache = new Queue<KeyCode>();
        this.currentMouthSettings = new MouthSettings();
    }

    public void ActivateGameplay()
    {
        StartCoroutine(this.UpdateCoroutine());
        StartCoroutine(this.FixedUpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            Cursor.visible = false;

            this.HandleInput();

            AudioPlayback.instance.PlayAudio(this.currentMouthSettings);

            yield return null;
        }
    }

    private IEnumerator FixedUpdateCoroutine()
    {
        Sentence sceneSentence = FullScript.allScenes[GameSceneDirector.instance.currentSceneId].GetSentence();
        RecordedSentence currentRecordedSentence = new RecordedSentence();

        for (int i = 0; i < sceneSentence.sentenceWords.Count; i++)
        {
            int numSyllablesSaved = 0;
            RecordedWord currentRecordedWord = new RecordedWord();

            WordDisplay.instance.DisplayNewWord(sceneSentence.sentenceWords[i]);
            WordDisplay.instance.HighlightSyllable(numSyllablesSaved);

            while (numSyllablesSaved < sceneSentence.sentenceWords[i].syllables.Count)
            {
                currentRecordedWord.SaveFrameSettings(this.currentMouthSettings);

                if (this.syllableRecorded == true)
                {
                    this.syllableRecorded = false;
                    numSyllablesSaved++;
                    WordDisplay.instance.HighlightSyllable(numSyllablesSaved);
                }

                yield return new WaitForFixedUpdate();
            }

            currentRecordedSentence.SaveWord(new RecordedWord(currentRecordedWord));
        }

        FullScript.recordedSentences.Add(new RecordedSentence(currentRecordedSentence));

        AudioPlayback.instance.StopAudio();

        this.EndGameplay();
    }

    private void EndGameplay()
    {        
        WordDisplay.instance.ClearWord();

        StopAllCoroutines();
        GameSceneDirector.instance.EndGameplay();
    }

    private void HandleInput()
    {
        this.currentMouseCommand = this.HandleMouseInput();
        this.currentMouthSettings = this.currentMouseCommand.Execute();

        this.currentVowelCommand = this.HandleScrollWheelInput();
        this.currentMouthSettings = this.currentVowelCommand.Execute();

        this.currentKeyboardCommand = this.HandleKeyboardInput();
        this.currentMouthSettings = this.currentKeyboardCommand.Execute();
    }

    private MouseCommand HandleMouseInput()
    {
        Vector3 playspaceMousePosition = ViewportToPlayspaceMousePosition();

        return new MouseCommand(new MouthSettings(this.currentMouthSettings), playspaceMousePosition.x, playspaceMousePosition.y);
    }

    private VowelCommand HandleScrollWheelInput()
    {
        return new VowelCommand(new MouthSettings(this.currentMouthSettings), Input.mouseScrollDelta.y);
    }

    private KeyboardCommand HandleKeyboardInput()
    {
        bool spacePressed = Input.GetKey(KeyCode.Space);
        string consonantKey = "";

        this.UpdateKeyCache();

        if (Input.GetKey(KeyCode.A))
        {
            consonantKey += "A";
        }
        if (Input.GetKey(KeyCode.W))
        {
            consonantKey += "W";
        }
        if (Input.GetKey(KeyCode.S))
        {
            consonantKey += "S";
        }
        if (Input.GetKey(KeyCode.D))
        {
            consonantKey += "D";
        }

        if (consonantKey.Length > 2)
        {
            consonantKey = this.ReplaceKeyWithCache();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            this.syllableRecorded = true;
        }

        return new KeyboardCommand(new MouthSettings(this.currentMouthSettings), consonantKey, spacePressed);
    }

    private void UpdateKeyCache()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.keyCache.Enqueue(KeyCode.A);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            this.keyCache.Enqueue(KeyCode.W);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            this.keyCache.Enqueue(KeyCode.S);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            this.keyCache.Enqueue(KeyCode.D);
        }

        while (this.keyCache.Count > 2)
        {
            this.keyCache.Dequeue();
        }
    }

    private string ReplaceKeyWithCache()
    {
        string newKey = "";

        if (this.keyCache.Contains(KeyCode.A))
        {
            newKey += "A";
        }
        if (this.keyCache.Contains(KeyCode.W))
        {
            newKey += "W";
        }
        if (this.keyCache.Contains(KeyCode.S))
        {
            newKey += "S";
        }
        if (this.keyCache.Contains(KeyCode.D))
        {
            newKey += "D";
        }

        return newKey;
    }

    public static Vector3 ViewportToPlayspaceMousePosition()
    {
        Vector3 viewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float playspacePositionX = (viewportPos.x - playspaceClampLeft) / (playspaceClampRight - playspaceClampLeft);
        float playspacePositionY = (viewportPos.y - playspaceClampBottom) / (playspaceClampTop - playspaceClampBottom);

        if (playspacePositionX < 0)
        {
            playspacePositionX = 0.0f;
        }
        else if (playspacePositionX > 1.0f)
        {
            playspacePositionX = 1.0f;
        }

        if (playspacePositionY < 0)
        {
            playspacePositionY = 0.0f;
        }
        else if (playspacePositionY > 1.0f)
        {
            playspacePositionY = 1.0f;
        }

        return new Vector3(playspacePositionX, playspacePositionY, 0.0f);
    }
}
