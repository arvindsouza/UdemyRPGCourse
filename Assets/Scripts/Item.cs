using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item type")]
    public bool isItem;
    public bool isWeapon, isArmor;

    [Header("Item details")]
    public int value;
    public Sprite itemSprite;
    public string itemName, desc;

    [Header("Item details")]
    public int amtToChange;
    public bool affectHP, affectMP, affectStr;

    [Header("Weapon/Armor details")]
    public int wpnStr;
    public int armStr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use(int charToUseOn)
    {
        CharStats selectedChar = GameManager.instance.playerStats[charToUseOn];

        if (isItem)
        {
            if (affectHP)
            {
                selectedChar.curHP += amtToChange;
                if(selectedChar.curHP > selectedChar.maxHP)
                {
                    selectedChar.curHP = selectedChar.maxHP;
                }
            }

            if (affectMP)
            {
                selectedChar.curHP += amtToChange;
                if(selectedChar.curMP > selectedChar.maxMP)
                {
                    selectedChar.curMP = selectedChar.maxMP;
                }
            }

            if (affectStr)
            {
                selectedChar.strength += amtToChange;
            }
        }

        if (isWeapon)
        {
            if(selectedChar.equippedWpn != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedWpn);
            }
            selectedChar.equippedWpn = itemName;
            selectedChar.weaponPower = wpnStr;
        }

        if (isArmor)
        {
            if (selectedChar.equippedArmor != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedArmor);
            }
            selectedChar.equippedArmor = itemName;
            selectedChar.armorPower = armStr;
        }

        GameManager.instance.RemoveItem(itemName);
    }
}
