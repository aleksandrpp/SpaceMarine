using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Wfc;
using Object = UnityEngine.Object;

namespace AK.SpaceMarine.Maze
{
    public interface IMaze : IDisposable
    {
        void Build(uint seed);
    }

    public class WfcMaze : IMaze
    {
        private MazeConfig _config;
        private ICollection<GameObject> _objects;

        public WfcMaze(MazeConfig config)
        {
            _config = config;
            _objects = new List<GameObject>();
        }

        public void Build(uint seed)
        {
            ModuleRegistry.Reset
                (_config.Modules.Select(m => (Connectivity) m.Connectivity));

            var buffer = new WaveBuffer(_config.Size.x, _config.Size.y, _config.Size.z);
            var observer = new Observer(seed);
            while (!observer.Observe(buffer))
            {
            }

            for (var iz = 0; iz < _config.Size.z; iz++)
            for (var iy = 0; iy < _config.Size.y; iy++)
            for (var ix = 0; ix < _config.Size.x; ix++)
                BuildModule(buffer, ix, iy, iz);
        }

        private void BuildModule(WaveBuffer buffer, int ix, int iy, int iz)
        {
            var wave = buffer[ix, iy, iz];
            if (!wave.IsObserved) return;

            var state = wave.ObservedState;
            var prefab = _config.Modules[state.Index].Object;
            if (prefab == null) return;

            var position = (math.float3(ix, iy, iz) - _config.Size / 2 + 0.5f) * _config.Scale + _config.Offset;
            var rotation = state.Pose.ToRotation();

            var t = Object.Instantiate(prefab, position, rotation).transform;
            var m = (float4x4) t.worldToLocalMatrix;
            m.c3 = new float4(0f, 0f, 0f, 1);
            t.localScale = math.abs(math.transform(m, _config.Scale));
            
            _objects.Add(t.gameObject);
        }

        public void Dispose()
        {
            foreach (var obj in _objects)
                Object.Destroy(obj);
        }
    }
}