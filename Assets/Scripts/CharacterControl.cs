using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour
{

    private CharacterController controller;
    private float speed = 2.5f;
    private float gravity = 12.0f;
    private Vector3 moveVector;
    private float verticalVelocity = 0.0f;

    //player movement restriction
    private float animationDuration = 3.0f;
    private float startTime;
    private bool isDeath = false;
    public Animator anim;

    //current word text variables
    public Text partialWordText;
    private String grabLetter;
    private String partialWord;


    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();

        controller = GetComponent<CharacterController>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update() {

        if(isDeath)  {
            return;
        }

        if(Time.time - startTime < animationDuration) {
            controller.Move(Vector3.forward * speed * Time.deltaTime);
            return;
        }


        moveVector = Vector3.zero;

        if (controller.isGrounded) {
            verticalVelocity = 0f;
        } else {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        //x - left and right
        moveVector.x = Input.GetAxisRaw("Horizontal") * speed/2;
        if(Input.GetMouseButton (0))
        {
            if (Input.mousePosition.x > Screen.width / 2)
                moveVector.x = 1;
            else
                moveVector.x = -1;
        }
        //y - up and down
        moveVector.y = verticalVelocity;
        //z - forward and backward
        moveVector.z = speed;

        controller.Move((moveVector* speed) * Time.deltaTime);

        if (Input.GetKeyDown("w"))
        {
            anim.Play("Jump", -1, 0f);
        }

        if (Input.GetKeyDown("s"))
        {
            anim.Play("SLIDE00", -1, 0f);
        }
    }


    public void SetSpeed(int modifier) {
        speed = 2.0f + modifier;
    }

    //when character collides with an object
    private void OnControllerColliderHit(ControllerColliderHit hit) {

        if (hit.point.z > transform.position.z + 0.1f && hit.gameObject.tag == "Enemy") { 
            anim.Play("DAMAGED01", -1, 0f);
            Death();
        }

        if(hit.point.z > transform.position.z + 0.1f && hit.gameObject.tag == "Coin") {
			
            //remove the coin from field of play
			Destroy(hit.gameObject);

            //take the letter associated with the coin
            grabLetter = hit.gameObject.GetComponentInChildren<TextMesh>().text.ToString();


            Debug.Log("LastLetterIssued: " + CoinPick.getCurrentLetter() + " CoinLetter " + grabLetter);

            //determine if that letter was correct
			if (grabLetter.Equals(CoinPick.getCurrentLetter())) {
				//update the partial word text
				partialWordText.text = null;
				partialWord = partialWord + grabLetter;
				partialWordText.text = partialWord;
				CoinPick.currentLetterIndex++;

				//check if word was spelled and then reset CoinPick indexes
				//while maintaining the word index
				if (partialWord.Equals(CoinPick.currentWord)) {
					int oldWordIndex = CoinPick.wordArrayIndex;
					CoinPick.ResetVars ();
					CoinPick.wordArrayIndex = oldWordIndex + 1;
					partialWord = null;

				}
				//increment letter index and return out of method
				return;

			}
            //end the game if the letter was incorrect
			else if (grabLetter.Equals(null)) {
				return;
			} 
			else 
			{
				Death ();
				return;
			}
        }
    }
    //call death and reset CoinPick indexes
    private void Death()
    {
        isDeath = true;
        GetComponent<Score>().OnDeath();
		CoinPick.ResetVars ();

    }
}
