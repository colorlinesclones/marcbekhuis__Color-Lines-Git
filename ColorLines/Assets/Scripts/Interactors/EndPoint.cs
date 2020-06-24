using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EndPoint : MonoBehaviour
{
    public Color wantsColor;
    public bool hasColor = false;

    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite fullSprite;

    private SpriteRenderer spriteRenderer;

    public void Setup()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = wantsColor;
        UpdateSprite(false);
    }

    public void UpdateSprite(bool hasColor)
    {
        this.hasColor = hasColor;
        if (hasColor)
        {
            spriteRenderer.sprite = fullSprite;
        }
        else
        {
            spriteRenderer.sprite = emptySprite;
        }
    }
}
