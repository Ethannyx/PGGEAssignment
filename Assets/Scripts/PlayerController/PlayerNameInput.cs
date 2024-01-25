using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class PlayerNameInput : MonoBehaviour
{
    private InputField playerNameInputField;
    private const string playerNamePrefKey = "PlayerName";

    // Start is called before the first frame update
    void Start()
    {
        InitializeInputField();
        SetDefaultPlayerName();
    }

    //Gets the component for the player to input name
    private void InitializeInputField()
    {
        playerNameInputField = GetComponent<InputField>();
    }

    //Register the input given by the player using playerPrefs
    private void SetDefaultPlayerName()
    {
        if (PlayerPrefs.HasKey(playerNamePrefKey))
        {
            string defaultName = PlayerPrefs.GetString(playerNamePrefKey);
            playerNameInputField.text = defaultName;
            SetPlayerName(defaultName);
        }
    }

    public void SetPlayerName(string playerName)
    {
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }

        PhotonNetwork.NickName = playerName;
        PlayerPrefs.SetString(playerNamePrefKey, playerName);

        Debug.Log("Nickname entered: " + playerName);
    }
}
