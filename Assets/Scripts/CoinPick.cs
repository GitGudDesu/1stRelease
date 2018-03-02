using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CoinPick : MonoBehaviour {
    public Text scoreText;
    private GameObject[] coins;
    static Random _random = new Random();
    // Use this for initialization
    void Start()
    {
        TextMesh textObject = GameObject.Find("New Text").GetComponent<TextMesh>();
        char c = (char)('A' + Random.Range(0, 26));
        textObject.text = c.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        char c = (char)('A' + Random.Range(0, 26));
        scoreText.text = ((char)c).ToString();
    }
}
