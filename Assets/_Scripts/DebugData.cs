using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugData : MonoBehaviour
{
    public MouthSettings currentMouthSettings;
    
    private MouseCommand currentMouseCommand;
    private KeyboardCommand currentKeyboardCommand;
    private VowelCommand currentVowelCommand;

    private Queue<KeyCode> keyCache;

    private TextMeshProUGUI debugText;

    private void Awake()
    {
        MouthSounds.Setup("Andrew");
    }

    // Start is called before the first frame update
    void Start()
    {
        this.debugText = GetComponentInChildren<TextMeshProUGUI>();
        this.keyCache = new Queue<KeyCode>();
        this.currentMouthSettings = new MouthSettings();
    }
    
    // Update is called once per frame
    void Update()
    {
        this.debugText.text = "";
        this.HandleInput();
    }

    private void HandleInput()
    {
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
        Vector3 viewportMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        this.debugText.text += "Mouse Pos: (" + viewportMousePosition.x + ", " + viewportMousePosition.y + ")\n";

        return new MouseCommand(this.currentMouthSettings, viewportMousePosition.x, viewportMousePosition.y);
    }

    private VowelCommand HandleScrollWheelInput()
    {
        this.debugText.text += "Vowel: " + this.currentMouthSettings.vowelKey + "\n";

        return new VowelCommand(this.currentMouthSettings, Input.mouseScrollDelta.y);
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

        this.debugText.text += "Consonant Key: " + consonantKey + "\nSpace: " + spacePressed.ToString();

        return new KeyboardCommand(this.currentMouthSettings, consonantKey, spacePressed);
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
}
