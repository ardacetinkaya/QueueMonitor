# QueueMonitor
 
<img align="right" src="https://user-images.githubusercontent.com/4550197/135899113-8dbfb8c3-3aa3-408a-96e1-18933b68e125.gif">Simple monitoring application to monitor Azure Storage Queues and RabbitMQ queues. I've just need a quick monitoring approach to monitor queues for my some other project, so this simple application appeared in a short time. Not a fancy app. but it solves my problems. Feel free to add new feautures and share with everyone.

Also this is a simple .NET Web App(Razor) with SignalR, it would be nice to start to learn .NET platform.

- It is possible to add more than one queue to monitor.
```json
  "Charts": {
    "Settings": [
      {
        "Title": "Messages",
        "QueueName": "SomeQueue",
        "ChartDescription": "Message counts",
        "Color": "#1b9e77",
        "Type": "AzureStorageQueue",
        "ConnectionString": "ConnectionString"
      }
    ]
  }
```
