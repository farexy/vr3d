using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour {

	public void Next() {
        SceneManager.LoadScene(2);
	}
}
