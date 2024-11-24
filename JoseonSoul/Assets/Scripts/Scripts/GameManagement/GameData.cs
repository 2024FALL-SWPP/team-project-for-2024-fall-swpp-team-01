using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currentSceneIndex;
    public Vector3 lastVisitedFire;

    public GameData(int currentSceneIndex, Vector3 lastVisitedFire)
    {
        this.currentSceneIndex = currentSceneIndex;
        this.lastVisitedFire = lastVisitedFire;
    }
}
