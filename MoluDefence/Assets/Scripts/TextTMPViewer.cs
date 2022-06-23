using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTMPViewer : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    private TextMeshProUGUI textPlayerHP;
    
    [SerializeField]
    private TextMeshProUGUI textPlayerGold;
    
    [SerializeField]
    private PlayerHP        playerHP;
    
    [SerializeField]
    private PlayerGold      playerGold;

    [Header("Wave")]
    [SerializeField]
    private TextMeshProUGUI textWave;

    [SerializeField]
    private TextMeshProUGUI textEnemyCount;

    [SerializeField]
    private WaveSystem      waveSystem;

    [SerializeField]
    private EnemySpawner    enemySpawner;
    
    private void Update()
    {
        textPlayerHP.text   = playerHP.CurrentHP + "/" + playerHP.MaxHP;
        textPlayerGold.text = playerGold.CurrentGold.ToString();
        
        textWave.text       = waveSystem.CurrentWave + "/" + waveSystem.MaxWave;
        textEnemyCount.text = enemySpawner.CurrentEnemyCount + "/" + enemySpawner.MaxEnemyCount;
    }
}
