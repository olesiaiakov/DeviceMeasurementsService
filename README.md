## Goal

> "The problem to solve is following:
>  You monitor devices, which are sending data to you
>  Each device have a unique name
>  Each device produces measurements"
 
Challange is:
- Compute number of messages you got or read from the devices
- The solution can be in any language (preferably C# or F#)
- The scope is open, you must decide how the “devices” will work in your system
- The solution should be posted on GitHub or a similar page for a review
- Please add documentation explaining us how to run your code

## Tech

Using technologies and libraries:
- [OWIN]
- [Autofac]
- [AutoMapper]
- [EntityFramework] (Code First approach)
- [Serilog]
- [Swagger]

For unit test
- [NUnit]
- [AutoFixture]
- [Effort.EF6]

# Structure

- `Areas` - Controllers and models for handling REST API requests
- `DataAcess` - Data Access Layer (database entities and context)
- `Migrations` - for creating an initial database structure
- `Processing` - processing service class for saving measurements to DB
- `Querying` - querying service class for getting statistics data from DB
- `Test` - unit tests

# Details
Let's assume that our devices are sending messages using REST API via HTTP.
We've got a self-host web API service named "DeviceMessagesConsumer".
On the other hand, there is a simple client "ClientSimulator" for simualting sending requests devices.

"DeviceMessagesConsumer" has 2 endpoints.
| Request | Endpoint |Description  |Request Example | Response example |
| ------ | ------ | ------ | ------ | ------ |
| Post | /api/v1/device-measurements/devices/{deviceId}/measurements |Create measurements for device | [ { "MeasuredParameterType": 1,  "Value": 123.4,     "MeasuredAt": "2021-04-08T11:54:24.908Z" } ] | 204 No Content
| Get | /api/v1/device-measurements/devices/statistics | Get statistics for all existed devices with measurements count | | [ {"Id": 1, "Name": "Sensor 1","MeasuresCount": 0}, {"Id": 2,"Name": "Sensor 2","MeasuresCount": 1} ]

## Install and run
1. Build the solution.

2. Check that in .config file sections "connectionString" and "baseAddress" are correct.

3. Setup Microsoft SQL Server and add new database named "DeviceMeasurements".

4. You need to apply migration on newly created database.
For JetBrains Rider: right-click on the project in solution, select EntityFramework - Update Database.
For Visual Studio: Open "Package Manager Console", type "update-database" and press Enter.
By default 3 devices will be added in [DeviceMeasurements].[dbo].[Devices] table. 

5. Run the DeviceMessagesConsumer.exe file.

7. Run the simulators (see below).

Please use swagger "http://localhost:9000/swagger/ui/index" to make it easier to submit your request.

You can check the statistic by accessing http://localhost:9000/api/v1/device-measurements/devices/statistics 

## ClientSimulator

"ClientSimulator" will simulate a device that will send  REST API requests with specified interval. It is a simple console application.
Command line arguments: `<deviceId> <requestsIntervalMs>`
There are also 3 bat files which simulate each of 3 devices sending requests with different interval.
- `StartDevice1Simulator.bat`
- `StartDevice2Simulator.bat`
- `StartDevice3Simulator.bat`


