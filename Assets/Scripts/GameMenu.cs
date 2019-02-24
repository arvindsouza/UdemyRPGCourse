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

    // Start is called before the first frame update
    void Start()
    {
        
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
    }

    public void CloseMenu()
    {
        for(int i=0; i<windows.Length; i++)
        {
            windows[i].SetActive(false);
        }
        theMenu.SetActive(false);
        GameManager.instance.gameMenuOpen = false;
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
        statusHP.text = playerStats[selected].curHP.ToString();
        statusMP.text = playerStats[selected].curMP.ToString();
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
}
