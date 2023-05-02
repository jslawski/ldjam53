using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroatBulge : MonoBehaviour
{
    [SerializeField]
    private LineRenderer throat;

    private float viewportPositionX = 0.342f;
    private float viewportPositionYMin = 0.175f;
    private float viewportPositionYMax = 0.826f;

    private AnimationCurve bulge;
    
    private float defaultWidth = 2.5f;
    private float maxBulgeWidth = 8.0f;
    private float bulgeHeight = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        this.BuildLineRenderer();
        this.UpdateBulge();
    }

    private void BuildLineRenderer()
    {
        this.throat.positionCount = 100;

        Vector3 throatBottom = Camera.main.ViewportToWorldPoint(new Vector3(this.viewportPositionX, this.viewportPositionYMin, -10f));
        Vector3 throatTop = Camera.main.ViewportToWorldPoint(new Vector3(this.viewportPositionX, this.viewportPositionYMax, -10f));

        throatBottom = new Vector3(throatBottom.x, throatBottom.y, -1.0f);
        throatTop = new Vector3(throatTop.x, throatTop.y, -1.0f);

        float distanceBetweenPositions = (throatBottom.y - throatTop.y) / this.throat.positionCount;

        for (int i = 0; i < this.throat.positionCount; i++)
        {
            float offset = (distanceBetweenPositions * i);

            this.throat.SetPosition(i, new Vector3(throatBottom.x, throatTop.y + offset, -1.0f));            
        }        
    }

    private void UpdateBulge()
    {
        Vector3 mousePlayspacePosition = PlayerController.ViewportToPlayspaceMousePosition();
        float bulgeMidpoint = mousePlayspacePosition.y;

        if (bulgeMidpoint > 1.0f - this.bulgeHeight)
        {
            bulgeMidpoint = 1.0f - this.bulgeHeight;
        }
        else if (bulgeMidpoint < this.bulgeHeight)
        {
            bulgeMidpoint = this.bulgeHeight;
        }

        float bulgeWidthPeak = Mathf.Lerp(this.defaultWidth, this.maxBulgeWidth, ((1 - mousePlayspacePosition.x / 1.0f) / 2.0f));
        float bulgeWidthTips = Mathf.Max((bulgeWidthPeak / 2.0f), this.defaultWidth);

        float bulgeLowerMidpoint = Mathf.Max(bulgeMidpoint - this.bulgeHeight, 0.0f);
        float bulgeUpperMidpoint = Mathf.Max(bulgeMidpoint + this.bulgeHeight, 1.0f);

        this.bulge = new AnimationCurve();

        //Endpoints
        this.bulge.AddKey(new Keyframe(0.0f, this.defaultWidth));
        this.bulge.AddKey(new Keyframe(1.0f, this.defaultWidth));

        //Bulge Midpoint
        this.bulge.AddKey(new Keyframe(bulgeMidpoint, bulgeWidthPeak));

        //Bulge tips
        this.bulge.AddKey(new Keyframe(bulgeMidpoint - this.bulgeHeight, bulgeWidthTips));        
        this.bulge.AddKey(new Keyframe(bulgeMidpoint + this.bulgeHeight, bulgeWidthTips));        
    }

    private void Update()
    {
        this.UpdateBulge();
        this.throat.widthCurve = this.bulge;

        this.BuildLineRenderer();
    }

    private void PrintViewportCoordinates()
    {        
        Vector3 throatBottom = this.throat.GetPosition(0);
        Vector3 throatTop = this.throat.GetPosition(100);

    }
}
