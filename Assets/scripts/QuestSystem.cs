using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSystem : MonoBehaviour
{

    //list of all the quests
    public List<Quest> quests = new List<Quest>();

    // UI Text to display current quest status
    public Text questText;  

    private void Start()
    {
        //adding quests to the list
        Quest fetchQuest = QuestFactory.CreateFetchQuest("buy monster", "go to the store to buy monster", "purchase monster energy");
        Quest objectiveQuest = QuestFactory.CreateObjectiveQuest("play 4 hours of overwatch", "get mad at the overwatch competetive mode", "enjoy overwatch (difficulty : impossible)");

        quests.Add(fetchQuest);
        quests.Add(objectiveQuest);

        DisplayQuestInf();
    }

    public void StartQuest(string questName)
    {
        // Start a quest by its name
        Quest quest = GetQuestByName(questName);
        if (quest != null && quest.state == QuestState.NotStarted)
        {
            quest.StartQuest();
            DisplayQuestInf();
        }
        else
        {
            Debug.Log("Quest already started or does not exist.");
        }
    }

    public void CompleteQuest(string questName)
    {
        // Complete a quest by its name
        Quest quest = GetQuestByName(questName);
        if (quest != null && quest.state == QuestState.InProgress)
        {
            quest.CompleteQuest();
            DisplayQuestInf();
        }
        else
        {
            Debug.Log("Quest not in progress or does not exist.");
        }
    }

    private Quest GetQuestByName(string questName)
    {
        // Find the quest by its name
        foreach (var quest in quests)
        {
            if (quest.questName == questName)
                return quest;
        }
        return null;
    }


    #region observer pattern
    private void DisplayQuestInf()
    {
        // Display the current quest status in UI
        foreach (var quest in quests)
        {
            if (quest.state == QuestState.InProgress)
            {
                questText.text = $"Quest: {quest.questName} - {quest.objective}";
                return;  
            }
        }

        //no active quests
        questText.text = "No active quests";  
    }
    #endregion
}
