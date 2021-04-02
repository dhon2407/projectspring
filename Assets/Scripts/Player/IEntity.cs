using UnityEngine;

namespace Player
{
    public interface IEntity
    {
        GameObject Owner { get; }
    }
}