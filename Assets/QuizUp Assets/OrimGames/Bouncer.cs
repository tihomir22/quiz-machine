using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectToBounce;
    private float height;
    private float width;
    private float mininumWidth;
    private float mininumHeight;
    private float maxinumWidth;
    private float maxinumHeight;

    private int framesCounter = 0;
    private int latestSecond = 0;
    private bool reduciendo = true;
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        var rect = this.objectToBounce.GetComponent<RectTransform>();
        this.width = rect.rect.width;
        this.height = rect.rect.height;
        this.mininumWidth = (float)(this.width * 0.75);
        this.mininumHeight = (float)(this.height * 0.75);
        this.maxinumWidth = this.width;
        this.maxinumHeight = this.height;
    }

    // Update is called once per frame
    void Update()
    {
        this.framesCounter++;
        var hanPasado5Frames = this.framesCounter % 2 == 0;
        if (this.objectToBounce && hanPasado5Frames)
        {
            var rect = this.objectToBounce.GetComponent<RectTransform>();
            this.width = rect.rect.width;
            this.height = rect.rect.height;
            var newWidth = 0;
            var newHeight = 0;
            if (this.reduciendo)
            {
                if (this.width >= this.mininumWidth)
                {
                    newWidth = (int)(this.width - 1);
                }
                else
                {
                    this.reduciendo = false;
                }
                if (this.height >= this.mininumHeight)
                {
                    newHeight = (int)(this.height - 1);
                }
                else
                {
                    this.reduciendo = false;
                }
            }
            else
            {
                if (this.width < this.maxinumWidth)
                {
                    newWidth = (int)(this.width + 1);
                }
                else
                {
                    this.reduciendo = true;
                }
                if (this.height < this.maxinumHeight)
                {
                    newHeight = (int)(this.height + 1);
                }
                else
                {
                    this.reduciendo = true;
                }
            }
            if (newWidth != 0 && newHeight != 0)
            {
                rect.sizeDelta = new Vector2(newWidth, newHeight);
            }
        }
    }
}
