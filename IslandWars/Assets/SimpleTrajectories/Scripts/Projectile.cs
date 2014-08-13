using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    //This is simply to reattach our camera to the cannon after it reaches it's target.
    //It's not needed for trajectory stuff at all.
    private Transform cannonCameraPosition;

    void Awake()
    {
        //cannonCameraPosition = GameObject.Find("CameraPosition").transform;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Target")
        {
            //If the projectile hits the target
            Destroy(other.transform.gameObject, 0.5f);

            Debug.Log("Actual time: " + (Time.time - TrajectoryHelper.LaunchTime));
        }

        //Destroy this projectile and reset some defaults
        Destroy();
    }

    void Update()
    {
        //If, for some reason, our projectile is underneath the world (y < 0) then we missed
        //our target, so let's delete the objects and reset some variables.
        if(transform.position.y < 0)
            Destroy();

		//Debug.Log (rigidbody.velocity);
    }

    private void Destroy()
    {
        if (Camera.main.transform.parent == transform)
        {
            Camera.main.transform.parent = cannonCameraPosition;
            Camera.main.transform.position = cannonCameraPosition.position;
        }
        //TestSceneScript.IsProjectileActive = false;

        //Clear the line renderer
        //TestSceneScript.LineRndr.SetVertexCount(0);

        //Destroy the projectile on impact
        Destroy(gameObject);
    }
}
