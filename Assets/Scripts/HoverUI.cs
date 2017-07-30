using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isPointerOverIt = false;

    // Update is called once per frame
    void Update()
    {
    }
    public void OnPointerEnter(PointerEventData dataName)
    {
        isPointerOverIt = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOverIt = false;
    }
}
