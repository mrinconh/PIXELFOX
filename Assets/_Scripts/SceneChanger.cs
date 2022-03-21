using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    
    public void Level1()
    {

        SceneManager.LoadScene("level1");
    }


    public void MenuScene()
    {
        SceneManager.LoadScene("menu");
    }



}
