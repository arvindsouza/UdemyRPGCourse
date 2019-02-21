using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{

    public GameObject uiScreen, player, gameManager;

    // Start is called before the first frame update
    void Start()
    {
      if(UITransition.instance == null)
           UITransition.instance = Instantiate(uiScreen).GetComponent<UITransition>();

        if (PlayerController.instance == null)
           PlayerController.instance = Instantiate(player).GetComponent<PlayerController>();

        if (GameManager.instance == null)
            Instantiate(gameManager);
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
