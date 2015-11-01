# PROJECT GALAXY #

### What is this repository for? ###

* Galaxy Protectors
* v1.05

### Requirements? ###

* Unity 5.1.2f1 or newer
* Android SDK
* Java SE Development Kit

### How to build? ###

* Open project with Unity
* Go to File->Build Settings and select "Android"
* Remove the key from player settings on Build Settings
* Press Build and wait

### How to to install/play? ###

* Go to Options->Security and enable "Unknown Sources" for app installation
* Run and install the .apk file
* Play!


### Official build ###

* https://play.google.com/store/apps/details?id=tk.CrownClownStudios.GalaxyProtectors

### How to test? ###

* Go to '_Scenes' folder under 'Assets' and open the 'Test' scene
* Go to 'Unity Test Tools' from the unity tool bar and open the "Unit Test Runner"
* Click "Run All" to run all the unit tests or select some tests 
  and click "Run Selected" to run the selected tests.
* To look at the testing script, go to "\Assets\UnityTestTools\Tests\UnitTest\Editor\Unit_Test.cs" 


### Directory Structure in Assets ###
 * Entities folders such as Player, Enemies, Powerups have respective sprites, prefabs and scripts in them
 * HUD folder contains sprites, prefabs and scripts related to HUD GameObjects such as health, powe-up timer and etc.
 * Scripts folder contains misc. scripts such as menu scripts, game manager scripts etc
 
