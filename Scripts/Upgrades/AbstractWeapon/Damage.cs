using System.Numerics;
using Unity.Mathematics;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Damage
{
    
    public Vector3 origin = Vector3.zero;
    public int damageeAmount;
    public float pushForce;
    
    public Damage(int dmg = 0, Vector3? pos = null, float force = 0)
    {
        damageeAmount = dmg;
        if (pos is null) origin = Vector3.zero;
        else origin = (Vector3)pos;
        pushForce = force;

        if (GameModifier.critChance > 0 && Random.Range(1, 100 + 1) <= GameModifier.critChance)
        {
            damageeAmount = (int) (damageeAmount * GameModifier.critMultiplier);
        }
    }
}
