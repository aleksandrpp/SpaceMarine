using Unity.Mathematics;

namespace Wfc {

public struct Observer
{
    #region Public members

    public Observer(uint seed)
      => _random = new Random(seed);

    public bool Observe(WaveBuffer waves)
    {
        var min_e = 1e+6f;
        var min_i = -1;

        for (var i = 0; i < waves.Length; i++)
        {
            if (waves[i].IsObserved) continue;
            var e = waves[i].Entropy + _random.NextFloat(0.5f);
            if (e < min_e)
            {
                min_e = e;
                min_i = i;
            }
        }

        if (min_i < 0) return true;

        waves[min_i].Collapse(_random.NextUInt());
        Propagate(waves, min_i);

        return false;
    }

    #endregion

    #region Private members

    Random _random;

    void Propagate(WaveBuffer waves, int index)
    {
        // Retrieve the wave state.
        var state = waves[index].ObservedState;
        var conn = ModuleRegistry.GetConnectivity(state.Index);
        conn = conn.GetRotated(state.Pose);

        // Boundary check
        var p = waves.IndexToCoords(index);
        var b0 = p == 0;
        var b1 = p == waves.Dimensions - 1;
        if (!b0.x) waves[p.x - 1, p.y, p.z].ForceDirection(Direction.XP, conn[Direction.XN]);
        if (!b1.x) waves[p.x + 1, p.y, p.z].ForceDirection(Direction.XN, conn[Direction.XP]);
        if (!b0.y) waves[p.x, p.y - 1, p.z].ForceDirection(Direction.YP, conn[Direction.YN]);
        if (!b1.y) waves[p.x, p.y + 1, p.z].ForceDirection(Direction.YN, conn[Direction.YP]);
        if (!b0.z) waves[p.x, p.y, p.z - 1].ForceDirection(Direction.ZP, conn[Direction.ZN]);
        if (!b1.z) waves[p.x, p.y, p.z + 1].ForceDirection(Direction.ZN, conn[Direction.ZP]);
    }

    #endregion
}

} //namespace Wfc
