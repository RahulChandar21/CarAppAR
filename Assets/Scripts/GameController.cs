using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static string currentSelectedCar = "myLamboConvert";
    //Static variable created to store the currently chosen car and pass it on to different scene.
    //Static variable value remains stable, does not vary relative to instances..

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void applicationQuit()
    {
        Application.Quit();
    }

    public void loadMainScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
