using UnityEngine;
using UnityEngine.Events;

public class Generator : MonoBehaviour
{
    public static Generator i;

    public float score;
    [SerializeField] TMPro.TextMeshProUGUI scoreCounter;
    public int created;

    [Header("Pillar")]
    public GameObject pillar;
    [SerializeField] float minDistance, maxDistance, minHeight, maxHeight;
    public float despawnLimit;
    [SerializeField] Transform startPillar;
    public Vector2 latestPillar;

    [SerializeField] SceneControl scene;

    [Header("Player Reference")]
    [SerializeField] Transform player; // Reference to the player transform
    [SerializeField] float generateAheadDistance = 10f; // Distance ahead of the player to generate new pillars

    float ypos;

    void Awake()
    {
        // Make gameplay singleton
        i = this;
        ypos = pillar.transform.position.y;

    }

    void Start()
    {
      
        //firstpillar();
        // Set the latest pillar position to the start pillar
        latestPillar = startPillar.position;

        // Create the first few pillars at the start of the game
        for (int i = 0; i < 5; i++) { NextPillar(); }
    }



    void Update()
    {
   
        // Display the score
        scoreCounter.text = (System.Math.Round(score)).ToString();

        // Back to the game if it has ended and the player presses space
        if (Player.i.overMenu.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
        {
            scene.ToGame();
        }

        // Check if the player is near the latest pillar and generate new ones if needed
        if (player.position.x + generateAheadDistance > latestPillar.x)
        {
            NextPillar();
        }
    }

    public void NextPillar()
    {
        // Determine the next position to spawn
        Vector2 nextPos = Vector2.zero;

        // Increase X axis with randomly chosen distance
        nextPos.x = latestPillar.x + Random.Range(minDistance, maxDistance);

        // Randomly generate Y position within the min and max height
        //nextPos.y = Random.Range(minHeight, maxHeight);

        nextPos.y = ypos;

        // Spawn the next pillar at the calculated position with no rotation

        if(pillar!=null)
        {
            GameObject pillarClone = Instantiate(pillar, nextPos, Quaternion.identity);
            pillarClone.tag = "Clone";
        }

        else
        {
            Debug.Log("Asds");
        }

        // Update the latest pillar position
        latestPillar = nextPos;
    }
}
