# Music-App

My first contract as Nova Realities was to create an AR music application called Firstage. Firstage stream video files from an AWS server which we then projected into AR using the Kudan AR engine. The performances are shot in the Firstage studio against a green screen. We then use a shader within Unity to key out the green background. The performances an be displayed in either marker-based or markerless AR. The UI is similar to Netflix in its library like appeal. The back-end is handled by AWS and Parse-server. 

These are just samples of the scripts. No assets or project files are included.

InitialisationScript.cs is used to swap between tracking modes. If marker-based tracking is selected, a marker name is also pulled from parse.

MenuControl.cs is a script used to control the whole UI flow of the application. It enables and disables UI elements based on how the user is navigating through the app.

PerformanceManager.cs is a script that controls the whole flow of the app when the AR screen is activated. It can be used to load/play/pause the performance. We also use this script to align the performance in AR and set the scale of the performance.
