using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections;
using UnityEditor;

public class BatchBuild : MonoBehaviour {
	static string[] scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();

	[MenuItem("Build/Android")]
	static void AndroidBuild() {
		var outputFile = Path.GetFullPath(Path.Combine(Application.dataPath, @"../output/androidBuild.apk"));

		if(File.Exists(outputFile)){
			File.Delete(outputFile);
		}

		var options = BuildOptions.Development;

		BuildPipeline.BuildPlayer(
			scenes,
			outputFile,
			BuildTarget.Android,
			options
		);
	}

	[MenuItem("Build/iOS")]
	static void iOSBuild() {
		var outputFile = Path.GetFullPath(Path.Combine(Application.dataPath, @"../output/iOS"));

		if(Directory.Exists(outputFile)){
			Directory.Delete(outputFile, true);
		}

		var options = BuildOptions.Development;

		BuildPipeline.BuildPlayer(
			scenes,
			outputFile,
			BuildTarget.iOS,
			options
		);
	}
}
