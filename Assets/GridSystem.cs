﻿using UnityEngine;

public class GridSystem : MonoBehaviour {
	public GameObject square;

	const float gridSpacing = 1.03f;
    const int X_MAX = 6;
    const int Z_MAX = 6;
    const float FOV = 32;
    const float ASPECT_RATIO = 2;
	
    // Use this for initialization
	void Start () {

        float cDistanceX = (gridSpacing * X_MAX) / (2 * Mathf.Tan(FOV * Mathf.Deg2Rad));
        float cDistanceZ = (gridSpacing * Z_MAX) / (2 * Mathf.Tan(FOV * Mathf.Deg2Rad / ASPECT_RATIO));
        float theta = 45; float phi = 45; //Starting Camera Angle 

        //Camera Angle
        this.transform.eulerAngles = new Vector3(phi, theta, 0);//phi * Mathf.Sin(theta * Mathf.Deg2Rad)
        Debug.Log(this.transform.eulerAngles.ToString());

        //Center of Stage
        Vector3 stageCenter = new Vector3(X_MAX * gridSpacing/2, 0, Z_MAX * gridSpacing/2);
        Debug.Log(stageCenter.ToString());
        //Camera Distance
        //Vector3 camCenter = new Vector3(cDistanceX * Mathf.Cos(theta * Mathf.Deg2Rad)*Mathf.Cos(phi * Mathf.Deg2Rad) + cDistanceZ * Mathf.Sin(theta * Mathf.Deg2Rad)*Mathf.Cos(phi * Mathf.Deg2Rad), cDistanceZ * Mathf.Sin(phi*Mathf.Deg2Rad), cDistanceX * Mathf.Sin(theta * Mathf.Deg2Rad) * Mathf.Cos(phi * Mathf.Deg2Rad) + cDistanceZ * Mathf.Cos(theta * Mathf.Deg2Rad) * Mathf.Cos(phi * Mathf.Deg2Rad));
        Vector3 camCenter = new Vector3(cDistanceZ * Mathf.Sin(-theta * Mathf.Deg2Rad) * Mathf.Cos(phi * Mathf.Deg2Rad), cDistanceZ * Mathf.Sin(phi * Mathf.Deg2Rad), -cDistanceZ * Mathf.Cos(-theta * Mathf.Deg2Rad) * Mathf.Cos(phi * Mathf.Deg2Rad));

        //Total Position
        this.transform.position = stageCenter + camCenter;

		for (int u = 0; u < X_MAX; u++) {
			for (int v = 0; v < Z_MAX; v++) {
				Instantiate(square, new Vector3(gridSpacing*u + square.transform.localScale.x/2, 0f, gridSpacing* v + square.transform.localScale.z / 2), Quaternion.identity);

			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		// Check if mouse is hovering over a grid
		RaycastHit hit = new RaycastHit ();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast (ray, out hit)) {
			Square square = hit.transform.gameObject.GetComponent<Square>();
			if (square == null) return;
			
			square.ShowHelper ();
			int id = square.GetHelperID ();

			RemoveObjectsWithTag("HelperCube",id);
		}
	}

	void RemoveObjectsWithTag(string tag, int id) {
		GameObject[] objectsToRemove = GameObject.FindGameObjectsWithTag (tag);

		foreach (GameObject obj in objectsToRemove) {
			if (obj.GetInstanceID () != id)
				Destroy (obj);
		}

	}
}
