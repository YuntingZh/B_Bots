using System.Collections;
using UnityEngine;

public class PliersController : MonoBehaviour
{
    public float grabDistance = 8.0f;
    public Transform leftGrabPoint;
    public Transform rightGrabPoint;
    public LayerMask grabbableLayer;
    public GameObject leftHandTarget;
    public GameObject rightHandTarget;
    public float handSpeed = 5.0f;
    public PlayerInputController playerInputController;

    private GameObject grabbedObject;
    private Collider2D[] colliders;
    private bool isHandMoving;
    private Vector3 originalLeftHandTargetPosition;
    private Vector3 originalRightHandTargetPosition;

    private enum Hand { Left, Right }
    private Hand grabbingHand;

    private void Start()
    {
        originalLeftHandTargetPosition = leftHandTarget.transform.localPosition;
        originalRightHandTargetPosition = rightHandTarget.transform.localPosition;
    }

    private void Update()
    {
        if (grabbedObject == null)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isHandMoving)
            {
                CheckForGrabbableObjects();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ReleaseObject();
            }
        }

        if (isHandMoving)
        {
            MoveHand();
        }
    }

    private void CheckForGrabbableObjects()
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, grabDistance, grabbableLayer);

        if (colliders.Length > 0)
        {
            grabbedObject = colliders[0].gameObject;
            DetermineCloserHand();
            StartCoroutine(MoveHandToGrabbedObject());
        }
    }

    private void DetermineCloserHand()
    {
        float leftHandDistance = Vector3.Distance(leftHandTarget.transform.position, grabbedObject.transform.position);
        float rightHandDistance = Vector3.Distance(rightHandTarget.transform.position, grabbedObject.transform.position);

        grabbingHand = leftHandDistance <= rightHandDistance ? Hand.Left : Hand.Right;
    }

    private IEnumerator MoveHandToGrabbedObject()
    {
        isHandMoving = true;

        GameObject activeHandTarget = grabbingHand == Hand.Left ? leftHandTarget : rightHandTarget;
        Transform activeGrabPoint = grabbingHand == Hand.Left ? leftGrabPoint : rightGrabPoint;

        // Check for null references and log a debug message
        if (activeHandTarget == null || grabbedObject == null)
        {
            Debug.LogError("activeHandTarget or grabbedObject is null.");
            if (activeHandTarget == null) Debug.LogError("activeHandTarget is not assigned in the Inspector.");
            if (grabbedObject == null) Debug.LogError("grabbedObject is null.");
            yield break;
        }

        while (Vector3.Distance(activeHandTarget.transform.position, grabbedObject.transform.position) > 0.1f)
        {
            // Check if grabbedObject is still not null before moving the hand
            if (grabbedObject != null)
            {
                activeHandTarget.transform.position = Vector3.MoveTowards(activeHandTarget.transform.position, grabbedObject.transform.position, handSpeed * Time.deltaTime);
                yield return null;
            }
            else
            {
                Debug.LogError("grabbedObject has been destroyed or removed from the scene.");
                break;
            }
        }

        isHandMoving = false;
        GrabObject(activeGrabPoint);
    }

    private void MoveHand()
    {
        GameObject activeHandTarget = grabbingHand == Hand.Left ? leftHandTarget : rightHandTarget;
        Transform activeGrabPoint = grabbingHand == Hand.Left ? leftGrabPoint : rightGrabPoint;
        activeHandTarget.transform.position = Vector3.MoveTowards(activeHandTarget.transform.position, activeGrabPoint.position, handSpeed * Time.deltaTime);
    }

    private void GrabObject(Transform activeGrabPoint)
    {
        grabbedObject.transform.position = activeGrabPoint.position;
        grabbedObject.transform.SetParent(activeGrabPoint);
    }

    private void ReleaseObject()
    {
        grabbedObject.transform.SetParent(null);
        grabbedObject = null;

        // Move the hand target back to the original position
        StartCoroutine(MoveHandToOriginalPosition());
    }

    private IEnumerator MoveHandToOriginalPosition()
    {
        isHandMoving = true;

        GameObject activeHandTarget = grabbingHand == Hand.Left ? leftHandTarget : rightHandTarget;
        Vector3 originalPosition = grabbingHand == Hand.Left ? originalLeftHandTargetPosition : originalRightHandTargetPosition;

        Vector3 targetPosition = playerInputController.transform.TransformPoint(originalPosition);
        while (Vector3.Distance(activeHandTarget.transform.position, targetPosition) > 0.1f)
        {
            activeHandTarget.transform.position = Vector3.MoveTowards(activeHandTarget.transform.position, targetPosition, handSpeed * Time.deltaTime);
            yield return null;
        }

        isHandMoving = false;
    }

    public void HandleGrabRelease()
    {
        if (grabbedObject == null)
        {
            CheckForGrabbableObjects();
        }
        else
        {
            ReleaseObject();
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, grabDistance);
        }
    }
}
