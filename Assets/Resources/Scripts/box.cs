using UnityEngine;
using System.Collections;

public class box : MonoBehaviour {

    private BezierCurve m_movement;


	// Use this for initialization
	void Start () {
        m_movement = this.GetComponent<BezierCurve>();
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = m_movement.Calculate();
	}
}
