using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public GameObject theMenu;
    public GameObject[] windows, statusButtons;
    private CharStats[] playerStats;

    public Text[] nameText, hpText, mpText, lvlText, expText;
    public Slider[] expSlider;
    public Image[] charImg;
    public GameObject[] charStatHolder;

    public Text statusName, statusHP, statusMP, statusStr, statusDef, statusEqpWpn, statusWpnPwr, statusEqpArm, statusArmPwr, statusExp;
    public Image statusImage;

    public ItemButton[] itemButtons;
    public string selectedItem;
    public Item activeItem;
    public Text itemName, itemDescription, useBtnText;

    public GameObject itemCharChoiceMenu;
    public Text[] itemCharChoiceNames;

    public static GameMenu instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (theMenu.activeInHierarchy)
            {
                CloseMenu();
            }
            else
            {
                theMenu.SetActive(true);
                updateMainStats();
                GameManager.instance.gameMenuOpen = true;
            }
        }
    }

    public void updateMainStats()
    {
        playerStats = GameManager.instance.playerStats;

        for(int i=0; i<playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                charStatHolder[i].SetActive(true);
                nameText[i].text = playerStats[i].charName;
                hpText[i].text = "HP: " + playerStats[i].curHP + "/" + playerStats[i].maxHP;
                mpText[i].text = "MP: " + playerStats[i].curMP + "/" + playerStats[i].maxMP;
                lvlText[i].text = "Lvl:" + playerStats[i].playerLevel;
                expText[i].text = playerStats[i].currentExp.ToString() + "/" + playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].maxValue = playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].value = playerStats[i].currentExp;
                charImg[i].sprite = playerStats[i].charImage;
            } else
            {
                charStatHolder[i].SetActive(false);
            }
        }
    }

    public void ToggleWindow(int windowNumber)
    {
        updateMainStats();

        for(int i=0; i<windows.Length; i++)
        {
            if(i == windowNumber)
            {
                windows[i].SetActive(!windows[i].activeInHierarchy);
            }
            else
            {
                windows[i].SetActive(false);
            }
        }

        itemCharChoiceMenu.SetActive(false);
    }

    public void CloseMenu()
    {
        for(int i=0; i<windows.Length; i++)
        {
            windows[i].SetActive(false);
        }
        theMenu.SetActive(false);
        GameManager.instance.gameMenuOpen = false;
        itemCharChoiceMenu.SetActive(false);
    }

    public void OpenStatus()
    {
        updateMainStats();
        StatusChar(0);
        //update info shown
        for(int i=0; i<statusButtons.Length; i++)
        {
           statusButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy) ;
            statusButtons[i].GetComponentInChildren<Text>().text = playerStats[i].charName;
        }
    }

    public void StatusChar(int selected)
    {
        statusName.text = playerStats[selected].charName;
        statusHP.text = playerStats[selected].curHP.ToString() + "/" + playerStats[selected].maxHP.ToString();
        statusMP.text = playerStats[selected].curMP.ToString() + "/" + playerStats[selected].maxMP.ToString();
        statusStr.text = playerStats[selected].strength.ToString();
        statusDef.text = playerStats[selected].defense.ToString();
        statusWpnPwr.text = playerStats[selected].weaponPower.ToString();
        if(playerStats[selected].equippedWpn != "")
        {
            statusEqpWpn.text = playerStats[selected].equippedWpn;
        }

        if (playerStats[selected].equippedArmor != "")
        {
            statusEqpArm.text = playerStats[selected].equippedArmor;
        }

        statusArmPwr.text = playerStats[selected].armorPower.ToString();
        statusExp.text = (playerStats[selected].expToNextLevel[playerStats[selected].playerLevel] - playerStats[selected].currentExp).ToString();
        statusImage.sprite = playerStats[selected].charImage;
    }

    public void ShowItems()
    {
        GameManager.instance.SortItems();

        for(int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            if(GameManager.instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amtText.text = GameManager.instance.noOfItems[i].ToString();
            } else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amtText.text = "";
            }
        }
    }

    public void SelectItem(Item newItem)
    {
        activeItem = newItem;

        if (activeItem.isItem)
        {
            useBtnText.text = "Use";
        }

        if(activeItem.isWeapon || activeItem.isArmor)
        {
            useBtnText.text = "Equip";
        }

        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.desc;
    }

    public void DiscardItem()
    {
        if(activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
        }
    }

    public void OpenItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(true);

        for(int i=0;i<itemCharChoiceNames.Length; i++)
        {
            itemCharChoiceNames[i].text = GameManager.instance.playerStats[i].charName;
            itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);
        }
    }

    public void CloseItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(false);
    }

    public void UseItem(int selectChar)
    {
        activeItem.Use(selectChar);
        CloseItemCharChoice();
    }
}
