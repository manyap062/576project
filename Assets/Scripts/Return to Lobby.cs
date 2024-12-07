using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturntoLobby : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {

        //If Space is hit while in a room, return to the lobby
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
