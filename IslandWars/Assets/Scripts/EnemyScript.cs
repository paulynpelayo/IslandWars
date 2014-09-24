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
	private tk2dSpriteAnimator EnemyAnimator, BlastAnim;
		
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
	private bool isBlasting = false;

	public AudioClip AttackSound, DyingSound;
	//private TargetList List;

	// Use this for initialization
	void Start () {
		EnemySprite = gameObject.GetComponent<tk2dSprite>();
		EnemyAnimator = gameObject.GetComponent<tk2dSpriteAnimator>();
		//BlastAnim = gameObject.GetComponentInChildren<tk2dSpriteAnimator>();
		BlastAnim = transform.FindChild ("Blast").GetComponent<tk2dSpriteAnimator>();
		//Debug.Log(BlastAnim);

	}
	
	// Update is called once per frame
	void Update () {

		ChangeState();

		if (isBlasting)
		{
			BlastAnim.gameObject.renderer.enabled = true;
			BlastAnim.gameObject.active = true;
			BlastAnim.Play();

			BlastAnim.AnimationCompleted = BlastDelegate;	
		}
	}

	void DieCompleteDelegate (tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip)
	{
		GameObject[] TriggerBoxes = GameObject.FindGameObjectsWithTag ("List");

		for (int x = 0; x < TriggerBoxes.Length; x++)
		{
			TargetList List = TriggerBoxes[x].GetComponent<TargetList>();
			List.RemoveFromList(gameObject);
		}

		LevelManager.getInstance().NumOfCoins += CoinsGiven;
		LevelManager.getInstance().EnemiesKilled += 1;
		Pool.returnTransform(transform);
	}

	void AttackDelegate (tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip)
	{
		Tower.getInstance().setDamage(Damage);
		audio.PlayOneShot(AttackSound);	
	}

	void BlastDelegate (tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip)
	{
		isBlasting = false;
		BlastAnim.gameObject.renderer.enabled = false;
		BlastAnim.gameObject.active = false;
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

				if (EnemyAnimator.GetClipByName("Attack") != null)
					
					EnemyAnimator.Play("Attack");
				
				
				EnemyAnimator.AnimationCompleted = AttackDelegate;

						
			break;

			case Enemystate.Dying:
				
					
				EnemyAnimator.Play("Die");				
				EnemyAnimator.AnimationCompleted = DieCompleteDelegate;

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

	public void GotDamage (int damage)
	{	
		Life -= damage;
		isBlasting = true;
		if (Life <= 0)
			audio.PlayOneShot(DyingSound);
			enemystate = Enemystate.Dying;
	}
	/*
	void OnTriggerEnter (Collider other)
	{	
		if (other.tag == "List" )
		{
			List = other.GetComponent<TargetList>();
			List.AddToList(gameObject);
		}
	}*/
}
