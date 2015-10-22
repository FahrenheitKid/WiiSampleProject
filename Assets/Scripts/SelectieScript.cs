using UnityEngine;
using System.Collections;
using System; 

/*
* Script that handles all selection functions
*/
public class SelectieScript : MonoBehaviour {

	//Public objects
	public LineRenderer lineRenderObject;
	public bool isActive;
	public GameObject playerCam;
	
	//Private scripts
	public SelectionGuiScript guiscript;
	private RayCastScript raycastscript;

	//Private objects
	public GameObject lastGameObjectHit;
	public string strlastGameObjectHit;
	private Color c;
	private Transform lastChildTransform;
	private Color tc;
	private bool colorChanged = false;
	private Transform[] childList;
	private int childIterator;
	private int totalCount;
	private bool nextPressed= false;
	private bool prevPressed = false;
	float mouseX, mouseY;
	
	public bool fireEvent = false;
	public bool GUIShown = false;
	
	void Start(){
		//Init scripts
		guiscript = gameObject.GetComponent("SelectionGuiScript") as SelectionGuiScript;
		raycastscript = gameObject.GetComponent("RayCastScript") as RayCastScript;
		lineRenderObject = playerCam.GetComponent("LineRenderer") as LineRenderer;
	}
	
	public void setSelectionmodeOn(){
		isActive = true;
	}
	public void setSelectionmodeOff(){
		isActive = false;
		lineRenderObject.enabled = false;
	}
	
	public void updateSelection(float mouseX, float mouseY){
		if (isActive && !GUIShown){
			if(lastGameObjectHit != null){
					lastGameObjectHit.GetComponent<Renderer>().material.color = c;
					Debug.Log(c.ToString());
					if (colorChanged)
					{
						foreach(Transform t in lastGameObjectHit.transform)
						{
							t.GetComponent<Renderer>().material.color = tc;
							
						}
						colorChanged = false;
						
					}
				}
				GameObject go = raycastscript.getTargetObjects(new Vector3(mouseX, mouseY, 0), playerCam.GetComponent<Camera>());	
				if(go != null && go.name != "Terrain")
					lastGameObjectHit = go;	
				lineRenderObject.enabled = true;
				Vector3 v = playerCam.transform.position;
				v.y = -0.5f;
				if (lastGameObjectHit != null)
				{
					childList =  new Transform[lastGameObjectHit.transform.childCount];
					totalCount = 0;
					childIterator = 0;
					foreach(Transform f in lastGameObjectHit.transform)
					{
						childList[totalCount] = f;
						Debug.Log(f.name);
						totalCount++;
					}
					c = lastGameObjectHit.GetComponent<Renderer>().material.color ;
					lastGameObjectHit.GetComponent<Renderer>().material.color =  Color.green;
				}
		}
	}
	
	public void showUi(float mouseposX, float mouseposY, string[] choices){
		//Vector3 mousePos = Input.mousePosition;
		mouseX = mouseposX / Screen.width;
		mouseY = mouseposY / Screen.height;
		guiscript.showGui(mouseX, mouseY, choices);	
	}
	
	public void hideUI(){
		guiscript.hideGui();
	}
	public void getNextChild(){
		if(childIterator<totalCount-1)
			{
				childIterator++;
				Debug.Log(childIterator+":"+totalCount+"=>" +childList[childIterator].name);
				
				if(lastChildTransform != null)
					lastChildTransform.GetComponent<Renderer>().material.color  =tc;
				
				lastChildTransform = childList[childIterator];
				tc = lastChildTransform.GetComponent<Renderer>().material.color;
				
				lastChildTransform.GetComponent<Renderer>().material.color = Color.red;
				colorChanged = true;
				
				lastGameObjectHit = lastChildTransform.gameObject;
			}
	}
	public void getPreviousChild(){
		if(childIterator>0)
			{
				childIterator--;
				Debug.Log(childIterator+":"+totalCount+"=>" +childList[childIterator].name);
				
				if(lastChildTransform != null)
					lastChildTransform.GetComponent<Renderer>().material.color  =tc;
				
				lastChildTransform = childList[childIterator];
				tc = lastChildTransform.GetComponent<Renderer>().material.color;
				
				lastChildTransform.GetComponent<Renderer>().material.color = Color.red;
				colorChanged = true;
			}
	}
	
	public GameObject getParent(){
		return lastGameObjectHit.transform.parent.gameObject;
	}
	
	public string getButtonOnPosition(float rawposx, float rawposy)
	{
		string name = guiscript.getButtonNameBehindPos(rawposx, rawposy);
		return name;
	}

}