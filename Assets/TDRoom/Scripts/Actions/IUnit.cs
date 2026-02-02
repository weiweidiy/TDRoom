
using UnityEngine;

public enum UnitType
{
    Player,
    Enemy
}

public interface IUnit
{
    UnitType UnitType { get; }

    int LineIndex { get; }

    void Hurt(int damage);

    Vector3 GetPosition();
}
