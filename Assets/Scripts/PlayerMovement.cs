using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
	public bool moveBasedOnRotation;
	public bool ADTurn;
	public float smooth = 5.0f;
    public float tiltAngle = 360.0f;
    Vector3 movement;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;
    bool sprint;
    private void Awake()
    {
        sprint = false;
        floorMask = LayerMask.GetMask("Floor");
        playerRigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprint = true;
        }
        else
        {
            sprint = false;
        }
        Move(h, v);
        Turning(h);
    }
    private void Move(float h, float v)
    {
        if(ADTurn)
		{
			movement = (v * Vector3.forward);
		}
		else
		{
			movement.Set(h, 0f, v);
		}
		if(moveBasedOnRotation || ADTurn)
		{
			movement = transform.rotation * movement;

		}
        if(sprint)
        {
            movement = movement.normalized * speed * Time.deltaTime * 1.5f;
        }
        else
        {
            movement = movement.normalized * speed * Time.deltaTime;
        }
		Debug.Log(movement);
		transform.position = transform.position + movement;
        //playerRigidbody.MovePosition(transform.position + movement);
    }
    private void Turning(float h)
    {
		
		if(ADTurn)
		{	
			transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y - (-h * tiltAngle * Time.deltaTime),0);
	
			//transform.Rotate(new Vector3(0, h, 0)*Time.deltaTime*tiltAngle);
			
			/*float rotateHorizontal = h * tiltAngle;
			Quaternion target = Quaternion.Euler(0, rotateHorizontal, 0);
			//moves back to default rotation
			transform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * smooth);*/
		}
		else
		{
			Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit floorHit;
			if(Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
			{
				Vector3 playerToMouse = floorHit.point - transform.position;
				playerToMouse.y = 0f;

				Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
				playerRigidbody.MoveRotation(newRotation);
			}
		}
        
    }
    
}
