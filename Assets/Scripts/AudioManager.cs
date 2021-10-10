using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages audio playback
/// </summary>
public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public static Music activeMusic = null;
    public static List<Music> allMusic = new List<Music>();

    public float musicTransitionSpeed = 0.5f;
    public bool musicSmoothTransitions = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public void PlaySFX(AudioClip sfx, float volume = 1f, float pitch = 1f)
    {
        AudioSource source = CreateNewAudioSource(string.Format("SFX [{0}]", sfx.name));
        source.clip = sfx;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();

        Destroy(source.gameObject, sfx.length);
    }

    public void PlayMusic(AudioClip music, float maxVolume = 1f, float pitch = 1f, float startingVolume = 0f, bool playOnStart = true, bool loop = true)
    {
        if (music != null)
        {
            for (int i = 0; i < allMusic.Count; i++)
            {
                Music m = allMusic[i];
                if (m.clip == music)
                {
                    activeMusic = m;
                    break;
                }
            }
            if (activeMusic == null || activeMusic.clip != music)
            {
                activeMusic = new Music(music, maxVolume, pitch, startingVolume, playOnStart, loop);
            }
        }
        else
        {
            activeMusic = null;
        }

        StopAllCoroutines();
        StartCoroutine(VolumeLevelling());
    }

    IEnumerator VolumeLevelling()
    {
        while (TransitionMusic())
        {
            yield return new WaitForEndOfFrame();
        }


    }

    bool TransitionMusic()
    {
        bool anyValueChanged = false;

        float speed = musicTransitionSpeed * Time.deltaTime;
        for (int i = allMusic.Count - 1; i >= 0; i--)
        {
            Music music = allMusic[i];
            if (music == activeMusic)
            {
                if (music.volume < music.maxVolume)
                {
                    music.volume = musicSmoothTransitions ? Mathf.Lerp(music.volume, music.maxVolume, speed) : Mathf.MoveTowards(music.volume, music.maxVolume, speed);
                    anyValueChanged = true;
                }
            }
            else
            {
                if (music.volume > 0)
                {
                    music.volume = musicSmoothTransitions ? Mathf.Lerp(music.volume, 0f, speed) : Mathf.MoveTowards(music.volume, 0f, speed);
                    anyValueChanged = true;
                }
                else
                {
                    allMusic.RemoveAt(i);
                    music.DestroyMusic();
                    continue;
                }
            }
        }

        return anyValueChanged;
    }

    public static AudioSource CreateNewAudioSource(string _name)
    {
        AudioSource newAudioSource = new GameObject(_name).AddComponent<AudioSource>();
        newAudioSource.transform.SetParent(instance.transform);
        return newAudioSource;
    }

    [System.Serializable]
    public class Music
    {
        public AudioSource source;
        public AudioClip clip { get { return source.clip; } set { source.clip = value; } }
        public float maxVolume = 1f;

        public Music(AudioClip clip, float _maxVolume, float pitch, float startingVolume, bool playOnStart, bool loop)
        {
            source = AudioManager.CreateNewAudioSource(string.Format("Music [{0}]", clip.name));
            source.clip = clip;
            source.volume = startingVolume;
            maxVolume = _maxVolume;
            source.pitch = pitch;
            source.loop = loop;

            AudioManager.allMusic.Add(this);

            if (playOnStart)
            {
                source.Play();
            }
        }

        public float volume
        {
            get { return source.volume; }
            set { source.volume = value; }
        }
        public float pitch
        {
            get { return source.volume; }
            set { source.volume = value; }
        }

        public void Play()
        {
            source.Play();
        }
        public void Stop()
        {
            source.Stop();
        }
        public void Pause()
        {
            source.Pause();
        }
        public void UnPause()
        {
            source.UnPause();
        }
        public void DestroyMusic()
        {
            AudioManager.allMusic.Remove(this);
            DestroyImmediate(source.gameObject);
        }



    }
}
