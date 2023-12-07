using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Ñlockwise,
    Ñounterclockwise
}

public class WeaponMovementController : MonoBehaviour
{
    [SerializeField] protected Transform ownerTransform;
    [SerializeField] protected Transform weaponModelTransform;
    [SerializeField] protected Transform rotationPoint;
    [SerializeField] protected Direction rotateDirection;
    protected Vector3 directionVector;
    [SerializeField] protected float rotationSpeed;

    private void Awake()
    {
        transform.SetParent(null);
    }
    private void Start()
    {
        CheckRotationToVector();
    }
    void FixedUpdate()
    {
        transform.Rotate(directionVector, rotationSpeed * Time.fixedDeltaTime);
        transform.position = rotationPoint.position;
    }
    private void Update()
    {
        //weaponModelTransform.RotateAround(rotationPoint.position,
        //   directionVector,
        //   rotationSpeed * Time.deltaTime);
       
    }
    public void ResetWeapon()
    {
        transform.position = rotationPoint.position;
        transform.eulerAngles = Vector3.zero;
        weaponModelTransform.localPosition = new Vector3(1.056f, 1, 0);
        weaponModelTransform.eulerAngles = Vector3.zero;
    }
    
    void CheckRotationToVector()
    {
        if (rotateDirection == Direction.Ñlockwise)
        {
            directionVector = Vector3.up;
        }
        else directionVector = -Vector3.up;
    }
    public void SetAntiDirection(Direction playerDirection)
    {
        if (playerDirection == Direction.Ñlockwise)
            rotateDirection = Direction.Ñounterclockwise;       
        else
            rotateDirection = Direction.Ñlockwise;
        CheckRotationToVector();
    }
  
    public void SetRotationSpeed(float rotateSpeed)
    {
        this.rotationSpeed = rotateSpeed;
    }
    public void SwapWeaponDirection()
    {
        if (rotateDirection == Direction.Ñlockwise)
            rotateDirection = Direction.Ñounterclockwise;
        else rotateDirection = Direction.Ñlockwise;

        CheckRotationToVector();
        weaponModelTransform.transform.Rotate(180,0, 0);
    }
    public Direction GetRotationDirection()
    { 
        return rotateDirection;
    }
}

