﻿// See https://aka.ms/new-console-template for more information
using Dumpify;
using Grpc.Net.Client;
using GrpcServer;

Console.WriteLine("Hello, World!");

var input = new HelloRequest { Name ="Abc" };
var customerInput = new CustomerLookUpModel { UserId = 1 };
var cts = new CancellationTokenSource();

//https://localhost:7041;http://localhost:5151
var channel = GrpcChannel.ForAddress("https://localhost:7041");

//
var client = new Greeter.GreeterClient(channel);
var reply = await client.SayHelloAsync(input);
Console.WriteLine(reply.Message);

//
var customerClient = new Customer.CustomerClient(channel);
var customerReply = customerClient.GetCustomerInfo(customerInput);
Console.WriteLine(customerReply.Dump());

using(var call = customerClient.GetNewCustomer(new NewCustomerRequest()))
{
    while(await call.ResponseStream.MoveNext(cts.Token))
    {
        var currentCustomer = call.ResponseStream.Current;
        currentCustomer.Dump();
    }
}

Console.ReadLine();
