using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    public string areaToLoad, areaTransitionName;
    public AreaEntrance theEntrance;
    public float loadDelay = 1f;
    private bool shouldLoadAfterFade;

    // Start is called before the first frame update
    void Start()
    {
        theEntrance.transitionName = areaTransitionName;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldLoadAfterFade)
            loadDelay -= Time.deltaTime;
        if (loadDelay <= 0)
        {
            shouldLoadAfterFade = false;
            GameManager.instance.fadingBetAreas = true;
            SceneManager.LoadScene(areaToLoad);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //SceneManager.LoadScene(areaToLoad);
            shouldLoadAfterFade = true;
            UITransition.instance.fadeToBlack();
            PlayerController.instance.areaTransitionName = areaTransitionName;
        }
    }
}
