using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;
using Agava.WebUtility;

public class CloudSave
{
    public void Save(PlayerData data)
    {
        string saveData = JsonUtility.ToJson(data);
        PlayerAccount.SetCloudSaveData(saveData);
    }

    public void Load()
    {
        PlayerAccount.GetCloudSaveData();
    }
}
