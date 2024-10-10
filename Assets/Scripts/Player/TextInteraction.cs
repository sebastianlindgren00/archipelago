using UnityEngine;
using TMPro;

public class TextInteraction : MonoBehaviour
{
    public void ReadText()
    {
        GameObject closestObject = GetComponent<PlayerInteraction>().GetClosestObject();
        if (!closestObject.name.Contains("text"))
            return;

        // Get text from the object
        string text = closestObject.GetComponentInChildren<TextMeshPro>().text;
        Debug.Log(text);
    }
}
