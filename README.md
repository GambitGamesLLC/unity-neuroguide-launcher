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

## LAUNCHER CONFIGURATION FILE INSTRUCTIONS

There are multiple configuration .json files located within the Resources folder. 

- This readme covers only the `config.json` file
- Each NeuroGuide experience app repository contains its own documentation on their own configuration .json file.

- `config.json` - Used by the NeuroGuide application that starts this Launcher.

```
{
	"config": {
		"version": 1,
		"timestamp": "2025-07-28 12:00:00",
		"path": "%LOCALAPPDATA%\\M3DVR\\Launcher\\config.json"
	},
	"app": {
		"longname": "M3DVR NeuroGuide Launcher",
		"shortname": "Launcher",
		"path": "%LOCALAPPDATA%\\M3DVR\\Launcher\\Launcher.exe"
	},
	"communication": {
		"address": "127.0.0.1",
		"port": 50000
	}
}
```

<b>`config` OBJECT  </b>
- `version` - Defines the version number of the configuration file, used to see if this is newer than a config file we're comparing against.  
- `timestamp` - If the version of both config files matches, we check this timestamp to see if one is newer.  
- `path` - The path to the config file on local storage. This path has its environment variables expanded and is deserialized, so it can be used for normal Path operations in Unity.  
  
<b>`app` OBJECT  </b>
- `longname` - Used by external software like the M3DVR Neuroguide launcher app to show the app name in a human readable format
- `shortname` - Used by external software like the M3DVR Neuroguide launcher app to display the app name in a short human readable format 
- `path` - The path to the executable for this project. Like other stored Path variables, this will have any environment variables expanded and will be deserialized.  

<b>`communication` OBJECT  </b>
- `address` - The address used by the UDP communication between the NeuroGuide software and the child apps this Launcher runs
- `port` - The port number used by the UDP communication

- `config.json` file is stored in our Resources folder, and can be updated to modify the default `config.json` that is stored in the `%LOCALAPPDATA%` 
- This `config.json` file is copied to our `%LOCALAPPDATA%` folder, specifically in the path specified in the `config:path` object  
- If there already exists a `config.json` at the specified path, we will compare it against the one in the Resources folder. If the local file is out of date or missing, it will be written using the version in Resources.
- It is recommended to have the configuration file that's copied to your `%LOCALAPPDATA%` folder stay at a higher version number than the file Resources folder, that way any changes to the configuration will be used when you restart the NeuroGuide experience and the launcher to test the new values.
- When you have found values you like, its recommended to make those the new defaults within the configuration files in the `Resources` folder of the in this project. That way your next build will include them.

---  

## NEUROGUIDE EXPERIENCE CONFIG FILE INSTRUCTIONS

- Each NeuroGuide experience app has its own configuration .json file in the Resources folder
- The name of the file should match the name of the experience.
- If these experience names are updated or there are new experiences added, these configuration files will need to match.
- In the case of a updated experience name or a new experience being added, you also need to modify the `List<string> appConfigFileNames` inside of `Main.cs` which stores the names of the .json files to locate.

You can find the description of each experience configuration .json file within the repositories readme files.

The values within the config files are passed to each NeuroGuide experience using the command line communication system within the [unity-process-manager](https://github.com/GambitGamesLLC/unity-process-manager) package.

---  

## DEPENDENCIES

Relies on several `Unity Asset Store` plugins as well as Open Source `Gambit Games` packages  

Please make sure the proper `scripting define symbols` and packages are imported into your project.  
When opening this project for the first time, the package manager should grab the appropriate versions of these packages for you.

Check the package repos directly for their `scripting define symbols`, `namespaces` and guides.  

- `DoTween` [Gambit Repo](https://github.com/GambitGamesLLC/unity-plugin-dotween) | [Unity Asset Store Link](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)  
- Used to perform tweens

- `Configuration Manager` [Gambit Repo](https://github.com/GambitGamesLLC/unity-config-manager.git?path=Assets/Plugins/Package)  
- Used for manipulation, saving, and loading of `.json` config files  

- `Process Manager` [Gambit Repo](https://github.com/GambitGamesLLC/unity-process-manager)  
- Allows us to read process command line variables passed in from the NeuroGuide launcher
  
- `Math Helper` [Gambit Repo](https://github.com/GambitGamesLLC/unity-math-helper)  
- Contains convenience functions for math functionality, such as Map(), which converts one value in a range to another  
  
- `Singleton Manager` [Gambit Repo](https://github.com/GambitGamesLLC/unity-singleton)
- Convenience function to easily create global singletons that retain Unity Lifecycle functionality such as a GameObject Instance

- `Static Coroutine` [Gambit Repo](https://github.com/GambitGamesLLC/unity-static-coroutine)
- Allows for Singletons to use the Unity Coroutine system.
  
- `TotalJSON` [Gambit Repo](https://github.com/GambitGamesLLC/unity-plugin-totaljson) | [Unity Asset Store Link](https://assetstore.unity.com/packages/tools/input-management/total-json-130344)  
- Used for JSON manipulation  

- `Rounded Corners` [Github Repo](https://github.com/gilzoide/unity-rounded-corners.git)
- Provides UIImages with the ability to have rounded corners
