using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperNote : MonoBehaviour
{
    void Awake()
    {
        TextMesh t = gameObject.AddComponent<TextMesh>();
        t.text = "new text set";
        t.fontSize = 30;
        t.transform.localEulerAngles += new Vector3(90, 0, 0);
        t.transform.localPosition += new Vector3(56f, 3f, 40f);
    }
}
