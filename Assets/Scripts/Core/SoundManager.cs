using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    AudioSource audioSource;
    
    public List<AudioClip> clips = new List<AudioClip>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(this);
            }
        }
    }

    private void Start()
    {

    }
       
    public void PlayClip(EAudioClip clip,float volume, bool loop = false, bool dontDestroy = false)
    {
        if(clips[(int)clip] == null)
        {
            return;
        }

        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = loop;
        audioSource.clip = clips[(int)clip];
        audioSource.volume = volume;
        audioSource.Play();
        if(!dontDestroy)
        {
            float destroyAfter = clips[(int)clip].length;
            StartCoroutine(RemoveClip(audioSource, destroyAfter));
        }
    }

    IEnumerator RemoveClip(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);

        audioSource.Stop();
        Destroy(audioSource);
    }
}

public enum EAudioClip
{
    NONE = -1,
    ENEMYDESTROY_SFX,
    WEAPONENEMY_SFX,
    FAILURE_SFX,
    DESTROY_SFX,
    BULLET_SFX,
    POWER_SFX

}

