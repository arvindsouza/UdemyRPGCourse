using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    public string transitionName;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerController.instance)
            if (transitionName == PlayerController.instance.areaTransitionName)
        {
                UITransition.instance.fadeFromBlack();
                GameManager.instance.fadingBetAreas = false;
            PlayerController.instance.transform.position = gameObject.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
