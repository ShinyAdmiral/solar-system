using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetScore : MonoBehaviour
{
    //game refernce
    Game game = null;

    //output
    [SerializeField] Text text = null;

    // Start is called before the first frame update
    void Start()
    {
        //get results
        game = GameObject.Find("GameManager").GetComponent<Game>();
        text.text = "Your Final\nScore is\n" + game.score.ToString();
        Destroy(game.gameObject);
    }
}
