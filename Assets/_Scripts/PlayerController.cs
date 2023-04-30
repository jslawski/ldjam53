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

    [SerializeField]
    private TextMeshProUGUI debugText;

    [SerializeField, Range(0.0f, 1.0f)]
    private static float playspaceClampTop = 0.8f;
    [SerializeField, Range(0.0f, 1.0f)]
    private static float playspaceClampBottom = 0.2f;
    [SerializeField, Range(0.0f, 1.0f)]
    private static float playspaceClampLeft = 0.3f;
    [SerializeField, Range(0.0f, 1.0f)]
    private static float playspaceClampRight = 0.7f;

    private RecordedWord testWord;

    private void Awake()
    {
        MouthSounds.Setup("Andrew");
    }

    // Start is called before the first frame update
    void Start()
    {
        this.keyCache = new Queue<KeyCode>();
        this.currentMouthSettings = new MouthSettings();
        this.testWord = new RecordedWord();
    }

    // Update is called once per frame
    void Update()
    {
        if (AudioPlayback.instance.isPlayingBack == true)
        {
            return;
        }

        this.debugText.text = "";
        this.HandleInput();

        this.UpdateDebugText();

        AudioPlayback.instance.PlayAudio(this.currentMouthSettings);

        if (this.currentMouthSettings.pushingAir == true)
        {
            this.testWord.SaveFrameSettings(new MouthSettings(this.currentMouthSettings));    
        }         
    }

    private void HandleInput()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            this.testWord.Playback();
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            this.debugText.enabled = !this.debugText.enabled;
        }

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

    private void UpdateDebugText()
    {
        this.debugText.text += "Mouse Pos: (" + this.currentMouthSettings.volume + ", " + this.currentMouthSettings.pitch + ")\n";
        this.debugText.text += "Vowel: " + this.currentMouthSettings.vowelKey + "\n";
        this.debugText.text += "Consonant Key: " + this.currentMouthSettings.consonantKey + "\nSpace: " + this.currentMouthSettings.pushingAir;
    }
}