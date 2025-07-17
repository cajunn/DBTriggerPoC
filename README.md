Steps to run the app -  

  - Create a local database called "DBTriggerTest" 
  - Run initial Migration which will create your *Products*, *Customers* and *Purchases* tables 

Steps to implement the Database Triggers and Audit Tables - 

  - Navigate to in the terminal /Data/Scripts
  - run: " py create-audit-tables-and-triggers.py "
  - this will create a script in /Data/Scripts/Output/
  - run this script on your localDb

You're good to go.  Run the app, Add, Edit and Delete products, customers and purchases using the MVC app and go check your Audit Tables, or directly in the UI
