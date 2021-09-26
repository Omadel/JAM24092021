using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ToggleMusic : MonoBehaviour {
    [SerializeField] private new AudioMixer audio;
    [SerializeField] private Toggle toggle;
    private float baseVolume;

    private void Start() {
        audio.GetFloat("MusicVolume", out baseVolume);
        toggle.onValueChanged.AddListener((enable) => audio.SetFloat("MusicVolume", enable ? -80 : baseVolume));
    }
}
