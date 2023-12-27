using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public GameObject flashHolder;
    public Sprite[] flashSprite;
    public SpriteRenderer[] flashSpriteRenderer;

    public float flashTime;

    void Start()
    {
        Deactivate();
    }

    public void Activate()
    {
        flashHolder.SetActive(true);

        int flashSpriteIndex = Random.Range(0, flashSprite.Length);
        for (int i = 0; i < flashSpriteRenderer.Length; i++)
        {
            flashSpriteRenderer[i].sprite = flashSprite[flashSpriteIndex];
        }

        Invoke ("Deactivate", flashTime);
    }

    void Deactivate()
    {
        flashHolder.SetActive(false);
    }
}
