using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoalSeeker : MonoBehaviour
{
    Goal[] mGoals;
    Action[] mActions;
    Action mChangeOverTime;
    const float TICK_LENGTH = 5.0f;
    public TextMeshProUGUI display;

    void Start()
    {
        mGoals = new Goal[3];
        mGoals[0] = new Goal("Mac and Cheese", 4);
        mGoals[1] = new Goal("Salad", 3);
        mGoals[2] = new Goal("Tomato Soup", 3);

        mActions = new Action[6];
        mActions[0] = new Action("serve mac and cheese");
        mActions[0].targetGoals.Add(new Goal("Mac and Cheese", -3f));
        mActions[0].targetGoals.Add(new Goal("Salad", +2f));
        mActions[0].targetGoals.Add(new Goal("Tomato Soup", +1f));

        mActions[1] = new Action("serve mac and cheese");
        mActions[1].targetGoals.Add(new Goal("Mac and Cheese", -2f));
        mActions[1].targetGoals.Add(new Goal("Salad", +1f));
        mActions[1].targetGoals.Add(new Goal("Tomato Soup", +1f));

        mActions[2] = new Action("serve salad");
        mActions[2].targetGoals.Add(new Goal("Mac and Cheese", +2f));
        mActions[2].targetGoals.Add(new Goal("Salad", -4f));
        mActions[2].targetGoals.Add(new Goal("Tomato Soup", +2f));

        mActions[3] = new Action("serve salad");
        mActions[3].targetGoals.Add(new Goal("Mac and Cheese", +1f));
        mActions[3].targetGoals.Add(new Goal("Salad", -2f));
        mActions[3].targetGoals.Add(new Goal("Tomato Soup", +1f));

        mActions[4] = new Action("serve tomato soup");
        mActions[4].targetGoals.Add(new Goal("Mac and Cheese", +1f));
        mActions[4].targetGoals.Add(new Goal("Salad", +2f));
        mActions[4].targetGoals.Add(new Goal("Tomato Soup", -3f));

        mActions[5] = new Action("serve tomato soup");
        mActions[5].targetGoals.Add(new Goal("Mac and Cheese", 0f));
        mActions[5].targetGoals.Add(new Goal("Salad", 0f));
        mActions[5].targetGoals.Add(new Goal("Tomato Soup", -4f));

        mChangeOverTime = new Action("tick");
        mChangeOverTime.targetGoals.Add(new Goal("Mac and Cheese", +2f));
        mChangeOverTime.targetGoals.Add(new Goal("Salad", +1f));
        mChangeOverTime.targetGoals.Add(new Goal("Tomato Soup", +2f));

        Debug.Log("Starting. One hour passes every " + TICK_LENGTH + " seconds.");
        InvokeRepeating("Tick", 0f, TICK_LENGTH);

        Debug.Log("Hit Q to view Stats. Hit E to do something.");
    }

    void Tick()
    {
        foreach (Goal goal in mGoals)
        {
            goal.value += mChangeOverTime.GetGoalChange(goal);
            goal.value = Mathf.Max(goal.value, 0);
        }
    }

    void PrintGoals()
    {
        string goalString = "# of customers who want: " + "\n";
        foreach (Goal goal in mGoals)
        {
            goalString += goal.name + ": " + goal.value + "\n";
        }
        goalString += "Total Customers: " + CurrentDiscontentment();
        display.text = goalString;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            Action bestThingToDo = ChooseAction(mActions, mGoals);
            display.text = "I think I will " + bestThingToDo.name;

            foreach (Goal goal in mGoals)
            {
                goal.value += bestThingToDo.GetGoalChange(goal);
                goal.value = Mathf.Max(goal.value, 0);
            }

            //Debug.Log("New Stats: ");
            //PrintGoals();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("# of customers who want: ");
            PrintGoals();
        }
    }

    Action ChooseAction(Action[] actions, Goal[] goals)
    {
        Action bestAction = null;
        float bestValue = float.PositiveInfinity;

        foreach (Action action in actions)
        {
            float thisValue = Discontentment(action, goals);;
            if (thisValue < bestValue)
            {
                bestValue = thisValue;
                bestAction = action;
            }
        }

        return bestAction;
    }

    float Discontentment(Action action, Goal[] goals)
    {
        float discontentment = 0f;

        foreach (Goal goal in goals)
        {
            float newValue = goal.value + action.GetGoalChange(goal);
            newValue = Mathf.Max(newValue, 0);

            discontentment += goal.GetDiscontentment(newValue);
        }

        return discontentment;
    }

    float CurrentDiscontentment()
    {
        float total = 0f;
        foreach (Goal goal in mGoals)
        {
            total += (goal.value);
        }
        return total;
    }
}