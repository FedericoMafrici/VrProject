using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public static class QuestIndicatorDatabase {
    //a table that contains all QuestIDs of quests that have requested to show the associated indicators
    //each QuestID is associated to a function that needs to be called by the "Indicator" component in order to determine wether the indicator should be shown or not
    public static HashSet<QuestID> indicatorDatabase = new HashSet<QuestID>();
    public static event Action<QuestID> IndicatorShowEvent;
    public static event Action<QuestID> IndicatorHideEvent;

    public static void RequestShowIndicators(QuestID questId) {
        indicatorDatabase.Add(questId);
        if (IndicatorShowEvent!= null) {
            IndicatorShowEvent(questId);
        }
    }

    public static void RequestHideIndicators(QuestID questId) {
        indicatorDatabase.Remove(questId);
        if (IndicatorHideEvent!= null) {
            IndicatorHideEvent(questId);
        }
    }

    public static void Reset() {

        //may break, if so just do a while loop that keeps removing the element with position 0 until there are no elements left
        //do NOT use indicatorDatabase.Clear() or needed events will not be thrown
        foreach(QuestID questID in indicatorDatabase) {
            indicatorDatabase.Remove(questID);
        }
    }
  
}

public class IndicatorEventArgs : EventArgs {
    public QuestID questId;

    IndicatorEventArgs(QuestID id) {
        questId = id;
    }
}
