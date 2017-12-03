using UnityEngine;
using System.Collections;

public class AnimarTextura : MonoBehaviour {

	public Renderer rend;
	public Vector2 uvAnimationRate = new Vector2( 1.0f, 1.0f );
	public string textureName = "_MainTex";//Textura principal
	private Vector2 uvOffset = Vector2.zero;//Vector del offset

	void Start () {
		rend = GetComponent<Renderer>();
		rend.enabled = true;

	}
				

	void Update() 
	{
		uvOffset += ( uvAnimationRate * Time.deltaTime );//Mover el offset con respecto al tiempo
		if( rend.enabled )
		{
			rend.material.SetTextureOffset(textureName,uvOffset);//Cambiar el offset para simular el movimiento de la animacion
		}
	}
}
