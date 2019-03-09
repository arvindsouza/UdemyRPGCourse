using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BattleItemsButton : MonoBehaviour
{
    public Item activeItem;
    public Text useBtnText, itemName, itemDescription;
    public static BattleItemsButton instance;
    public GameObject itemsMenu, useButton, selectChars;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectItem(Item newItem)
    {
        activeItem = newItem;

        if (activeItem.isItem)
        {
            useBtnText.text = "Use";
        }

        if (activeItem.isWeapon || activeItem.isArmor)
        {
            useBtnText.text = "Equip";
        }

        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.desc;
    }

    public void ShowUseButton()
    {
        useButton.SetActive(true);
    }

    public void showCharSelect()
    {
        selectChars.SetActive(true);
    }

    public void UseItem(int selectChar)
    {
        activeItem.Use(selectChar);
        itemsMenu.SetActive(false);
        for(int i = 0; i < BattleManager.instance.activeFighters.Count; i++)
        {
            if (BattleManager.instance.activeFighters[i].isPlayer)
            {
                for(int j = 0; j < GameManager.instance.playerStats.Length; j++)
                {
                    if(BattleManager.instance.activeFighters[i].charName.ToLower() == GameManager.instance.playerStats[j].charName.ToLower())
                    {
                        BattleManager.instance.activeFighters[i].curHP = GameManager.instance.playerStats[j].curHP;
                        BattleManager.instance.activeFighters[i].curMP = GameManager.instance.playerStats[j].curMP;
                    }
                }
            }
        }
        BattleManager.instance.NextTurn();
    }
}
