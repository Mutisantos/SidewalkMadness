using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {

	private GameObject loadingBar;
	private GameObject contMenu;
	private Slider slider;
	public bool isLoadingScene = false;

	void Start(){
		if (isLoadingScene) {
			loadingBar = GameObject.Find ("Loader");
			slider = GameObject.Find ("LoadingBar").GetComponent<Slider>();
			contMenu = GameObject.Find ("ContentPanel");

			loadingBar.SetActive (false);
			contMenu.SetActive (true);
		}
	}


	public void changeMode(bool mode){
		GameManager.instance.setSurvivalMode (mode);
	}

	public void myOwnLoadScene(int index){
		SoundManager.instance.changeMusic (index);
		if (isLoadingScene) {
			StartCoroutine (LoadCoroutineScene (index));
		} else {
			GameManager.instance.setAlive(false);
			SceneManager.LoadScene (index);
		}
	}

	//QUIT-EXIT
	public void FinishGame(){
		Application.Quit ();
	}

	IEnumerator LoadCoroutineScene(int index){
		//Si se cambia de escena desde un boton, el personaje entonces ya no esta vivo
		if (isLoadingScene) {
			loadingBar.SetActive (true);
			contMenu.SetActive (false);
		}
		AsyncOperation op = SceneManager.LoadSceneAsync(index);
		op.allowSceneActivation = false;
		while (!op.isDone) {
			//float progress = Mathf.Clamp01 (op.progress / 0.9f);//0.9 por el inicio de carga de escena
			if (isLoadingScene) {
				//slider.value = progress; //si hay cargas pesadas
				yield return new WaitForSeconds (0.1f);
				slider.value = 0.3f;
				yield return new WaitForSeconds (0.1f);
				slider.value = 0.6f;
				yield return new WaitForSeconds (0.1f);
				slider.value = 0.9f;
				yield return new WaitForSeconds (0.1f);
				slider.value = 1f;
			}


			op.allowSceneActivation = true;
			yield return new WaitForSeconds (0f);
		}
	}
}
