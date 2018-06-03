using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSprite : MonoBehaviour
{
    private Image image;
    public Sprite on;
    public Sprite off;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void ToggleGraphic(bool value)
    {
        image.sprite = value ? on : off;
    }
}