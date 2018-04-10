using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour {
	public enum STATE {  WANDER, CHASE, LASTDEST };
    public MeshRenderer _meshRenderer;

    NavMeshAgent _controller;
    Transform _target;
    public STATE _currentState;
    
    void Start ()
	{
        _controller = GetComponent<NavMeshAgent>();
        _target = GameObject.Find("GoodGuy").transform;
        _currentState = STATE.WANDER;
        InvokeRepeating("wander", 0f, 3f);
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material.color = Color.white;

    }

    void Update()
	{
        Debug.DrawLine(transform.position, _target.position);
        //do linecast to see if objects in the way of line of sight, if not chase and if not fleeing
        RaycastHit hitInfo;
        Physics.Linecast(transform.position, _target.position, out hitInfo);
       
        if (hitInfo.collider.name == "GoodGuy")
        {
            if (_currentState != STATE.CHASE)
            {//cancel all other invokes, then invoke chase every .5 seconds
                CancelInvoke();
                InvokeRepeating("chase", 0f, .5f);
                _currentState = STATE.CHASE;
                _meshRenderer.material.color = Color.red;
            }
        } //if not fleeing, and no line of sight, and not already heading to last destination
        else if (_currentState == STATE.CHASE)
        {
            CancelInvoke();
            _currentState = STATE.LASTDEST;
            _meshRenderer.material.color = Color.yellow;
        }
        else if (_currentState == STATE.LASTDEST )
        { 
            if (!_controller.pathPending)
            { //check if path has been reached (last sighting of player)
                if (_controller.remainingDistance <= _controller.stoppingDistance)
                {
                    if (!_controller.hasPath || _controller.velocity.sqrMagnitude == 0f)
                    { //if so, wander!
                        _currentState = STATE.WANDER;
                        _meshRenderer.material.color = Color.white;
                        CancelInvoke(); //cancel all other invokes, then invoke wander every 3 seconds
                        InvokeRepeating("wander", 0f, 3f);
                    }
                }
            }
        }

    }
   
    public void wander()
    {
        //ranges of quad x: -12.4 to 12.4 y:1 z: -20.6 to 4.2
        //will sometimes choose stuff inside boxes but function will be called again in a few seconds
        Vector3 randomDest = new Vector3(Random.Range(-12.4f, 12.4f), 1f, Random.Range(-20.6f, 4.2f));
        _controller.SetDestination(randomDest);
    }
    public void chase()
    {
        _controller.SetDestination(_target.position);
    }
}
