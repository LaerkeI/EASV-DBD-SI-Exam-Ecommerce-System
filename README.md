# EASV-DBD-SI-Ecommerce-System

### RabbitMQ Messaging
Problemet med at OrderEventConsumer i InventoryManagementService ikke modtog beskeder fra OrderService:  
Consumer d�r og lytter ikke p� beskeder. 
Man b�r i stedet bruge en background service, 
s� man er garanteret at den k�rer i sin egen thread.