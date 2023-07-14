using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private D_Audio soundsData;
    private List<Sound> soundList;

    private Sound currentThemePlaying;

    private void OnEnable()
    {
        EventManager.StartListening("onSceneLoadingOver", PlaySoundOnSceneLoading);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onSceneLoadingOver", PlaySoundOnSceneLoading);
    }

    private void Awake()
    {
        soundsData = Resources.Load<D_Audio>("Data/Audio Data/AudioData");
        soundList = soundsData.sounds;

        foreach (Sound sound in soundList)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    void Start() { }

    void Update() { }

    public void PlaySoundOnSceneLoading(Dictionary<string, object> message)
    {
        if (!(bool)message["playThemeOnEnter"])
            return;

        string themeAudioName = (message["sceneName"] as string) + "Theme";

        StartCoroutine(FadeOutAudio(currentThemePlaying, 2));
        StartCoroutine(PlayAudioWithDelay(themeAudioName, (float)message["delayTime"], true));
    }

    public void PlayAudio(string name, bool isSceneTheme = false)
    {
        Sound soundToPlayer = soundList.Find(sound => sound.name == name);

        if (soundToPlayer == null)
        {
            Debug.LogWarning("Sound \"" + name + "\" is not present in the game sound list");
            return;
        }

        if (isSceneTheme)
        {
            currentThemePlaying = soundToPlayer;
            StartCoroutine(FadeInAudio(soundToPlayer, soundToPlayer.fadeInDuration));
            return;
        }

        soundToPlayer.source.Play();
    }

    public IEnumerator PlayAudioWithDelay(string audioName, float delay, bool isSceneTheme = false)
    {
        yield return new WaitForSeconds(delay);
        PlayAudio(audioName, isSceneTheme);
    }

    public IEnumerator FadeInAudio(Sound sound, float duration)
    {
        float currentTime = 0;
        sound.source.volume = 0;

        sound.source.Play();

        while (currentTime <= sound.fadeInDuration)
        {
            currentTime += Time.deltaTime;
            sound.source.volume = Mathf.Lerp(0, sound.volume, currentTime / duration);

            yield return null;
        }

        yield break;
    }

    public IEnumerator FadeOutAudio(Sound sound, float duration)
    {
        float currentTime = 0;

        while (currentTime <= sound.fadeOutDuration)
        {
            Debug.Log(currentTime);
            Debug.Log(sound.fadeOutDuration);
            currentTime += Time.deltaTime;
            sound.source.volume = Mathf.Lerp(sound.volume, 0, currentTime / duration);

            yield return null;
        }

        sound.source.Stop();
        yield break;
    }
}
