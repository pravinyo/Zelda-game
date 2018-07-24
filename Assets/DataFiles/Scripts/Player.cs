using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	// Use this for initialization
	public float speed;
	Animator anim;
	public Image[] hearts;
	public int maxHealth;
	public bool invisibilityFrame;
	public int currentHealth;
	public GameObject sword;
	public float thrustPower;
	public bool canMove;
	public bool canAttack;
	SpriteRenderer sr;
	float invisibilityFrameTimer = 1f;
    //float timer = 0.15f;
	void Start () {
		anim = GetComponent<Animator>();
		if(PlayerPrefs.HasKey("maxHealth")){
			loadGame();
		}else
		{
            currentHealth = maxHealth;
		}
		
		canMove = true;
		canAttack =true;
		invisibilityFrame = false;
		sr = GetComponent<SpriteRenderer>();
		getHealth();
	}

	void getHealth(){
		for(int i=0;i<hearts.Length;i++){
            hearts[i].gameObject.SetActive(false);
		}
		for(int i=0;i<currentHealth;i++){
			hearts[i].gameObject.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		Movement();
		if(Input.GetKeyDown(KeyCode.P)){
			if(currentHealth<1){
                currentHealth--;
			}
		}
        if (Input.GetKeyDown(KeyCode.L))
        {
            if(currentHealth>5){
                currentHealth++;
			}
        }
		if(Input.GetKeyDown(KeyCode.Space)){
			Attack();
		}
		if(invisibilityFrame == true){
			invisibilityFrameTimer -= Time.deltaTime;
			int temp = Random.Range(0,100);
			if(temp<50){
                sr.enabled = false;
			}else{
                sr.enabled = true;
			}
			if(invisibilityFrameTimer<=0){
				invisibilityFrameTimer = 1f;
				invisibilityFrame = false;
                sr.enabled = true;
			}
		}
        getHealth();
	}

	void Attack(){
		if(!canAttack) return;
		canMove = false;
		canAttack = false;
		thrustPower = 250;
		GameObject newSword = Instantiate(sword,transform.position,sword.transform.rotation);
		#region //swordRotation
		if(currentHealth == maxHealth){
			newSword.GetComponent<Sword>().specialAttack=true;
			canMove = true;
			thrustPower=550;
		}
		int swordDir = anim.GetInteger("dir");
		anim.SetInteger("attackDir",swordDir);
		if(swordDir == 0){
			newSword.transform.Rotate(0,0,0);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
		}else if(swordDir == 1){
			newSword.transform.Rotate(0,0,-180);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.down * thrustPower);
		}else if (swordDir == 2)
        {
            newSword.transform.Rotate(0,0,90);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.left * thrustPower);
        }else if (swordDir == 3)
        {
            newSword.transform.Rotate(0, 0,-90);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
        }
		#endregion
		
	}

	void Movement(){
		if(!canMove) return;
		if(Input.GetKey(KeyCode.W)){
			transform.Translate(new Vector3(0,speed*Time.deltaTime,0));
			anim.SetInteger("dir",0);
			anim.speed =1;
		}else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
            anim.SetInteger("dir", 1);
            anim.speed = 1;
        }else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime,0, 0));
            anim.SetInteger("dir", 2);
            anim.speed = 1;
        }else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            anim.SetInteger("dir", 3);
            anim.speed = 1;
        }else{
			anim.speed =0;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if(currentHealth<=0){
			SceneManager.LoadScene(0);
		}
        if (col.gameObject.tag == "EnemyBullet")
        {
			if(!invisibilityFrame)
            {
                currentHealth--;
				invisibilityFrame = true;
			}
			col.gameObject.GetComponent<Bullet>().createParticle();
            Destroy(col.gameObject);
        }

		if(col.gameObject.tag == "Potion"){
			currentHealth = maxHealth;
			Destroy(col.gameObject);
            if (maxHealth >= 5) return;
			maxHealth++;
            currentHealth = maxHealth;
		}
	}

	public void saveGame(){
		PlayerPrefs.SetInt("maxHealth",maxHealth);
		PlayerPrefs.SetInt("currentHealth",currentHealth);
	}

	public void loadGame(){
		maxHealth = PlayerPrefs.GetInt("maxHealth");
		currentHealth = PlayerPrefs.GetInt("currentHealth");
	}
}
