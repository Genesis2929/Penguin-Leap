using UnityEditor;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    public AudioSource sound;
    bool passed;
    [SerializeField] bool creating;
    Generator generate;
    Player player;
    Point point;
    [SerializeField] Rigidbody2D rb; // Rigidbody2D for 2D physics
    //[SerializeField] Rigidbody2D rb1; // Rigidbody2D for 2D physics
    //[SerializeField] Rigidbody2D rb2; // Rigidbody2D for 2D physics
    public bool locked;

    [Header("Materials")]
    [SerializeField] MeshRenderer render; // MeshRenderer for visual rendering
    [SerializeField] MeshRenderer render1; // MeshRenderer for visual rendering
    [SerializeField] MeshRenderer render2; // MeshRenderer for visual rendering
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material defaultMaterial1;
    [SerializeField] Material defaultMaterial2;

    [SerializeField] Material goldenMaterial;
    [SerializeField] Material lockMaterial;
    [SerializeField] Material freezeMaterial;

    [SerializeField] bool golden;
    [SerializeField] AudioClip goldenGet;
  

    void Start()
    {
    
        // Get the gameplay, player, and point
        generate = Generator.i;
        player = Player.i;
        point = Point.i;

        // Increase the generate count
        generate.created++;

        // If this pillar is at the golden rate, make it golden
        if (generate.created % point.goldenRate == 0)
        {
            SetMaterial(goldenMaterial);
            golden = true;
        }
        else
        {
            //SetMaterial(defaultMaterial);
            render.material = defaultMaterial;
            render1.material = defaultMaterial1;
            render2.material = defaultMaterial2;
        }
    }

    void Update()
    {
        // When the pillar goes out of the despawn limit on the X axis
        //Vector2 worldpos = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y);
        //Vector2 screenpos = Camera.main.WorldToScreenPoint(worldpos);
        if ((player.transform.position.x - transform.position.x) > generate.despawnLimit)
        //if(screenpos.x > Screen.width)
        {
            // Destroy the object
            if (CompareTag("Clone"))
            {
                 Destroy(gameObject);
                //Debug.Log("This is the original prefab.");
            }
        }

        // If this is the first time the player has passed this pillar
        if (transform.position.x < player.transform.position.x && !passed)
        {
            // Start the next pillar if this pillar needs to create
            if (creating)
            {
                generate.NextPillar();
            }

            // Mark as passed
            passed = true;
        }

        // Change Rigidbody2D to static if locked and update material
        if (locked)
        {
            rb.bodyType = RigidbodyType2D.Static;
            SetMaterial(lockMaterial);
        }

        // Freeze behavior based on player power
        if (player.power.freezed)
        {
            rb.bodyType = RigidbodyType2D.Static; // Freeze physics
            SetMaterial(freezeMaterial);
        }
        else if (!locked)
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // Unfreeze physics
            render.material = defaultMaterial;
            render1.material = defaultMaterial1;
            render2.material = defaultMaterial2;
        }
    }

    public void GettingPoint()
    {
        // Increase power normally if this pillar is not golden
        if (!golden)
        {
            point.IncreasePassPoint();
        }
        else
        {
            // Increase golden power if it is golden
            point.IncreaseGoldenPower();

            //if(SoundManager.i !=null)
            //SoundManager.i.source.PlayOneShot(goldenGet);
            sound.PlayOneShot(goldenGet);


           
        }
    }

    private void SetMaterial(Material material)
    {
        // Change the material of the MeshRenderer
        render.material = material;
        render1.material = material;
        render2.material = material;
    }
}
