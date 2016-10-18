using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class UpdateVerts : MonoBehaviour {
	public Color newColor = Color.gray;
	private Mesh mesh;
	private Color[] colors;


	/* This function will color trees according to newColor setted in Unity
	 * Using MeshFilter for vertices in mesh newColor will be specified.
	 */
	void Update () {
        mesh = GetComponent<MeshFilter> ().mesh;
		colors = new Color[mesh.vertices.Length];
		int i = 0;
		while (i < mesh.vertices.Length) {
			colors [i] = newColor;
			i++;
		}
		mesh.colors = colors;
    }
}
