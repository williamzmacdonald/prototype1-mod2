using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heartbeat : MonoBehaviour {
    public float runningbpm; 
    public float restingbpm;
    public float smoothTime;//time to smooth from resting->running and vice versa
    public float bpm; //total bpm
    public List<GameObject> enemies; //stores all enemies within radius
    float proximitybpm; //bpm based on monster proximity
    float targetbpm; //target bpm dependent on walking/running
    float speedOfChange; //gets rate of change of targetbpm
    float oldbpm; //keep track of previous bpm for invoking call
    Vector3 closestEnemy; //stores closest enemy position
    public AudioSource heartbeatSource;
    Text text;
    float timeSinceHeartbeat;
	// Use this for initialization
	void Start () {
        text = GameObject.Find("BPMText").GetComponent<Text>();
        bpm = 70f;
        oldbpm = 55f;
        targetbpm = 70f;
        restingbpm = 70f;
        runningbpm = 140f;
        smoothTime = 10f;
        timeSinceHeartbeat = 0f;
        heartbeatSource = GetComponent<AudioSource>();
        heartbeatSource.volume = .58f;

    }
	
	// Update is called once per frame
	void Update () {
        timeSinceHeartbeat += Time.deltaTime;
        text.text = "BPM: " + bpm.ToString("#");
        if (bpm <= 80f)
        {
            text.color = Color.green;
        }
        else if(bpm <= 135f)
        {
            text.color = Color.yellow;
        }
        else
        {
            text.color = Color.red;
        }
        if (enemies.Count >= 1)
        {
            closestEnemy = enemies[0].transform.position;
            foreach (GameObject en in enemies) //find closest enemy in list
            {
                if ((en.transform.position - transform.position).magnitude < (closestEnemy - transform.position).magnitude)
                    closestEnemy = en.transform.position;
            }
            //magnitude of the proximity of the enemy ranges from 0-90, so I multiplied the value by .77 to get a value
            //between the range of 0 and 70ish, and then subtracted from 70 to give me a heartbeat ranging 
            //from 0 to 70ish, but with a higher value the closer they are to you
            proximitybpm = 70f-((closestEnemy - transform.position).magnitude*.77f);
            if (proximitybpm < 0f)
                proximitybpm = 0f;
        }
        else
        {
            proximitybpm = 0f;
        }
        //targetbpm is based on player's running vs walking while the proximitybpm is based off of enemies
        bpm = targetbpm + proximitybpm;
        if(Mathf.Abs(oldbpm-bpm) > 2f)
        {
            CancelInvoke();
            float timeLeft = 1/(oldbpm / 60f) - timeSinceHeartbeat;
            if (timeLeft < 0f)
                timeLeft = 0f;
            InvokeRepeating("PlayHeartbeat", timeLeft, 1/(bpm / 60f));
            oldbpm = bpm;

        }


    }

    //these functions need to be called from the player movement script when the player is running vs walking
    public void Running()
    { //smooths out the transition from walkign to running and vice versa
        targetbpm = Mathf.SmoothDamp(targetbpm, runningbpm, ref speedOfChange, smoothTime);
    }
    public void Walking()
    {
        targetbpm = Mathf.SmoothDamp(targetbpm, restingbpm, ref speedOfChange, smoothTime);
    }
    public void PlayHeartbeat()
    {
        heartbeatSource.Play();
        heartbeatSource.volume = bpm / 210f + .25f;
        timeSinceHeartbeat = 0f;

    }
}
