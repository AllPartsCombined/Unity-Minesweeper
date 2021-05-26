using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedImage : MonoBehaviour
{

    public Image image;
    public Sprite[] sprites;
    public int fps = 6;
    public bool loop = true;
    public bool startOnStart = true;

    private int index = 0;

    void Awake()
    {
        if (image == null)
            image = GetComponent<Image>();
        if (startOnStart)
            StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        index = 0;
        while (index < sprites.Length)
        {
            image.sprite = sprites[index];
            index++;
            yield return new WaitForSeconds(1f / fps);
        }
        if (loop)
            StartCoroutine(PlayAnimation());
    }
}
