using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoCanvas: MonoBehaviour
{
    // CANVAS SIZE LUÔN LÀ 1080 X 1920
    [SerializeField] private CanvasScaler SelfCanvas;
    void Start()
    {
        if ((float)Screen.width / Screen.height - 0.63f > 0)
        {
            // => tablet hoặc màn 2:3 => fit height
            SelfCanvas.matchWidthOrHeight = 1;
        }
        else
        {
            // => phone => fit width
            SelfCanvas.matchWidthOrHeight = 0f;
        }
    }
}
