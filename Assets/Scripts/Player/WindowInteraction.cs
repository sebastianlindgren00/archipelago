using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WindowInteraction : MonoBehaviour
{
    public void InteractWindow()
    {
        GameObject closestObject = GetComponent<PlayerInteraction>().GetClosestObject();
        if (!closestObject.name.Contains("Window"))
            return;

        // Get text from the object
        AudioClip audioClip = closestObject.GetComponentInChildren<AudioSource>().clip;
        AudioSource audioSource = closestObject.GetComponentInChildren<AudioSource>();
        if (audioSource != null && audioClip != null)
        {
            audioSource.Play();
        }
    }
}
