using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestOrderAssigner {
    private static int nQuests = 0;

    public static void GetOrderNumber(Quest quest) {
        quest.orderNumber = nQuests;
        nQuests++;
    }
}
