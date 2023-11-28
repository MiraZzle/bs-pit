using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource stampSound;

    [SerializeField]
    private AudioSource chooseAnswerSound;

    [SerializeField]
    private AudioSource writeTextSound;

    [SerializeField]
    private AudioSource spotlightSound;

    public void PlayStampSE() {
        stampSound.Play();
    }

    public void PlaySpotlightSE() {
        spotlightSound.Play();
    }

    public void PlayWriteTextSE() {
        if (!writeTextSound.isPlaying) {
            writeTextSound.Play();
        }
    }

    public void PlayMouseClickSE() {
        chooseAnswerSound.Play();
    }
}
