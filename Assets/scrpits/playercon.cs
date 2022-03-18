using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class playercon : MonoBehaviour
{
    public TextMeshProUGUI countText;
    public TextMeshProUGUI livesText;
    public GameObject WinTextObject;
    public GameObject LoseTextObject;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;
    private Rigidbody2D rd2d;
     public float speed;
     private bool facingRight = true;
     private bool isOnGround;
     public Transform groundcheck;
     public float checkRadius;
     public LayerMask allGround;
     public float jumpForce;
     private int count;
    private int lives;
    
     Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;
        lives = 3;
        count = 0;
        SetCountText();
        SetlivesText();
        WinTextObject.SetActive(false);
        LoseTextObject.SetActive(false);
        
    }
     void SetCountText() //the set count text this also teleports
    {
        countText.text = "Count: " + count.ToString();
         if (count == 4)
        {
            transform.position = new Vector2(44.0f, -4.4f);
             lives = 3;
            SetlivesText();
        }
        else if (count >= 8)
        {
             musicSource.clip = musicClipTwo;
        musicSource.Play();
        musicSource.loop = false;
            WinTextObject.SetActive(true);
        }
    }
     void SetlivesText() //the lives text, might need to change this so then the death animation can play
    {
        livesText.text = "lives: " + lives.ToString();
        if (lives == 0)
        {   
            LoseTextObject.SetActive(true);
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    
    void FixedUpdate()
    {
         float hozMovement = Input.GetAxis("Horizontal");

        float vertMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

          if (Input.GetKey("escape")) 
       {
       Application.Quit();
       }

       isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround); // ground check

        
        if (hozMovement == 0 && isOnGround) //plays idle animation when doing nothing and is on ground
        {
            anim.SetInteger("State",0);
        }
        else // plays the running animation when moving on the x-axis
        {
            anim.SetInteger ("State", 1);
        } 

        if (isOnGround == false)
        {
             anim.SetInteger("State", 2);
        }

      if (facingRight == false && hozMovement > 0) //this is the flip
     {
      Flip();   
     }

     else if (facingRight == true && hozMovement < 0)
     {
         Flip();
     }    
    }
    void Flip() //function for the flip
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
    private void OnTriggerEnter2D (Collider2D other) // the triggers
    {
         if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
         else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            lives = lives - 1;
            SetlivesText();
        }
    }
     private void OnCollisionStay2D(Collision2D collision) //the ground check to jump
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
               rd2d.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
            }
        }
    }
}
