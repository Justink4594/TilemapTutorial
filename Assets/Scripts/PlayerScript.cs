using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public Text score;
    public Text lives;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public AudioClip  musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;
    public float checkRadius;
    public LayerMask allGround;
    Animator anim;

    private bool isOnGround;
    private bool facingRight = true;
    public Transform groundcheck;
    private Rigidbody2D rd2d;
    private int scorevalue;
    private int livesvalue;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        scorevalue = 0;
        livesvalue = 3;

        SetScoreText();
        SetlivesText();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        musicSource.clip = musicClipOne;
        musicSource.Play();
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }

        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if (vertMovement > 0)
        {
            anim.SetInteger("State",2);
        }

        if (vertMovement == 0)
        {
            anim.SetInteger("State",0);
        }

        if (hozMovement > 0)
        {
            anim.SetInteger("State",1);
        }

        if (hozMovement < 0)
        {
            anim.SetInteger("State",1);
        }


        if (hozMovement > 0 && vertMovement > 0)
        {
            anim.SetInteger("State" , 2);
        }

        if (hozMovement <0 && vertMovement >0)
        {
            anim.SetInteger("State" , 2);
        }



        
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State",0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State",0);
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetInteger("State", 2);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetInteger("State",0);
        }
        
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x*-1;
        transform.localScale = Scaler;
    }
    void SetScoreText()
    {
        score.text = "Score:" + scorevalue.ToString();
        if (scorevalue >= 8)
        {
            winTextObject.SetActive(true);
            speed = 0;
            musicSource.clip = musicClipTwo;
            musicSource.Play();
        }
    }

    void SetlivesText()
    {
        lives.text = "Lives:" + livesvalue.ToString();
        if (livesvalue <= 0)
        {
            loseTextObject.SetActive(true);
            speed = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            Destroy(collision.collider.gameObject);
            scorevalue = scorevalue + 1;
            SetScoreText();
        } 
        if ((scorevalue == 4) && (collision.collider.tag == ("Coin")))
         {
            transform.position = new Vector3(53.0f,0.0f,0.0f);
            livesvalue = 3;
            SetlivesText();
         }
         
        if (collision.collider.tag == "Enemy")
        {
            Destroy(collision.collider.gameObject);
            livesvalue = livesvalue - 1;
            SetlivesText();
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); //the 3 in this line of code is the player's "jumpforce," and you change that number to get different jump behaviors.  You can also create a public variable for it and then edit it in the inspector.
            }
        }
    }
}