using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAppWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject playButton;
    [SerializeField]
    private GameObject stopButton;

    [SerializeField]
    private string songName;

    public void QueueMusicLoFi()
    {
        PlayMusic(songName);

        playButton.SetActive(false);
        stopButton.SetActive(true);
    }

    public void StopMusic()
    {
        songName = "";
        PlayMusic(songName);

        playButton.SetActive(true);
        stopButton.SetActive(false);
    }

    public void PlayMusic(string songName)
    {
        AudioClip clip = Resources.Load("Audio/Music/" + songName) as AudioClip;

        AudioManager.instance.PlayMusic(clip);
    }

}
