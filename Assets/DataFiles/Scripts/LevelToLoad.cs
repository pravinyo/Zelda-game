﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelToLoad : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D col)
    {
		if(col.gameObject.tag == "Player"){
			GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().saveGame();
			SceneManager.LoadScene(1);
		}
	}
}
