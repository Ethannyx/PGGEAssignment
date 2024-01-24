using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    //Back button code which brings player back to menu
    public void toMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
