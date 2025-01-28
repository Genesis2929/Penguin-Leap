using UnityEngine.UI;
using UnityEngine;

public class Volumeslider : MonoBehaviour
{
	[SerializeField] Slider slider;

    [SerializeField] AudioSource backgroundsound;
    [SerializeField] AudioSource sound;

    void Start()
    {
		//Update the current volume
        if(SoundManager.i != null) 
        slider.value = SoundManager.i.volume;
        //Debug.Log(slider.value);
    }

    void Update()
    {
		//Update the manager volume
        //if(SoundManager.i!= null)
        
            sound.volume = slider.value;
            backgroundsound.volume = slider.value/2;
        PlayerPrefs.SetFloat("SoundVolume",slider.value);
        


    }
}
