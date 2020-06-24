using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomButtonColor : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        ColorBlock colorBlock = new ColorBlock();
        colorBlock.pressedColor = new Color(0.2f * Random.Range(0, 5), 0.2f * Random.Range(0, 5), 0.2f * Random.Range(0, 5), 1);
        colorBlock.normalColor = new Color(1, 1, 1, 1);
        colorBlock.selectedColor = new Color(1, 1, 1, 1);
        colorBlock.highlightedColor = new Color(1, 1, 1, 1);
        colorBlock.disabledColor = new Color(0.5f, 0.5f, 0.5f, 1);
        colorBlock.colorMultiplier = 1;
        colorBlock.fadeDuration = 0.1f;
        button.colors = colorBlock;
    }
}
