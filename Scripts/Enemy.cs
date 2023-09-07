using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public abstract class Enemy : MonoBehaviour
{
    protected abstract float AttackDamage {get; set;}
    protected abstract float MovementSpeed { get;}
    protected abstract int Health { get;}
    protected abstract float AttackSpeed { get;}

    private Vector3 _position;
   
    protected abstract void Patrol();
    protected abstract void Attack();
    protected abstract void Idle();
    protected abstract void Pursuit();
    protected abstract void Flee();

    public abstract void TakeDamage(int damageAmount);

    private Vector3 _target;

    private void Awake()
    {
        _target = FindObjectOfType<PlayerAttributes>().gameObject.transform.position;
    }

    protected IEnumerator StateController(States state, Enemy enemy) 
    {
        while (true)
        {
            print("Controlling States");
            switch (state)
            {
                case States.Idling:
                    enemy.Idle();
                    print((_target - enemy.transform.position).magnitude); //  WHAT THE FUCK IS WRONG WITH YOU YOU STUPID BITCH KILL YOURSELF FOR THE LOVE OF GOD YOU CANT MAKE IT YOU CANT MAKE IT KILL YOURSLEF BITCH!!!
                    if ((_target - enemy.transform.position).magnitude < 2)
                    {
                        state = States.Patrolling;
                    }
                    yield return new WaitForSeconds(1);
                    break;
                case States.Patrolling:
                    enemy.Patrol();
                    Debug.DrawRay(enemy.transform.position,  _target - enemy.transform.position * 25, Color.magenta, 10);
                    if (Physics.Raycast(enemy.transform.position, _target * 25, out var hit) && hit.collider.CompareTag("Player"))
                    {
                        state = States.Pursuing;
                    }
                    else
                    {
                        print(hit.collider.tag);
                    }
                    yield return new WaitForSeconds(0.01f);
                    break;
                case States.Pursuing:
                    enemy.Pursuit();
                    state = (_target - enemy.transform.position).magnitude > 15 ? States.Patrolling : States.Pursuing;
                    yield return new WaitForSeconds(1);
                    break;
                case States.Attacking:
                    enemy.Attack();
                    yield return new WaitForSeconds(enemy.AttackSpeed);
                    break;
                case States.Fleeing:
                    enemy.Flee();
                    yield return new WaitForSeconds(4);
                    break;
                default:
                    enemy.Idle();
                    throw new ArgumentOutOfRangeException(nameof(state), state, "Passed an invalid state to the StateController");
            }
            yield return null;
        }
    }
    
    protected enum States
    {
        Idling,
        Patrolling,
        Pursuing,
        Attacking,
        Fleeing
    }
}
