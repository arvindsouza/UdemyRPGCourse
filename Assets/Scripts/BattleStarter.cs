using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour
{
    public BattleType[] potentialBattles;
    public bool activateOnEnter, activateOnStay, activateOnExit;

    private bool inArea;
    public float timeBetweenBattles = 10f;
    private float betweenBattleCounter;

    public bool deactivateAfterStart;
    public bool cantFlee;

    public bool shouldCompleteQuest;
    public string questToComplete;

    // Start is called before the first frame update
    void Start()
    {
        betweenBattleCounter = Random.Range(timeBetweenBattles * .5f, timeBetweenBattles * 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(inArea && PlayerController.instance.canMove)
        {
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Horizontal") != 0)
            {
                betweenBattleCounter -= Time.deltaTime;
            }

            if(betweenBattleCounter <= 0)
            {
                betweenBattleCounter = Random.Range(timeBetweenBattles * .5f, timeBetweenBattles * 1.5f);
                StartCoroutine(startBattleCo());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (activateOnEnter)
            {
                StartCoroutine(startBattleCo());
            } else
            {
                inArea = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.tag == "Player")
        {
            if (activateOnExit)
            {

            }
            else
            {
                inArea = false;
            }
        }
    }

    public IEnumerator startBattleCo()
    {
        UITransition.instance.fadeToBlack();
        GameManager.instance.battleActive = true;

        int selectedBattle = Random.Range(0, potentialBattles.Length);

        BattleManager.instance.rewardItems = potentialBattles[selectedBattle].rewardItem;
        BattleManager.instance.rewardsXP = potentialBattles[selectedBattle].rewardXP;

        yield return new WaitForSeconds(1.5f);
        BattleManager.instance.BattleStart(potentialBattles[selectedBattle].enemies, cantFlee);
        UITransition.instance.fadeFromBlack();

        if (deactivateAfterStart)
        {
            gameObject.SetActive(false);
        }

        BattleReward.instance.markQuestComplete = shouldCompleteQuest;
        BattleReward.instance.questToMark = questToComplete;
    }
}
