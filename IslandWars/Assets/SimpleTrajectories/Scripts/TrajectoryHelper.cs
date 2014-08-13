using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Attach this component to any object that needs to travel along a ballistic path.
/// 
/// For the simplest setup, attach this script to a projectile that has a rigidbody
/// attached and is supposed to follow a trajectory path. Then use this script to 
/// assign a velocity to your rigidbody. Example:
/// 
/// public Transform endOfBarrelTransform;
/// public Vector3 targetPosition;
/// public float angle;
/// 
/// Then, when the projectile is created, use those variables to create the velocity:
/// "rigidbody.velocity = myTrajectoryHelper.GetVelocityWithAngleAndTarget(endOfBarrelTransform, targetPosition, angle);"
/// </summary>
public class TrajectoryHelper : MonoBehaviour 
{
    public static float LaunchTime;

    //These are simply to compare the real results with the calculated results, these
    //can be removed, along with the Debug.Log calls they use, without issues.
    private float actualTotalTime, actualDistanceTravelled, actualHeightReached, actualPeakTime;
    private List<Vector3> pathPointsList = new List<Vector3>();

    /// <summary>
    /// With a starting point, target point and a specific angle, returns the velocity required
    /// to reach the target.
    /// </summary>
    /// <param name="initialTransform">Starting position of this projectile (i.e. end of the barrel).</param>
    /// <param name="destinationPos">The target position of this projectile.</param>
    /// <param name="angle">The initial angle of this projectile (i.e. rotation of the barrel)</param>
    /// <returns>Returns the velocity required to reach the target with the specified angle.</returns>
    public Vector3 GetVelocityWithAngleAndTarget(Transform initialTransform, Vector3 destinationPos, float angle)
    {
        Vector3 initialPos = initialTransform.position;

        //Formula to calculate initial velocity
        //x = horizontal distance, y = altitude change
        //Vo = sqrt( (0.5 * gravity * x² * (tan²angle + 1))  /  (x * tan(angle) - y) )

        float angleInRads = angle * Mathf.Deg2Rad;

        Vector3 direction = destinationPos - initialPos;
        
        //We need to grab the horizontal displacement (distance), so the Y
        //value isn't needed.
        direction.y = 0;
        float zDistance = direction.magnitude;

        float halfGravityDistanceSquared = 0.5f * Physics.gravity.magnitude * (zDistance * zDistance);
        float tanThetaSquared = Mathf.Tan(angleInRads) * Mathf.Tan(angleInRads) + 1;
        float topHalf = halfGravityDistanceSquared * tanThetaSquared;

        float heightDiff = destinationPos.y - initialPos.y;

        float bottomHalf = zDistance * Mathf.Tan(angleInRads) - heightDiff;

        float topDivideBottom = topHalf / bottomHalf;

        float initialVelocity = Mathf.Sqrt(topDivideBottom);
        //Debug.Log("Vo: " + initialVelocity);

        //Translate our 2D X/Y initial velocity to a 3D vector

		return initialVelocity * initialTransform.right;
    }

    /// <summary>
    /// Returns the angle required for the projectile to reach it's target
    /// when the initial velocity is already set (i.e. "15 m/s").
    /// </summary>
    /// <param name="initialVelocity">The initial velocity of this object.</param>
    /// <param name="initialPosition">The starting point of the projectile (i.e. end of the barrel).</param>
    /// <param name="targetPosition">The target location of this projectile.</param>
    /// <param name="useHighAngle">All trajectory angles (except 45 degrees) have 2 possible
    /// paths they can take; a higher angle (greater max height, longer duration. i.e. a mortar shell) 
    /// and a lower angle (lower max height, shorter duration. i.e. a bullet)</param>
    /// <returns>Returns the angle (in degrees) required for the specified velocity to reach the target.</returns>
    public float GetAngle(float initialVelocity, Vector3 initialPosition, Vector3 targetPosition, bool useHighAngle)
    {
        Vector3 direction = targetPosition - initialPosition;
        Vector3 distance = direction;
        distance.y = 0;
        float x = distance.magnitude; //direction.z;
        float y = direction.y;
        float v = initialVelocity;
        float g = Physics.gravity.magnitude;

        //http://en.wikipedia.org/wiki/Trajectory_of_a_projectile#Angle_required_to_hit_coordinate_.28x.2Cy.29
        float v2 = v * v;
        float v4 = v2 * v2;
        float x2 = x * x;

        //ALL angles (except 45 degrees) will have 2 trajectories to reach the target location
        //(if the velocity is great enough to reach the target). The higher angle will produce
        //a longer duration + higher shot (like a mortar shell), while the lower angle will 
        //produce a more direct shot (like a bullet) that takes less time and has a lower max height.
        //The difference is the +/- sign right after the "Mathf.Atan2(v2..." part of the equation
        //below (Quadratic formula)
        if (useHighAngle)
        {
            float theta = Mathf.Atan2(v2 + Mathf.Sqrt(v4 - g * (g * x2 + 2 * y * v2)), g * x);
            return theta * Mathf.Rad2Deg;
        }
        else
        {
            float theta = Mathf.Atan2(v2 - Mathf.Sqrt(v4 - g * (g * x2 + 2 * y * v2)), g * x);
            return theta * Mathf.Rad2Deg;
        }
    }

    /// <summary>
    /// How long will it take to reach the highest point in our trajectory?
    /// </summary>
    /// <param name="velocity">The initial velocity</param>
    /// <returns>The duration between the launch time and reaching the highest point in the trajectory.</returns>
    public float TimeToReachApex(Vector3 initialVelocity, float initialAngle)
    {
        //time_apex = velocity * sin(angle) / gravity
        float initialVelocityY = initialVelocity.magnitude * Mathf.Sin(initialAngle * Mathf.Deg2Rad);

        //float time = initialVelocity.magnitude / Physics.gravity.magnitude;
        float time_apex = initialVelocityY / Physics.gravity.magnitude;

        return time_apex;
    }

    /// <summary>
    /// Returns the amount of time it takes to reach the target from the launch point. 
    /// </summary>
    /// <param name="initialVelocity">The initial velocity of the object</param>
    /// <param name="launchAngle">The initial launch angle</param>
    /// <param name="startingYHeight">Where does this projectile originate (end of the barrel)</param>
    /// <param name="targetYHeight">Where does this projectile end at?</param>
    /// <returns></returns>
    public float TimeToReachTarget(Vector3 initialVelocity, float launchAngle, float startingYHeight, float targetYHeight)
    {
        float heightDiff = startingYHeight - targetYHeight;
        //finalYVelocity^2 = initialYVelocity^2 + (2 * gravity * heightDiff)
        //IF the launch height and destination height are at the same level, finalVelocity
        //is always equal to the negative initialYVelocity. In this case, the launce height
        //and destination height are different, so we need to solve for the finalYVelocity first
        float initialYVelocity = initialVelocity.magnitude * Mathf.Sin(launchAngle * Mathf.Deg2Rad);
        float initialYVelocitySquared = initialYVelocity * initialYVelocity;
        float twoGravityHeight = 2 * (Physics.gravity.magnitude * heightDiff);
        float result = initialYVelocitySquared + twoGravityHeight;
        float finalYVelocity = Mathf.Sqrt(result);

        //Now we can use the following equation to find out how long we're in the air:
        //-finalYVelocity = initialYVelocity + (gravity * time)
        //rearranged: (-finalYVelocity - initialYVelocity) / -gravity
        float time = (-finalYVelocity - initialYVelocity) / -Physics.gravity.magnitude;
        return time;
    }

    public float GetMaxRange(Vector3 initialVelocity, float initialAngle, float startYPos, float targetYPos)
    {
        float heightDiff = startYPos - targetYPos;

        /*  "((velocity * cos(angle))"  */
        float initialXVelocity = initialVelocity.magnitude * Mathf.Cos(initialAngle * Mathf.Deg2Rad);

        //"((velocity * cos(angle)) / gravity)"
        float initialXVelocityDividedByGravity = initialXVelocity / Physics.gravity.magnitude;

        /*  "(velocity * sin(angle))"   */
        float initialYVelocity = initialVelocity.magnitude * Mathf.Sin(initialAngle * Mathf.Deg2Rad);

        //"(velocity * sin(angle))^2"
        float initialYVelocitySquared = initialYVelocity * initialYVelocity;

        //"(2 * gravity * heightDiff)"
        float twoTimesGravityTimesHeightDiff = 2 * Physics.gravity.magnitude * heightDiff;

        //"sqrt((velocity * sin(angle))^2 + (2 * gravity * heightDiff))"
        float sqrt = Mathf.Sqrt(initialYVelocitySquared + twoTimesGravityTimesHeightDiff);

        float result = initialXVelocityDividedByGravity * (initialYVelocity + sqrt);

        //Debug.LogWarning("**ACTUAL Hdistance: " + Vector3.Distance(
        //    new Vector3(target.x, 0, target.z),
        //    new Vector3(barrelPosition.x, 0, barrelPosition.z)));
        return result;
    }

    /// <summary>
    /// Returns a List of points along the trajectory path.
    /// </summary>
    /// <param name="initialVelocity">The initial velocity of the projectile.</param>
    /// <param name="startPos">The starting position of the projectile (i.e. end of the barrel).</param>
    /// <param name="numOfPoints">The number of points along the path. More points = smoother curve.</param>
    /// <param name="totalTime">The duration of the projectile's trajectory. TimeToReachTarget() can be used to calculate this time.</param>
    /// <returns></returns>
    public List<Vector3> GetTrajectoryPath(Vector3 initialVelocity, Vector3 startPos, int numOfPoints, float totalTime)
    {
        //Formulas to get the position on each axis at a specified time:
        //Vix = initial velocity on the x plane.
        //x = Vix * t
        //y = Viy*t + (0.5 * g * time²)
        //z = Viz * t

        //Clear any previously saved points.
        pathPointsList.Clear();

        for (int i = 0; i < numOfPoints; i++)
        {
            float timeMarker = totalTime * ((float)i / (numOfPoints - 1));
            
            float currentX = initialVelocity.x * timeMarker;
            float currentY = initialVelocity.y * timeMarker + (0.5f * Physics.gravity.y * (timeMarker * timeMarker));
            float currentZ = initialVelocity.z * timeMarker;

            pathPointsList.Add(startPos + new Vector3(currentX, currentY, currentZ));
        }

        return pathPointsList;
    }
}
