using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    public static FaceController instance;

    [SerializeField]
    private SkinnedMeshRenderer mouth;

    private Coroutine mouthSizeCoroutine = null;

    private float minMouthSize = 0.6f;
    private float maxMouthSize = 1.2f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void UpdateMouthShapeConsonant(string consonantKey, bool pushingAir, float volume)
    {
        if (pushingAir == false)
        {
            if (consonantKey == "")
            {
                this.ResetMouth();
                return;
            }
        }

        switch (consonantKey)
        {
            case "A":
                this.ActivateMouthShape(12, volume);
                break;
            case "AW":
                this.ActivateMouthShape(9, volume);
                break;
            case "AS":
                this.ActivateMouthShape(0, volume);
                break;
            case "W":
                this.ActivateMouthShape(8, volume);
                break;
            case "WS":
                this.ActivateMouthShape(3, volume);
                break;
            case "SD":
                this.ActivateMouthShape(10, volume);
                break;
            case "S":
                this.ActivateMouthShape(7, volume);
                break;
            case "WD":
                this.ActivateMouthShape(2, volume);
                break;
            case "AD":
                this.ActivateMouthShape(5, volume);
                break;
            case "D":
                this.ActivateMouthShape(1, volume);
                break;
            default:
                this.ResetMouth();
                break;
        }
    }

    public void UpdateMouthShapeVowel(string vowelKey, float volume)
    {
        switch (vowelKey)
        {
            case "A":
            case "AA":
            case "E":
            case "EE":
            case "I":
            case "II":
                this.ActivateMouthShape(4, volume);
                break;         
            case "O":
            case "U":
                this.ActivateMouthShape(6, volume);
                break;
            case "OO":
            case "UU":
                this.ActivateMouthShape(11, volume);
                break;
            default:
                this.ResetMouth();
                break;
        }
    }

    private void ResetMouth()
    {
        this.StopAllCoroutines();

        if (this.mouth.gameObject.transform.localScale.x != 1.0f)
        {
            StartCoroutine(this.ReturnToDefaultSize());
        }
        
        for (int i = 0; i < 13; i++)
        {
            this.LerpToBlendShapeWeight(i, 0);
        }
    }

    private void ActivateMouthShape(int shapeIndex, float volume)
    {
        this.StopAllCoroutines();

        for (int i = 0; i < 13; i++)
        {
            if (i == shapeIndex)
            {
                this.LerpToBlendShapeWeight(i, 100);
            }
            else
            {
                this.LerpToBlendShapeWeight(i, 0);
            }
        }
    }

    private void LerpToBlendShapeWeight(int shapeIndex, float targetWeight)
    {
        if (this.mouth.GetBlendShapeWeight(shapeIndex) == targetWeight)
        {
            return;
        }

        StartCoroutine(this.LerpBlendShape(shapeIndex, targetWeight));
    }

    private IEnumerator LerpBlendShape(int shapeIndex, float targetWeight)
    {
        float lerpDuration = 0.2f;

        float totalWeightChange = targetWeight - this.mouth.GetBlendShapeWeight(shapeIndex);

        float weightChangePerFrame = (totalWeightChange / lerpDuration) * Time.fixedDeltaTime;

        while (this.mouth.GetBlendShapeWeight(shapeIndex) != targetWeight)
        {
            float newWeight = this.mouth.GetBlendShapeWeight(shapeIndex) + weightChangePerFrame;
            this.mouth.SetBlendShapeWeight(shapeIndex, newWeight);

            if (Mathf.Abs(targetWeight - this.mouth.GetBlendShapeWeight(shapeIndex)) <= 3)
            {
                this.mouth.SetBlendShapeWeight(shapeIndex, targetWeight);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void UpdateMouthSize(float volume)
    {
        float volumePercentage = volume / 0.75f;
        float targetMouthSize = Mathf.Lerp(this.minMouthSize, this.maxMouthSize, volumePercentage);

        if (this.mouthSizeCoroutine == null)
        {
            this.mouthSizeCoroutine = StartCoroutine(this.LerpMouthSize(volume));
        }
        else
        {
            this.mouth.gameObject.transform.localScale = new Vector3(targetMouthSize, targetMouthSize, targetMouthSize);
        }
    }
    
    private IEnumerator LerpMouthSize(float volume)
    {
        float volumePercentage = volume / 0.75f;
        float targetMouthSize = Mathf.Lerp(this.minMouthSize, this.maxMouthSize, volumePercentage);

        float lerpDuration = 0.2f;
        float totalSizeChange = targetMouthSize - this.mouth.gameObject.transform.localScale.x;
        float sizeChangePerFrame = (totalSizeChange / lerpDuration) * Time.fixedDeltaTime;

        while (this.mouth.gameObject.transform.localScale.x != targetMouthSize)
        {
            float newSize = this.mouth.gameObject.transform.localScale.x + sizeChangePerFrame;
            this.mouth.gameObject.transform.localScale = new Vector3(newSize, newSize, newSize);

            if (Mathf.Abs(targetMouthSize - newSize) <= 0.001f)
            {
                this.mouth.gameObject.transform.localScale = new Vector3(targetMouthSize, targetMouthSize, targetMouthSize);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void ResetMouthSize()
    {
        StartCoroutine(ReturnToDefaultSize());
    }

    public IEnumerator ReturnToDefaultSize()
    {
        if (this.mouthSizeCoroutine != null)
        {
            StopCoroutine(this.mouthSizeCoroutine);
            this.mouthSizeCoroutine = null;
        }

        float targetMouthSize = 1.0f;

        float lerpDuration = 0.2f;
        float totalSizeChange = targetMouthSize - this.mouth.gameObject.transform.localScale.x;
        float sizeChangePerFrame = (totalSizeChange / lerpDuration) * Time.fixedDeltaTime;

        while (this.mouth.gameObject.transform.localScale.x != targetMouthSize)
        {
            float newSize = this.mouth.gameObject.transform.localScale.x + sizeChangePerFrame;
            this.mouth.gameObject.transform.localScale = new Vector3(newSize, newSize, newSize);

            if (Mathf.Abs(targetMouthSize - newSize) <= 0.001f)
            {
                this.mouth.gameObject.transform.localScale = new Vector3(targetMouthSize, targetMouthSize, targetMouthSize);
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
