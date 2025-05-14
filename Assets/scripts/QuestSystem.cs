using System.Collections.Generic;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{

    //list of all the quests
    public List<Quest> quests = new List<Quest>(); 

    private void Start()
    {
       //adding quests to the list
        quests.Add(new Quest("buy monster", "go to the store to buy monster", "purchase monster energy"));
        quests.Add(new Quest("play 4 hours of overwatch", "get mad at the overwatch competetive mode", "enjoy overwatch (difficulty : impossible)"));
    }

    public void StartQuest(string questName)
    {
        Quest quest = GetQuestByName(questName);
        if (quest != null && quest.state == QuestState.NotStarted)
        {
            quest.StartQuest();
        }
        else
        {
            Debug.Log("Quest already started or does not exist.");
        }
    }

    public void CompleteQuest(string questName)
    {
        Quest quest = GetQuestByName(questName);
        if (quest != null && quest.state == QuestState.InProgress)
        {
            quest.CompleteQuest();
        }
        else
        {
            Debug.Log("Quest not in progress or does not exist.");
        }
    }

    private Quest GetQuestByName(string questName)
    {
        foreach (var quest in quests)
        {
            if (quest.questName == questName)
                return quest;
        }
        return null;
    }
}
