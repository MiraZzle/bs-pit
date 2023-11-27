using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundVolumeControl : MonoBehaviour
{
    // this scripts manages the sound control button that is present on the pause screen


    [SerializeField] Image[] soundIcons = new Image[4];  // icons for the button

    int currentIconIndex;  // 0 = lowest volume, 3 = highest volume
    const string prefName = "index";

    void SavePreferences() {
        // saves the index to player preferences
        PlayerPrefs.SetInt(prefName, currentIconIndex);
    }

    void LoadPreference() {
        // loads the index from player preferences
        currentIconIndex = PlayerPrefs.GetInt(prefName);
    }

    void UpdateVolume() {
        // enables the correct icon and adjusts the volume accordingly
        for (int i = 0; i < soundIcons.Length; i++) {
            soundIcons[i].enabled = (i == currentIconIndex);
        }
        // volume(0) = 0, volume(soundIcons.Length - 1) = 1
        AudioListener.volume = currentIconIndex / (float)(soundIcons.Length - 1);

    }

    public void OnButtonPress() {
        // button presses cycle through the volume levels
        currentIconIndex = (currentIconIndex + 1) % soundIcons.Length;

        SavePreferences();
        UpdateVolume();
    }

    void Start() {
        // sets the max volume if player preferences aren't set
        if (!PlayerPrefs.HasKey(prefName)) {
            PlayerPrefs.SetInt(prefName, soundIcons.Length - 1);
        }

        LoadPreference();
        UpdateVolume();
    }
}
