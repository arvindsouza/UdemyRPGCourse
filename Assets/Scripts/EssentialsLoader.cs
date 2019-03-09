using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{

    public GameObject uiScreen, player, gameManager, audioManager, battleManager;

    // Start is called before the first frame update
    void Start()
    {
        if (UITransition.instance == null)
            Instantiate(uiScreen);

        if (PlayerController.instance == null)
            Instantiate(player);
        if (GameManager.instance == null)
            Instantiate(gameManager);

        if (AudioManager.instance == null)
            Instantiate(audioManager);

        if (BattleManager.instance == null)
            Instantiate(battleManager);
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
