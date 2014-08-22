using UnityEngine;
using System.Collections;

public class PeopleScript : MonoBehaviour {

	#region playerstate

	public enum Playerstate
	{
		Idle,
		Shooting, 
		Moving,
		Reserve
	}
	private Playerstate _playerstate;
	public Playerstate playerstate
	{
		get { return _playerstate; }
		set 
		{
			_playerstate = value;
			ChangeState();
		}
	}

	#endregion

	#region tk2d
	 
	private tk2dSprite PlayerSprite;
	private tk2dSpriteAnimator PlayerAnimator;

	#endregion

	private TargetList ListOfTarget;
	private GameObject Target;
	private TrajectoryHelper Helper;
	private bool CanShoot = true;
	private Transform WeaponPoint;
	public Transform WeaponPrefab;

	// Use this for initialization
	void Start () {

		WeaponPoint = transform.FindChild("WeaponPoint");
		PlayerSprite = gameObject.GetComponent<tk2dSprite>();
		PlayerAnimator = gameObject.GetComponent<tk2dSpriteAnimator>();
		Helper = gameObject.GetComponent<TrajectoryHelper>();

		ChangeTargetSide();
		playerstate = Playerstate.Idle;

	}
	
	// Update is called once per frame
	void Update () {

		ChangeState();
	}

	void ChangeState()
	{
		switch (_playerstate)
		{
			case Playerstate.Idle: 
				
				CanShoot = true;

				PlayerAnimator.Stop();
				PlayerSprite.SetSprite(PlayerSprite.Collection.Count);
							          
				if (transform.position.x != 0)
				{	
					Target = ListOfTarget.getNearestEnemy();
				
					if (Target != null)
						playerstate = Playerstate.Shooting;			
				}
				else playerstate = Playerstate.Reserve;		

			break;

			case Playerstate.Shooting: 
				
				Target = ListOfTarget.getNearestEnemy();
				
				if (Target == null)
					playerstate = Playerstate.Idle;
				else
				{
					if (CanShoot)
					{	
						PlayerAnimator.Play();					
						CanShoot = false;

					}
					else if (!CanShoot && !PlayerAnimator.IsPlaying(PlayerAnimator.CurrentClip))
				  	{						
						Transform Weapon = Instantiate(WeaponPrefab) as Transform;					
						
						Weapon.position = WeaponPoint.position;
						Weapon.localScale = transform.localScale;

						Vector3 Rotation = new Vector3(Weapon.eulerAngles.x, Weapon.eulerAngles.y, (Weapon.eulerAngles.z + transform.eulerAngles.z) * transform.localScale.x);
						Weapon.eulerAngles = Rotation;
											
						Weapon.rigidbody.velocity = Helper.GetVelocityWithAngleAndTarget(WeaponPoint, Target.transform.position,  Weapon.eulerAngles.z) * transform.localScale.x;
						
						CanShoot = true;
					}
				}

				break;	

			case Playerstate.Moving: 
				
				CanShoot = true;
				
				PlayerAnimator.Stop();
				PlayerSprite.SetSprite(PlayerSprite.Collection.Count);							          
				
				ChangeTargetSide();				
				
			break;

			case Playerstate.Reserve:

			break;

		}
	}

	public void ChangeTargetSide()
	{
		string targetName = gameObject.name;
		Vector2 Scale;
		
		if (transform.position.x > 0)
		{
			targetName = targetName + "_RBox";
			Scale = new Vector2 (1, 1);
			transform.localScale = Scale;
		}
		else if (transform.position.x < 0)
		{
			targetName = targetName + "_LBox";
			Scale = new Vector2 (-1, 1);
			transform.localScale = Scale;
		}

		ListOfTarget = GameObject.Find(targetName).GetComponent<TargetList>();

	}

}
