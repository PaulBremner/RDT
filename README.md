# RDT
Latest RDT interface build with laser pointer tagging in VR mode
Laser tagging added for the right hand. 

To Use:
Touch the right d-pad the menu pops up around the controller. Move thumb around to select desired marker, selcted marker is in green, and currently selected marker has a white outline. 
Click the d-pad to select a marker type. Point the laser and click the trigger to spawn a marker of the selected type.

Things to check if there are problems:
- The laser tagging script is attached to the VRMode parent, and all 5 prefabs are in the list of prefabs
- The needed actions are present in the VRInput action list, configured in the SteamVR binding UI, and assigned in the RadialMenuHandler script: 
  - showMenu boolean action assigned to trackpad touch
  - optionSelected boolean action assigned to trackpad click
  - getSelection vector2 action assigned to trackpad position
  - the input should be set to per hand in the SteamVR input tab. All the movement stuff is set to the left hand and unchanged, 
  the right hand d-pad should be set with trackpad input with the above settings.
- all highlight objects have the togglehighlight script
- all references are set in the UI - hand set to righthand in menu handler, 
onclick functions for the buttons set to LaserTag.SetMarker passing an int according to the button order in the hierarchy (0-4)


WARNING: there is a bug somewhere in the steamVR code that causes unity to crash on stopping a running scene, and I've not figured out where or why. 
It usually only happens if controllers are turned on and at least one of them is not fully visible to the base stations when you hit stop.
