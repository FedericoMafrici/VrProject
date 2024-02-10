using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestOrderAssigner {
    private static int nQuests = 0;

    public static int GetOrderNumber() {
        int result = nQuests;
        nQuests++;
        return result;
    }
}
