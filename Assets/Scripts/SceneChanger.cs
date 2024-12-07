using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour
{
    public string scene_to_load;
    public void LoadLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hello");
        SceneManager.LoadScene(scene_to_load);
    }

}
