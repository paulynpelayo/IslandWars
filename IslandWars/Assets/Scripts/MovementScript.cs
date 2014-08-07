using UnityEngine;

/// <summary>
/// Simply moves the current game object
/// </summary>
public class MovementScript : MonoBehaviour
{
	private int Speed;
	public Vector2 Direction = new Vector2(-1, 0);	
	private Vector2 Movement;

	void Start()
	{
		Enemy E = gameObject.GetComponent<Enemy>();
		Speed = E.Speed;
	}

	void Update()
	{
		Movement = new Vector2(Speed * Direction.x, Speed * Direction.y);
	}
	
	void FixedUpdate()
	{
		rigidbody2D.velocity = Movement;
	}
}