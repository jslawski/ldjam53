using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MouthSettings currentMouthSettings;

    private MouseCommand currentMouseCommand;
    private KeyboardCommand currentKeyboardCommand;
    private VowelCommand currentVowelCommand;

    private Queue<KeyCode> keyCache;

    // Start is called before the first frame update
    void Start()
    {
        this.keyCache = new Queue<KeyCode>();
    }

    // Update is called once per frame
    void Update()
    {
        this.HandleInput();
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
        Vector3 viewportMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        return new MouseCommand(this.currentMouthSettings, viewportMousePosition.x, viewportMousePosition.y);        
    }

    private VowelCommand HandleScrollWheelInput()
    {
        return new VowelCommand(this.currentMouthSettings, Input.mouseScrollDelta.y);        
    }

    private KeyboardCommand HandleKeyboardInput()
    {
        bool spacePressed = Input.GetKey(KeyCode.Space);
        string consonantKey = "";

        this.UpdateKeyCache();

        if (this.keyCache.Contains(KeyCode.A))
        {
            consonantKey += "A";
        }
        if (this.keyCache.Contains(KeyCode.W))
        {
            consonantKey += "W";
        }
        if (this.keyCache.Contains(KeyCode.S))
        {
            consonantKey += "S";
        }
        if (this.keyCache.Contains(KeyCode.D))
        {
            consonantKey += "D";
        }

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
}
