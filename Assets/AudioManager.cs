using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public bool loop = false;
        public float volume = 1f;
        public float pitch = 1f;
    }

    [SerializeField] public List<Sound> sounds = new List<Sound>();
    // Start is called before the first frame update
    public void PlaySound(string name, AudioSource audioSource)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        audioSource.clip = sound.clip;
        audioSource.volume = sound.volume;
        audioSource.pitch = sound.pitch;
        audioSource.loop = sound.loop;

        audioSource.Play();
    }

    public void StopSound(string name, AudioSource audioSource)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        audioSource.Stop();
    }

    public void PauseSound(string name, AudioSource audioSource)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        audioSource.Pause();
    }

    public void ResumeSound(string name, AudioSource audioSource)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        audioSource.UnPause();
    }
}
