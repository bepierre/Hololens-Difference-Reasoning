HoloLens Difference Reasoning
===

The aim of this project is to compute and to display the 3D differences between a pre-loaded model of the scene and the current scan in real-time using Microsoft's HoloLens.
It was implemented using Unity.

All scripts and shaders written for this project as well as taken from external libraries are in the Assets folder.

The Scripts folder contains LoadDepth.cs RenderDepth.cs and RenderDepthDifference.cs all written by us.

Each script has an associated shader (also written by us) in Resources/Shaders.

RenderDepth (script and shader together) is not used in the final version of the project but is very useful for debugging. It simply renders the current depth map on a chosen camera.

LoadDepth is a shorter version of RenderDepth that doesn't render the DepthMap but simply loads it to the GPU to make it accessible for RenderDepthDifference which will render the depth difference on the screen.

For the spatial mapping we took scripts from the HoloToolkit. They are in Assets/Scripts/HoloToolkit SpatialMapping.

Assets/Resources contain the shaders, scans and materials.

Scans are 3D models of our lab for example to do some testing on the HoloLens emulator and to be compared to the spatial mapping on the HoloLens.

Assets/Scenes contain 4 scenes. 
ChairTestScene to compare a room before and after moving a chair, on the HoloLens emulator, with the corresponding scans. 
CubeTestScene that compares simple shapes for the initial testing. 
ScanTestScene which does the same than ChairTestScene with different scan. 
SpatialMapping which is the scene that is used to do real-time difference reasoning on the HoloLens. It contains one scan that will be compared to the live spatial mapping


// NEW THINGS HAVE BEEN ADDED SINCE THEN, Including:

- A simple UI (using new MRDL toolkit)

- Possibility to do manual alignement

- Slightly different organization of objects in unity to fit the MRDL toolkit

- New Scenes etc...

If you have any questions don't hesitate -> bepierre@student.ethz.ch

PLEASE USE THIS UNITY : https://unity3d.com/de/unity/beta/unity2017.2.0b2
