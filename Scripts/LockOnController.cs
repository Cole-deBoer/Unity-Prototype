using Cinemachine;
using UnityEngine;
public class LockOnController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private new CinemachineVirtualCamera camera;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform head;
    internal bool lockedOn;
    private Vector3 _targetPoint;
    private RotateWithMouse _rotateWithMouse;

    private void Start()
    {
        _rotateWithMouse = FindObjectOfType<RotateWithMouse>();
    }

    private void Update()
    {
        CheckNotNull();
        LockOn(SendPoint());
    }

    private void CheckNotNull()
    {
        if (camera.LookAt ? !camera.LookAt : !head) return;
        _rotateWithMouse.turn.x = playerTransform.localRotation.eulerAngles.y;
        lockedOn = false;
        camera.LookAt = head;
    }

    private Transform SendPoint()
    {
        if (lockedOn) return camera.LookAt;
        
        if (!Input.GetKey(KeyCode.F)) return head; 
        var point = head;
        var hits = Physics.SphereCastAll(mainCamera.ScreenPointToRay(Input.mousePosition).origin, 3f, mainCamera.ScreenPointToRay(Input.mousePosition).direction, 100f); 
        var closestTarget = Mathf.Infinity;
        
        foreach (var hit in hits)
        {
            if (!hit.collider.CompareTag("Enemy")) continue;
            var directionToTarget = hit.point - playerTransform.position;
            var distanceSquaredToTarget = directionToTarget.sqrMagnitude;
            if (distanceSquaredToTarget < closestTarget)
            {
                closestTarget = distanceSquaredToTarget;
                point = hit.transform;
            }
            lockedOn = true;
        }
        return point;
    }

    private void LockOn(Transform target)
    {
        camera.LookAt = target;
        _targetPoint.x = camera.LookAt.position.x;
        _targetPoint.y = playerTransform.position.y;
        _targetPoint.z = camera.LookAt.position.z;
        playerTransform.LookAt(_targetPoint);
        LockOff(target);
    }

    private void LockOff(Transform target) 
    {
        if (!Input.GetKeyUp(KeyCode.F) && !((target.position - playerTransform.position).magnitude > 70f)) return;
        lockedOn = false;
        camera.LookAt = head;
        _rotateWithMouse.turn.x = playerTransform.localRotation.eulerAngles.y;
    }
}
