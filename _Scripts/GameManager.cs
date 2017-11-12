using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// GameManager follows singleton pattern, only one instance
	public static GameManager instance = null;
	public int levelNum = 1;
    public bool CameraDoesMove;

    private string sceneName;
	private GameObject player;
	private GameObject resetButton;
	private GameObject playButton;
	private GameObject pieces;
	//private Vector2[] pieceOrigins;
	private Vector2 playerOrigin;
	

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		}
		else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	// This function is run once every time a scene is loaded
	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		//door = GameObject.Find("Door");
		resetReferences();

		playerOrigin = player.transform.position;

		// if (pieces != null) {
		// 	savePiecePositions();
		// }

		beginBuildPhase();
    }

    private void resetReferences() {
    	player = GameObject.Find("Player");
    	pieces = GameObject.Find("Pieces");
    	resetButton = GameObject.Find("ResetButton");
		playButton = GameObject.Find("PlayButton");
    }

    // Called by player when it collides with the door
    private void nextLevel() {
    	levelNum++;
    	changeScene("Level" + levelNum);
    }

    private void beginBuildPhase() {
    	//Show RESET and PLAY buttons
    	resetButton.SetActive(true);
    	playButton.SetActive(true);
    	//Disable player movement
    	player.GetComponent<PlayerMovement>().moveable = false;
        Camera.main.GetComponent<MoveCamera>().isBuildPhase = true;
        Camera.main.GetComponent<MoveCamera>().ChangeCamToBuild();
    }

    private void beginPlayPhase() {
    	//Make pieces unclickable
    	if (pieces != null) {
    		freezePieces();
    	}
    	//Hide play button
    	playButton.SetActive(false);
    	//Enable player movement
    	player.GetComponent<PlayerMovement>().moveable = true;
        Camera.main.GetComponent<MoveCamera>().isBuildPhase = false;
        Camera.main.GetComponent<MoveCamera>().ChangeCamToPlay();
    }

    private void freezePieces() {
    	int pieceCount = pieces.transform.childCount;

    	for (int pieceIndex = 0; pieceIndex < pieceCount; pieceIndex++) {
    		GameObject curPiece = pieces.transform.GetChild(pieceIndex).gameObject;

    		curPiece.GetComponent<PieceController>().moveable = false;
    	}
    }

  //   private void savePiecePositions() {
		// int pieceCount = pieces.transform.childCount;
		// pieceOrigins = new Vector2[pieceCount];

  //   	for (int pieceIndex = 0; pieceIndex < pieceCount; pieceIndex++) {
  //   		GameObject curPiece = pieces.transform.GetChild(pieceIndex).gameObject;

  //   		// Save original piece position for reset
  //   		pieceOrigins[pieceIndex] = curPiece.transform.position;
  //   	}
  //   }

    private void resetPiecePositions() {
    	int pieceCount = pieces.transform.childCount;

    	//Move pieces back to start
    	for (int pieceIndex = 0; pieceIndex < pieceCount; pieceIndex++) {
    		GameObject curPiece = pieces.transform.GetChild(pieceIndex).gameObject;

    		// Move piece back to original position
    		curPiece.GetComponent<PieceController>().resetPosition();

    		curPiece.GetComponent<PieceController>().moveable = true;
    	}
    }

    private void reset() {
    	if (pieces != null) {
    		resetPiecePositions();
    	}

    	player.transform.position = playerOrigin;

    	beginBuildPhase();
    }

    public void changeScene(string sceneName) {
    	SceneManager.LoadScene(sceneName);
    }
}
