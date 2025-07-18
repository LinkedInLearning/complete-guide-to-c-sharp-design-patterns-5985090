/*
Strategy Pattern

Summary:
The Strategy Pattern defines a family of algorithms, encapsulates each one, and makes them interchangeable at runtime. It allows the algorithm to vary independently from clients that use it by delegating behavior to strategy objects.

Problem to Solve:
In an e-commerce checkout system, different payment methods (credit card, bank transfer) have different validation rules and processing logic. The challenge is to handle multiple payment strategies while maintaining a consistent checkout interface and allowing easy addition of new payment methods without modifying existing code.
*/

using System;

namespace DotNetDesignPatterns.Patterns.Behavioral
{
    /// <summary>
    /// Result interface that encapsulates payment processing outcomes from different strategies.
    /// </summary>
    public interface IPaymentResult
    {
        bool IsSuccessful { get; }
        string Message { get; }
        string TransactionId { get; }
        decimal ProcessedAmount { get; }
    }

    /// <summary>
    /// Context interface that provides a unified interface for different payment strategies.
    /// </summary>
    public interface ICheckoutService
    {
        IPaymentResult ProcessCreditCardPayment(decimal amount, string cardNumber);
        IPaymentResult ProcessBankTransferPayment(decimal amount, string accountNumber);
    }

    internal class PaymentResult : IPaymentResult
    {
        public bool IsSuccessful { get; }
        public string Message { get; }
        public string TransactionId { get; }
        public decimal ProcessedAmount { get; }

        public PaymentResult(bool isSuccessful, string message, string transactionId = "", decimal processedAmount = 0)
        {
            IsSuccessful = isSuccessful;
            Message = message;
            TransactionId = transactionId;
            ProcessedAmount = processedAmount;
        }
    }

    internal class CheckoutService : ICheckoutService
    {

        public IPaymentResult ProcessCreditCardPayment(decimal amount, string cardNumber)
        {
            if (amount <= 0)
                return new PaymentResult(false, "Payment amount must be greater than zero");

            if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 16)
                return new PaymentResult(false, "Invalid credit card number");

            var transactionId = $"CC_{Guid.NewGuid().ToString()[..8]}";
            return new PaymentResult(
                isSuccessful: true,
                message: "Credit card payment processed successfully",
                transactionId: transactionId,
                processedAmount: amount
            );
        }

        public IPaymentResult ProcessBankTransferPayment(decimal amount, string cardNumber)
        {
            if (amount <= 0)
                return new PaymentResult(false, "Payment amount must be greater than zero");

            if (amount <= 10)
                return new PaymentResult(false, "Bank transfer minimum amount is $10");


            var transactionId = $"BT_{Guid.NewGuid().ToString()[..8]}";
            return new PaymentResult(
                isSuccessful: true,
                message: "Bank transfer initiated successfully",
                transactionId: transactionId,
                processedAmount: amount
            );
        }

    }


    /// <summary>
    /// Factory for creating the strategy pattern with checkout service that handles multiple payment strategies.
    /// </summary>
    public static class StrategyPattern
    {
        public static ICheckoutService Create()
        {
            return new CheckoutService();
        }
    }

    /*
    Requirements:

    To pass the tests, implement the following:

    1. PaymentResult class implementing IPaymentResult:
       - Store success status, message, transaction ID, and processed amount
       - Should be immutable with all properties set via constructor

    2. CheckoutService class implementing ICheckoutService:
       - ProcessCreditCardPayment method:
         * Validate card number (at least 16 digits)
         * Success message: "Credit card payment processed successfully"
         * Transaction ID prefix: "CC_"
       - ProcessBankTransferPayment method:
         * Validate account number (at least 10 digits)  
         * Minimum amount: $10
         * Success message: "Bank transfer initiated successfully"
         * Transaction ID prefix: "BT_"

    3. Common validation for both payment methods:
       - Amount must be greater than 0
       - Return appropriate error messages for validation failures
       - Generate unique transaction IDs for successful payments

    4. Update Create method to:
       - Return a checkout service instance that implements both payment strategies
       - Service should handle strategy switching internally based on method called

    The tests will verify that:
    - Both payment methods process valid requests successfully
    - Validation rules are enforced for each payment type
    - Different strategies produce appropriate transaction IDs and messages
    - Error handling works correctly for invalid inputs
    */
}
