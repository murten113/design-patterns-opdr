using System;
using UnityEngine;

public enum QuestState
{
    //all the possible states of a quest
    NotStarted,
    InProgress,
    Completed
}


public class Quest
{
    
    public string questName;
    public string description;
    public QuestState state;
    public string objective;

    public Quest(string name, string desc, string obj)
    {
        // Constructor to initialize the quest
        questName = name;
        description = desc;
        objective = obj;
        state = QuestState.NotStarted;
    }

    public void StartQuest()
    {
        state = QuestState.InProgress;
        Debug.Log($"{questName} started: {description}");
    }

    public void CompleteQuest()
    {
        state = QuestState.Completed;
        Debug.Log($"{questName} completed!");
    }
}
