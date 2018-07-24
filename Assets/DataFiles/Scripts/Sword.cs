using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

	// Use this for initialization
	float timer = 0.15f;
	public bool specialAttack;
	public GameObject swordParticle;
	float specialTimer = 1f;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if(timer<=0){
			GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetInteger("attackDir",5);
		}
		if(!specialAttack){
            if (timer <= 0)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
                //In get component angle bracker we wrie script name
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
                Destroy(gameObject);
            }
		}else{
			specialTimer -= Time.deltaTime;
			if(specialTimer<=0){
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
				Instantiate(swordParticle,transform.position,transform.rotation);
                Destroy(gameObject);
			}
		}
	}

	public void createParticle(){
        Instantiate(swordParticle, transform.position, transform.rotation);
	}
}
