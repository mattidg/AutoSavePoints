It's a Checkpoint System, but worse.

Have the game Auto Save after a user defined amount of completed missions. The save will overwrite any other saves including user, mod, or mission saves so you can get stuck in a cycle of reloading and dying, refund requests will be ignored. User settings is located in the this mod's folder <Drive>:\SteamLibrary\steamapps\workshop\content\3018410\3382633068, currently set to auto save when 2 missions are completed or 5 minutes have passed and no warning will be shown.

JSON Values:
"showSaveWarning": false,
"autoSaveTime": 300,
"autoSaveMissions": 2

showSaveWarning can only be true or false, default is false

autoSaveTime can be any integer greater than 0 this is in seconds, default is 300 seconds (5 minutes)

autoSaveMissions can be any integer greater than 0, default is 2

Image is from me

GitHub: https://github.com/mattidg/AutoSavePoints

Changelog:

1.1.0:
Added .JSON settings file in the mod folder

1.0.0:
Ported mod released