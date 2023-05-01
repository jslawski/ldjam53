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
        this.gameObject.SetActive(true);

        this.eyes.SetActive(false);
        this.mouth.gameObject.SetActive(true);
        this.ChangeLipMaterial(true);
    }

    public void EnableGameplayMode()
    {
        this.gameObject.SetActive(true);

        this.eyes.SetActive(true);
        this.mouth.gameObject.SetActive(true);


        this.ChangeLipMaterial(false);
    }

    private void ChangeLipMaterial(bool isCutscene)
    {
        Material[] mats = this.mouth.materials;

        if (isCutscene == true)
        {
            mats[3] = this.cutsceneMouth;
        }
        else
        {
            mats[3] = this.gameplayMouth;
        }

        this.mouth.materials = mats;
    }

    public void HideFace()
    {
        this.gameObject.SetActive(false);
        this.eyes.SetActive(false);
        this.mouth.gameObject.SetActive(false);
    }

    public void OrientFace(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        this.gameObject.transform.position = position;
        this.gameObject.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        this.gameObject.transform.localScale = scale;
    }

    public Transform GetFaceTransform()
    {
        return this.gameObject.transform;
    }
}
