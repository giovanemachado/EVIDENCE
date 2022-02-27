using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    public RawImage Bg;
    public float x;
    public float y;

    // Update is called once per frame
    void Update()
    {
        Bg.uvRect = new Rect(Bg.uvRect.position + new Vector2(x, y) * Time.deltaTime, Bg.uvRect.size);
    }
}
