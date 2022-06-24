using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum SystemType {Money = 0, Build}
public class SystemTextViewer : MonoBehaviour
{
    private TextMeshProUGUI _textSystem;
    private TMPAlpha        _tmpAlpha;

    private void Awake()
    {
        _textSystem = GetComponent<TextMeshProUGUI>();
        _tmpAlpha   = GetComponent<TMPAlpha>();
    }

    public void PrintText(SystemType type)
    {
        switch (type)
        {
            case SystemType.Money:
                _textSystem.text = "System : Not enough gold";
                break;
            case SystemType.Build:
                _textSystem.text = "System : Invalid build tower";
                break;
        }
        _tmpAlpha.FadeOut();
    }
}
