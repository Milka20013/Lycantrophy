using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData 
{
    [Serializable]
    public struct PlayerData
    {
        public PlayerData(Vector3 position)
        {
            this.position = position;
        }
        public Vector3 position;
    }

    public PlayerData playerData;
    public GameData()
    {
        playerData = new PlayerData(Vector3.zero);
    }
}
