using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    List<AudioSource> sfx = new List<AudioSource>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            sfx.Add(child.GetComponent<AudioSource>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void PlaySound(string id)
    {
        AudioSource sound = Find(id);
        sound.Play();
    }
    AudioSource Find(string id)
    {
        foreach (AudioSource song in sfx)
        {
            if (song.name == id)
            {
                return song;
            }
        }
        Debug.Log("String id " + id + " incorrect");
        return null;
    }
}
