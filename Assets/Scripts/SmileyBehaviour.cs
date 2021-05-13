using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SmileyBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Image defaultImage, clickedImage, winImage, deadImage;

    private bool hover;

    private void Start()
    {
        Initialize();
        MinefieldUI.Instance.OnWin += HandleWin;
        MinefieldUI.Instance.OnLose += HandleDead;
        MinefieldUI.Instance.OnInit += Initialize;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!hover)
            {
                SetImage(clickedImage);
            }
        }
        else
            SetImage(defaultImage);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover = false;
    }

    private void SetImage(Image image)
    {
        defaultImage.gameObject.SetActive(false);
        clickedImage.gameObject.SetActive(false);
        image.gameObject.SetActive(true);
    }

    public void HandleWin()
    {
        winImage.gameObject.SetActive(true);
    }

    public void HandleDead()
    {
        deadImage.gameObject.SetActive(true);
    }

    public void Initialize()
    {
        winImage.gameObject.SetActive(false);
        deadImage.gameObject.SetActive(false);
    }
}
