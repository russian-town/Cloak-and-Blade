using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;
using Agava.WebUtility;
using System;

public class CloudSave
{
    private PlayerData _cloudSaveData;

    public void Save(PlayerData data)
    {
        string saveData = JsonUtility.ToJson(data);

#if UNITY_WEBGL && !UNITY_EDITOR
        if (PlayerAccount.IsAuthorized)
        {
            PlayerAccount.SetCloudSaveData(saveData);
            return;
        }
#endif

        UnityEngine.PlayerPrefs.SetString(Constants.PlayerProgress, saveData);
        UnityEngine.PlayerPrefs.Save();
        Debug.Log("Saved.");
    }

    public PlayerData Load()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (PlayerAccount.IsAuthorized)
        {
            PlayerAccount.GetCloudSaveData(SaveCallback);
            return _cloudSaveData;
        }
#endif

        if (UnityEngine.PlayerPrefs.HasKey(Constants.PlayerProgress) == false)
            return null;

        string data = UnityEngine.PlayerPrefs.GetString(Constants.PlayerProgress);

        if (string.IsNullOrEmpty(data))
            return null;

        PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
        return playerData;
    }

    private void SaveCallback(string data)
    {
        _cloudSaveData = JsonUtility.FromJson<PlayerData>(data);
    }
}
