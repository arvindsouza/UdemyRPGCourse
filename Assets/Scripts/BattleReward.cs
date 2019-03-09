using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BattleReward : MonoBehaviour
{
    public static BattleReward instance;

    public Text xpText, itemText;
    public GameObject rewardsScreen;

    public string[] rewardItems;
    public int xpEarned;

    public bool markQuestComplete;
    public string questToMark;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OpenRewardScreen(50, new string[] { "iron sword", "iron armor" });
        }
    }

    public void OpenRewardScreen(int xp, string[] items)
    {
        xpEarned = xp;
        rewardItems = items;

        xpText.text = "Everyone earned " + xpEarned + " xp!";
        itemText.text = "";

        for(int i = 0; i < rewardItems.Length; i++)
        {
            itemText.text += items[i] + "\n";
        }

        rewardsScreen.SetActive(true);
    }

    public void CloseRewardScreen()
    {
        for(int i = 0; i < GameManager.instance.playerStats.Length; i++)
        {
            if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
            {
                GameManager.instance.playerStats[i].AddExp(xpEarned);
            }
        }

        for(int i = 0; i < rewardItems.Length; i++)
        {
            GameManager.instance.AddItem(rewardItems[i]);
        }
        rewardsScreen.SetActive(false);
        GameManager.instance.battleActive = false;

        if (markQuestComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        }
    }
}
