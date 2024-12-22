
using RideSharingApp.Services;

var paymentService = new PaymentService();
var notificationService = new NotificationService();
var rideService = new RideService(paymentService, notificationService);

// Simulating ride requests and operations
rideService.RequestRide("Alice");
rideService.RequestRide("Bob");

rideService.AcceptRide(1, "John");
rideService.CompleteRide(1);

rideService.AcceptRide(2, "David");
rideService.CancelRide(2);

/*
SRP:
Ride represents the domain entity for a ride.
RideService handles ride-specific business logic.
PaymentService handles payment processing.
NotificationService handles notifications.

OCP:
Add new payment gateways (e.g., Stripe, PayPal) by extending IPaymentService.
Add new notification methods (e.g., SMS, Push Notifications) by extending INotificationService.

LSP:
RideService depends on abstractions (IPaymentService, INotificationService), allowing substitutable implementations.

ISP:
Separate interfaces for ride operations, payment processing, and notifications.

DIP:
RideService depends on IPaymentService and INotificationService, not concrete implementations.
 */
