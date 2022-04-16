using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void Level3()
    {

        SceneManager.LoadScene("Level 3");
    }


    public void Level2()
    {

        SceneManager.LoadScene("Level 2");
    }


    public void Level1()
    {

        SceneManager.LoadScene("Level1");
    }


    public void MenuScene()
    {
        SceneManager.LoadScene("menu");
    }



}
