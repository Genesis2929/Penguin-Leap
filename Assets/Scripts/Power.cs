//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class Power : MonoBehaviour
//{
//    public int powerPoint;
//    [SerializeField] TextMeshProUGUI powerCounter;

//    [Header("Jump")]
//    public int groundJumpCost;
//    public int airJumpCost;
//    int jumpCost;
//    public float jumpForce;
//    [SerializeField] GameObject groundJump, airJump, jumpEffect;

//    [Header("Boost")]
//    public int boostCost;
//    public float boostSpeed;
//    public float boostDuration;
//    [SerializeField] GameObject boostEffect;
//    [SerializeField] TrailRenderer boostTrail;

//    [Header("Block")]
//    public int blockCost;
//    public bool placeBlock = false;
//    public GameObject block;

//    [Header("Lock")]
//    public int lockCost;
//    public bool locking = false;

//    [Header("Freeze")]
//    public int freezeCost;
//    public float freezeDuration;
//    float freezeCounter;
//    public bool freezed;
//    public Image freezeProgress;

//    [Header("Interface")]
//    public TextMeshProUGUI groundJumpCostUI;
//    public TextMeshProUGUI airJumpCostUI, boostCostUI, lockCostUI, blockCostUI, freezeCostUI;
//    public Button groundJumpButton, airJumpButton, boostButton, blockButton, lockButton, freezeButton;

//    public AudioClip jumpSound, boostSound, lockSound, blockSound, freezeSound;

//    Rigidbody2D rb;
//    Vector2 mousePos;
//    Player p;

//    void Start()
//    {
//        // Get the player and its Rigidbody
//        p = Player.i;
//        rb = p.rb;

//        // Update the cost UI
//        groundJumpCostUI.text = groundJumpCost.ToString();
//        airJumpCostUI.text = airJumpCost.ToString();
//        boostCostUI.text = boostCost.ToString();
//        lockCostUI.text = lockCost.ToString();
//        blockCostUI.text = blockCost.ToString();
//        freezeCostUI.text = freezeCost.ToString();
//    }

//    void Update()
//    {
//        // Update the power counter
//        powerCounter.text = "Power: " + powerPoint;

//        // Check for a valid camera
//        if (Camera.main == null)
//        {
//            Debug.LogError("Main camera not found.");
//            return;
//        }

//        // Validate mouse position
//        if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= Screen.width &&
//            Input.mousePosition.y >= 0 && Input.mousePosition.y <= Screen.height)
//        {
//            // Get the mouse position
//            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//        }
//        else
//        {
//            Debug.LogWarning("Mouse position is out of the screen bounds.");
//            return;
//        }

//        // Control the cost
//        CostManagement();

//        // Run power functions
//        JumpChange();
//        PlacingBlock();
//        StartLocking();
//        BeginFreeze();

//        // Run abilities if powers are not locked
//        if (!Player.i.lockPower.activeInHierarchy)
//        {
//            if (Input.GetKeyDown(KeyCode.Alpha1))
//            {
//                if (airJumpButton.interactable && !Player.i.isGround)
//                {
//                    Jumping();
//                }
//                else if (Player.i.isGround)
//                {
//                    Jumping();
//                }
//            }
//            if (Input.GetKeyDown(KeyCode.Alpha2) && boostButton.interactable) Boosting();
//            if (Input.GetKey(KeyCode.Alpha3) && blockButton.interactable) Block();
//            if (Input.GetKey(KeyCode.Alpha4) && lockButton.interactable) Locking();
//            if (Input.GetKey(KeyCode.Alpha5)) Freezing();
//        }
//    }

//    void CostManagement()
//    {
//        // Update button interaction depending on cost
//        groundJumpButton.interactable = powerPoint >= groundJumpCost;
//        airJumpButton.interactable = powerPoint >= airJumpCost;
//        boostButton.interactable = powerPoint >= boostCost;
//        blockButton.interactable = powerPoint >= blockCost;
//        lockButton.interactable = powerPoint >= lockCost;
//        freezeButton.interactable = powerPoint >= freezeCost;
//    }

//    #region Jump
//    public void Jumping()
//    {
//        // Consume the cost
//        powerPoint -= jumpCost;

//        // Create jump effect
//        Instantiate(jumpEffect, Player.i.transform.position, Quaternion.identity);

//        // Jump upward using jump force
//        rb.linearVelocity = Vector2.up * jumpForce;

//        if (SoundManager.i != null)
//            SoundManager.i.source.PlayOneShot(jumpSound);
//    }

//    void JumpChange()
//    {
//        // Ground jump if the player is touching the ground and use the ground cost
//        if (p.isGround)
//        {
//            groundJump.SetActive(true);
//            airJump.SetActive(false);
//            jumpCost = groundJumpCost;
//        }
//        // Air jump if the player is in the air and use the air cost
//        else
//        {
//            groundJump.SetActive(false);
//            airJump.SetActive(true);
//            jumpCost = airJumpCost;
//        }
//    }
//    #endregion

//    #region Boost
//    public void Boosting()
//    {
//        // Consume the cost
//        powerPoint -= boostCost;

//        // Create boost effect
//        Instantiate(boostEffect, Player.i.transform.position, Quaternion.identity);

//        if (SoundManager.i != null)
//            SoundManager.i.source.PlayOneShot(boostSound);

//        boostTrail.emitting = true;

//        // Increase the player speed with boost speed, then reset it after the duration
//        p.speed += boostSpeed;
//        Invoke("ResetBoost", boostDuration);
//    }

//    void ResetBoost()
//    {
//        p.speed -= boostSpeed;
//        boostTrail.emitting = false;
//    }
//    #endregion

//    #region Block
//    public void Block()
//    {
//        if (blockButton.interactable)
//        {
//            // Placing block
//            placeBlock = true;
//            powerPoint -= blockCost;
//        }
//    }

//    void PlacingBlock()
//    {
//        if (placeBlock)
//        {
//            Time.timeScale = 0;
//            blockButton.interactable = false;

//            if (Input.GetMouseButtonDown(0))
//            {
//                if (SoundManager.i != null)
//                    SoundManager.i.source.PlayOneShot(blockSound);

//                Instantiate(block, mousePos, Quaternion.identity);
//                placeBlock = false;
//                Time.timeScale = 1;
//                blockButton.interactable = true;
//            }
//        }
//    }
//    #endregion

//    #region Locking
//    public void Locking()
//    {
//        if (lockButton.interactable)
//        {
//            locking = true;
//            powerPoint -= lockCost;
//        }
//    }

//    void StartLocking()
//    {
//        if (locking)
//        {
//            lockButton.interactable = false;

//            if (Input.GetMouseButtonDown(0))
//            {
//                int layer = LayerMask.NameToLayer("Pillar");
//                RaycastHit2D ray = Physics2D.Raycast(mousePos, Vector2.up, 0, 1 << layer);

//                if (ray)
//                {
//                    Pillar pillar = ray.collider.transform.parent.GetComponent<Pillar>();
//                    if (!pillar.locked)
//                    {
//                        if (SoundManager.i != null)
//                            SoundManager.i.source.PlayOneShot(lockSound);

//                        pillar.locked = true;
//                        locking = false;
//                        lockButton.interactable = true;
//                    }
//                }
//            }
//        }
//    }
//    #endregion

//    #region Freezing
//    public void Freezing()
//    {
//        if (!freezed && powerPoint >= freezeCost)
//        {
//            freezed = true;
//            powerPoint -= freezeCost;

//            if (SoundManager.i != null)
//                SoundManager.i.source.PlayOneShot(freezeSound);
//        }
//    }

//    void BeginFreeze()
//    {
//        if (freezed)
//        {
//            freezeProgress.fillAmount = 1 - (freezeCounter / freezeDuration);
//            freezeCounter += Time.deltaTime;

//            if (freezeCounter >= freezeDuration)
//            {
//                freezeCounter = 0;
//                freezed = false;
//                freezeProgress.fillAmount = 1;
//            }
//        }
//    }
//    #endregion
//}




using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Power : MonoBehaviour
{
    public AudioSource sound;
    public int powerPoint;
    [SerializeField] TextMeshProUGUI powerCounter;

    [Header("Jump")]
    public int groundJumpCost;
    public int airJumpCost;
    int jumpCost;
    public float jumpForce;
    [SerializeField] GameObject groundJump, airJump, jumpEffect;

    [Header("Boost")]
    public int boostCost;
    public float boostSpeed;
    public float boostDuration;
    [SerializeField] GameObject boostEffect;
    [SerializeField] TrailRenderer boostTrail;

    [Header("Block")]
    public int blockCost;
    public bool placeBlock = false;
    public GameObject block;

    [Header("Lock")]
    public int lockCost;
    public bool locking = false;

    [Header("Freeze")]
    public int freezeCost;
    public float freezeDuration;
    float freezeCounter;
    public bool freezed;
    public Image freezeProgress;

    [Header("Interface")]
    public TextMeshProUGUI groundJumpCostUI;
    public TextMeshProUGUI airJumpCostUI, boostCostUI, lockCostUI, blockCostUI, freezeCostUI;
    public Button groundJumpButton, airJumpButton, boostButton, blockButton, lockButton, freezeButton;

    public GameObject airjumpgameobject, groundjumpgameobject;

    public AudioClip jumpSound, boostSound, lockSound, blockSound, freezeSound;

    Rigidbody2D rb;
    Vector2 mousePos;
    Player p;
    [SerializeField] private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    [SerializeField] private Animator animator; // Reference to the Animator
    [SerializeField] private Sprite newSpritejump; // The new sprite to set
    [SerializeField] private RuntimeAnimatorController newAnimatorControllerjump;
    [SerializeField] private Sprite newSpriteidle; // The new sprite to set
    [SerializeField] private RuntimeAnimatorController newAnimatorControlleridle; 
    [SerializeField] private Sprite newSpritewalk; // The new sprite to set
    [SerializeField] private RuntimeAnimatorController newAnimatorControllerwalk;


    [SerializeField] private Sprite originalSprite; // Original sprite to revert to
    [SerializeField] private RuntimeAnimatorController originalAnimatorController;
    private bool isChanging;

    

    //public AudioSource sound;
    //public float audiovolume = 5f;
    void Start()
    {
        // Get the player and its Rigidbody
        p = Player.i;
        rb = p.rb;

        // Update the cost UI
        groundJumpCostUI.text = groundJumpCost.ToString();
        airJumpCostUI.text = airJumpCost.ToString();
        boostCostUI.text = boostCost.ToString();
        lockCostUI.text = lockCost.ToString();
        blockCostUI.text = blockCost.ToString();
        freezeCostUI.text = freezeCost.ToString();

        originalSprite = spriteRenderer.sprite;
        originalAnimatorController = animator.runtimeAnimatorController;
        //ChangeAppearance(newSpriteidle, newAnimatorControlleridle);
        if (spriteRenderer != null && newSpriteidle != null)
            spriteRenderer.sprite = newSpriteidle;

        // Change the Animator Controller
        if (animator != null && newAnimatorControlleridle != null)
            animator.runtimeAnimatorController = newAnimatorControlleridle;
        //sound.volume = audiovolume;

    }

    void Update()
    {
        // Update the power counter
        powerCounter.text = "Power: " + powerPoint;

        // Check for a valid camera
        if (Camera.main == null)
        {
            Debug.LogError("Main camera not found.");
            return;
        }

        // Validate mouse position
        if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= Screen.width &&
            Input.mousePosition.y >= 0 && Input.mousePosition.y <= Screen.height)
        {
            // Get the mouse position
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            //Debug.LogWarning("Mouse position is out of the screen bounds.");
            return;
        }

        // Control the cost
        CostManagement();
        //Debug.Log(p.isGround);
        // Run power functions
        JumpChange();
        PlacingBlock();
        StartLocking();
        BeginFreeze();

        // Run abilities if powers are not locked
        if (!Player.i.lockPower.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (airJumpButton.interactable && !Player.i.isGround)
                {
                    Jumping();
                }
                else if (Player.i.isGround)
                {
                    Jumping();
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && boostButton.interactable) Boosting();
            if (Input.GetKey(KeyCode.Alpha3) && blockButton.interactable) Block();
            if (Input.GetKey(KeyCode.Alpha4) && lockButton.interactable) Locking();
            if (Input.GetKey(KeyCode.Alpha5)) Freezing();
        }
    }

    void CostManagement()
    {
        // Update button interaction depending on cost
        groundJumpButton.interactable = powerPoint >= groundJumpCost;
        airJumpButton.interactable = powerPoint >= airJumpCost;
        boostButton.interactable = powerPoint >= boostCost;
        blockButton.interactable = powerPoint >= blockCost;
        lockButton.interactable = powerPoint >= lockCost;
        freezeButton.interactable = powerPoint >= freezeCost;
    }

    //public void ChangeAppearance(Sprite sprite, RuntimeAnimatorController animcontroller)
    //{
    //    // Change the sprite of the SpriteRenderer
    //    if (spriteRenderer != null && sprite != null)
    //        spriteRenderer.sprite = sprite;

    //    // Change the Animator Controller
    //    if (animator != null && animcontroller != null)
    //        animator.runtimeAnimatorController = animcontroller;
    //}
    public void ChangeAppearance(Sprite newSprite, RuntimeAnimatorController newAnimController)
    {
        // Ensure the appearance change is not happening while another is in progress
        if (!isChanging)
        {
            isChanging = true;

            // Change the sprite of the SpriteRenderer
            if (spriteRenderer != null && newSprite != null)
                spriteRenderer.sprite = newSprite;

            // Change the Animator Controller
            if (animator != null && newAnimController != null)
                animator.runtimeAnimatorController = newAnimController;

            // Start coroutine to revert back after animation
            StartCoroutine(WaitForAnimationToEnd());
        }
    }

    private IEnumerator WaitForAnimationToEnd()
    {
        // Wait until the current animation has finished
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        yield return new WaitForSeconds(animationDuration);

        // Revert the sprite and animator back to original
        if (spriteRenderer != null)
            spriteRenderer.sprite = originalSprite;

        if (animator != null)
            animator.runtimeAnimatorController = originalAnimatorController;

        isChanging = false; // Reset flag to allow future changes
    }
    #region Jump
    public void Jumping()
    {
      
        if(powerPoint >= jumpCost)
        {
            powerPoint -= jumpCost;

            // Update button interactivity immediately
            CostManagement();

            // Create jump effect
            Instantiate(jumpEffect, Player.i.transform.position, Quaternion.identity);

            // Jump upward using jump force
            rb.linearVelocity = Vector2.up * jumpForce;

            //if (SoundManager.i != null)
                //SoundManager.i.source.PlayOneShot(jumpSound);
                sound.PlayOneShot(jumpSound);

            ChangeAppearance(newSpritejump, newAnimatorControllerjump);
            p.isGround = false; 

        }
        // Consume the cost
    }

    void JumpChange()
    {
        // Ground jump if the player is touching the ground and use the ground cost
        if (p.isGround)
        {
            groundJump.SetActive(true);
            airJump.SetActive(false);
            jumpCost = groundJumpCost;
        }
        // Air jump if the player is in the air and use the air cost
        else
        {
            groundJump.SetActive(false);
            airJump.SetActive(true);
            jumpCost = airJumpCost;
        }
    }
    #endregion

    #region Boost
    public void Boosting()
    {
        if (powerPoint >= boostCost)
        {
                // Consume the cost
                powerPoint -= boostCost;

            // Update button interactivity immediately
            CostManagement();

            // Create boost effect
            Instantiate(boostEffect, Player.i.transform.position, Quaternion.identity);

            //if (SoundManager.i != null)
                //SoundManager.i.source.PlayOneShot(boostSound);
                sound.PlayOneShot(boostSound);

            boostTrail.emitting = true;

            // Increase the player speed with boost speed, then reset it after the duration
            p.speed += boostSpeed;
            Invoke("ResetBoost", boostDuration);

        }
    }

    void ResetBoost()
    {
        p.speed -= boostSpeed;
        boostTrail.emitting = false;
    }
    #endregion

    #region Block
    public void Block()
    {
        if (blockButton.interactable)
        {
            // Placing block
            placeBlock = true;
            powerPoint -= blockCost;

            // Update button interactivity immediately
            CostManagement();
        }
    }

    void PlacingBlock()
    {
        if (placeBlock)
        {
            Time.timeScale = 0;
            blockButton.interactable = false;

            if (Input.GetMouseButtonDown(0))
            {
                //if (SoundManager.i != null)
                    //SoundManager.i.source.PlayOneShot(blockSound);
                    sound.PlayOneShot(blockSound);

                Instantiate(block, mousePos, Quaternion.identity);
                placeBlock = false;
                Time.timeScale = 1;
                blockButton.interactable = true;
            }
        }
    }
    #endregion

    #region Locking
    public void Locking()
    {
        if (lockButton.interactable)
        {
            locking = true;
            powerPoint -= lockCost;

            // Update button interactivity immediately
            CostManagement();
        }
    }

    void StartLocking()
    {
        if (locking)
        {
            lockButton.interactable = false;

            if (Input.GetMouseButtonDown(0))
            {
                int layer = LayerMask.NameToLayer("P12");
                RaycastHit2D ray = Physics2D.Raycast(mousePos, Vector2.up, 0, 1 << layer);

                if (ray)
                {
                    Pillar pillar = ray.collider.transform.GetComponent<Pillar>();
                    if (!pillar.locked)
                    {
                        //if (SoundManager.i != null)
                            //SoundManager.i.source.PlayOneShot(lockSound);
                            sound.PlayOneShot(lockSound);

                        pillar.locked = true;
                        locking = false;
                        lockButton.interactable = true;
                    }
                }
            }
        }
    }
    #endregion

    #region Freezing
    public void Freezing()
    {
        if (!freezed && powerPoint >= freezeCost)
        {
            freezed = true;
            powerPoint -= freezeCost;

            // Update button interactivity immediately
            CostManagement();

            //if (SoundManager.i != null)
                //SoundManager.i.source.PlayOneShot(freezeSound);
                sound.PlayOneShot(freezeSound);
        }
    }

    void BeginFreeze()
    {
        if (freezed)
        {
            freezeProgress.fillAmount = 1 - (freezeCounter / freezeDuration);
            freezeCounter += Time.deltaTime;

            if (freezeCounter >= freezeDuration)
            {
                freezeCounter = 0;
                freezed = false;
                freezeProgress.fillAmount = 1;
            }
        }
    }
    #endregion
}
