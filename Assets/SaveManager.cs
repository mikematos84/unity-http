using UnityEngine;
using System.Collections.Generic;

public class SaveManager {
	public static int topScore { get; private set; }
	public static int gems { get; set; }
	public static string currentTheme { get; set; }
	public static int currentTile { get; set; }
	public static int currentBackground { get; set; }
	public static int currentLevel { get; set; }
	public static Dictionary<int, int> topScoreLevels { get; private set; }
	public static List<string> unlocks { get; private set; }

	public static void SaveScore (int score) {
		if (score > topScore) {
			//PlayerPrefs.SetInt ("top score", score);
			ES2.Save<int> (score, "GameInfo.txt?tag=topScore&encrypt=true&password=fcde34ux6sj");
			// submit to the leaderboard

			if (topScore > 0) {
				// do an animation
				// Master.instance.OnNewHighScore ();
			}

			topScore = score;
		}
	}

	public static int GetLevelScore (int level) {
		int currScore;

		if (topScoreLevels.TryGetValue (level, out currScore))
			return currScore;
		else
			return 0;
	}

	public static bool SaveScoreLevels (int level, int score) {
		int currScore;
		bool toReturn = false;

		if (topScoreLevels.TryGetValue (level, out currScore)) {
			if (currScore < score) {
				topScoreLevels [level] = score;
				toReturn = true;
			}
		}
		else {
			topScoreLevels.Add (level, score);
		}

		ES2.Save<int,int> (topScoreLevels, "GameInfo.txt?tag=topScoreLevels&encrypt=true&password=fcde34ux6sj");
		return toReturn;
	}

	public static void SaveCurrentLevel (int levelID) {
		currentLevel = levelID;
		ES2.Save<int> (currentLevel, "GameInfo.txt?tag=currentLevel&encrypt=true&password=fcde34ux6sj");
	}

	public static void UpdateUnlockedItems (IEnumerable<string> items) {
		List<string> unlocked = new List<string> (items);
		string toSave = string.Empty;
		unlocks = unlocked;

		for (int i = 0; i < unlocked.Count; i++) {
			toSave += unlocked[i] + ",";
		}

		//PlayerPrefs.SetString ("unlocked", toSave);
		ES2.Save<string> (unlocked, "GameInfo.txt?tag=unlocked&encrypt=true&password=fcde34ux6sj");
		Debug.Log (toSave);
	}

	public static void Save () {
		//PlayerPrefs.SetString ("theme", currentTheme);
		//PlayerPrefs.SetInt ("tile", currentTile);
		//PlayerPrefs.SetInt ("background", currentBackground);

		ES2.Save<string> (currentTheme, "GameInfo.txt?tag=theme&encrypt=true&password=fcde34ux6sj");
		ES2.Save<int> (currentTile, "GameInfo.txt?tag=tile&encrypt=true&password=fcde34ux6sj");
		ES2.Save<int> (currentBackground, "GameInfo.txt?tag=background&encrypt=true&password=fcde34ux6sj");
	}

	public static bool Load () {
		bool isFirstGame = true;

		if (ES2.Exists ("GameInfo.txt?encrypt=true&password=fcde34ux6sj")) {
			isFirstGame = false;

			Debug.Log ("~~~~Exists~~~~");

			ES2Data info = ES2.LoadAll ("GameInfo.txt?encrypt=true&password=fcde34ux6sj");

			Debug.Log ("~~~~topScore"+(info.TagExists ("topScore")?" Exists~~~~":" None~~~~"));

			Debug.Log ("~~~~theme"+(info.TagExists ("theme")?" Exists~~~~":" None~~~~"));
			Debug.Log ("~~~~tile"+(info.TagExists ("tile")?" Exists~~~~":" None~~~~"));
			Debug.Log ("~~~~background"+(info.TagExists ("background")?" Exists~~~~":" None~~~~"));

			Debug.Log ("~~~~unlocked"+(info.TagExists ("unlocked")?" Exists~~~~":" None~~~~"));

			if (info.TagExists ("currentLevel"))
				currentLevel = info.Load<int> ("currentLevel");
			// else
				// currentLevel = Resources.LoadAll<LevelData>("LevelData")[0].backupID;

			if (info.TagExists ("topScoreLevels"))
				topScoreLevels = info.LoadDictionary<int,int> ("topScoreLevels");
			else
				topScoreLevels = new Dictionary<int, int> ();

			if (info.TagExists ("topScore"))
				topScore = info.Load<int> ("topScore");
			else
				topScore = 0;

			if (info.TagExists ("theme"))
				currentTheme = info.Load<string> ("theme");
			else
				currentTheme = "original";

			if (info.TagExists ("tile"))
				currentTile = info.Load<int> ("tile");
			else
				currentTile = 0;

			if (info.TagExists ("background"))
				currentBackground = info.Load<int> ("background");
			else
				currentBackground = 0;

			if (info.TagExists ("unlocked"))
				unlocks = info.LoadList<string> ("unlocked");
			else
				unlocks = new List<string> () {
					"original",
					"colorblind",
					"tile 1",
					"wallpaper 1",
				};
		}
		else {
			topScore = 0;
			currentTheme = "original";
			currentTile = 0;
			currentBackground = 0;
			unlocks = new List<string> () {
				"original",
				"colorblind",
				"tile 1",
				"wallpaper 1",
			};
		}

		return isFirstGame;
	}
}
