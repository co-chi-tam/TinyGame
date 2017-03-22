using UnityEngine;
using System.Collections;

public class SpriteMesh : MonoBehaviour {

	private SpriteRenderer m_SpriteRenderer;

	void Start () {
		m_SpriteRenderer = this.GetComponent<SpriteRenderer> ();
		var spriteVerticles = m_SpriteRenderer.sprite.vertices;
		var spriteTriangles = m_SpriteRenderer.sprite.triangles;
		var spriteUV = m_SpriteRenderer.sprite.uv;
		var go = new GameObject ();
		var meshFilter = go.AddComponent<MeshFilter> ();
		var skinnedMeshRenderer = go.AddComponent<SkinnedMeshRenderer> ();
		var newMesh = new Mesh ();
		var verticles = new Vector3[spriteVerticles.Length];
		var triangles = new int[spriteTriangles.Length];
		var uvs = new Vector2[spriteUV.Length];
		var normals = new Vector3 [spriteUV.Length];
		for (int i = 0; i < spriteVerticles.Length; i++) {
			verticles [i] = spriteVerticles [i];
		}
		for (int i = 0; i < spriteTriangles.Length; i++) {
			triangles [i] = spriteTriangles [i];
		}
		for (int i = 0; i < spriteUV.Length; i++) {
			uvs [i] = spriteUV [i];
			normals [i] = Vector3.forward;
		}
		newMesh.vertices = verticles;
		newMesh.triangles = triangles;
		newMesh.uv = uvs;
		newMesh.normals = normals;
		skinnedMeshRenderer.material = new Material (Shader.Find ("Unlit/Transparent"));
		skinnedMeshRenderer.material.mainTexture = m_SpriteRenderer.sprite.texture;
		_SolveTangentsForMesh (newMesh);
		newMesh.RecalculateBounds ();
		newMesh.RecalculateNormals ();
		newMesh.Optimize ();
		skinnedMeshRenderer.sharedMesh = newMesh;
		meshFilter.mesh = newMesh;
	}

	private void _SolveTangentsForMesh(Mesh mesh) {
		int vertexCount = mesh.vertexCount;
		Vector3[] vertices = mesh.vertices;
		Vector3[] normals = mesh.normals;
		Vector2[] texcoords = mesh.uv;
		int[] triangles = mesh.triangles;
		int triangleCount = triangles.Length / 3;

		Vector4[] tangents = new Vector4[vertexCount];
		Vector3[] tan1 = new Vector3[vertexCount];
		Vector3[] tan2 = new Vector3[vertexCount];

		int tri = 0;

		for (int i = 0; i < (triangleCount); i++) {
			int i1 = triangles[tri];
			int i2 = triangles[tri + 1];
			int i3 = triangles[tri + 2];

			Vector3 v1 = vertices[i1];
			Vector3 v2 = vertices[i2];
			Vector3 v3 = vertices[i3];

			Vector2 w1 = texcoords[i1];
			Vector2 w2 = texcoords[i2];
			Vector2 w3 = texcoords[i3];

			float x1 = v2.x - v1.x;
			float x2 = v3.x - v1.x;
			float y1 = v2.y - v1.y;
			float y2 = v3.y - v1.y;
			float z1 = v2.z - v1.z;
			float z2 = v3.z - v1.z;

			float s1 = w2.x - w1.x;
			float s2 = w3.x - w1.x;
			float t1 = w2.y - w1.y;
			float t2 = w3.y - w1.y;

			float r = 1.0f / (s1 * t2 - s2 * t1);
			Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
			Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

			tan1[i1] += sdir;
			tan1[i2] += sdir;
			tan1[i3] += sdir;

			tan2[i1] += tdir;
			tan2[i2] += tdir;
			tan2[i3] += tdir;

			tri += 3;
		}

		for (int i = 0; i < (vertexCount); i++) {
			Vector3 n = normals[i];
			Vector3 t = tan1[i];

			// Gram-Schmidt orthogonalize
			Vector3.OrthoNormalize(ref n, ref t);

			tangents[i].x = t.x;
			tangents[i].y = t.y;
			tangents[i].z = t.z;

			// Calculate handedness
			tangents[i].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[i]) < 0.0f) ? -1.0f : 1.0f;
		}

		mesh.tangents = tangents;
	}

}
