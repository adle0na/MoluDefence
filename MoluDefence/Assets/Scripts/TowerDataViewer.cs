using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image            imageTower;
    
    [SerializeField] 
    private TextMeshProUGUI  textDamage;
    
    [SerializeField] 
    private TextMeshProUGUI  textRate;
    
    [SerializeField]
    private TextMeshProUGUI  textRange;
    
    [SerializeField]
    private TextMeshProUGUI  textLevel;

    [SerializeField]
    private TowerAttackRange towerAttackRange;

    [SerializeField]
    private Button           upGradeBtn;

    [SerializeField]
    private SystemTextViewer systemTextViewer;

    private TowerWeapon      currentTower;
    
    private void Awake()
    {
        OffPanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    public void OnPanel(Transform towerWeapon)
    {
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        gameObject.SetActive(true);
        UpdateTowerDate();
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }
    
    public void OffPanel()
    {
        gameObject.SetActive(false);
        towerAttackRange.OffAttackRange();
    }

    private void UpdateTowerDate()
    {
        imageTower.sprite = currentTower.TowerSprite;
        textDamage.text   = "Damage : " + currentTower.Damage;
        textRate.text     = "Rate : "   + currentTower.Rate;
        textRange.text    = "Range : "  + currentTower.Range;
        textLevel.text    = "Level : "  + currentTower.Level;

        upGradeBtn.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;
    }

    public void OnClickEventTUpgrade()
    {
        bool isSuccess = currentTower.Upgrade();

        if (isSuccess == true)
        {
            UpdateTowerDate();
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        }
        else
        {
            systemTextViewer.PrintText(SystemType.Money);
        }
    }

    public void OnclickEventTSell()
    {
        currentTower.Sell();
        OffPanel();
    }
}
