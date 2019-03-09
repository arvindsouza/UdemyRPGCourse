using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BattleTargetButton : MonoBehaviour
{
    public string moveName;
    public int activeFighterTarget;
    public Text targetName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Press()
    {
        BattleManager.instance.PlayerAttack(moveName, activeFighterTarget);
    }
}
