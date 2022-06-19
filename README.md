## Steps to get setup
* Copy (only those with waiting '.meta' files listed) game dlls from 'Shipbreaker_Data\Managed' into the Dll folder
* Update "Assets/shipbreaker_settings.json" with the correct game path
* Open unity, and run "Shipbreaker/Update game catalog"

## Steps to build
* Run "Shipbreaker/Build"

## Notes
* Unity must be restarted after each build before you can load a new object from the game files
* When using asset references, it must be the assets GUID, not the addressable name/path
* Currently, there is no way to reuse the shader from the game, so we are stuck with a default lit shader, which doesn't behave the same
* Spawnpoint hardpoint is not working