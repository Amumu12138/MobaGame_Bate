using UnityEngine;

public class DrawPolygon
{
    Vector3 point1;
    Vector3 point2;
    Vector3 point3;
    Vector3 point4;
    Vector3 point5;
    Vector3 point6;
    Vector2 origin = new Vector2(-0.5f, -0.6f);

    float size = 20;
    MeshFilter meshFilter;

    public DrawPolygon(Transform _trans)
    {
        if (_trans == null)
        {
            Debug.LogError("_trans value error");
            return;
        }

        Transform tempTran = _trans.FindChild("Image");
        if (tempTran == null)
        {
            Debug.LogError("tempTran value null");
            return;
        }

        tempTran.localPosition = new Vector3(16, -15, 0);
        meshFilter = tempTran.GetComponent<MeshFilter>();
    }

    public void DrawGraphics(float _value1, float _value2, float _value3, float _value4, float _value5, float _value6)
    {
        if (meshFilter == null)
        {
            Debug.LogError("meshFilter value null");
            return;
        }

        point1 = new Vector3(0f * (_value1 * size), 4.2f * (_value1 * size), 0);        // µÃ·Ö
        point2 = new Vector3(3.7f * (_value2 * size), 2.3f * (_value2 * size), 0);      // Àº°å
        point3 = new Vector3(3.7f * (_value3 * size), -2.4f * (_value3 * size), 0);     // ÇÀ¶Ï
        point4 = new Vector3(0f * (_value4 * size), -4.3f * (_value4 * size), 0);       // ÌåÄÜ
        point5 = new Vector3(-3.6f * (_value5 * size), -2.3f * (_value5 * size), 0);    // ¸ÇÃ±
        point6 = new Vector3(-3.8f * (_value6 * size), 2.2f * (_value6 * size), 0);     // Öú¹¥

        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[] { point1, point2, point3, point4, point5, point6, origin };
        mesh.triangles = new int[] { 6, 0, 1, 6, 1, 2, 6, 2, 3, 6, 3, 4, 6, 4, 5, 6, 5, 0 };

        meshFilter.mesh = mesh;
    }
}