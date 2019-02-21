using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    public string charName;
    public int playerLevel = 1;
    public int currentExp, maxLevel = 100, baseExp = 1000;
    public int[] expToNextLevel;


    public int curHP, maxHP=100, curMP, maxMP=50, strength, defense, weaponPower, armorPower;
    public int[] mpLevelBonus;
    public string equippedWpn, equippedArmor;
    public Sprite charImage;

    // Start is called before the first frame update
    void Start()
    {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseExp;

        for(int i=2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.05f);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddExp(int expToAdd)
    {
        currentExp += expToAdd;

        if (playerLevel < maxLevel)
        {
            if (currentExp >= expToNextLevel[playerLevel])
            {
                currentExp -= expToNextLevel[playerLevel];
                playerLevel++;

                //determine which stat to increase on level up based on odd or even
                if (playerLevel % 2 == 0)
                {
                    strength++;
                }
                else
                {
                    defense++;
                }

                //hp increase on level up
                maxHP = Mathf.FloorToInt(maxHP * 1.05f);
                curHP = maxHP;

                //MP
                maxMP += mpLevelBonus[playerLevel];
                curMP = maxMP;
            }
        }

        if(playerLevel >= maxLevel)
        {
            currentExp = 0;
        }
    }
}
