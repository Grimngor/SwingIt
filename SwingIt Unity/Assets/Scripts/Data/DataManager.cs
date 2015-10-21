using UnityEngine;
using System.Collections;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.SocialPlatforms;

public class DataManager : MonoBehaviour {


	TimeSpan totalPlaytime;
	bool saving;

	void Awake () {

		//Activamos Google Play para que permita guardado en la nube
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			// enables saving game progress.
			.EnableSavedGames()
				.Build();
		PlayGamesPlatform.InitializeInstance(config);
		DontDestroyOnLoad(gameObject);
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = false;
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate();
		
		StartCoroutine("tryLogin");
	}

	private IEnumerator tryLogin(){
		yield return null;
		Social.localUser.Authenticate((bool success) => {
			
		});
	}

	public void SaveGame(){
		saving = true;
		OpenSavedGame ("data.jma");
	}

	public void LoadGame(){
		saving = false;
		OpenSavedGame ("data.jma");
	}

	#region Guardado Nube

	void OpenSavedGame(string filename) {
		ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
		savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
		                                                    ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
	}
	
	public void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game) {
		if (status == SavedGameRequestStatus.Success) {
			if (saving){
				//Falta generar el byte de los datos
				SaveGameData(game,new byte[]{});
			}
			else{
				LoadGameData(game);
			}
			// handle reading or writing of saved game.
		} else {
			// handle error
		}
	}
	
	void LoadGameData (ISavedGameMetadata game) {
		ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
		savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
	}
	
	public void OnSavedGameDataRead (SavedGameRequestStatus status, byte[] data) {
		if (status == SavedGameRequestStatus.Success) {
			// handle processing the byte array data
		} else {
			// handle error
		}
	}

	void SaveGameData (ISavedGameMetadata game, byte[] savedData) {
		ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
		
		SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
		builder = builder
			.WithUpdatedPlayedTime(totalPlaytime)
				.WithUpdatedDescription("Saved game at " + DateTime.Now);
		SavedGameMetadataUpdate updatedMetadata = builder.Build();
		savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWritten);
	}
	
	public void OnSavedGameWritten (SavedGameRequestStatus status, ISavedGameMetadata game) {
		if (status == SavedGameRequestStatus.Success) {
			// handle reading or writing of saved game.
		} else {
			// handle error
		}
	}
	#endregion
}
