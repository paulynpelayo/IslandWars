using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	#region enemystate

	public enum Enemystate
	{
		Idle,
		Walking,
		Attacking,
		Dying
	}
	private Enemystate _enemystate;
	public Enemystate enemystate
	{
		get { return _enemystate; }
		set 
		{
			Start();
			_enemystate = value;
			ChangeState();
		}
	}

	#endregion

	#region tk2d
	
	private tk2dSprite EnemySprite;
	private tk2dSpriteAnimator EnemyAnimator;
	
	#endregion

	#region attributes

	public int Life;
	public float Speed;
	public int Damage;
	public int Strength;
	public int CoinsGiven;

	#endregion

	private TransformPool Pool;
	private Transform TargetPoint;
	private TargetList List;

	// Use this for initialization
	void Start () {
		EnemySprite = gameObject.GetComponent<tk2dSprite>();
		EnemyAnimator = gameObject.GetComponent<tk2dSpriteAnimator>();
	}
	
	// Update is called once per frame
	void Update () {

		ChangeState();
	}

	void ChangeState()
	{
		switch (_enemystate)
		{
			case Enemystate.Idle:

				//EnemyAnimator.Stop();

			break;

			case Enemystate.Walking:

				EnemyAnimator.Play("Walking");

				transform.position = Vector3.MoveTowards(transform.position, TargetPoint.position, Speed * Time.deltaTime);
				
				if (Vector3.Distance (transform.position, TargetPoint.position) < 0.001f)
					enemystate = Enemystate.Attacking;
				

			break;

			case Enemystate.Attacking:

				EnemyAnimator.Stop();

			break;

			case Enemystate.Dying:

				List.RemoveFromList(gameObject);
				Pool.returnTransform(transform);				

			break;

		}
	}

	public void SetPool (TransformPool pool)
	{
		Pool = pool;
	}

	public void SetTarget (Transform targetPos)
	{
		TargetPoint = targetPos;
	}

	public void SetTargetList (Transform list)
	{
		List = list.GetComponent<TargetList>();
	}

	public void GotDamage (int damage)
	{
		Life -= damage;

		if (Life <= 0)
			enemystate = Enemystate.Dying;
	}

	void OnTriggerEnter (Collider other)
	{	
		if (other.tag == "List" )
		{
			List = other.GetComponent<TargetList>();
			List.AddToList(gameObject);
		}/*
		else if (other.tag == "Weapon")
		{	
			Destroy (other.gameObject);

			int WeaponDamage = other.GetComponent<WeaponScript>().Damage;

			Life -= WeaponDamage;

			if (Life <= 0)
			{
				enemystate = Enemystate.Dying;
			}

		}*/
	}
}
