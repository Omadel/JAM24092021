using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ToggleAudio : MonoBehaviour {
    [SerializeField] private new AudioMixer audio;
    [SerializeField] private Toggle toggle;
    private float baseVolume;

    private void Start() {
        audio.GetFloat("MasterVolume", out baseVolume);
        toggle.onValueChanged.AddListener((enable) => audio.SetFloat("MasterVolume", enable ? baseVolume : -80));
    }
}
