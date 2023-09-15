using System.Runtime.CompilerServices;
using UnityEngine;

namespace AK.AStar
{
    public static class Math
    {
        public const float 
            CornerCost = 1.41421356237f,
            HorizontalCost = 1,
            VerticalCost = 1;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ManhattanDistance(Tile a, Tile b, float d = 1) // 468
        {
            var (dx, dy) = Delta(a, b);
            return d * (dx + dy);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EuclideanDistance(Tile a, Tile b, float d = 1) // 361
        {
            var (dx, dy) = Delta(a, b);
            return d * Mathf.Sqrt(dx * dx + dy * dy);
        }

        public enum Diagonal
        {
            Octile,
            Chebyshev
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DiagonalDistance(Tile a, Tile b, Diagonal diagonal, float d = 1) // 470-437
        {
            float d2 = diagonal switch
            {
                Diagonal.Octile => CornerCost,
                Diagonal.Chebyshev => 1,
                _ => 1
            };
            
            var (dx, dy) = Delta(a, b);
            return d * (dx + dy) + (d2 - 2) * Mathf.Min(dx, dy);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int, int) Delta(Tile a, Tile b)
        {
            return (Mathf.Abs(a.X - b.X), Mathf.Abs(a.Y - b.Y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetNeighbourCost(Tile a, Tile b)
        {
            if (HorizontalNeighbour(a, b))
                return HorizontalCost;
            
            if (VerticalNeighbour(a, b))
                return VerticalCost;
            
            return CornerCost;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HorizontalNeighbour(Tile a, Tile b)
        {
            return Mathf.Abs(a.X - b.X) == 1 && a.Y == b.Y;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool VerticalNeighbour(Tile a, Tile b)
        {
            return Mathf.Abs(a.Y - b.Y) == 1 && a.X == b.X;
        }
    }
}