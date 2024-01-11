using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public static class QuestMarkerDatabase {
    //a table that contains all QuestIDs of quests that have requested to show the associated indicators
    //each QuestID is associated to a function that needs to be called by the "Indicator" component in order to determine wether the indicator should be shown or not
    public static HashSet<QuestID> markerDatabase = new HashSet<QuestID>();
    public static event Action<QuestID> MarkerShowEvent;
    public static event Action<QuestID> MarkerHideEvent;

    public static void RequestShowIndicators(QuestID questId) {
        markerDatabase.Add(questId);
        if (MarkerShowEvent!= null) {
            MarkerShowEvent(questId);
        }
    }

    public static void RequestHideIndicators(QuestID questId) {
        markerDatabase.Remove(questId);
        if (MarkerHideEvent!= null) {
            MarkerHideEvent(questId);
        }
    }

    public static void Reset() {

        //may break, if so just do a while loop that keeps removing the element with position 0 until there are no elements left
        //do NOT use indicatorDatabase.Clear() or needed events will not be thrown
        foreach(QuestID questID in markerDatabase) {
            markerDatabase.Remove(questID);
        }
    }
  
}

public class IndicatorEventArgs : EventArgs {
    public QuestID questId;

    IndicatorEventArgs(QuestID id) {
        questId = id;
    }
}
