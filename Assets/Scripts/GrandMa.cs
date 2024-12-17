using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GrandMa : MonoBehaviour
{
    public NavMeshAgent granny;
    public GameObject player;
    public TMPro.TextMeshProUGUI winText;
    public TMPro.TextMeshProUGUI loseText;
    public Button returnLobbyButton;
    public Button playAgainButton;

    // Start is called before the first frame update
    void Start()
    {
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        returnLobbyButton.gameObject.SetActive(false);
        playAgainButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = new Vector3();
        target = player.transform.position;
        granny.SetDestination(target);


    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "main_char")
        {
            loseText.gameObject.SetActive(true);
            Debug.Log("Game Over!");
            returnLobbyButton.gameObject.SetActive(true);
            playAgainButton.gameObject.SetActive(true);
            returnLobbyButton.onClick.AddListener(GoToLobby);
            playAgainButton.onClick.AddListener(RestartLevel);
        }

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

}

