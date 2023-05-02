using UnityEngine;
public class GrabObject : MonoBehaviour
{
    public Transform grabObject; // Reference to the grabbable object
    public float maxReachDistance = 5f; // The maximum distance the hand can reach

    private bool isGrabbing = false; // Whether the object is currently grabbed
    private Transform originalParent; // The original parent of the grabbable object

    private void Start()
    {
        // Record the original parent of the grabbable object
        originalParent = grabObject.parent;
    }

    private void Update()
    {
        // If the grab key is pressed and the object is not currently grabbed, try to grab the object
        if (Input.GetKeyDown(KeyCode.Space) && !isGrabbing)
        {
            // Calculate the target position of the hand
            Vector3 targetPosition = transform.position + transform.forward * maxReachDistance;

            // Check if the target position is within reach of the grabbable object
            if (Vector3.Distance(targetPosition, grabObject.position) <= grabObject.localScale.magnitude)
            {
                // TODO:Use FABRIK IK to control the position and orientation of the hand
                

                // Make the grabbable object a child of the hand
                grabObject.SetParent(transform);


                // Mark the object as grabbed
                isGrabbing = true;
            }
        }

        // If the grab key is released and the object is currently grabbed, release the object
        if (Input.GetKeyUp(KeyCode.Space) && isGrabbing)
        {
            // Reset the parent of the grabbable object
            grabObject.SetParent(originalParent);


            // TODO:Stop using FABRIK IK to control the hand position and orientation
            

            // Mark the object as released
            isGrabbing = false;
        }
    }
}
