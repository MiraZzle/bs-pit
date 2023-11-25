using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private AudioSource _source;

    void Awake() { _source = GetComponent<AudioSource>(); }

    public bool IsPlaying() { return _source.isPlaying; }
    public void Play(AudioClip clip) { _source.Play(); }
    public void Stop() { _source.Stop(); }
    public void PlaySound(AudioClip clip) { _source.PlayOneShot(clip); }
}
