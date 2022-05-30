using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject player;

    public GameObject RestartPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerDeath();
    }

    void CheckPlayerDeath()
    {
        if (player.GetComponent<Health>().isDead)
        {
            //Active UI
            RestartPanel.SetActive(true);
        }
    }

    public void ReStartScene()
    {
        SceneManager.LoadScene(0);
    }
}
