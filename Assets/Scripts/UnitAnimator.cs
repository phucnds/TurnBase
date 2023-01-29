using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += moveAction_OnStartMoving;
            moveAction.OnStopMoving += moveAction_OnStopMoving;
        }

        if(TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.onShoot += shootAction_onShoot;
        }
    }

    

    private void moveAction_OnStartMoving()
    {
        animator.SetBool("IsWalking", true);
    }

    private void moveAction_OnStopMoving()
    {
        animator.SetBool("IsWalking", false);
    }

    private void shootAction_onShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");
        Transform bulletProjectilePrefabTransform = Instantiate(bulletProjectilePrefab,shootPointTransform.position,Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectilePrefabTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPostition();
        targetUnitShootAtPosition.y = shootPointTransform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
    }
}
