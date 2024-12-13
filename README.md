# EASV-DBD-SI-Ecommerce-System

### RabbitMQ Messaging
Problemet med at OrderEventConsumer i InventoryManagementService ikke modtog beskeder fra OrderService:  
Consumer dør og lytter ikke på beskeder. 
Man bør i stedet bruge en background service, 
så man er garanteret at den kører i sin egen thread.

### Jenkins
testing if pipeline runs when push to master