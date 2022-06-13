## Steps to get setup
* Update modded_catalog.json to point to your game files
* Copy (only those with waiting '.meta' files listed) game dlls from 'Shipbreaker_Data\Managed' into the Dll folder
* Update 'Assets\Editor\LoadGameAssets.cs' to the right paths

## Steps to build
* Ensure your asset is marked as addressable.
* Open the addressable window, and select Build in the top right (Note, I build directly into the game folder. I recommend doing the same, so the catalog is correct)
* Build the project itself (It will fail, but a catalog is still produced - which is all we need)
* Copy the catalog into the game mod directory (BepInEx\plugins\TestProj)

## Notes
* Unity must be restarted after each build
* When using asset references, it must be the assets GUID, not the addressable name/path