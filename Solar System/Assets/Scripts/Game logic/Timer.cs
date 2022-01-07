using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] float remainingTime = 30f;
    [SerializeField] Text text = null;
    [SerializeField] Game game = null;

    // Update is called once per frame
    void Update()
    {
        //update time
        remainingTime -= Time.deltaTime;

        text.text = "Time\n"+ Mathf.Round(remainingTime).ToString();

        //move Rooms
        if (remainingTime < 0) {
            game.stop = true;
            SceneManager.LoadScene("Results");
        }
    }
}
