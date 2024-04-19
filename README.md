# RabbitMQ configuration
```
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.13-management
```
# Config files - initial localization
They are copied to e:// - path hardcoded. There is one place containing 
config files for both startup projects.
Please modify in code in case You do not have e partition.

# Multiple startup projects
Receiver and Transmiter are executed together.

# Step by step
Execute dockerized rabbitmq.
Copy config files to e://(from Logic.UT/Data)
Execute multiple startup projects (Receiver and Transmiter)
Press START to run simulator.