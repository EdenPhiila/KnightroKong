using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    private float xAxis, yAxis;
    public Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    public BoxCollider2D bc;
    AudioSource audioSource;

    [Header("Horizontal Movement")]
    [SerializeField] private float walkSpeed = 1;
    [Space(5)]

    [Header("Vertical Movement")]
    [SerializeField] private float jumpForce = 35;
    private float jumpBufferCounter;
    [SerializeField] private float jumpBufferFrames;
    private float coyoteTimeCounter = 0;
    [SerializeField] private float coyoteTime;
    [Space(5)]

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatIsGround;
    [Space(5)]

    [Header("Player States")]
    public bool win;
    [SerializeField] private bool jumping;
    [SerializeField] private bool lookingRight;
    public bool invincible;
    [SerializeField] private bool climbing;
    public bool dead;

    [Header("Scene Transition")]
    public string nextLevel;
    public string thisLevel;

    [Header("Audio Clips")]
    public AudioClip death;
    public AudioClip jump;
    public AudioClip bash;
    public AudioClip swordGet;
    public AudioClip shieldGet;
    public AudioClip mainSong;
    public AudioClip shieldSong;
    public GameObject gameMusic;
    public GameObject shieldMusic;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead || win) return;
        GetInputs();
        UpdateJumpVariables();
        Move();
        Jump();
        Flip();
        Exit();
        if (invincible && !climbing) anim.SetBool("Shielded", true);
        else anim.SetBool("Shielded", false);
    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
    }

    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(transform.localScale.y * -1, transform.localScale.y);
            lookingRight = false;
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(transform.localScale.y * 1, transform.localScale.y);
            lookingRight = true;
        }
    }

    private void Move()
    {
        if (climbing)
        {
            rb.velocity = new Vector2(walkSpeed * xAxis, walkSpeed * yAxis);
        }
        else
        {
            rb.velocity = new Vector2(walkSpeed * xAxis, rb.velocity.y);
            anim.SetBool("Walking", rb.velocity.x != 0 && Grounded());
        }
    }

    protected void OnTriggerStay2D(Collider2D _other)
    {
        if (_other.gameObject.CompareTag("Ladder") && Grounded())
        {
            if (yAxis > 0 || yAxis < 0)
            {
                climbing = true;
                anim.SetBool("Climbing", true);
                bc.enabled = false;
                GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.gameObject.CompareTag("Ladder"))
        {
            climbing = false;
            anim.SetBool("Climbing", false);
            bc.enabled = true;
            GetComponent<Rigidbody2D>().gravityScale = 1.5f;
        }
    }

        public bool Grounded()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Jump()
    {

        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0 && !jumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce);

            jumping = true;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 3)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);

            jumping = false;

        }

        anim.SetBool("Jumping", !Grounded());
    }

    void UpdateJumpVariables()
    {
        if (Grounded())
        {
            jumping = false;
            coyoteTimeCounter = coyoteTime;
        }

        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferFrames;
            if (Grounded())
            {
                PlaySound(jump);
            }
        }

        else
        {
            jumpBufferCounter = jumpBufferCounter - Time.deltaTime * 10;
        }
    }

    public void Hit() 
    {
        if (invincible)
        {
            PlaySound(bash);
            anim.SetTrigger("Bashing");
        }
        else 
        {
            dead = true;
            NextLevel();
        }
    }

    public void Shield() 
    {
        StartCoroutine(PowerUp());
    }

    IEnumerator PowerUp() 
    {
        PlaySound(shieldGet);
        invincible = true;
        yield return new WaitForSeconds(10);
        invincible = false;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void NextLevel()
    {
        StartCoroutine(SceneTransition());
    }

    IEnumerator SceneTransition()
    {
        if (win)
        {
            anim.SetBool("Win", true);
            PlaySound(swordGet);
            yield return new WaitForSeconds(4);
            SceneManager.LoadScene(nextLevel);
        }
        else if (dead)
        {
            anim.SetBool("Death", true);
            PlaySound(death);
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(thisLevel);
        }
    }

    void Exit() 
    {
        if (Input.GetButtonDown("Close")) 
        {
            Application.Quit();
        }
    }
}
