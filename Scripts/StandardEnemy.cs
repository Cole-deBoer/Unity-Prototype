using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StandardEnemy : Enemy
{
   [SerializeField] private float currentHealth;
   
   protected override float AttackDamage { get; set; }
   protected override float MovementSpeed => 15;
   protected override int Health => 200;
   protected override float AttackSpeed => 0.5f;

   private States _standardEnemyState;
   private Rigidbody _enemyRb;
   private PlayerAttributes _playerAttributes;

   private void Awake()
   {
       _playerAttributes = FindObjectOfType<PlayerAttributes>();
       currentHealth = Health; 
       StartCoroutine(StateController(_standardEnemyState, this));
       _enemyRb = gameObject.GetComponent<Rigidbody>();
   }

   protected override void Patrol()
   {
      print("Patrolling");
      transform.Rotate(Vector3.up, MovementSpeed * 4 * Time.deltaTime);
   }

   protected override void Attack()
   {
      print("Attacking");
   }

   protected override void Idle()
   {
      print("idling");
   }

   protected override void Pursuit()
   {
      print("Pursuing");
      Debug.DrawRay(transform.position, _playerAttributes.transform.position -transform.position, Color.magenta, 3);
      transform.LookAt(_playerAttributes.transform);
      _enemyRb.AddForce(transform.forward * MovementSpeed, ForceMode.Force);
   }

   protected override void Flee()
   {
      print("Fleeing");
   }

   public override void TakeDamage(int damageAmount)
   {
      currentHealth -= damageAmount;
      if (!(currentHealth < 1f)) return;
      gameObject.GetComponent<MeshRenderer>().material.color = Color.red; 
      Destroy(gameObject, 0.5f);
   }
}
