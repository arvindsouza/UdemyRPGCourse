using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool gameMenuOpen, dialogActive, fadingBetAreas;
    public string[] itemsHeld;
    public int[] noOfItems;
    public Item[] referenceItems;


    public CharStats[] playerStats;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SortItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMenuOpen || dialogActive || fadingBetAreas)
            PlayerController.instance.canMove = false;
        else
            PlayerController.instance.canMove = true;
    }

    public Item GetItemDetails(string itemToGrab)
    {
        for(int i=0; i<referenceItems.Length; i++)
        {
            if(itemToGrab.Equals(referenceItems[i].itemName, StringComparison.CurrentCultureIgnoreCase))
            {
                Debug.Log(referenceItems[i]);
                return referenceItems[i];
            }
        }
        return null;
    }

    public void SortItems()
    {
        bool itemAfterSpace = true;

        while (itemAfterSpace)
        {
            itemAfterSpace = false;
        for (int i = 0; i < itemsHeld.Length - 1; i++)
            {
                if (itemsHeld[i] == "")
                {
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    noOfItems[i] = noOfItems[i + 1];
                    noOfItems[i + 1] = 0;

                    if (itemsHeld[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    public void AddItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;

        for(int i = 0; i < itemsHeld.Length; i++)
        {
            if(itemsHeld[i] == "" || itemsHeld[i].ToLower() == itemToAdd.ToLower())
            {
                newItemPosition = i;
                foundSpace = true;
                break;
            }
        }

        if (foundSpace)
        {
            bool itemExists = false;
            for(int i = 0; i < referenceItems.Length; i++)
            {
                if(referenceItems[i].itemName.ToLower() == itemToAdd.ToLower())
                {
                    itemExists = true;
                    break;
                }
            }

            if (itemExists)
            {
                itemsHeld[newItemPosition] = itemToAdd;
                noOfItems[newItemPosition]++;
            }
            else
            {
                Debug.LogError(itemToAdd + " doesn't exist");
            }
        }

        GameMenu.instance.ShowItems();
    }

    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        for(int i=0; i<itemsHeld.Length; i++)
        {
            if(itemsHeld[i].ToLower() == itemToRemove.ToLower())
            {
                foundItem = true;
                itemPosition = i;
                break;
            }
        }

        if (foundItem)
        {
            noOfItems[itemPosition]--;
            if(noOfItems[itemPosition] <= 0)
            {
                itemsHeld[itemPosition] = "";
            }
            GameMenu.instance.ShowItems();
        } else
        {
            Debug.LogError("Couldn't find " + itemToRemove);
        }
    }
}
