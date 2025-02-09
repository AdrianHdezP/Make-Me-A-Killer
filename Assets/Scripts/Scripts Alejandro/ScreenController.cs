using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
   public float screenW;
   public float screenH;

   public float maxX;
   public float minX;

   public float maxY;
   public float minY;


    private void Awake()
    {
        GetScreenSize();
    }
    public void GetScreenSize()
    {
        RectTransform parent = GetComponent<RectTransform>();

        screenW = parent.sizeDelta.x * parent.transform.lossyScale.x;
        screenH = parent.sizeDelta.y * parent.transform.lossyScale.y;

        maxX = parent.transform.position.x + screenW * 0.5f;
        minX = parent.transform.position.x - screenW * 0.5f;

        maxY = parent.transform.position.y + screenH * 0.5f;
        minY = parent.transform.position.y - screenH * 0.5f;
    }
}
