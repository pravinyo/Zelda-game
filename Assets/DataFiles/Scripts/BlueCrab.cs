using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCrab : MonoBehaviour {

	public int health;
	public float speed;
	int direction;
	float timer=1f;
	float changeTimer = 0.2f;
	bool shouldChange;
	public GameObject particleEffect;
	SpriteRenderer spriteRenderer;
	public Sprite FacingUp,FacingDown,FacingLeft,FacingRight;
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		direction = Random.Range(0,3);
		shouldChange = false;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if(timer<=0){
			timer=1f;
			direction = Random.Range(0,3);
		}
		Movement();
		if(shouldChange){
			changeTimer -= Time.deltaTime;
			if(changeTimer<=0){
				changeTimer = 0.2f;
				shouldChange = false;
			}
		}
	}

	void Movement(){
		if(direction==0){
			transform.Translate(0,-speed*Time.deltaTime,0);
			spriteRenderer.sprite = FacingDown;
		}else if(direction == 1){
			transform.Translate(-speed*Time.deltaTime,0,0);
			spriteRenderer.sprite = FacingLeft;
		}else if (direction == 2)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            spriteRenderer.sprite = FacingRight;
        }else if (direction == 3)
        {
            transform.Translate(0,speed * Time.deltaTime, 0);
            spriteRenderer.sprite = FacingUp;
        }
	}

	void OnCollisionEnter2D(Collision2D col){
		 if(col.gameObject.tag == "Player"){
			 health--;
			 if(!col.gameObject.GetComponent<Player>().invisibilityFrame){
                col.gameObject.GetComponent<Player>().currentHealth--;
                col.gameObject.GetComponent<Player>().invisibilityFrame=true;
			 }
            if (health <= 0)
            {
                Instantiate(particleEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
		 }

		 if(col.gameObject.tag == "wall"){
			 if(shouldChange) return;
			 if(direction == 0){
				 direction =1;
			 }else if(direction == 1){
				 direction =0;
			 }else if(direction ==2){
				 direction =3;
			 }else if(direction == 3){
				 direction =2;
			 }
			 shouldChange = true;
		 }
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Sword"){
			health--;
			if(health<=0){
				Instantiate(particleEffect,transform.position,transform.rotation);
				Destroy(gameObject);
			}
			col.GetComponent<Sword>().createParticle();
			GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack=true;
			Destroy(col.gameObject);
		}
	}
}
