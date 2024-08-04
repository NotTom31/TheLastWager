using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    public static Flip Instance;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.TimerObject = this;
        Play();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        GetComponent<Animation>().Play();
    }
}
