using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// There are 2 scenarios setup in the test scene:
/// 1) ConstantAngle - If you have an object that has a set angle 
///     and you want to hit a target at any location, then this scenario is
///     what you're looking for. You specify a start position (end of the barrel),
///     end position (target) and the launch angle (angle of the cannon) to the
///     TrajectoryHelper.GetVelocityWithAngleAndTarget method and you get a velocity
///     in return.
/// 2) ConstantVelocity - In a more real world case, you can't modify the velocity
///     on the fly to reach a target that you want to. When you shoot the same 
///     rounds out of a gun, they get released at the same velocity and it's up to 
///     you, as the shooter, to angle the gun to reach the target. This also uses
///     the TrajectoryHelper.GetAngle method, which finds the angle required in 
///     order for the specified velocity to reach the target. The TrajectoryHelper.
///     GetMaxRange method can be used to calculate the max range, which might be
///     useful for a tower defense game for example.
/// </summary>
public class TestSceneScript : MonoBehaviour
{
    enum CurrentState
    {
        ConstantAngle,
        ConstantVelocity
    }
    private CurrentState currentState = CurrentState.ConstantAngle;
    private int stateIndex = 0;

    //If our mouse is over the GUI menu, don't fire projectiles.
    public static bool IsMouseOverGUI = false;

    //Only 1 projectile should be active at a time (this is just a preference
    //for this test scene)
    public static bool IsProjectileActive = false;
    public static LineRenderer LineRndr;


    public Transform TallObject;

    //CannonBarrel gets rotated up/down (angle)
    public Transform CannonBarrel;

    //CannonParent is the empty game object that holds the parts of our cannon.
    //Modeling program has Z direction as UP, while Unity has it as Y so the
    //rotations on the model get imported strangely. Parenting the model to
    //an empty game object lets us rotate it as expected.
    public Transform CannonParent;

    //This is the point that the projectile is spawned, place at the end of the cannon.
    public Transform CannonBarrelExit;

    //Our prefab objects that are created when we fire a projectile.
    public Transform PrefabTarget;
    public Transform PrefabProjectile;

    //How fast should the cannon turn? In degrees per second.
    public float CannonTurningSpeed = 90;

    private TrajectoryHelper trajectoryHelper;
    private Rect guiRect = new Rect(0, 0, Screen.width * 0.33f, Screen.height * 0.4f);
    private float cannonHeight = 1.6f;
    private float cannonAngle;
    private string cannonAngleString = "";
    private string errorString = "";
    private bool followProjectile = false;

    //This is used for the "SetVelocity" scenario. When we set a specific velocity
    //there are (possibly) 2 available trajectories we can follow. (1) has a higher
    //max height + longer duration (like a mortar shell) while the other (2) is a 
    //more direct shot with a lower max height and shorter duration (like a bullet)
    private bool useHighAngle = true;
    private float constantVelocity = 15f;
    private string velocityString = "";

    void Awake()
    {
        //Grab our references
        trajectoryHelper = GetComponent<TrajectoryHelper>();
        LineRndr = GetComponent<LineRenderer>();

        //Set the initial cannonAngle variable to match our cannon's current rotation
        cannonAngle = 360 - CannonBarrel.eulerAngles.x;
    }

    void OnGUI()
    {
        IsMouseOverGUI = false;

        GUILayout.BeginArea(guiRect);
        GUILayout.BeginVertical("box");

        if (guiRect.Contains(Event.current.mousePosition))
            IsMouseOverGUI = true;

        GUILayout.Label("Click on the ground to shoot the cannon.");

        followProjectile = GUILayout.Toggle(followProjectile, "Follow projectile w/ camera?");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Cannon Height:");
        cannonHeight = GUILayout.HorizontalSlider(cannonHeight, 1.6f, 15f);
        if (GUI.changed)
        {
            CannonParent.position = new Vector3(CannonParent.position.x, cannonHeight, CannonParent.position.z);
        }
        GUILayout.EndHorizontal();

        if (currentState == CurrentState.ConstantAngle)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(cannonAngleString);
            cannonAngle = GUILayout.HorizontalSlider(cannonAngle, 5f, 85f);
            //Quaternion expecterRotation = new Quaternion(cannonAngle, Cannon.rotation.y, Cannon.rotation.z, Cannon.rotation.w);
            if (GUI.changed)
            {
                CannonBarrel.eulerAngles = new Vector3(
                    360 - cannonAngle,
                    CannonBarrel.rotation.eulerAngles.y,
                    CannonBarrel.rotation.eulerAngles.z);

                cannonAngleString = "Cannon Angle: " + cannonAngle.ToString("F2"); ;
            }

            GUILayout.EndHorizontal();
        }
        else if (currentState == CurrentState.ConstantVelocity)
        {
            //GUILayout.BeginHorizontal();
            //GUILayout.Label(cannonAngleString);
            //cannonAngle = GUILayout.HorizontalSlider(cannonAngle, 5f, 85f);
            //if (GUI.changed)
            //{
            //    CannonBarrel.eulerAngles = new Vector3(
            //        360 - cannonAngle,
            //        CannonBarrel.rotation.eulerAngles.y,
            //        CannonBarrel.rotation.eulerAngles.z);
            //}
            //GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(velocityString);
            constantVelocity = GUILayout.HorizontalSlider(constantVelocity, 5f, 25f);
            GUILayout.EndHorizontal();

            useHighAngle = GUILayout.Toggle(useHighAngle, "Use higher angle?");
        }

        string[] states = System.Enum.GetNames(typeof(CurrentState));
        stateIndex = GUILayout.SelectionGrid(stateIndex, states, 1);
        if (GUI.changed)
        {
            //if (Event.current.type == EventType.Repaint)
            currentState = (CurrentState)stateIndex;
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();

        if (!string.IsNullOrEmpty(errorString))
        {
            GUI.color = Color.yellow;
            GUI.Label(new Rect(guiRect.xMax, 20, Screen.width - guiRect.width, 200), errorString);
        }
    }

    void Update()
    {
        cannonAngleString = "Cannon Angle: " + cannonAngle.ToString("F2"); ;
        velocityString = "Velocity: " + constantVelocity.ToString("F2");

        //When the release the left mouse click, make sure we're not moused-over the GUI
        //and there isn't a projectile active
        if (Input.GetMouseButtonUp(0) && 
            !IsMouseOverGUI && 
            !IsProjectileActive)
        {
            RaycastHit hit;
            if (Physics.Raycast(
                Camera.main.ScreenPointToRay(Input.mousePosition),
                out hit))
            {
                Vector3 targetPosition = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
                //Vector3 targetPosition = new Vector3(0, hit.point.y + 0.1f, 20);

                //Place our target texture where we clicked (with a 0.1f buffer so Z-fighting doesn't occur)
                Transform target = (Transform)Instantiate(
                    PrefabTarget,
                    targetPosition,
                    PrefabTarget.rotation);
                //As a precaution, destroy the target 15 seconds after it's created.
                Destroy(target.gameObject, 15f);

                //Start rotating towards our target point. Once we reach that point
                //we'll call the FireProjectile method
                StartCoroutine(RotateTowardsTarget(target));
            }
        }
    }

    IEnumerator RotateTowardsTarget(Transform target)
    {
        Quaternion fromQuat = CannonParent.transform.rotation;
        Vector3 posDifference = target.position - CannonParent.position;
        Quaternion toQuat = Quaternion.LookRotation(
            posDifference - Vector3.Project(posDifference, CannonParent.up),
            CannonParent.up);

        //Apply a constant rotation each tick (avoids the usual slowdown you
        //see at the end of a regular Lerp function)
        float dist = Quaternion.Angle(CannonParent.rotation, toQuat);
        for (float i = 0.0f; i < 1.0f; i += (CannonTurningSpeed * Time.deltaTime) / dist)
        {
            CannonParent.rotation = Quaternion.Lerp(fromQuat, toQuat, i); // rotates unit in the direction of movement

            yield return 0; // wait for 1 frame
        }

        CannonParent.rotation = toQuat;

        FireProjectile(target);
    }

    private void FireProjectile(Transform target)
    {
        Vector3 velocity = Vector3.zero;
        Vector3 targetPosition = target.position;

        if (currentState == CurrentState.ConstantAngle)
        {
            //Use our trajectoryHelper class to find the initial velocity based
            ////on our current angle and our target destination.
            velocity = trajectoryHelper.GetVelocityWithAngleAndTarget(
                CannonBarrelExit,
                targetPosition,
                cannonAngle);
        }
        else if (currentState == CurrentState.ConstantVelocity)
        {
            //Find the angle required to reach the destination w/ the specified velocity.
            //i.e. a projectile speed of "15". If this is stored as a Vector3 instead of
            //a float, use myVector3Speed.magnitude to turn it into a float.
            cannonAngle = trajectoryHelper.GetAngle(
                constantVelocity,
                CannonBarrelExit.position,
                targetPosition,
                useHighAngle);

            if (float.IsNaN(cannonAngle))
            {
                Debug.LogWarning("Trajectory not possible with a velocity of: " + constantVelocity + ". Try increasing the velocity.");
                errorString = "Trajectory not possible with a velocity of: " + constantVelocity + ". Try increasing the velocity.";
                StopCoroutine("ResetErrorString");
                StartCoroutine("ResetErrorString", 4f);

                Destroy(target.gameObject);
                return;
            }

            //Run the calculation a few more times. This is required because we do
            //NOT know the angle to start out with (so we use the current position
            //of the barrel, then rotate to the angle we calculated.). This helps
            //increase the accuracy of the projectile. This shouldn't be run more
            //than about 5 times, since the results diminish with each run.
            for (int i = 0; i < 3; i++)
            {
                //Set the cannon to the angle we found
                CannonBarrel.eulerAngles = new Vector3(
                    360 - cannonAngle,
                    CannonBarrel.eulerAngles.y,
                    CannonBarrel.eulerAngles.z);

                cannonAngle = trajectoryHelper.GetAngle(
                    constantVelocity,
                    CannonBarrelExit.position,
                    targetPosition,
                    useHighAngle);
            }
            //Debug.Log("Angle: " + angle);

            //Velocity is set to a specific speed in the direction our barrel is facing
            velocity = constantVelocity * CannonBarrelExit.forward;
        }

        //Do some error checking before we proceed so we can cancel our if we
        //hit anything bad (like an invalid/non-existent velocity).
        if (float.IsNaN(velocity.magnitude))
        {
            Debug.LogWarning("Invalid trajectory. Double check that you have a reasonable angle assigned. (i.e. if you have an angle of 5 degrees and are trying to shoot something 50 units above you, it won't be possible)");
            errorString = "Invalid trajectory. Double check that you have a reasonable angle assigned. (i.e. if you have an angle of 5 degrees and are trying to shoot something 50 units above you, it won't be possible)";
            StopCoroutine("ResetErrorString");
            StartCoroutine("ResetErrorString", 10f);

            //Destroy the target prefab we just created above.
            Destroy(target.gameObject);
            return;
        }
        if (velocity == Vector3.zero)
        {
            //This shouldn't be hit, because we have 3 scenarios which are all accounted for.
            Debug.LogWarning("Velocity wasn't assigned?");
            Destroy(target.gameObject);
            return;
        }

        //Grab the list of points along our path and assign them to the line renderer.
        List<Vector3> points = trajectoryHelper.GetTrajectoryPath(
            velocity,
            CannonBarrelExit.position,
            10,
            trajectoryHelper.TimeToReachTarget(
                velocity,
                cannonAngle,
                CannonBarrelExit.position.y,
                targetPosition.y));
        LineRndr.SetVertexCount(points.Count);
        for (int i = 0; i < points.Count; i++)
        {
            LineRndr.SetPosition(i, points[i]);
        }


        Debug.Log("*******Velocity: " + velocity + " Magnitude: " + velocity.magnitude);
        Debug.Log("Angle: " + (360 - CannonBarrel.eulerAngles.x));
        Debug.Log("Calc total time: " + trajectoryHelper.TimeToReachTarget(
            velocity,
            360 - CannonBarrel.eulerAngles.x,
            CannonBarrelExit.position.y,
            targetPosition.y));
        Debug.Log("Calc apex time: " + trajectoryHelper.TimeToReachApex(
            velocity,
            360 - CannonBarrel.eulerAngles.x));
        Debug.Log("Calc distance: " + trajectoryHelper.GetMaxRange(
            velocity,
            360 - CannonBarrel.eulerAngles.x,
            CannonBarrelExit.position.y,
            targetPosition.y));

        //Create a projectile
        Transform projectile = (Transform)GameObject.Instantiate(
            PrefabProjectile,
            CannonBarrelExit.position,
            PrefabProjectile.rotation);

        //Flip our switch since we just spawned a projectile.
        IsProjectileActive = true;

        //Assign the velocity we found to the projectile.
        projectile.rigidbody.velocity = velocity;
        TrajectoryHelper.LaunchTime = Time.time;

        //Attach the camera to our bullet so we can follow it's trajectory
        if (followProjectile)
        {
            Camera.main.transform.parent = projectile.transform;
        }
    }

    IEnumerator ResetErrorString(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        errorString = "";
    }
}
