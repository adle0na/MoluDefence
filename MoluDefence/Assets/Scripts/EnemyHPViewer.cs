using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPViewer : MonoBehaviour
{
    private EnemyHP _enemyHP;
    private Slider  _hpSlider;

    public void Setup(EnemyHP enemyHp)
    {
        this._enemyHP = enemyHp;
        _hpSlider     = GetComponent<Slider>();
    }

    private void Update()
    {
        _hpSlider.value = _enemyHP.CurrentHP / _enemyHP.MaxHP;
    }

}
