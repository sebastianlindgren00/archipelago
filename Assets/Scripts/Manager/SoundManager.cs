using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioClips;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // TODO: Trigger doesn't work 
    public void StartChaseSound()
    {
        _audioSource.clip = _audioClips[0];
        _audioSource.Play();
    }
}
