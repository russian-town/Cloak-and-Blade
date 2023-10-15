using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;
using Agava.WebUtility;
using System;

public class CloudSave
{
    public event Action<string> DataLoaded;
    public event Action<string> ErrorCallback;

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
    }

    public bool TryLoadCloudSaves()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (YandexGamesSdk.IsInitialized == false)
            return null;

        if (PlayerAccount.IsAuthorized)
            PlayerAccount.GetCloudSaveData(DataLoaded, ErrorCallback);

            return true;
#endif

        return false;
    }

    public PlayerData LoadLocalSaves()
    {
        if (UnityEngine.PlayerPrefs.HasKey(Constants.PlayerProgress) == false)
            return null;

        string data = UnityEngine.PlayerPrefs.GetString(Constants.PlayerProgress);

        if (string.IsNullOrEmpty(data))
            return null;

        PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
        return playerData;
    }
}
