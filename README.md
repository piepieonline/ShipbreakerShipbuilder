## Steps to get setup
* Install Unity 2020.3.17f (https://unity3d.com/unity/whats-new/2020.3.17)
* Copy (only those with waiting '.meta' files listed) game dlls from 'Shipbreaker_Data\Managed' into the Dll folder
* Update "Assets/shipbreaker_settings.json" with the correct game path
* Open unity, and run "Shipbreaker/Update game catalog"

## Steps to build
* Discard all changes to the SampleScene
* Run "Shipbreaker/Build"

## First steps
* Open SampleScene
* Navigate to "CustomOperation/BoxRef"
* Drag "BoxRef.prefab" into the scene
 * If this doesn't look right, when the Editor has finished loading, click "Addressables" then "Draw Editor", "Clear Addressables" and then "Update View" 
* Move the atmospheric regulator into the box
* Click the BoxRef prefab, and apply overrides
* Discard changes to the scene
* Build ("Shipbreaker/Build")
* Open "Hardspace - Shipbreaker\BepInEx\plugins\TestProj\settings.json" and change the "assetReferenceGameObject" to "f2ade62975d33c5408fc695ba5be1d27"
* Run the game, and enter freeplay on an existing tile

## Creating custom ships
* 

## Custom ship notes
* The game doesn't work with negative scales

## Joints
* Joints are how the game knows to connect multiple separate assets
* Mandatory Joint Containers: These work by connecting all child objects. Useful for attaching trim to a wall, for example (can be cut in game, but won't come apart otherwise)
* StructurePartAsset - Joint setup asset: These work by attaching at runtime, whatever is connected to them - used by things like cutpoints?

## Rooms
* RoomContainerDefinition: Define how the room works. At the moment, I am reusing existing definitions only.
* RoomSubVolumeDefinition: (green transparent boxes) should fully encompass that piece's internal area - multiple can potentially be used for complex shapes? Must be a child of a RoomContainerDefinition
* RoomOpeningDefinition: (red transparent boxes), define how the volumes connect - including blocking (walls), allowing expansion (overlap) and doors (portals)
 * Flow axis (red arrows) defines which direction should the air flow upon breaching
* Room overlaps need to be carefully managed, as extra/misplaced overlaps will cause instant breaching when loading

## Inspecting game content
* Find the '.prefab' in keys.txt, and copy it's address from the line below
* Open SampleScene, and paste it into "Load Addressables/Address" (contains the mack airlock by default)
* Click 'Load GO'

## Other important gotachyas
* If something doesn't load correctly, make sure that everything that needs to be marked as addressable, is!
* I am caching all (game based) addressables at the moment. This means that the view of an addressable prefab won't update unless you remove it's prefab from "Assets/EditorCache"
* When using asset references, it must be the assets GUID, not the addressable name/path
* Currently, there is no way to reuse the shader from the game, so we are stuck with a default lit shader, which doesn't behave the same
* Spawnpoint hardpoint is not working