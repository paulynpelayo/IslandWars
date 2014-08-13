Simple Trajectories created by Jeff Brown - http://robotfootgames.com


//////Installation in your own project:
1) Add the TrajectoryHelper class to the object(s) that are going to be shooting
	or throwing your projectiles (i.e. the cannon or gun).
2) The simplest way to get things in motion is to use the GetVelocityWithAngleAndTarget
	method in the TrajectoryHelper class. When you spawn a projectile, set the
	velocity on your rigidbody projectile by calling something similar to the following:
	"rigidbody.velocity = myTrajectoryHelper.GetVelocityWithAngleAndTarget(cannonTransform, targetPosition, angleInDegrees);"
3) You need to make sure that your gun/cannon/whatever is rotated to face the object
	you're shooting at BEFORE you find/assign the velocity. If you play through the
	test scene you'll notice how the cannon is rotated to face the target before we
	fire at the target. This is because the velocity is calculated along the Y/Z plane
	and gets the direction from the forward direction of the cannon's barrel.


//////Notes:
-Only gravity is taken into account. Acceleration, wind speed, drag, etc are ignored.
-Basic error checking is done to prevent errors from being thrown. Obvious error-prone
 areas are setting the angle 90 (straight up) or 0 (if on solid ground). The angles
 in the test scene are clamped between 5 and 85 to avoid this.
