﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EspatulaMove : MonoBehaviour {

	public float speed = 1;
	public float pivotOffset = 5;
	public float maxVertical, maxHorizontal;
	public float maxPositionOffset;
	public bool attacking = false;
	Animator anim;

	GameManager gameManager;

	public int tries = 3;

	public Text txt;
	public void init(GameManager gm)
	{
	
		gameManager = gm;
		txt.text = tries.ToString();
	}

	// Use this for initialization
	void Start () {
		maxVertical = Camera.main.orthographicSize;    
		maxVertical -= maxPositionOffset;

		maxHorizontal = maxVertical * Screen.width / Screen.height;
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!attacking) {
			Vector2 movement = new Vector2 (InputManager.Instance.GetAxisHorizontal (), InputManager.Instance.GetAxisVertical ());
			Vector3 newPosition = transform.position + (Vector3)movement * speed * Time.deltaTime;
			if (newPosition.x  > maxHorizontal) {
				newPosition.x = maxHorizontal;
			} else if (newPosition.x < -maxHorizontal) {
				newPosition.x = -maxHorizontal;
			}

			if (newPosition.y > maxVertical+pivotOffset) {
				newPosition.y = pivotOffset +maxVertical;
			} else if (newPosition.y < pivotOffset-maxVertical) {
				newPosition.y = pivotOffset-maxVertical;
			}

			transform.position = newPosition;

			if (InputManager.Instance.GetButtonDown (InputManager.MiniGameButtons.BUTTON1)) {
				Attack();
			}
		}
	}

	void Attack(){
		attacking = true;
		anim.SetTrigger ("attack");
	}
	public void StopAttack(){
		attacking = false;
		tries--;
		txt.text = tries.ToString();

		if (tries <= 0) {
			gameManager.EndGame (IMiniGame.MiniGameResult.LOSE);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
	
		if (other.gameObject.CompareTag ("Player")) {
			gameManager.EndGame (IMiniGame.MiniGameResult.WIN);
		}
	}
}
