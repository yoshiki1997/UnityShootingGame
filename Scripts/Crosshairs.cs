using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshairs : MonoBehaviour
{
    public LayerMask targetMask;
    public SpriteRenderer dot;
    public Color dotHeightLiteColor;
    Color originalDotColor;

    void Start()
    {
        Cursor.visible = false;
        originalDotColor = dot.color;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * -40 * Time.deltaTime);
    }

    public void DetectedTarget(Ray ray)
    {
        if (Physics.Raycast(ray, 100, targetMask))
        {
            dot.color = dotHeightLiteColor;
        }
        else
        {
            dot.color = originalDotColor;
        }
    }
}
