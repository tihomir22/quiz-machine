using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadinganim_2 : MonoBehaviour {


	// Update is called once per frame
	void Update () {

           transform.Rotate(new Vector3(0,0,-Time.deltaTime), 20f * Time.deltaTime);

	}
}
