using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class PainterManager : MonoBehaviour {
	public ParticleSystem particleSystemTemplate;

	private bool newPaintVertices;
	private bool paintingOn;
	private Color paintColor;

	private Vector3 previousPosition;
	public GameObject cubeObj;

	private List<ParticleSystem> particleSystemList; // Stores all particle systems
	private List<Vector3> currVertices; //Stores current camera positions to paint
	private ParticleSystem ps; // Stores current particle system

	void OnEnable(){
		UnityARSessionNativeInterface.ARFrameUpdatedEvent += ARFrameUpdated;
	}

	void OnDestroy(){
		UnityARSessionNativeInterface.ARFrameUpdatedEvent -= ARFrameUpdated;
	}

	// Use this for initialization
	void Start () {
		paintingOn = false;
		newPaintVertices = false;
		particleSystemList = new List<ParticleSystem> ();
		ps = Instantiate (particleSystemTemplate);
		currVertices = new List<Vector3> ();
		paintColor = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
		if (paintingOn && newPaintVertices) {
			if (currVertices.Count > 0) {
				ParticleSystem.Particle[] particles = new ParticleSystem.Particle[currVertices.Count];
				int index = 0;
				foreach(Vector3 vtx in currVertices){
					particles[index].position = vtx;
					particles[index].color = paintColor;
					particles[index].size = 0.1f;
					index++;
				}
				ps.SetParticles(particles, currVertices.Count);
				newPaintVertices = false;
			}
		}
	}

	public void TooglePaint(){
		Debug.Log ("toggle");
		if (ps.particleCount > 0) {
			SaveParticleSystem ();
		}
		paintingOn = !paintingOn;
	}

	public void RandomizeColor(){
		Debug.Log ("randomize");
		paintColor = Random.ColorHSV ();
	}

	public void Reset(){
		Debug.Log ("reset");
		foreach (ParticleSystem p in particleSystemList) {
			Destroy (p);
		}
		particleSystemList = new List<ParticleSystem> ();

		Destroy (ps);

		ps = Instantiate (particleSystemTemplate);
		currVertices = new List<Vector3> ();
	}

	private void SaveParticleSystem(){
		particleSystemList.Add (ps);
		ps = Instantiate (particleSystemTemplate);
		currVertices = new List<Vector3> ();
	}


	private void ARFrameUpdated(UnityARCamera arCamera){
		Vector3 paintPosition = GetCameraPosition(arCamera) + (Camera.main.transform.forward * 0.2f);
		if (Vector3.Distance (paintPosition, previousPosition) > 0.025f) {
			if (paintingOn) currVertices.Add (paintPosition);
			previousPosition = paintPosition;
			newPaintVertices = true;
		}
	}

	private Vector3 GetCameraPosition(UnityARCamera cam){
		Matrix4x4 matrix = new Matrix4x4 ();
		matrix.SetColumn (3, cam.worldTransform.column3);
		return UnityARMatrixOps.GetPosition (matrix);
	}

}
