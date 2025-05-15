
// Factory Pattern
public class QuestFactory
{
    // Factory method to create a Fetch or Objective quest
    public static Quest CreateFetchQuest(string questName, string description, string objective)
    {
        return new Quest(questName, description, objective);
    }

    public static Quest CreateObjectiveQuest(string questName, string description, string objective)
    {
        return new Quest(questName, description, objective); 
    }
}
