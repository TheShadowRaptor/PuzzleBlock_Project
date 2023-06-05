using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    public class Sound
    {
        public enum SoundName
        {
            title,
            gameplayLight,
            gameplayDark

        }

        public SoundName name;
        public AudioClip clip;
        public bool loop = false;
        public float volume = 1f;
        public float pitch = 1f;
    }

    [SerializeField] public List<Sound> sounds = new List<Sound>();
    // Start is called before the first frame update
    private void Update()
    {
        if (MasterSingleton.Instance.GameManager.State == GameManager.GameState.mainmenu)
        {
            PlaySound(Sound.SoundName.title, this.gameObject.GetComponent<AudioSource>());
        }
    }

    public void PlaySound(Sound.SoundName name, AudioSource audioSource)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        if (audioSource.isPlaying) return;

        audioSource.clip = sound.clip;
        audioSource.volume = sound.volume;
        audioSource.pitch = sound.pitch;
        audioSource.loop = sound.loop;

        audioSource.Play();
    }

    public void StopSound(Sound.SoundName name, AudioSource audioSource)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        audioSource.Stop();
    }

    public void PauseSound(Sound.SoundName name, AudioSource audioSource)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        audioSource.Pause();
    }

    public void ResumeSound(Sound.SoundName name, AudioSource audioSource)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        audioSource.UnPause();
    }

    public void SwapMusic(Sound.SoundName name)
    {
        AudioSource audioSource = this.gameObject.GetComponent<AudioSource>();
        if (audioSource.clip.name == "Title")
        {
            audioSource.Stop();
        }
        Sound sound = sounds.Find(s => s.name == name);
        if (audioSource.isPlaying) return;
        PlaySound(name, audioSource);
    }
}
