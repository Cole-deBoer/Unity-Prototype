using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private int _launchForce = 1;
    internal bool canAttack;
    private bool _isAttacking;
    [SerializeField] private Animator animator;

    private PlayerAttributes _attributes;

    private void Start()
    {
        _attributes = FindObjectOfType<PlayerAttributes>();
    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (!Input.GetMouseButtonDown(0) && canAttack) return;
        canAttack = false;
        animator.SetBool(animator.parameters[0].name, true);
        StartCoroutine(ResetAttackCooldown(_attributes.attackSpeed, animator.parameters[0]));
    }

    internal IEnumerator ResetAttackCooldown(float timeToWait, AnimatorControllerParameter animationParameter)
    {
        _isAttacking = true;
        yield return new WaitForSeconds(timeToWait); 
        animator.SetBool(animationParameter.name, false);
        canAttack = true;
        _isAttacking = false;
        _attributes.attack = _attributes.baseAttack;
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (!hit.CompareTag("Enemy") || !_isAttacking) return;
        int damage;
        if (_attributes.attack >= _attributes.defense)
        {
            damage = _attributes.attack * 2 - _attributes.defense;
        }
        else
        {
            damage = _attributes.attack * _attributes.attack / _attributes.defense;
        }
        hit.GetComponent<Enemy>().TakeDamage(damage);
        print("Damage Amount: " + damage);

        var directionToShoot = -(transform.position - hit.transform.position);
        hit.attachedRigidbody.AddForce(directionToShoot * _launchForce, ForceMode.Impulse);
    }
}
