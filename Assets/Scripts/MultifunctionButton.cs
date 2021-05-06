using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class MultifunctionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum State { Default, Hover, Clicked }
    public PointerEventData.InputButton buttonType;
    public Image image;
    [Header("Sprite Modes")]
    public bool setSprite = true;
    public Sprite defaultSprite;
    public Sprite hoverSprite;
    public Sprite clickedSprite;

    [Header("Events")]
    public UnityEvent OnClickDown;
    public UnityEvent OnClickUp;
    public UnityEvent OnClickHeld;

    private bool mouseHeld;
    private bool previousMouseHeld;
    private bool hovering;
    private State state;

    private void Update()
    {
        previousMouseHeld = mouseHeld;

        int toCheck = 0;
        switch (buttonType)
        {
            case PointerEventData.InputButton.Right:
                toCheck = 1;
                break;
            case PointerEventData.InputButton.Middle:
                toCheck = 2;
                break;
        }
        if (Input.GetMouseButton(toCheck))
        {
            mouseHeld = true;
            if (hovering)
            {
                if (!previousMouseHeld)
                    HandleClickDown();
                OnClickHeld.Invoke();
            }
        }
        else
        {
            mouseHeld = false;
            if (hovering && previousMouseHeld)
                HandleClickUp();
        }

    }

    private void HandleClickUp()
    {
        SetState(State.Hover);
        OnClickUp.Invoke();
    }

    private void HandleClickDown()
    {
        SetState(State.Clicked);
        OnClickDown.Invoke();
    }

    private void SetState(State newState)
    {
        if (newState == state)
            return;
        state = newState;
        if (!setSprite)
            return;
        Sprite newSprite = defaultSprite;
        switch (state)
        {
            case State.Hover:
                if (hoverSprite != null)
                    newSprite = hoverSprite;
                break;
            case State.Clicked:
                if (clickedSprite != null)
                    newSprite = clickedSprite;
                break;
        }
        image.sprite = newSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
        if (mouseHeld)
            SetState(State.Clicked);
        else
            SetState(State.Hover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
        SetState(State.Default);
    }
}
