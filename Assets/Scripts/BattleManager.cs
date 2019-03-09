using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    private bool battleActive;
    public GameObject battleScene;
    public Transform[] playerPositions, enemyPositions;
    public BattleChar[] playerPrefabs, enemyPrefabs;

    public List<BattleChar> activeFighters = new List<BattleChar>();

    public int curTurn;
    public bool turnWaiting;
    public GameObject uiButtonsHolder;

    public BattleMove[] movesList;

    public GameObject enemyAttackEffect;
    public DamageNumber theDamage;

    public Text[] playerNames, playerHP, playerMP;

    public GameObject targetMenu;
    public BattleTargetButton[] targetButtons;

    public GameObject magicMenu;
    public BattleMagicSelect[] magicButtons;

    public BattleNotification battleNotice;

    public int chanceToFlee = 50;
    private bool fleeing;

    public GameObject itemsMenu;
    public ItemButton[] itemButtons;
    public GameObject[] charSelect;

    public string gameOverScene;

    public int rewardsXP;
    public string[] rewardItems;

    public bool cantFlee;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            BattleStart(new string[] { "Eyeball", "Spider", "FlyThing" }, false);
        }

        if (battleActive)
        {
            if (turnWaiting)
            {
                if (activeFighters[curTurn].isPlayer)
                {
                    uiButtonsHolder.SetActive(true);
                } else
                {
                    uiButtonsHolder.SetActive(false);
                    StartCoroutine(EnemyMoveCo());
                }
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                NextTurn();
            }
        }
    }

    public void BattleStart(string[] enemiesToSpawn, bool cantFlee)
    {
        if (!battleActive)
        {
            this.cantFlee = cantFlee;
            battleActive = true;
            GameManager.instance.battleActive = true;
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            battleScene.SetActive(true);
            AudioManager.instance.PlayBgMusic(0);

            for(int i=0; i < playerPositions.Length; i++)
            {
                if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    for(int j = 0; j < playerPrefabs.Length; j++)
                    {
                        if (playerPrefabs[j].charName.ToLower() == GameManager.instance.playerStats[i].charName.ToLower())
                        {
                            BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
                            newPlayer.transform.parent = playerPositions[i];
                            activeFighters.Add(newPlayer);

                            CharStats thePlayer = GameManager.instance.playerStats[i];
                            activeFighters[i].curHP = thePlayer.curHP;
                            activeFighters[i].maxHP = thePlayer.maxHP;
                            activeFighters[i].curMP = thePlayer.curMP;
                            activeFighters[i].maxMP = thePlayer.maxMP;
                            activeFighters[i].str = thePlayer.strength;
                            activeFighters[i].def = thePlayer.defense;
                            activeFighters[i].wpnPower = thePlayer.weaponPower;
                            activeFighters[i].armPower = thePlayer.armorPower;
                        }
                    }
                }
            }

            for(int i=0; i<enemiesToSpawn.Length; i++)
            {
                if(enemiesToSpawn[i] != "")
                {
                    for(int j=0; j < enemyPrefabs.Length; j++)
                    {
                        if(enemiesToSpawn[i].ToLower() == enemyPrefabs[j].charName.ToLower())
                        {
                            BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];
                            activeFighters.Add(newEnemy);
                        }
                    }
                }
            }

            turnWaiting = true;
            curTurn = Random.Range(0, activeFighters.Count);
            UpdateUIStats();
        }
    }

    public void NextTurn()
    {
        curTurn++;

        if(curTurn >= activeFighters.Count)
        {
            curTurn = 0;
        }

        turnWaiting = true;
        UpdateBattle();
        UpdateUIStats();
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true, allPlayersDead = true;

        for(int i=0; i < activeFighters.Count; i++)
        {
            if(activeFighters[i].curHP < 0)
            {
                activeFighters[i].curHP = 0;
            }

            if(activeFighters[i].curHP == 0)
            {
                if (activeFighters[i].isPlayer)
                {
                    activeFighters[i].theSprite.sprite = activeFighters[i].deadSprite;
                } else
                {
                    activeFighters[i].enemyFade();
                }
            } else
            {
                if (activeFighters[i].isPlayer)
                {
                    allPlayersDead = false;
                    activeFighters[i].theSprite.sprite = activeFighters[i].aliveSprite;
                }
                else
                {
                    allEnemiesDead = false;
                }
            }
        }

        if(allEnemiesDead || allPlayersDead)
        {
            if (allEnemiesDead)
            {
                // end battle with victory
                StartCoroutine(EndBattle());
            } else
            {
                //end battle in failure
                StartCoroutine(GameOverCo());
            }

            //battleScene.SetActive(false);
            //GameManager.instance.battleActive = false;
            //battleActive = false;
        } else
        {
            while(activeFighters[curTurn].curHP == 0)
            {
                curTurn++;
                if(curTurn >= activeFighters.Count)
                {
                    curTurn = 0;
                }
            }
        }
    }

    public IEnumerator EnemyMoveCo()
    {
        turnWaiting = false;
        yield return new WaitForSeconds(1);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    public void EnemyAttack()
    {
        List<int> players = new List<int>();
        for(int i = 0; i < activeFighters.Count; i++)
        {
            if(activeFighters[i].isPlayer && activeFighters[i].curHP > 0)
            {
                players.Add(i);
            }
        }

        int target = players[Random.Range(0, players.Count)];

        //activeFighters[target].curHP -= 30;

        int selectAttack = Random.Range(0, activeFighters[curTurn].movesAvailable.Length);
        int movePower = 0;
        for(int i=0; i < movesList.Length; i++)
        {
            if(movesList[i].moveName == activeFighters[curTurn].movesAvailable[selectAttack])
            {
                movePower = movesList[i].movePower;
                Instantiate(movesList[i].effect, activeFighters[target].transform.position, activeFighters[target].transform.rotation);
            }
        }

        Instantiate(enemyAttackEffect, activeFighters[curTurn].transform.position, activeFighters[curTurn].transform.rotation);
        DealDamage(target, movePower);
    }

    public void DealDamage(int target, int movePower)
    {
        float atkPwr = activeFighters[curTurn].str + activeFighters[curTurn].wpnPower;
        float defPwr = activeFighters[target].def + activeFighters[target].armPower;

        float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);
        int damageDealt = Mathf.RoundToInt(damageCalc);

        Debug.Log(activeFighters[curTurn].charName + ": " + damageDealt + "to " + activeFighters[target].charName);

        activeFighters[target].curHP -= damageDealt;
        UpdateUIStats();
        Instantiate(theDamage, activeFighters[target].transform.position, activeFighters[target].transform.rotation).SetDamage(damageDealt);

    }

    public void UpdateUIStats()
    {
        for(int i = 0; i < playerNames.Length; i++)
        {
            if(activeFighters.Count > i)
            {
                if (activeFighters[i].isPlayer)
                {
                    BattleChar playerData = activeFighters[i];
                    playerNames[i].gameObject.SetActive(true);
                    playerNames[i].text = playerData.charName;
                    playerHP[i].text = Mathf.Clamp(playerData.curHP, 0, int.MaxValue) + "/" + playerData.maxHP;
                    playerMP[i].text = Mathf.Clamp(playerData.curMP,0,int.MaxValue) + "/" + playerData.maxMP;
                } else
                {
                    playerNames[i].gameObject.SetActive(false);

                }
            } else
            {
                playerNames[i].gameObject.SetActive(false);
            }
        }
    }

    public void PlayerAttack(string moveName, int selectedTarget)
    {
        int movePower = 0;
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName.ToLower() == moveName.ToLower())
            {
                movePower = movesList[i].movePower;
                Instantiate(movesList[i].effect, activeFighters[selectedTarget].transform.position, activeFighters[selectedTarget].transform.rotation);
            }
        }

        Instantiate(enemyAttackEffect, activeFighters[curTurn].transform.position, activeFighters[curTurn].transform.rotation);
        DealDamage(selectedTarget, movePower);
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        NextTurn();
    }

    public void OpenTargetMenu(string moveName)
    {
        targetMenu.SetActive(true);

        List<int> enemies = new List<int>();
        for(int i = 0; i < activeFighters.Count; i++)
        {
            if (!activeFighters[i].isPlayer)
            {
                enemies.Add(i);
            }
        }

        for(int i = 0; i < targetButtons.Length; i++)
        {
            if(enemies.Count > i &&activeFighters[enemies[i]].curHP > 0)
            {
                targetButtons[i].gameObject.SetActive(true);
                targetButtons[i].moveName = moveName;
                targetButtons[i].activeFighterTarget = enemies[i];
                targetButtons[i].targetName.text = activeFighters[enemies[i]].charName;
            } else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenMagicMenu()
    {
        magicMenu.SetActive(true);
        for(int i = 0; i < magicButtons.Length; i++)
        {
            if(activeFighters[curTurn].movesAvailable.Length > i)
            {
                magicButtons[i].gameObject.SetActive(true);
                magicButtons[i].spellName = activeFighters[curTurn].movesAvailable[i];
                magicButtons[i].nameText.text = magicButtons[i].spellName;
                for (int j = 0; j < movesList.Length; j++)
                {
                    if(movesList[j].moveName.ToLower() == magicButtons[i].spellName.ToLower())
                    {
                        magicButtons[i].spellCost = movesList[j].moveCost;
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();
                    }
                }
            } else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void Flee()
    {
        if (cantFlee)
        {
            battleNotice.notiText.text = "Can't flee";
            battleNotice.Activate();
        }
        else
        {
            int fleeSuccess = Random.Range(0, 100);
            if (fleeSuccess < chanceToFlee)
            {
                //end battle
                battleActive = false;
                battleScene.SetActive(false);
                StartCoroutine(EndBattle());
                fleeing = true;
            }
            else
            {
                NextTurn();
                battleNotice.notiText.text = "Couldn't Escape";
                battleNotice.Activate();
            }
        }
    }

    public void OpenItemMenu()
    {
        itemsMenu.SetActive(true);

        for(int i = 0; i < GameManager.instance.playerStats.Length; i++)
        {
            if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
            {
                charSelect[i].gameObject.SetActive(true);
            } else
            {
                charSelect[i].gameObject.SetActive(false);
            }
        }
        for(int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "" )
            {
                if (GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).isItem)
                {
                    itemButtons[i].buttonImage.gameObject.SetActive(true);
                    itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                    itemButtons[i].amtText.text = GameManager.instance.noOfItems[i].ToString();
                }
                else
                {
                    itemButtons[i].buttonImage.gameObject.SetActive(false);
                    itemButtons[i].amtText.text = "";
                }
            }
            else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amtText.text = "";
            }
        }
    }

    public IEnumerator EndBattle()
    {
        battleActive = false;
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        magicMenu.SetActive(false);
        itemsMenu.SetActive(false);

        yield return new WaitForSeconds(.5f);

        UITransition.instance.fadeToBlack();

        yield return new WaitForSeconds(1.5f);

        for(int i = 0; i < activeFighters.Count; i++)
        {
            if (activeFighters[i].isPlayer)
            {
                for(int j = 0; j < GameManager.instance.playerStats.Length; j++)
                {
                    if(activeFighters[i].charName.ToLower() == GameManager.instance.playerStats[j].charName.ToLower())
                    {
                        GameManager.instance.playerStats[j].curHP = activeFighters[i].curHP;
                        GameManager.instance.playerStats[j].curMP = activeFighters[j].curMP;
                    }
                }
            }
            Destroy(activeFighters[i].gameObject);
        }

        UITransition.instance.fadeFromBlack();
        battleScene.SetActive(false);

        activeFighters.Clear();
        curTurn = 0;
        // GameManager.instance.battleActive = false;
        if (fleeing)
        {
            GameManager.instance.battleActive = false;
            fleeing = false;
        } else
        {
            //open rewards
            BattleReward.instance.OpenRewardScreen(rewardsXP, rewardItems);
            
        }

        AudioManager.instance.PlayBgMusic(FindObjectOfType<CameraController>().musicToPlay);
        
    }

    public IEnumerator GameOverCo()
    {
        UITransition.instance.fadeToBlack();
        battleActive = false;
        yield return new WaitForSeconds(1.5f);
        battleScene.SetActive(false);
        SceneManager.LoadScene(gameOverScene);
    }
}
