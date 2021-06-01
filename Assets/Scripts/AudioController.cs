using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip explosionClip;
    public AudioClip timeClip;
    public AudioSource source;

    public bool soundOn { get; private set; }

    private void Start()
    {
        if (source == null)
            source = GetComponent<AudioSource>();
        GameController.Instance.OnLose += PlayExplosionSound;
        Timer.Instance.OnTimeUpdated += PlayTimeSound;
        soundOn = PlayerPrefs.GetInt("sound", 0) == 1;
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("sound", soundOn ? 1 : 0);
    }

    private void PlayTimeSound(int time)
    {
        if (soundOn && timeClip != null && time > 0)
            source.PlayOneShot(timeClip);
    }

    private void PlayExplosionSound()
    {
        if (soundOn && explosionClip != null)
            source.PlayOneShot(explosionClip);
    }

    public void ToggleSound()
    {
        soundOn = !soundOn;
    }



}
