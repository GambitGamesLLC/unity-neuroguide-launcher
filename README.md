# unity-neuroguide-launcher
Unity3D project that launches the processes for NeuroGuide experiences

This application, along with the child apps that it launches, need to be placed within the `%LOCALAPPDATA` path on your Windows PC.

Please refer to the official [Neuroguide Launcher Installation Guide](https://docs.google.com/document/d/1LlBwy8y8CJdUcBKdF3T6FI2JTMg8hdeoMj30FSdo1g0/edit?usp=sharing) for details on the steps to installing a build on a PC.

<img width="727" height="406" alt="image" src="https://github.com/user-attachments/assets/ff08eb7c-9791-4753-af03-d08b5eb416fa" />

---

## SETUP INSTRUCTIONS

- For the NeuroGuide applications to launch when clicked, they need to exist at the same `path` variables set in the configuration .json data files referenced in this project.
- [Flow](https://github.com/GambitGamesLLC/unity-neuroguide-flow) should be `%LOCALAPPDATA%\M3DVR\Flow\Flow.exe`
- [Energy](https://github.com/GambitGamesLLC/unity-neuroguide-energy) should be `%LOCALAPPDATA%\M3DVR\Energy\Energy.exe`

---

## PLAY INSTRUCTIONS

- Open `Scenes/Main`
- Press Play in the editor
- Click on the icons of the apps to launch them.

---  

## BUILD INSTRUCTIONS

- No special build instructions, simply make a Windows desktop build
- Your build location needs to be `%LocalAppData%\M3DVR\Launcher`
- This location follows the expected path that the NeuroGuide application from Nestre will expect to find the Launcher and the `config.json` file within the same folder.

---  

## CONFIGURATION FILE INSTRUCTIONS

You can find the appropriate `configuration json` file within the Resources folder of the [unity-neuroguide-launcher](https://github.com/GambitGamesLLC/unity-neuroguide-launcher). 
This configuration file only exists as part of that repository and is not stored in this one.

**If this app is run via the NeuroGuide launcher, it will use the data passed to it by the Launcher, which comes from a configuration .json file**

- A `configuration json` file is stored in our Resources folder of the NeuroGuide Launcher project, and can be updated to modify the application  
- This `configuration json` file is copied to our `%LOCALAPPDATA%` folder, specifically in the path specified in the `config:path` object  
- If there already exists a `configuration json` at the specified path, we will compare it against the one in the Resources folder. If the local file is out of date or missing, it will be written using the version in Resources.
- It is recommended to have the configuration file that's copied to your `%LOCALAPPDATA%` folder stay at a higher version number than the file inside of the [unity-neuroguide-launcher](https://github.com/GambitGamesLLC/unity-neuroguide-launcher) Unity Project, that way any changes to the configuration will be used when you restart this experience and the launcher to test the new values.
- When you have found values you like, its recommended to make those the new defaults within the configuration files in the `Resources` folder of the [unity-neuroguide-launcher](https://github.com/GambitGamesLLC/unity-neuroguide-launcher) project. That way your next build will include them.

- Locate and open the configuration json file within the resources folder, which has contents similar to this
```json
{
	"config": {
		"version": 2,
		"timestamp": "2025-07-28 12:00:00",
		"path": "%LOCALAPPDATA%\\M3DVR\\Launcher\\Energy.json"
	},
	"app": {
		"name": "Energy",
		"path": "%LOCALAPPDATA%\\M3DVR\\Energy\\Energy.exe",
		"length": 3,
		"debug": true,
		"logs": false,
		"threshold": 0.9
	}
}
```
  
<b>`config` OBJECT  </b>
- `version` - Defines the version number of the configuration file, used to see if this is newer than a config file we're comparing against.  
- `timestamp` - If the version of both config files matches, we check this timestamp to see if one is newer.  
- `path` - The path to the config file on local storage. This path has its environment variables expanded and is deserialized, so it can be used for normal Path operations in Unity.  
  
<b>`app` OBJECT  </b>
- `name` - Used by external software like the M3DVR Neuroguide launcher app to show the app name in a human readable format  
- `path` - The path to the executable for this project. Like other stored Path variables, this will have any environment variables expanded and will be deserialized.  
- `length` - How long should this experience last (in seconds) if the user was in a "reward" state the entire time?
- `debug` - Do we want to enable debug mode for this app? This will fake incoming UDP port traffice as if the NeuroGuide Software was sending us messages
- `logs` - Do we want Unity console logs to be shown in our visual console for debugging?  
- `threshold` - Normalized 0-1 value representing how far into the experience you need to be before triggering the threshold state of the app. EX: For 0.9, that would be 90% into the experience.

---  

## DEPENDENCIES

Relies on several `Unity Asset Store` plugins as well as Open Source `Gambit Games` packages  

Please make sure the proper `scripting define symbols` and packages are imported into your project.  
When opening this project for the first time, the package manager should grab the appropriate versions of these packages for you.

Check the package repos directly for their `scripting define symbols`, `namespaces` and guides.  

- `DoTween` [Gambit Repo](https://github.com/GambitGamesLLC/unity-plugin-dotween) | [Unity Asset Store Link](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)  
- Used to perform tweens

- `In-game Debug Console` [Unity Asset Store Link](https://assetstore.unity.com/packages/tools/gui/in-game-debug-console-68068)  
- Used to display an in-game console for debugging purposes when the 'logs' variable is enabled in the Main component or passed in via the Process data system from the Launcher

- `Skybox Series Free` [Unity Asset Store Link](https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633)  
- Used one of their assets for the skybox, should be removed outside of just what's needed for this project

- `SpaceSkies Free` [Unity Asset Store Link](https://assetstore.unity.com/packages/2d/textures-materials/sky/spaceskies-free-80503)  
- Used one of their assets for the skybox, should be removed outside of just what's needed for this project

- `Configuration Manager` [Gambit Repo](https://github.com/GambitGamesLLC/unity-config-manager.git?path=Assets/Plugins/Package)  
- Used for manipulation, saving, and loading of `.json` config files  

- `NeuroGuide Manager` [Gambit Repo](https://github.com/GambitGamesLLC/unity-neuroguide-manager.git)  
- Reads data from the NeuroGuide Software via UDP ports  
  
- `Process Manager` [Gambit Repo](https://github.com/GambitGamesLLC/unity-process-manager)  
- Allows us to read process command line variables passed in from the NeuroGuide launcher
  
- `Math Helper` [Gambit Repo](https://github.com/GambitGamesLLC/unity-math-helper)  
- Contains convenience functions for math functionality, such as Map(), which converts one value in a range to another  
  
- `Singleton Manager` [Gambit Repo](https://github.com/GambitGamesLLC/unity-singleton)
- Convenience function to easily create global singletons that retain Unity Lifecycle functionality such as a GameObject Instance
  
- `TotalJSON` [Gambit Repo](https://github.com/GambitGamesLLC/unity-plugin-totaljson) | [Unity Asset Store Link](https://assetstore.unity.com/packages/tools/input-management/total-json-130344)  
- Used for JSON manipulation  
  
