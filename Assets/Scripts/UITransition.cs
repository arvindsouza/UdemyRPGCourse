using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UITransition : MonoBehaviour
{
    public Image transitionScreen;
    public float fadeSpeed;

    public static UITransition instance;

    private bool shouldFadeToBlack, shouldFadeFromBlack;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFadeToBlack)
        {
            transitionScreen.color = new Color(transitionScreen.color.r, transitionScreen.color.g, transitionScreen.color.b, Mathf.MoveTowards(transitionScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (transitionScreen.color.a == 1f)
                shouldFadeToBlack = false;
        }
        else
      if (shouldFadeFromBlack)
        {
            transitionScreen.color = new Color(transitionScreen.color.r, transitionScreen.color.g, transitionScreen.color.b, Mathf.MoveTowards(transitionScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (transitionScreen.color.a == 0f)
                shouldFadeFromBlack = false;
        }
        }

    public void fadeToBlack()
    {
        shouldFadeToBlack = true;
        shouldFadeFromBlack = false;
    }

    public void fadeFromBlack()
    {
        shouldFadeToBlack = false;
        shouldFadeFromBlack = true;
    }
}
