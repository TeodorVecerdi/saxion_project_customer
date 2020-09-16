using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour {
    private static SoundManager instance;

    public List<AudioKeyValuePair> SoundList;
    public Dictionary<string, AudioSource> Sounds {
        get {
            if (soundDictionary != null) {
                return soundDictionary;
            }

            isMusic = new Dictionary<string, bool>();
            soundDictionary = new Dictionary<string, AudioSource>();
            foreach (var sound in SoundList) {
                isMusic[sound.Key] = sound.IsMusic;
                var audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = sound.Sound;
                audioSource.loop = sound.Loop;
                audioSource.playOnAwake = sound.Autoplay;
                soundDictionary[sound.Key] = audioSource;
            }

            return soundDictionary;
        }
    }

    private Dictionary<string, AudioSource> soundDictionary;
    private Dictionary<string, bool> isMusic;
    private Dictionary<string, float> pausedMusic = new Dictionary<string, float>();
    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    public static float MusicVolume {
        get => instance.musicVolume;
        set {
            instance.musicVolume = value;
            foreach (var sound in instance.Sounds) {
                if (instance.isMusic[sound.Key] && sound.Value != null)
                    sound.Value.volume = instance.musicVolume;
            }

            PlayerPrefs.SetFloat("MusicVolume", instance.musicVolume);
            PlayerPrefs.Save();
        }
    }

    public static float SfxVolume {
        get => instance.sfxVolume;
        set {
            instance.sfxVolume = value;
            foreach (var sound in instance.Sounds) {
                if (!instance.isMusic[sound.Key] && sound.Value != null)
                    sound.Value.volume = instance.sfxVolume;
            }

            PlayerPrefs.SetFloat("SfxVolume", instance.sfxVolume);
            PlayerPrefs.Save();
        }
    }

    public static void PlaySound(string soundKey, bool stopIfPlaying = false, bool skipIfAlreadyPlaying = false, bool resumeIfPaused = false) {
        if (instance.Sounds[soundKey] != null) {
            if (stopIfPlaying)
                instance.Sounds[soundKey].Stop();
            
            if(skipIfAlreadyPlaying && instance.Sounds[soundKey].isPlaying) 
                return;
            
            var offset = 0f;
            if (resumeIfPaused && instance.pausedMusic.ContainsKey(soundKey)) {
                offset = instance.pausedMusic[soundKey];
                instance.pausedMusic.Remove(soundKey);
            }
            
            if (instance.Sounds[soundKey].loop)
                instance.Sounds[soundKey].Play();
            else
                instance.Sounds[soundKey].PlayOneShot(instance.Sounds[soundKey].clip);

            instance.Sounds[soundKey].time = offset;
        }
    }

    public static void PauseSound(string soundKey) {
        if (instance.Sounds[soundKey] != null && instance.Sounds[soundKey].isPlaying) {
            instance.pausedMusic[soundKey] = instance.Sounds[soundKey].time;
            instance.Sounds[soundKey].Stop();
        }
    }

    public static void StopSound(string soundKey) {
        if (instance.Sounds[soundKey] != null) {
            instance.Sounds[soundKey].Stop();
        }
    }

    public static bool IsPlaying(string soundKey) {
        return instance.Sounds[soundKey] != null && instance.Sounds[soundKey].isPlaying;
    }

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;

        var musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        MusicVolume = musicVolume;
        var sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0.75f);
        SfxVolume = sfxVolume;
    }

    [Serializable]
    public class AudioKeyValuePair {
        [SerializeField] public string Key;
        [SerializeField] public AudioClip Sound;
        [SerializeField] public bool Loop;
        [SerializeField] public bool Autoplay;
        [SerializeField] public bool IsMusic;
    }
}