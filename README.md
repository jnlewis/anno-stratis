# Anno
Decentralized Ticket Reservation Platform Built on the Stratis Blockchain.

## Introduction
Anno is a decentralized reservation platform with the aim to improve the integrity of online ticket reservations by providing an easy solution for businesses to utilize the programmable logic and immutability of smart contracts on the blockchain.

Businesses can integrate their existing web and mobile apps with Anno to immediately enable graceful handling of sales, ticket verification, cancellations, refunds, and direct payments from customers without a payment provider.

Target users are companies and organizations that provide reservation facility to their user base – which includes event organizers, hotels, cinemas, airlines, transportation companies, tour agencies, theme parks, restaurants and daily deals sites.

In short, Anno provides an abstraction to the processes of ticket reservations and verifications.

## Main Features
*	Ticket sales and reservations, payment transactions, ticket verification and redemption, cancellations and refunds, statistical data for reports and analytics.

## Technologies
*	The API is developed in Microsoft ASP.NET Web API 2 and Entity Framework 6.2.
*	The smart contract is developed in C# and runs on Stratis blockchain.

## Official Website & Live Product Preview
<a href="http://anno.network" target="_blank">http://anno.network</a><br />
<a href="http://anno.network/stratispreview" target="_blank">http://anno.network/stratispreview</a>

## API
For the complete API documentation, please see <a href="https://documenter.getpostman.com/view/469639/SWE56K4S" target="_blank">Anno API Documentation</a>.

## Why Use a Blockchain?
The use of a blockchain enables direct payments from clients thus eliminating the need for a payment gateway, while also providing resilience to common failures found on centralized systems such as data loss or overselling. All bookings are safely recorded, and refunds are certain and processed with no delays. The blockchain can also avert the problem of overselling can tickets reselling where resellers buy a bulk of tickets (commonly in concerts) and resells for a higher price.

## Off-Chain Database
Anno commits transactions to the blockchain, but is also backed by a custom built off-chain database. The purpose of the database is to support instant querying of large datasets at no cost. This is to enable the platform’s data analytics feature. Although the off-chain database is centralized by nature, it is our design goal to ensure that it can be rebuilt entirely from the decentralized data stored on the blockchain.
For the proof-of-concept stage, the off-chain database is built on MySQL with the option to move to NoSQL solutions in the future for scalability.

## General Process Overview
*	All interactions on Anno are completed through the Anno API by using an API Key unique to each user of the platform. The user should already have an internet facing product (such as web or mobile app) which will integrate with Anno via HTTP requests.
*	The three main entities on the platform are Host, Customer, and Event – each of which has their own account address on the blockchain with an active balance of Anno Tokens.
*	When a user signs up on Anno, their profile is created as a host, where they can create events (services or products) and add customers who will buy tickets to their events. An API Key will be granted to the host for accessing the platform.
*	Events created by the host can be configured to have multiple option tiers with different pricing structure.
*	When making a reservation on an event, the host will invoke Anno API to indicate that a particular customer intends to book an event. Anno then verify the booking, and transfer funds from the customer account to the event's escrow account. A booking confirmation and ticket numbers are generated for the user.
*	Funds in event's escrow are kept until the event has started, after which these funds can be claimed by the host (user) of that event. The event's escrow guarantees customer refunds upon cancellation.
*	If the host cancels an event before the event has started, all funds in that event's escrow will be returned to the customers.
*	If a ticket is cancelled before the event has started, the paid price for that ticket is returned to the customer.
*	When a customer is ready to redeem their ticket, the user will invoke Anno API with the given ticket number for verification. Tickets can no longer be refunded once redeemed.

