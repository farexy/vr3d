using UnityEngine;
using System.Collections;

public class PlayLevel : MonoBehaviour {

   public void Play () {
       Destroy(transform.gameObject.GetComponentInParent<Canvas>().gameObject);
	}
}
