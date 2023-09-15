using AK.SpaceMarine.Actors;
using UnityEngine;

namespace AK.SpaceMarine.Parts
{
    public interface IFactory<out T> where T : Actor
    {
        T Create(Vector3 position, Quaternion rotation);
    }
}