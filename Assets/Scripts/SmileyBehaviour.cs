using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SmileyBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Image image;
    public Sprite defaultSprite, clickedSprite;

    private bool hover; 

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!hover)
            {
                image.sprite = clickedSprite;
            }
        }
        else
            image.sprite = defaultSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover = false;
    }
}
