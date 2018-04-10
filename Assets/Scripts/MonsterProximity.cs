using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterProximity : MonoBehaviour {
    Heartbeat _heartbeat;
    // Use this for initialization
    private void Start()
    {
        _heartbeat = GetComponentInParent<Heartbeat>();   
    }
    void OnTriggerEnter(Collider collision)
    { //any time a monster enters the detection radius add it to the list
       

        if (collision.gameObject.tag == "Monster")
        {
            _heartbeat.enemies.Add(collision.gameObject);
        }

    }
    void OnTriggerExit(Collider collision)
    { //any time a monster exits the detection radius remove it from the list
        if (collision.gameObject.tag == "Monster")
        {
            _heartbeat.enemies.Remove(collision.gameObject);
        }

    }


}
