using System.Linq;
using UnityEngine;

public class WindowScript : MonoBehaviour
{
    public Camera portalCam;       // offscreen camera rendering to RT (disabled, Render() manually)
    public RenderTexture target;   // RT assigned window material

    // unit quad corners (BL, BR, TL, TR), window transform supplies size/rotation
    static readonly Vector3[] localCorners =
    {
        new Vector3(-0.5f,-0.5f,0f),
        new Vector3( 0.5f,-0.5f,0f),
        new Vector3(-0.5f, 0.5f,0f),
        new Vector3( 0.5f, 0.5f,0f),
    };

    void LateUpdate()
    {
        if (!portalCam || !Camera.main) return;

        var main = Camera.main;

        // Eye at viewer position; orientation window-based
        portalCam.transform.position = main.transform.position;
        portalCam.transform.rotation = transform.rotation;

        // Project window corners into this camera's view space
        var V = portalCam.worldToCameraMatrix;

        Vector3[] cs = new Vector3[4];
        for (int i = 0; i < 4; i++)
            cs[i] = V.MultiplyPoint(transform.TransformPoint(localCorners[i]));

        // window must be in front (Unity camera looks down -Z)
        if (cs.Any(p => p.z >= -0.0001f)) return;

        float n = portalCam.nearClipPlane;
        float f = portalCam.farClipPlane;

        float[] xs = cs.Select(p => (p.x / -p.z) * n).ToArray();
        float[] ys = cs.Select(p => (p.y / -p.z) * n).ToArray();

        float left = xs.Min();
        float right = xs.Max();
        float bottom = ys.Min();
        float top = ys.Max();

        // Off-axis projection that matches the window rectangle
        portalCam.projectionMatrix = Matrix4x4.Frustum(left, right, bottom, top, n, f);

        // Render once per frame per window
        portalCam.targetTexture = target;
        portalCam.Render();
    }
}
