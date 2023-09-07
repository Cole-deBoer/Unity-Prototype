using UnityEngine;
public class PlayerAttributes : MonoBehaviour
{
   internal float PlayerMaxHealth => 100f;
   internal float playerCurrentHealth = 100f;
   internal float spellCooldown;
   internal int baseAttack = 10;
   internal int attack = 10;
   internal int defense = 5;
   internal float attackSpeed = 0.5f;
}
