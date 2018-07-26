# Psychbot
A mental health chatbot that helps with finding the best expert for any patient.

**Resources**

This project consists of a visual studio solution as well as an aiml library.
Visual studio can be downloaded using: https://visualstudio.microsoft.com/vs/ 

All other sources can be obtained via: https://sourceforge.net/projects/aimlbot/files/

This sourceforge project includes the aiml libraries and some data files for trivial speech.
It is used as a basis for a number of command line applications that use Aiml in the backend. 
An example of a guide for such an application would be: 
https://www.instructables.com/id/How-to-Make-a-Robot-That-Talks-Back-Using-AIML-in-/

**Building the app**

Before running the code make sure to add the reference to the aiml library.
If not automatically done for you, in your IDE implement a new reference, using the following path for the library: chatbot/bin/debug/aimlbot.dll

This system uses a windows forms application as its way of communicating to the user. This allows it to have a more intricate front-end than just a command line interface. 
To my knowledge, this type of application cannot be acquired anywhere past the stage of a command line chat application and the translation to a visual system using the existing Aiml libraries was all done by me.
This means that the algortihm that takes user input and delivers it to the back-end bot, as well as taking the response and printing that to a visual front-end were created by me.

