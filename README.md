# Prereqs
- Docker Desktop,
- .NET Core 8.0

# RabbitMQ - docker
```
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.13-management
```
# Config files - initial localization
There is one place containing 
config files for both startup projects.
Paths to them are pass as params.

# Multiple startup projects
Receiver and Transmiter are executed together.

# Step by step
- Execute dockerized rabbitmq.
- Copy config files to e://(from Logic.UT/Data) and pass path to them as paramas to exe as described in project-scoped README.md.
- Execute multiple startup projects (Receiver and Transmiter)
- Press START to run simulator.
