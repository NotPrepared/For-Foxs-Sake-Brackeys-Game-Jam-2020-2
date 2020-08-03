using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// ReSharper disable once CheckNamespace
public class AudioControlSyncController : MonoBehaviour
{
    [SerializeField]
    private List<Slider> baseVolumeSliders = new List<Slider>();
    
    [SerializeField]
    private List<Toggle> muteToggles = new List<Toggle>();

    private void Start()
    {
        var audioController = AudioController.instance;
        
        baseVolumeSliders.ForEach(slider => slider.onValueChanged.AddListener(vol => audioController.BaseVolume = vol));
        muteToggles.ForEach(toggle => toggle.onValueChanged.AddListener(isMuted => audioController.IsMuted = isMuted));
        
        audioController.onVolumeChanged.AddListener(vol => baseVolumeSliders.ForEach(slider => slider.SetValueWithoutNotify(vol)));
        audioController.onMutedChanged.AddListener(isMuted => muteToggles.ForEach(toggle => toggle.SetIsOnWithoutNotify(isMuted)));
    }
}