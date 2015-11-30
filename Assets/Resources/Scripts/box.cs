using UnityEngine;
using System.Collections;

public class box : MonoBehaviour {

    private BezierCurve m_movement;

	// Use this for initialization
	void Start () {
        m_movement = this.transform.FindChild("BezierCurve").GetComponent<BezierCurve>();
        m_movement.Initialize();
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = m_movement.Calculate();
	}
}
