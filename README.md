# High_Load_Email_Notifcation_Service
Email Notification Service that process high volume requests and load. The code uses a Broker service that designates published requests to various consumers to handle

The Application consistes of 3 different and important service

The API request service that processes the request and connects to RabbitMQ to queue it.
The Email Service that is triggered by the consumer worker service that processes abd sends the email
The Worker service that listens and consumes the event
