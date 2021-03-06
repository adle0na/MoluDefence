using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Image bloodScreen;

    [SerializeField]
    private float maxHP = 20;
    private float currentHP;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");
        
        if (currentHP <= 0)
        {
            Debug.Log("GameOver");
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        Color color = bloodScreen.color;
        color.a = 0.4f;
        bloodScreen.color = color;

        while (color.a >= 0.0f)
        {
            color.a -= Time.deltaTime;
            bloodScreen.color = color;

            yield return null;
        }
    }
}
