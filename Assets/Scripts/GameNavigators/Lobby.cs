using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{

    public void OnClickSinglePlayer()
    {
        //Loads the singleplayer scene
        SceneManager.LoadScene("SinglePlayer");
    }

    public void OnClickMultiPlayer()
    {
        //Loads the multiplayer scene
        SceneManager.LoadScene("Multiplayer_Launcher");
    }

}
