using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statusScr : MonoBehaviour
{
    public Sprite[] sprites;
    
    private SpriteRenderer renderer;

    private void Awake()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetSprite(int spriteNumber)
    {
        renderer.sprite = sprites[spriteNumber];
    }
}
