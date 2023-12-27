using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum AudioChannel {Master, Sfv, Music};
    public float masterVolumePercent {get; private set;}
    public float sfvVolumePercent {get; private set;}
    public float musicVolumePercent {get; private set;}

    AudioSource sfv2Dsource;
    AudioSource[] musicSources;
    int activeMusicSourceIndex;

    public static AudioManager instance;

    SoundLibrary library;

    Transform audioListener;
    Transform playerT;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            library = GetComponent<SoundLibrary>();

            musicSources = new AudioSource[2];
            for (int i = 0; i < 2; i++)
            {
                GameObject newMusicSource = new GameObject("Music Source " + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource> ();
                newMusicSource.transform.parent = transform;
            }
            GameObject newSfv2Dsource = new GameObject("2D sfv Source");
            sfv2Dsource = newSfv2Dsource.AddComponent<AudioSource> ();
            newSfv2Dsource.transform.parent = transform;

            audioListener = FindObjectOfType<AudioListener> ().transform;
            if (FindObjectOfType<Player> () != null)
            {
                playerT = FindObjectOfType<Player> ().transform;
            }

            masterVolumePercent = PlayerPrefs.GetFloat("master vol", 1);
            sfvVolumePercent = PlayerPrefs.GetFloat("sfv vol", 1);
            musicVolumePercent = PlayerPrefs.GetFloat("music vol", 1);
        }
    }

    void Update()
    {
        if (playerT != null)
        {
            audioListener.position = playerT.position;
        }
    }

    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        switch (channel)
        {
            case AudioChannel.Master:
                masterVolumePercent = volumePercent;
                break;

            case AudioChannel.Sfv:
                sfvVolumePercent = volumePercent;
                break;

            case AudioChannel.Music:
                musicVolumePercent = volumePercent;
                break;
        }

        musicSources[0].volume = musicVolumePercent * masterVolumePercent;
        musicSources[1].volume = musicVolumePercent * masterVolumePercent;

        PlayerPrefs.SetFloat("master vol", masterVolumePercent);
        PlayerPrefs.SetFloat("sfv vol", sfvVolumePercent);
        PlayerPrefs.SetFloat("music vol", musicVolumePercent);
        PlayerPrefs.Save();
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossFade(fadeDuration));
    }

    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, sfvVolumePercent * masterVolumePercent);
        }
    }

    public void PlaySound(string soundName, Vector3 pos)
    {
        PlaySound(library.GetClipFromName(soundName), pos);
    }

    public void PlaySound2D(string soundName)
    {
        sfv2Dsource.PlayOneShot(library.GetClipFromName(soundName), masterVolumePercent * sfvVolumePercent);
    }

    IEnumerator AnimateMusicCrossFade(float duration)
    {
        float percent = 0;
        while(percent >= 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
            musicSources[1-activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);
            yield return null;
        }
    }
}
