using System;
using System.Collections.Generic;
using AK.SpaceMarine.Actors;
using AK.SpaceMarine.Parts;
using AK.SpaceMarine.Weapons;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace AK.SpaceMarine
{
    public interface IWorld : IDisposable
    {
        ICollection<Actor> Actors { get; }

        IHero Hero { get; set; }

        WorldData Data { get; }

        IDictionary<BulletConfig, IObjectPool<Bullet>> Bullets { get; }

        void CreateActor(ActorConfig config, Vector3 position, Quaternion rotation);

        void Remove(Actor actor);

        void CreateFromTags<T>() where T : Object, ITag;

        int GetActorCount<T>();

        bool TryGetActorFirst<T>(Func<T, bool> func, out T result);

        bool TryGetActorNearest<T>(IRange range, Func<T, bool> func, out T result) where T : IPosition;

        void SaveData(UserData userData);
    }

    public class World : IWorld
    {
        public ICollection<Actor> Actors { get; }

        public IHero Hero { get; set; }

        public WorldData Data { get; }

        public IDictionary<BulletConfig, IObjectPool<Bullet>> Bullets { get; }

        private Action<Actor, bool> _onActor;

        public World(Action<Actor, bool> onActor)
        {
            _onActor = onActor;

            Data = new WorldData();
            Actors = new HashSet<Actor>();
            Bullets = new Dictionary<BulletConfig, IObjectPool<Bullet>>();
        }

        public void CreateFromTags<T>() where T : Object, ITag
        {
            foreach (var tag in Object.FindObjectsOfType<T>())
            {
                CreateActor(tag.Config, tag.Position, tag.Rotation);
            }
        }

        public void CreateActor(ActorConfig config, Vector3 position, Quaternion rotation)
        {
            if (config is not IFactory<Actor> factory)
            {
                Debug.LogWarning($"{config.name} not implement IFactory");
                return;
            }

            Actor actor = factory.Create(position, rotation);
            actor.Bind(this);

            _onActor?.Invoke(actor, true);

            Actors.Add(actor);
        }
        
        public int GetActorCount<T>()
        {
            int count = 0;
            foreach (var actor in Actors)
            {
                if (actor is not T)
                    continue;

                count++;
            }
            
            return count;
        }

        public bool TryGetActorFirst<T>(Func<T, bool> func, out T result)
        {
            foreach (var actor in Actors)
            {
                if (actor is not T typeActor)
                    continue;

                if (!func(typeActor))
                    continue;

                result = typeActor;
                return true;
            }

            result = default;
            return false;
        }

        public bool TryGetActorNearest<T>(IRange range, Func<T, bool> func, out T result) where T : IPosition
        {
            (T actor, float distance) nearest = default;

            foreach (var actor in Actors)
            {
                if (actor is not T typeActor)
                    continue;

                if (!func(typeActor))
                    continue;

                float distance = (typeActor.Position - range.Position).sqrMagnitude;

                if (nearest.actor == null)
                {
                    nearest = (typeActor, distance);
                    continue;
                }

                if (distance < nearest.distance)
                    nearest = (typeActor, distance);
            }

            if (nearest.actor != null)
            {
                result = nearest.actor;
                return true;
            }

            result = default;
            return false;
        }

        public void Remove(Actor actor)
        {
            if (actor is IDrop drop)
                drop.Drop();

            if (actor is IReward reward)
                reward.Reward(Data);

            _onActor?.Invoke(actor, false);

            Actors.Remove(actor);
            actor.Dispose();
        }


        public void Dispose()
        {
            foreach (var actor in Actors)
                actor.Dispose();

            Actors.Clear();

            foreach (var bullet in Bullets.Values)
                bullet.Clear();
        }

        public void SaveData(UserData userData)
        {
            userData.BestScore = Mathf.Max(userData.BestScore, Data.Score);
            userData.LastScore = Data.Score;
            userData.SaveToFile();
        }
    }
}