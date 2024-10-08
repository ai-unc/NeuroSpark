# Unit Test

## Verify GUI Display Height Of the Controllers
1. Open the Unity Hub 
2. Launch AwakeVR
3. Go to Game Mode by press play
4. Complete controller calibration procedure
5. Move Left Controller up and down <br/>
   Does the upper yellow bargraph move with respect to the controller's position ? <br/>
   &emsp; If Yes - Test Passes <br/>
   &emsp; If No - Test Fails <br/>
6. Move Right Controller up and down <br/>
   Does the lower yellow bargraph move with respect to the controller's position ? <br/>
   &emsp; If Yes - Test Passes <br/>
   &emsp; If No - Test Fails <br/>

## Verify Slide Card Loads Image / Be Able To Read From The File System
7. Do you see a "default Slide" on the screen ? <br/>
   If Yes - This means that you Slides FAILED to load <br/>
   If No - Do you see Slide1.jpg of SlideSet1 ? <br/>
   &emsp; If Yes - Test Passes <br/>
   &emsp; If No - Test Fails <br/>        

## Verify Slide Count Loop
8. Continually press the right arrow until you see the first slide of the test again <br/>
   Do you ever see the first slide again ? <br/>
   &emsp; If Yes - Test Passes <br/>
   &emsp; If No - Test Fails 
9. Continually press the lef arrow until you see the last slide of the test again <br/>
   Do you ever see the last slide again ? <br/>
   &emsp; If Yes - Test Passes <br/>
   &emsp; If No - Test Fails 

## Verify We Can Loop Through All Tests
10. Continually press the down arrow until you see the first slide of the first test again <br/>
   Did you go to the first test after finishing the color test ? <br/>
   &emsp; If Yes - Test Passes <br/>
   &emsp; If No - Test Fails 
   
   

# Integration Test

## Verify The Application Runs On Quest 3
1. Power up the headset 
2. Pair the bluetooth footswitch
3. Start the VR Neuromonitoring app
   Did the application start ? <br/>
   &emsp; If Yes - Test Passes <br/>
   &emsp; If No - Test Fails 
   
## Verify Reading Quest 3 Controller Position    
4. Complete controller calibration procedure
5. Move Left Controller up and down <br/>
   Does the upper yellow bargraph move with respect to the controller's position ? <br/>
   &emsp; If Yes - Test Passes <br/>
   &emsp; If No - Test Fails <br/>
6. Move Right Controller up and down <br/>
   Does the lower yellow bargraph move with respect to the controller's position ? <br/>
   &emsp; If Yes - Test Passes <br/>
   &emsp; If No - Test Fails <br/>

## Verify Application Able To Read From The Quest 3 File System
7. Do you see a "default Slide" on the screen ? <br/>
   If Yes - This means that you Slides FAILED to load <br/>
   If No - Do you see Slide1.jpg of SlideSet1 ? <br/>
   &emsp; If Yes - Test Passes <br/>
   &emsp; If No - Test Fails <br/>

## Verify Foot Pedal Functionality
8. Press right arrow pedal
   Does the test go to the next slide ? <br/>
   &emsp; If Yes - Test Passes <br/>
   &emsp; If No - Test Fails
9. Press left arrow pedal
   Does the test go to the previous slide ? <br/>
   &emsp; If Yes - Test Passes <br/>
   &emsp; If No - Test Fails
10. Press down arrow pedal
    Does the program change to the next test ? <br/>
    &emsp; If Yes - Test Passes <br/>
    &emsp; If No - Test Fails
11. Press up arrow pedal
    Does the Quest 3 change to pass-through mode ? <br/>
    &emsp; If Yes - Test Passes <br/>
    &emsp; If No - Test Fails
12. Press up arrow pedal again
    Does Quest 3 return to the application ? <br/>
    &emsp; If Yes - Test Passes <br/>
    &emsp; If No - Test Fails
