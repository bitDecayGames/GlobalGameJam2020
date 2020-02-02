using UnityEngine;

namespace Utils
{
    public static class VectorMath
    {
        public static Vector2 V3toV2(Vector3 vIn)
        {
            return new Vector2(vIn.x, vIn.y);
        }

        public static Vector3 V2toV3(Vector2 vIn)
        {
            return new Vector3(vIn.x, vIn.y, 0);
        }
    }
}