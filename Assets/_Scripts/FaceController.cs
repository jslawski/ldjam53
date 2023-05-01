using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    public static FaceController instance;

    [SerializeField]
    private SkinnedMeshRenderer mouth;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void UpdateMouthShapeConsonant(string consonantKey, bool pushingAir)
    {
        if (consonantKey == "" && pushingAir == false)
        {
            this.ResetMouth();
            return;
        }

        switch (consonantKey)
        {
            case "A":
                this.ActivateMouthShape(12);
                break;
            case "AW":
                this.ActivateMouthShape(9);
                break;
            case "AS":
                this.ActivateMouthShape(0);
                break;
            case "W":
                this.ActivateMouthShape(8);
                break;
            case "WS":
                this.ActivateMouthShape(3);
                break;
            case "SD":
                this.ActivateMouthShape(10);
                break;
            case "S":
                this.ActivateMouthShape(7);
                break;
            case "WD":
                this.ActivateMouthShape(2);
                break;
            case "AD":
                this.ActivateMouthShape(5);
                break;
            case "D":
                this.ActivateMouthShape(1);
                break;
            default:
                this.ResetMouth();
                break;
        }
    }

    public void UpdateMouthShapeVowel(string vowelKey)
    {
        switch (vowelKey)
        {
            case "A":
            case "AA":
            case "E":
            case "EE":
            case "I":
            case "II":
                this.ActivateMouthShape(4);
                break;         
            case "O":
            case "U":
                this.ActivateMouthShape(6);
                break;
            case "OO":
            case "UU":
                this.ActivateMouthShape(11);
                break;
            default:
                this.ResetMouth();
                break;
        }
    }

    private void ResetMouth()
    {
        this.StopAllCoroutines();

        for (int i = 0; i < 13; i++)
        {
            this.LerpToBlendShapeWeight(i, 0);
        }
    }

    private void ActivateMouthShape(int shapeIndex)
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
        float lerpDuration = 0.1f;

        float totalWeightChange = targetWeight - this.mouth.GetBlendShapeWeight(shapeIndex);

        float weightChangePerFrame = (totalWeightChange / lerpDuration) * Time.fixedDeltaTime;

        Debug.LogError("Weight Change per Frame: " + weightChangePerFrame);

        while (this.mouth.GetBlendShapeWeight(shapeIndex) != targetWeight)
        {
            float newWeight = this.mouth.GetBlendShapeWeight(shapeIndex) + weightChangePerFrame;
            this.mouth.SetBlendShapeWeight(shapeIndex, newWeight);

            Debug.LogError("New Weight: " + newWeight);

            if (Mathf.Abs(targetWeight - this.mouth.GetBlendShapeWeight(shapeIndex)) <= 3)
            {
                this.mouth.SetBlendShapeWeight(shapeIndex, targetWeight);
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
