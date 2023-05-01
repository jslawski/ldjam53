using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFaceMode : MonoBehaviour
{
    [SerializeField]
    private GameObject eyes;
    [SerializeField]
    public SkinnedMeshRenderer mouth;

    [SerializeField]
    private Material cutsceneMouth;
    [SerializeField]
    private Material gameplayMouth;

    public void EnableCutsceneMode()
    {
        this.eyes.SetActive(false);
        this.mouth.materials[3] = cutsceneMouth;
    }

    public void EnableGameplayMode()
    {
        this.eyes.SetActive(true);
        this.mouth.materials[3] = gameplayMouth;
    }

    public void OrientFaceForPlayback(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        this.gameObject.transform.position = position;
        this.gameObject.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        this.gameObject.transform.localScale = scale;
    }
}
