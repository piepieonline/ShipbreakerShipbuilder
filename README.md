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
* Navigate to "_CustomShips/ExampleBox"
* Drag "ExampleBox.prefab" into the scene hierarchy
 * If this doesn't look right, when the Editor has finished loading, click the "Shipbreaker" menu, then "Force View Refresh" 
* Move and rotate the atmospheric regulator into the box
* Click the ExampleBox in the hierarchy, and apply overrides
* Discard changes to the scene
* Build ("Shipbreaker/Build")
* Run the game, open the freeplay menu, and find "Example Box" at the end of the list
* Close the game
* In the hierarchy, disable "East", "CutPointER" and "CutPointEB"
* Move the airlock such that the inner wall sits inline with the open space, but not touching any walls
* Clone "CutPointEB", enable it, and move it so that it just touches the floor and the airlock inner wall (It'll need to be rotated 180 degrees)
* Build
* Open the ship in-game
* Congratulations, you've created your first ship!

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
* Click 'Load GameObject'

## Other important gotachyas
* If something doesn't load correctly, make sure that everything that needs to be marked as addressable, is!
* I am caching all (game based) addressables at the moment. This means that the view of an addressable prefab won't update unless you remove it's prefab from "Assets/EditorCache"
* When using asset references, it must be the assets GUID, not the addressable name/path
* Currently, there is no way to reuse the shader from the game, so we are stuck with a default lit shader, which doesn't behave the same
* Spawnpoint hardpoint is not working