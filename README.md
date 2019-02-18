# Serilog RabbitMQ Example
Example use case for .NET Core logging through Serilog and RabbitMQ

## What is this?
A really simple example of how you can do network logging from ASP.NET Core.

## Why use Serilog?
Serilog seems to be the most well maintained logging framework I was able to find that has good first-party support for lots of logging methods (including RabbitMQ). It's also very simple to get setup and doesn't have much boilerplate. Finally, it integrates nicely with ASP.NET Core and supports the latest .NET Standard.

## Why use RabbitMQ?
RabbitMQ provides a simple, reliable architecture for network communication. While REST is useful for requesting data, the synchronicity is not ideal for logging, and making it asynchronous isn't often simple. RabbitMQ also provides a buffer in the event that the logging server has stopped responding to requests. A message queue is simply better suited to logging which is inherently message passing as opposed to REST which is meant for transactions/queries.

## How well does this integrate with ASP.NET Core?
It integrates perfectly, as it uses Serilog as the logging service which implements [ILogger](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/). Serilog handles all the RabbitMQ communication for you, so you can continue logging as you always have. On the server side, you simply have a RabbitMQ receive loop and you'll get all the logs in almost-realtime. Then it's up to you to choose what you want to do with them. In my example, Serilog stringifies the logs into JSON before sending them as RabbitMQ messages, meaning that parsing at the other end is trivially simple.

## What do I need to do to set this up?
Firstly, you'll need to install RabbitMQ server somewhere. To configure it, you simply need to add an exchange called `app-logging`, a queue called `Logs`, and then bind the exchange to the `Logs` queue with the routing key `Logs`. Note that all these names can be chosen as you like. You'll just have to change the config in the examples if you change the names.

The simplest way to set up the exchange, queue, and binding is using the RabbitMQ management plugin as described [here](https://www.rabbitmq.com/management.html). The default username and password for RabbitMQ is `guest` and `guest`.

Alternatively, you can use the [RabbitMQ Cli Tool](https://www.rabbitmq.com/management-cli.html) to do the exchange, queue, and binding creation. Note that you'll still have to enable the management interface plugin for this to work.

Finally, simply run the respective projects using `dotnet run` within their project directories. All the logs from the example web application will automatically be routed through RabbitMQ. The log server will then receive the logs from RabbitMQ and write them to Webapp.log as JSON.

## What does the license mean?
The Unlicense attached to this project means that you can use it in it's entirety for any reason whatsoever with no obligations. If you do reference large portions of the code or use it in it's entirret, then an attribution would be appreciated. That said, it's still not a legal obligation.
