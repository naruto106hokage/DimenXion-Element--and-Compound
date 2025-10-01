Package name: DatabaseManager

Last update: 8th March 2018

Use: It is used to manage user activity data. 

Technical Specification:
Supported OS: Android
Minimum sdk version: 15
Maximum sdk version: 26
Supports Unity 5.6.2f1+


Changes: 
The InsertFinalDataToDatabase function now writes user activity data to database file of master app inside that device.
Now there are no functions to send data to server.


Steps to implement the new database system:

Case 1: New Project
Simply import the DatabaseManager package and use AddEntry function to add entries.
Use InsertFinalDataToDatabase function to write user activity data to MasterApp database.

Case 2: Existing project
Import the DatabaseManager package and delete the HandleSyncButton script, DatabaseConnection script and sync to server button. 
The rest of the steps are the same as in case of a new project.



Harsh Priyadarshi
harsh.priyadarshi@umety.com