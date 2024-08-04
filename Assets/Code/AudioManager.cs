using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AudioManager : MonoBehaviour
{
    List<AudioSource> music = new List<AudioSource>();
    List<AudioSource> fade_out = new List<AudioSource>();
    List<AudioSource> fade_in = new List<AudioSource>();
    List<AudioSource> queue_add = new List<AudioSource>();
    List<AudioSource> queue_remove = new List<AudioSource>();

    const float FADE_SPEED = 0.1f;
    public AudioSource tracker;

    float prev_progress = 0.0f;
    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            music.Add(child.GetComponent<AudioSource>());
        }
        //StartCoroutine(mus());
    }

    /*IEnumerator mus()
    {
        Fade("MainBase", true);
        Fade("MainMelody", true);
        yield return new WaitForSeconds(5);
        Queue("MainGroove", true);
        yield return new WaitForSeconds(11);
        Fade("MainSynth", true);
    }*/

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if ((int)timer % 16 == 0 && (queue_add.Count != 0 || queue_remove.Count != 0))
        {
            foreach(AudioSource q in queue_add)
            {
                q.volume = 1.0f;
                queue_add.Remove(q);
            }
            foreach (AudioSource q in queue_remove)
            {
                q.volume = 0.0f;
                queue_remove.Remove(q);
            }
        }


    if (fade_in.Count > 0)
        {
            foreach (AudioSource fade in fade_in.ToArray())
            {
                if (fade.volume < 1.0f)
                {
                    fade.volume += Time.deltaTime * FADE_SPEED;
                } 
                else
                {
                    fade.volume = 1.0f;
                    fade_in.Remove(fade);
                }
            }
        }

        if (fade_out.Count > 0)
        {
            foreach (AudioSource fade in fade_out)
            {
                if (fade.volume > 0.0f)
                {
                    fade.volume -= Time.deltaTime * FADE_SPEED;
                }
                else
                {
                    fade.volume = 0.0f;
                    fade_out.Remove(fade);
                }
            }
        }
        
        if(tracker.time < prev_progress)
        {
            timer = Time.deltaTime;
        }
        prev_progress = tracker.time;
    }


    AudioSource Find(string id)
    {
        foreach (AudioSource song in music)
        {
            if(song.name == id)
            {
                return song;
            }
        }
        Debug.Log("String id " + id + " incorrect");
        return null;
    }

    void Fade(string id, bool on){
        AudioSource source = Find(id);
        if (on)
        {
            fade_in.Add(source);
        }
        else
        {
            fade_out.Add(source);
        }
    }
    void Queue(string id, bool on)
    {
        AudioSource source = Find(id);
        if (on)
        {
            queue_add.Add(source);
        }
        else
        {
            queue_remove.Add(source);
        }
    }

    void PlayRandom(List<string> from_ids)
    {
        Queue(from_ids[Random.Range(0, from_ids.Count)], true);
    }

    void MuteAll()
    {
        foreach(AudioSource song in music)
        {
            song.volume = 0.0f;
        }
        fade_out = new List<AudioSource>();
        queue_add = new List<AudioSource>();
        queue_remove = new List<AudioSource>();

    }
}
