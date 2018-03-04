# EmailNotifier
 Simple app using cosmosdb and azure servicebus topics
 
 The app consists of an EmailNotifier.Admin.Web project and a EmailNotifier.Sender project.
 
 1) The Web project allows you to view a list of users, add new users and flag users to send emails to. 
 It uses cosmosdb to persist user information and publishes to an Azure Service Bus topic
 
 2) The Sender project subscribes to the topic and handles messages received. 
 Note: the send email method is not implemented, it is there just to illustrate the concept of handling th messages.
 
 
 SETUP
 
 PRE-REQUISITES---------------------------------
 
 Create the following in Azure
 
 i-A cosmosdb database and collection
 ii-A servicebus with a topic and subscription
 
 
 CONFIG -----EmailNotifier.Admin.Web -------------------------
 
 Open the web config file and replace the following app keys with yours
 
     <add key="endpoint" value="[your_cosmosdb_endpoint]" />

    <add key="authKey" value="[your_cosmosdb_auth_key]" />

    <add key="database" value="[your_cosmosdb_name]" />

    <add key="collection" value="[your_cosmosdb_collection]" />

    <add key="serviceBusConnectionString" value="[your_servicebus_connection_string]" />

    <add key="topic" value="[your_service_bus_topic]" />
    
  CONFIG -----EmailNotifier.Sender -------------------------
 
 Open the App.config file and replace the following keys with yours
 
     <add key="serviceBusConnectionString" value="[your_servicebus_connection_string]" />

    <add key="topic" value="[your_service_bus_topic]" />

    <add key="subscription" value="[your_service_bus_topic_subscription_name]" />


RUN THE APPLICATION

NOTE: App was written using VS2017 and .NET Framework 4.61
