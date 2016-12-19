# Overview
This is a MassTransit sample with AzureServiceBus transport that demonstrates sending and receiving messages without having to use Uri addresses.

It currently uses StructureMap IoC framework as part of the configuration, but it can be swapped for pretty much anything.

# Setup

## Create an Azure Service Bus namespace
* login to portal.azure.com
* create a messaging instance
* get the SaS key name and value (default is 

## Local
Clone or download to your local machine, then open in Visual Studio 2015. Locate the MessageHost/App.Config file. Update it with your SB base URI, and the shared access signature key info.

Next, compile the solution to restore all the nuget packages automatically.

# Sample
The sample code contains two commands so far that get sent directly to their corresponding queue. MassTransit is setup so that every message type being sent (Messages and Commands) will get their own queue.

Pub/Sub is currently not shown, but may be added.

# License
Feel free to take and do what you like with the code (MIT).
