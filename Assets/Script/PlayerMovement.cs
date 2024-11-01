using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    @PlayersInoutActions controls;
    private float score;
    private float life;
    [SerializeField] Text scoreText;
    [SerializeField] Text lifeText;
    [SerializeField] Text FinalScoreText;
    [SerializeField] GameObject EndPanel;
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject Panel;
    [SerializeField] AudioSource Source;
    [SerializeField] AudioClip Run;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip die;
    [SerializeField] AudioClip coin;
    [SerializeField] AudioClip lose;
    [SerializeField] AudioClip win;
    public float LevelScore;
   

    private bool ch = false;
    private Rigidbody2D rb;
    private Animator anim;
    public float dirX = 0f;
    public float dirY = 0f;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;
    private enum MovementState { idle, running, jumping, falling }
    private MovementState state = MovementState.idle;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] GameObject menuPanel;
    private void Awake()
    {
        controls = new PlayersInoutActions();
        controls.Enable();

        controls.Player.Move.performed += ctx =>
        {

            dirX = ctx.ReadValue<float>();
        };

        controls.Player.Jump.performed += cty =>
        {
            dirY = cty.ReadValue<float>();
        };
    }

    void Start()
    {
        Time.timeScale = 1f;
        score = 0f;
        life = 3f;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        scoreText.text = "score : " + score.ToString();
        lifeText.text = "life : " + life.ToString();
        

        if(life == 0)
        {
            Source.clip = lose;
            Source.Play();
            EndPanel.SetActive(true);
            FinalScoreText.text = "score : " + score.ToString();
            Time.timeScale = 0f;
        }
    }

    public void jum()
    {
        Source.clip = jump;
        Source.Play();
    }
    void Update()
    {

        
        rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);


        
        if(IsGrounded())
        {
           
            rb.velocity = new Vector3(rb.velocity.x, dirY * 6.5f, 0);

        }

        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }

        else if (dirX < 0f)
        {
           
            state = MovementState.running;
            sprite.flipX = true;
        }

        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > 0.1f) 
        {
            state = MovementState.jumping;
        }

        else if (rb.velocity.y < -0.1f) 
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    private void OnTriggerEnter2D(Collider2D EndGame)
    {
        if (EndGame.tag == "CoinBro")
        {
            Source.clip = coin;
            Source.Play();
            Destroy(EndGame.gameObject);
           
            score += 5;
        }

        if (EndGame.tag == "CoinSil")
        {
            Source.clip = coin;
            Source.Play();
            Destroy(EndGame.gameObject);
           
            score += 10;
        }

        if (EndGame.tag == "CoinGold")
        {
            Source.clip = coin;
            Source.Play();
            Destroy(EndGame.gameObject);

            score += 20;
        }

        if (EndGame.tag == "se" && score >= LevelScore )
        {
            Source.clip = win;
            Source.Play();
            WinPanel.SetActive(true);
        }
        else if (EndGame.tag == "se" && score < LevelScore)
        {
            Time.timeScale = 0f;
            Panel.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Spike")
        {
            Source.clip = die;
            Source.Play();
            rb.transform.position = new Vector2(-3.24f, 1.61f);
           
            
            
            life -= 1;
            
        }
    }


    public void Replay()
    {
        WinPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }


    public void ReplayFromL2()
    {
        WinPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
        Time.timeScale = 1f;
    }


    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        menuPanel.SetActive(false);
    }

    public void Resume1()
    {
        Time.timeScale = 1f;
        Panel.SetActive(false);
    }

    public void Menu()
    {
        Time.timeScale = 0f;
        menuPanel.SetActive(true);
    }

}
