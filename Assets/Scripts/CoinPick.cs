using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data;
using Mono.Data.SqliteClient;
using UnityEditor;




public class CoinPick : MonoBehaviour {
    private GameObject[] coins;
    public Text wordText;
    static UnityEngine.Random _random = new UnityEngine.Random();

    //string thingys
    public ArrayList wordList = new ArrayList();
    public static String currentWord;
    public String letter;
    public static int wordArrayIndex = 0;
    public static int currentLetterIndex = 0;
    public static char[] letterArray = new char[20];
    private string chars = "abcdefghijklmnopqrstuvwxyz";
    public static int counter = 1;
    public static String lastCorrectLetter;


    // Use this for initialization
    void Start()
    {
        //direct db connection to where the db is stored in app
        //and open connection
        const string connectionString = "URI=file:Assets\\Plugins\\MumboJumbos.db";
        IDbConnection dbcon = new SqliteConnection(connectionString);
        dbcon.Open();

        //create query for user name
        IDbCommand dbcmd = dbcon.CreateCommand();
        const string sql =
            "SELECT * " +
            "FROM wordList";
        dbcmd.CommandText = sql;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            string currentWord = reader.GetString(2);

            Debug.Log(currentWord);

            wordList.Add(currentWord);
        }



    }

    // Update is called once per frame
    void Update() {
		coins = GameObject.FindGameObjectsWithTag("Coin");
		if (currentLetterIndex == 0) {
			wordText.text = currentWord;
		}

		//loop through all the coins and make sure they are value assigned
		for (int i = 0; i < coins.Length; i++) 
		{
			//create a textObject from the first coin object instance
			TextMesh textObject = coins[i].GetComponent<TextMesh> ();

			//check to see if we have written to coin
			if (textObject != null && textObject.text.Length == 0) { 
				
				//String Variables for Letter Updates
				String hold = textObject.text;
				currentWord = (string)wordList [wordArrayIndex];

				//convert word to char array
				letterArray = currentWord.ToCharArray ();

				//decide if we should change the letter on update
				if (hold.Length == 0) {
					letter = letterArray [currentLetterIndex].ToString ();

					if (counter % 2 == 0) {
						textObject.text = letter;
						lastCorrectLetter = letter;
						counter++;
					//if counter is odd then input a random letter
					} else {
						char c = chars [UnityEngine.Random.Range (0, 26)];
						if (((char)c).ToString () != letter) {
							textObject.text = ((char)c).ToString ();
							counter++;
						//if the random letter is the correct letter update lastCorrectLetter
						} else {
							textObject.text = letter;
							lastCorrectLetter = letter;
							counter++;
						}
					}
					Debug.Log ("Set " + letter + " " + currentWord);

				}
			}
		}
    }

    public static String getCurrentLetter()
    {
		return CoinPick.letterArray [currentLetterIndex].ToString ();
    }

    public static void ResetVars()
    {
        currentLetterIndex = 0;
        lastCorrectLetter = null;
        letterArray = null;
        wordArrayIndex = 0;
        counter = 0;
    }
}
