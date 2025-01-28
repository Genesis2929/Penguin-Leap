using UnityEngine;
using UnityEngine.UI;
public class SoundManager : MonoBehaviour
{
    public static SoundManager i;
	public AudioSource source;
	public float volume = 0.8f;
	public AudioSource backgroundaudiosource;
	public Slider volumeslider;

	void Awake()
	{
       
  //      //Create singelton and don't destroy on load when neede
  //      if (SoundManager.i == null) {i = this;DontDestroyOnLoad(this);}
		////If the game mananger are not in dontdestroyonload than destroy it
		//if(gameObject.scene.name != "DontDestroyOnLoad") {Destroy(gameObject);}
	}

	//private void Start()
	//{
 //       if (PlayerPrefs.HasKey("SoundVolume"))
 //       {
 //           float vol = PlayerPrefs.GetFloat("SoundVolume");
 //           volume = vol;
           
 //           if (backgroundaudiosource != null)
 //               backgroundaudiosource.volume = volume / 2;

 //       }
 //   }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("SoundVolume"))
        {
            float vol = PlayerPrefs.GetFloat("SoundVolume");
            volume = vol;
            if (backgroundaudiosource != null)
                backgroundaudiosource.volume = volume / 2;

        }
        if (volumeslider!= null)
        {
            volumeslider.value = volume;
        }
	
    }

    void Update()
	{
		//Debug.Log(this.gameObject.name);
		//Update volum

		soundupdate();

		PlayerPrefs.SetFloat("SoundVolume", volume);
		PlayerPrefs.Save();
		//Debug.Log(source.volume);	
	}

	void soundupdate()
	{
        source.volume = volume;
		//Debug.Log(volume)
        if (backgroundaudiosource != null)
            backgroundaudiosource.volume = volume / 2;


        if (PlayerPrefs.HasKey("SoundVolume"))
        {
            float vol = PlayerPrefs.GetFloat("SoundVolume");
            volume = vol;
            if (backgroundaudiosource != null)
                backgroundaudiosource.volume = volume / 2;

        }

    }
}
