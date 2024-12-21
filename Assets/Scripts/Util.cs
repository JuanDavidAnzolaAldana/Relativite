using UnityEngine;

public static class Util
{
    public static Quaternion FromVectors(Vector3 y,Vector3 z) {
        float a = y.y + z.z;
        float b = y.z - z.y;
        float root = Mathf.Sqrt(a * a + b * b);
        float r = Mathf.Sqrt(root + a) / 2;
        float i = Mathf.Sign(b) * Mathf.Sqrt(root - a) / 2;
        a = y.y - z.z;
        b = y.z + z.y;
        root = Mathf.Sqrt(a * a + b * b);
        float j = Mathf.Sqrt(root + a) / 2;
        float k = Mathf.Sign(b) * Mathf.Sqrt(root - a) / 2;
        if(y.x != 0) {
            if(Mathf.Sign(i * j - k * r) != Mathf.Sign(y.x)) {
                j = -j;
                k = -k;
            }
        }else if(Mathf.Sign(r * j - i * k) != Mathf.Sign(z.x)){
            j = -j;
            k = -k;
        }
        return new Quaternion(i,j,k,r);
    }
    public static Quaternion AngleVector(Vector3 angle) {
        float norm = angle.magnitude;
        if(norm == 0) {
            return Quaternion.identity;
        }
        angle *= Mathf.Sin(norm / 2) / norm;
        return new Quaternion(angle.x, angle.y, angle.z, Mathf.Cos(norm / 2));
    }
    public static Vector3[] Orthonormalize(Vector3 v1, Vector3 v2) {
        v1 = v1.normalized;
        v2 -= Vector3.Dot(v1, v2) * v1;
        v2 = v2.normalized;
        return new Vector3[] { v1, v2 };
    }
    public static Vector3[] Orthonormalize(Vector3 v1, Vector3 v2, Vector3 v3) {
        v1 = v1.normalized;
        v2 -= Vector3.Dot(v1, v2) * v1;
        v2 = v2.normalized;
        v3 -= Vector3.Dot(v1, v3) * v1;
        v3 -= Vector3.Dot(v2, v3) * v2;
        v3 = v3.normalized;
        return new Vector3[] { v1, v2, v3 };
    }
}
