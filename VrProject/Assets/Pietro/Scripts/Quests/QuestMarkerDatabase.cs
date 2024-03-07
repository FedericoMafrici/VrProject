using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Search;
#endif
using UnityEngine;

public struct AdditionalQuestInfo {
    public bool isCollectingQuest;

    public static AdditionalQuestInfo Default() {
        AdditionalQuestInfo defaultInfo;
        defaultInfo.isCollectingQuest = false;
        return defaultInfo;
    }
}

public static class QuestMarkerDatabase {
    //a table that contains all QuestIDs of quests that have requested to show the associated indicators
    //each QuestID is associated to a function that needs to be called by the "Indicator" component in order to determine wether the indicator should be shown or not
    public static Dictionary<QuestID, AdditionalQuestInfo> markerDatabase = new Dictionary<QuestID, AdditionalQuestInfo>();
    public static event Action<MarkerEventArgs> MarkerShowEvent;
    public static event Action<QuestID> MarkerHideEvent;

    public static void RequestShowMarkers(QuestID questID) {
        RequestShowMarkers(questID, AdditionalQuestInfo.Default());
    }

        public static void RequestShowMarkers(QuestID questId, AdditionalQuestInfo questInfo) {
        markerDatabase.Add(questId, questInfo);
        if (MarkerShowEvent!= null) {
            MarkerShowEvent(new MarkerEventArgs(questId, questInfo));
        }
    }

    public static void RequestHideMarkers(QuestID questId) {
        markerDatabase.Remove(questId);
        if (MarkerHideEvent!= null) {
            MarkerHideEvent(questId);
        }
    }
    
    public static void Reset() {

        //may break, if so just do a while loop that keeps removing the element with position 0 until there are no elements left
        //do NOT use indicatorDatabase.Clear() or needed events will not be thrown
        foreach(QuestID questID in markerDatabase.Keys) {
            RequestHideMarkers(questID);
        }
    }
  
}

public class MarkerEventArgs : EventArgs {
    public QuestID questId;
    public AdditionalQuestInfo questInfo;

    public MarkerEventArgs(QuestID id, AdditionalQuestInfo info) {
        questId = id;
        questInfo = info;
    }
}
