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
    private TextMeshProUGUI  textUpgradeCost;
    
    [SerializeField]
    private TextMeshProUGUI  textSellCost;

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
        UpdateTowerData();
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }
    
    public void OffPanel()
    {
        gameObject.SetActive(false);
        towerAttackRange.OffAttackRange();
    }

    private void UpdateTowerData()
    {
        if (currentTower.WeaponType == WeaponType.Cannon || currentTower.WeaponType == WeaponType.Laser)
        {
            imageTower.rectTransform.sizeDelta = new Vector2(88, 59);
            textDamage.text = "Damage : " + currentTower.Damage + "+" + "<color=red>" + currentTower.AddedDamage.ToString("F1") + "</color>";
        }
        else
        { 
            imageTower.rectTransform.sizeDelta = new Vector2(59, 59);
            
            if (currentTower.WeaponType == WeaponType.Slow)
                textDamage.text = "Slow : " + currentTower.Slow * 100 + "%";
            else if (currentTower.WeaponType == WeaponType.Buff)
                textDamage.text = "Buff : " + currentTower.Buff * 100 + "%";
        }
        
        imageTower.sprite = currentTower.TowerSprite;

        textRate.text        = "Rate : "   + currentTower.Rate;
        textRange.text       = "Range : "  + currentTower.Range;
        textLevel.text       = "Level : "  + currentTower.Level;
        textUpgradeCost.text = currentTower.UpgradeCost.ToString();
        textSellCost.text    = currentTower.SellCost.ToString();

        upGradeBtn.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;
    }

    public void OnClickEventTUpgrade()
    {
        bool isSuccess = currentTower.Upgrade();

        if (isSuccess == true)
        {
            UpdateTowerData();
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
