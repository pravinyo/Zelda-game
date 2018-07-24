using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour {

	Animator anim;
	public float speed;
	int direction;
    float changeTimer = 0.2f;
    bool shouldChange;
	float directionTimer=0.7f;
	public int health;
	bool canAttack;
	float attackTimer = 2f;
	public GameObject deathParticle;
	public GameObject projectile;
	public float thrustPower;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		canAttack =false;
        shouldChange = false;
	}
	
	// Update is called once per frame
	void Update () {
		directionTimer-=Time.deltaTime;
		if(directionTimer<=0){
			directionTimer=0.7f;
			direction = Random.Range(0,3);
		}
		Movement();
		attackTimer -= Time.deltaTime;
		if(attackTimer<=0){
			attackTimer = 2f;
			canAttack = true;
		}
		attack();
        if (shouldChange)
        {
            changeTimer -= Time.deltaTime;
            if (changeTimer <= 0)
            {
                changeTimer = 0.2f;
                shouldChange = false;
            }
        }
		
	}
	void attack(){
		if(!canAttack) return;
		canAttack = false;

		GameObject newProjectile = Instantiate(projectile,transform.position,transform.rotation);
        if (direction == 0)
        {
            newProjectile.transform.Rotate(0, 0, 0);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
        }
        else if (direction == 1)
        {
            newProjectile.transform.Rotate(0, 0, -180);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.down * thrustPower);
        }
        else if (direction == 2)
        {
            newProjectile.transform.Rotate(0, 0, 90);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.left * thrustPower);
        }
        else if (direction == 3)
        {
            newProjectile.transform.Rotate(0, 0, -90);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
        }
	}
	void Movement(){
		if(direction ==0){
			transform.Translate(0,-speed*Time.deltaTime,0);
			anim.SetInteger("dir",direction);
		}else if (direction == 1)
        {
            transform.Translate(0,speed * Time.deltaTime, 0);
            anim.SetInteger("dir", direction);
        }else if (direction == 2)
        {
            transform.Translate( -speed * Time.deltaTime,0, 0);
            anim.SetInteger("dir", direction);
        }else if(direction ==3){
			transform.Translate(speed*Time.deltaTime,0,0);
			anim.SetInteger("dir",direction);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag =="Sword"){
			health--;
			col.gameObject.GetComponent<Sword>().createParticle();
			GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
			Destroy(col.gameObject);
			if(health<=0){
				Instantiate(deathParticle,transform.position,transform.rotation);
				Destroy(gameObject);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "Player")
        {
            health--;
            if (!col.gameObject.GetComponent<Player>().invisibilityFrame)
            {
                col.gameObject.GetComponent<Player>().currentHealth--;
                col.gameObject.GetComponent<Player>().invisibilityFrame = true;
            }
            if (health <= 0)
            {
                Instantiate(deathParticle, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }

        if (col.gameObject.tag == "wall")
        {
            if (shouldChange) return;
            if (direction == 0)
            {
                direction = 1;
            }
            else if (direction == 1)
            {
                direction = 0;
            }
            else if (direction == 2)
            {
                direction = 3;
            }
            else if (direction == 3)
            {
                direction = 2;
            }
            shouldChange = true;
        }
	}
}
