Steps to run the app -  

  - Create a local database called "DBTriggerTest" 
  - Run initial Migration which will create your Products table 

Steps to implement the Database Triggers - 

  - Navigate to /Data/Scripts
  - Create the Product Audit log table by running "CreateAuditLogTable.sql" 
  - Create the Product table triggers by running "ProductTriggers.sql" 

You're good to go.  Run the app, Add, Edit and Delete products using the MVC app and go check your Products/ProductsAudit tables to track the lifecycle of your data.  
