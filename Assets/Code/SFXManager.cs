using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    List<AudioSource> sfx = new List<AudioSource>();
    List<List<AudioSource>> groups = new List<List<AudioSource>>();
    List<string> ids = new List<string>();

    List<AudioSource> queue = new List<AudioSource>();
    public AudioSource tracker;

    float prev_progress = 0.0f;

    float timer = 0.0f;
    // Start is called before the first frame update


    private void Awake()
    {
        // Singleton pattern nerdge
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }



    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<AudioSource>() != null)
            {
                sfx.Add(child.GetComponent<AudioSource>());
            }
            else
            {
                List<AudioSource> ot = new List<AudioSource>();
                foreach (Transform sound in child.transform)
                {
                    sfx.Add(sound.GetComponent<AudioSource>());
                    ot.Add(sound.GetComponent<AudioSource>());
                }
                ids.Add(child.name);
                Debug.Log("ids: " + ids.ToString());
                groups.Add(ot);
            }
        }
        Debug.Log("ids: " + ids.ToString());

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if ((int)(timer*10f) % 5 == 0 && queue.Count != 0)
        {
            foreach (AudioSource q in queue)
            {
                q.Play();
                queue.Remove(q);
            }
        }

        if (tracker.time < prev_progress)
        {
            timer = Time.deltaTime;
        }
        prev_progress = tracker.time;
    }


    public void QueueNextBeat(string id, float pitch = 1f, float randomness = 0.0f, bool randomFromGroup = false)
    {
        AudioSource targ;
        if (randomFromGroup)
        {
            Debug.Log("ids: " + ids.ToString());
            targ = GetRandomFromGroup(id);
        }
        else
        {
            targ = Find(id);
        }

        targ.pitch = pitch;
        if (randomness != 0f)
        {
            targ.pitch = Random.Range(pitch - randomness, pitch + randomness);
        }

        queue.Add(targ);

    }



    private AudioSource GetRandomFromGroup(string id)
    {
        if (ids.IndexOf(id) == -1)
        {
            print("idnotexist: " + id);
            return new AudioSource();
        }
        else
        {
            List<AudioSource> group = groups[ids.IndexOf(id)];
            return group[Random.Range(0, group.Count - 1)];

        }

    }
    public void PlayRandomFromGroup(string id, float pitch = 1f, float randomness = 0.0f)
    {
        PlaySound(GetRandomFromGroup(id).name, pitch, randomness);

    }
    public void PlaySound(string id, float pitch = 1f, float randomness = 0.0f)
    {

        AudioSource sound = Find(id);
        sound.pitch = pitch;
        if (randomness != 0f)
        {
            sound.pitch = Random.Range(pitch - randomness, pitch + randomness);
        }
        sound.Play();
    }
    private AudioSource Find(string id)
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
