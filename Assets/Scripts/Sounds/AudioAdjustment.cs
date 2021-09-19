using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioAdjustment : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] Slider soundSlider;

    public void SetSfxLevel() { 
        masterMixer.SetFloat("volSfx", Mathf.Log10(soundSlider.value) * 20f);
    }

}
