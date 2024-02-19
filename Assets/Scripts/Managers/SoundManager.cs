using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioList= new List<AudioClip>();
    private AudioSource audioSource;

     private void Start()
    {
        audioSource= GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        EventManager.playAudio += PlayAudio;
        EventManager.playAudioWithVolume += PlayAudioWithVolume;
    }
    private void OnDisable()
    {
        EventManager.playAudio -= PlayAudio;
        EventManager.playAudioWithVolume -= PlayAudioWithVolume;
    }

    private void PlayAudio(int index)
    {
        audioSource.PlayOneShot(audioList[index]);
    }
    private void PlayAudioWithVolume(int index, float volume)
    {
        audioSource.PlayOneShot(audioList[index],volume);
    }

}
