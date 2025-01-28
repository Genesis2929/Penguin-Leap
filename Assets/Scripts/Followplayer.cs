//using UnityEngine;

//public class Followplayer : MonoBehaviour
//{
//    public ParticleSystem particleSystem;
//    void LateUpdate()
//    {
//        transform.position = Player.i.transform.position;
//    }


//    void Start()
//    {
//        // Get the top-center position of the screen in world coordinates
//        Vector3 screenTopCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, Camera.main.nearClipPlane));

//        // Set the particle system position
//        particleSystem.transform.position = new Vector3(screenTopCenter.x, screenTopCenter.y, 0f);
//    }
//}


using UnityEngine;

public class Followplayer : MonoBehaviour
{
    public GameObject player;
    public ParticleSystem particleSystem;
    ParticleSystem.Particle[] particles;
    public float fallspeed= 2f;
    public Vector2 playerprevpos;
    void Start()
    {
        playerprevpos = player.transform.position;  
        // Set the particle system to follow the player's position, but adjust the Y position later in code
        particleSystem.transform.position = transform.position;

        // Set gravity modifier to zero to prevent the system from using the world's gravity
        var mainModule = particleSystem.main;
        mainModule.gravityModifier = 0f; // Disables gravity for the particles

        // Set the start speed of the particles to 0, so we can manually control their velocity
        mainModule.startSpeed = 0f; // No initial speed; we'll control it via script
    }

    void LateUpdate()
    {
        // Keep the particle system at the player’s position, but adjust the Y axis separately
        transform.position = new Vector2(player.transform.position.x, transform.position.y);
        playerposparticle();
        // Get all active particles
        particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        int particleCount = particleSystem.GetParticles(particles);

        // Update the particles to move downward at a constant speed
        for (int i = 0; i < particleCount; i++)
        {
            // Apply downward velocity to each particle (negative Y direction)
            particles[i].velocity = new Vector3(0f, -fallspeed, 0f); // Move downward (Y-axis negative)

            // Check if the particle has gone below the screen's bottom and destroy it
            Vector3 particlePos = particles[i].position;

            // Check if the particle has gone below the screen’s bottom
            if (particlePos.y < Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).y)
            {
                // Set the particle's lifetime to 0, causing it to be destroyed
                particles[i].remainingLifetime = 0;
            }
        }

        // Apply the updated particles back to the system
        particleSystem.SetParticles(particles, particleCount);


        playerprevpos = player.transform.position;
    }

    void playerposparticle()
    {
        float disty = playerprevpos.y - player.transform.position.y;

        transform.position = new Vector2(transform.position.x, transform.position.y+ disty);
    }
}
