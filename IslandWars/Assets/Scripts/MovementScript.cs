using UnityEngine;

/// <summary>
/// Simply moves the current game object
/// </summary>
public class MovementScript : MonoBehaviour
{
	private int Speed;
	public Vector2 Direction = new Vector2(-1, 0);	
	private Vector3 Movement;

	void Start()
	{
		Enemy E = gameObject.GetComponent<Enemy>();
		Speed = E.Speed;
	}

	void Update()
	{
		Movement = new Vector3(Speed * Direction.x, Speed * Direction.y, 0);
	}
	
	void FixedUpdate()
	{
		rigidbody.velocity = Movement;
	}
}