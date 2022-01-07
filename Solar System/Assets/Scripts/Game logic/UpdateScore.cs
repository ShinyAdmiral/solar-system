using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScore : MonoBehaviour
{
    [SerializeField] Text text = null;
    [SerializeField] Game game = null;

    // Update is called once per frame
    void Update()
    {
        text.text = "Score\n" + game.score.ToString();
    }
}
