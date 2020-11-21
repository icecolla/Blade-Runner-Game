using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator ac;
    private CapsuleCollider selfCollider;
    private Rigidbody theRb;
    private GameObject[] ragdollObj;

    private AudioSource audioSource;
    public AudioClip runClip;
    public AudioClip jumpClip;
    public AudioClip slideClip;
    private bool isAudioPlayed = false;
    public AudioClip impactClip;

    public static Vector3 startPosition;
    private Vector3 theRbVelocity;

    public float sideSpeed = 5f;
    public float jumpSpeed = 75f;
    public float gravityValue = 1.5f;

    private int laneNumber = 1;
    private int lanesCount = 2;

    public float firstLanePos = 1.5f;
    public float laneDistance = -1.5f;

    private bool isRolling = false;
    private bool isJumping = false;
    
    private float jumpTime;
    private float slideTime;

    private Vector3 ccCenterNorm = new Vector3(0, .28f, 0);
    private Vector3 ccCenterRoll = new Vector3(0, -.3f, 0);
    private float ccHeightNorm = 1.76f;
    private float ccHeightRoll = .4f;

    private ParticleSystem impactObstacle;
    private ParticleSystem pickupCoin;

    public static bool isImmortal = false;
    public delegate void OnPowerupUse(PowerUpController.PowerUp.Type type);
    public static event OnPowerupUse PowerupUseEvent;


    public void Init()
    {
        startPosition = transform.position;

        //GameManager.gameManager.runnin = true;
        selfCollider = GetComponent<CapsuleCollider>();
        ac = GetComponentInChildren<Animator>();
        theRb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        ragdollObj = GameObject.FindGameObjectsWithTag("Ragdoll");
        foreach (var item in ragdollObj)
        {
            if (item != this.gameObject)
            {
                item.GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        pickupCoin = Instantiate(Resources.Load<ParticleSystem>("PS/PS_Pick_Up_Bonus"), transform.position + new Vector3(0, .5f, 0), Quaternion.identity,  transform);
        impactObstacle = Instantiate(Resources.Load<ParticleSystem>("PS/PS_Impacts"), transform);
        UpdateAnimClipTimes();
    }

    private void FixedUpdate()
    {
        theRb.AddForce(new Vector3(0, Physics.gravity.y * gravityValue, 0), ForceMode.Acceleration);

        Movement();
    }

    private void Update()
    {
        SideMovement();
    }

    void Movement()
    {
        if (isGrounded() == true)
        {
            ac.ResetTrigger("Falling");

            if (GameManager.gameManager.runnin)
            {
                if (Input.GetAxisRaw("Vertical") > 0 && !isRolling)
                {
                    StartCoroutine(doJump());
                }
                else if (Input.GetAxisRaw("Vertical") < 0 && !isJumping && !isRolling)
                {
                    StartCoroutine(doRoll());
                }
            }


        }
    }

    IEnumerator doJump()
    {
        isJumping = true;
        ac.SetBool("Jumping", true);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(jumpClip);
        }

        theRb.AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.Force);

        yield return new WaitForSeconds(jumpTime);
        ac.SetBool("Jumping", false);
        ac.SetTrigger("Falling");

        yield return new WaitForSeconds(.4f);

        isJumping = false;
    }

    IEnumerator doRoll()
    {
        float duration = .9125f;
        float cooldownDuration = .123f;

        isRolling = true;
        ac.SetBool("Rolling", true);

        if (!isAudioPlayed)
        {
            audioSource.PlayOneShot(slideClip);
            isAudioPlayed = true;
        }

        selfCollider.center = ccCenterRoll;
        selfCollider.height = ccHeightRoll;

        while (duration > 0)
        {
            if (GameManager.gameManager.runnin)
            {
                duration -= Time.deltaTime;
            }
            yield return null;
        }

        StopRolling();

        while (cooldownDuration > 0)
        {
            if (GameManager.gameManager.runnin)
            {
                cooldownDuration -= Time.deltaTime;
            }
            yield return null;
        }

        //yield return new WaitForSeconds(slideTime - slideTime/3);

        //yield return new WaitForSeconds(.4f);
        isRolling = false;
        isAudioPlayed = false;
    }

    private void StopRolling()
    {
        ac.SetBool("Rolling", false);
        selfCollider.center = ccCenterNorm;
        selfCollider.height = ccHeightNorm;
    }

    void SideMovement()
    {
        CheckInput();

        Vector3 newPos = transform.position;
        newPos.z = Mathf.Lerp(newPos.z, firstLanePos + (laneNumber * laneDistance), sideSpeed * Time.fixedDeltaTime);
        transform.position = newPos;
    }

    void CheckInput()
    {
        int sign = 0;

        if (!GameManager.gameManager.runnin || isRolling || isJumping)
            return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            sign = -1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            sign = 1;
        }
        else
            return;

        laneNumber += sign;
        laneNumber = Mathf.Clamp(laneNumber, 0, lanesCount);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isImmortal)// && !other.gameObject.CompareTag("DeathPlane"))
        {
            other.enabled = !enabled;
            return;
        }

        if (other.CompareTag("Coin"))
        {
            AudioManager.audioMan.PlayCoinEffect();
            GameManager.gameManager.AddCoins(1);
            pickupCoin.Play();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("PowUpMult"))
        {
            AudioManager.audioMan.PlayCoinEffect();

            PowerupUseEvent(PowerUpController.PowerUp.Type.MULTIPLIER);
            pickupCoin.Play();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("PowUpImrtl"))
        {
            AudioManager.audioMan.PlayCoinEffect();

            PowerupUseEvent(PowerUpController.PowerUp.Type.IMMORTALITY);
            pickupCoin.Play();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("PowUpCoins"))
        {
            AudioManager.audioMan.PlayCoinEffect();

            PowerupUseEvent(PowerUpController.PowerUp.Type.COINS_SPAWN);
            pickupCoin.Play();
            Destroy(other.gameObject);
        }

        if (GameManager.gameManager.runnin && other.CompareTag("Obstacle"))
        {
            //StartCoroutine(Death());
            StartCoroutine(RagdollDeath());
        }


    }   

    IEnumerator RagdollDeath()
    {
        ac.enabled = !ac.enabled;
        audioSource.PlayOneShot(impactClip);

        foreach (var item in ragdollObj)
        {
            if (item != this.gameObject)
            {
                item.GetComponent<Rigidbody>().isKinematic = false;
                item.GetComponent<Collider>().enabled = true;
            }
        }

        GameManager.gameManager.runnin = false;
        GameManager.gameManager.currentMoveSpeed = 0;

        impactObstacle.Play();
        yield return new WaitForSeconds(.25f);

        impactObstacle.Stop();

        yield return new WaitForSeconds(4f);

        GameManager.gameManager.ShowResult();

        PowerUpController.puc.ResetAllPowerUps();

    }

    public void Respawn()
    {
        StopAllCoroutines();
        isImmortal = false;
        isRolling = false;
        isJumping = false;
        StopRolling();
        audioSource.PlayOneShot(runClip);
    }

    //IEnumerator Death()
    //{
    //    GameManager.gameManager.runnin = false;
    //    GameManager.gameManager.currentMoveSpeed = 0;

    //    impactObstacle.Play();

    //    ac.SetTrigger("Death");

    //    yield return new WaitForSeconds(.25f);

    //    impactObstacle.Stop();

    //    ac.ResetTrigger("Death");

    //    GameManager.gameManager.ShowResult();
    //}

    public void ResetPosition()
    {
        ac.enabled = !ac.enabled;

        foreach (var item in ragdollObj)
        {
            if (item != this.gameObject)
            {
                item.GetComponent<Rigidbody>().isKinematic = true;
                item.GetComponent<Collider>().enabled = false;
            }
        }

        transform.position = startPosition;
        theRb.velocity = Vector3.zero;
        laneNumber = 1;
    }

    public void Paused()
    {
        theRbVelocity = theRb.velocity;
        theRb.isKinematic = true;
        ac.speed = 0;
    }

    public void UnPaused()
    {
        theRb.isKinematic = false;
        theRb.velocity = theRbVelocity;
        ac.speed = 1;
    }

    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = ac.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "a_RunningJump":
                    jumpTime = clip.length;
                    break;
                case "a_RunningSlide":
                    slideTime = clip.length;
                    break;
            }
        }
    }

    private bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1f);
    }
}
