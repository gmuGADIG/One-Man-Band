using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneLoader : MonoBehaviour
{
	[SerializeField] SceneTransitioner st;
	public void loadScene(string Scene)
	{
		st.StartScene(Scene);
	}
}
