using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public Text dialogText, nameText;
    public GameObject dialogBox, nameBox;
    public string[] dialogLines;
    public int curLine;
    private bool firstStart = true;

    public static DialogManager instance;

    private string questToMark;
    private bool markQuestComplete;
    private bool shouldMarkQuest;

    // Start is called before the first frame update
    void Start()
    {
        //dialogText.text = dialogLines[curLine];
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogBox.activeInHierarchy)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                if (!firstStart)
                {
                    curLine++;

                    if (curLine >= dialogLines.Length)
                    {
                        dialogBox.SetActive(false);
                        GameManager.instance.dialogActive = false;


                        if (shouldMarkQuest)
                        {
                            shouldMarkQuest = false;
                            if (markQuestComplete)
                            {
                                QuestManager.instance.MarkQuestComplete(questToMark);
                            } else
                            {
                                QuestManager.instance.MarkQuestIncomplete(questToMark);
                            }
                        }
                    }
                    else
                    {
                        CheckIfName();
                        dialogText.text = dialogLines[curLine];
                    }
                }
                else
                {
                    firstStart = false;
                }
            }
        }
    }

    public void ShowDialog(string[] theLines, bool isPerson)
    {
        dialogLines = theLines;
        curLine = 0;
        CheckIfName();
        GameManager.instance.dialogActive = true;
        dialogText.text = dialogLines[curLine];
        dialogBox.SetActive(true);

        nameBox.SetActive(isPerson);
    }

    public void CheckIfName()
    {
        if (dialogLines[curLine].StartsWith("n-"))
        {
            nameText.text = dialogLines[curLine].Replace("n-","");
            curLine++;
        }
    }

    public void ShouldActivateQuestAtEnd(string questName, bool markComplete)
    {
        questToMark = questName;
        markQuestComplete = markComplete;

        shouldMarkQuest = true;
    }
}
