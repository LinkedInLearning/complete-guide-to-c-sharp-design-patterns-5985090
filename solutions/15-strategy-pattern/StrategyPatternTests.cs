using NUnit.Framework;
using System;
using System.Linq;

// Use conditional imports based on project

using DotNetDesignPatterns.Patterns.Behavioral;


namespace DotNetDesignPatterns.Tests.Patterns.Behavioral
{
    /// <summary>
    /// Tests for the Strategy Pattern implementation.
    /// 
    /// The Strategy pattern defines a family of algorithms, encapsulates each one,
    /// and makes them interchangeable. It lets the algorithm vary independently
    /// from clients that use it, promoting open/closed principle and composition over inheritance.
    /// 
    /// Required implementation components:
    /// 1. Strategy interface (IPaymentStrategy) that defines the common algorithm interface
    /// 2. Concrete strategies (CreditCardStrategy, PayPalStrategy, BankTransferStrategy) that implement specific algorithms
    /// 3. Context class (CheckoutService) that uses a strategy and can switch between them
    /// 4. Factory method to create the context with default strategy
    /// 
    /// This implementation demonstrates different payment processing strategies that can be
    /// selected at runtime, each with their own validation and processing logic.
    /// These tests verify the Strategy pattern implementation works correctly
    /// with proper strategy switching and delegation behavior.
    /// </summary>
    [TestFixture]
    public class StrategyPatternTests
    {
        private ICheckoutService _checkoutService = null!;

        [SetUp]
        public void Setup()
        {
            // ALWAYS use the factory method - works for both Solutions and Examples
            _checkoutService = StrategyPattern.Create();
        }

        /// <summary>
        /// Tests that credit card payment processes successfully with valid card.
        /// </summary>
        [Test]
        public void CreditCardPayment_WithValidCard_ShouldProcessSuccessfully()
        {
            // Act - Use checkout service which internally switches to credit card strategy
            var result = _checkoutService.ProcessCreditCardPayment(100.00m, "1234567890123456");

            // Assert
            Assert.That(result.IsSuccessful, Is.True, "Credit card payment should be successful");
            Assert.That(result.ProcessedAmount, Is.EqualTo(100.00m), "Processed amount should match input");
            Assert.That(result.TransactionId, Does.StartWith("CC_"), "Transaction ID should have CC prefix");
            Assert.That(result.Message, Is.EqualTo("Credit card payment processed successfully"));
        }

        /// <summary>
        /// Tests that credit card payment fails with invalid card number.
        /// </summary>
        [Test]
        public void CreditCardPayment_WithInvalidCard_ShouldFail()
        {
            // Act - Use checkout service with invalid card
            var result = _checkoutService.ProcessCreditCardPayment(100.00m, "123");

            // Assert
            Assert.That(result.IsSuccessful, Is.False, "Credit card payment should fail with invalid card");
            Assert.That(result.Message, Is.EqualTo("Invalid credit card number"));
        }

        /// <summary>
        /// Tests that bank transfer payment processes successfully with valid account.
        /// </summary>
        [Test]
        public void BankTransferPayment_WithValidAccount_ShouldProcessSuccessfully()
        {
            // Act - Use checkout service which internally switches to bank transfer strategy
            var result = _checkoutService.ProcessBankTransferPayment(25.00m, "1234567890");

            // Assert
            Assert.That(result.IsSuccessful, Is.True, "Bank transfer should be successful");
            Assert.That(result.ProcessedAmount, Is.EqualTo(25.00m), "Processed amount should match input");
            Assert.That(result.TransactionId, Does.StartWith("BT_"), "Transaction ID should have BT prefix");
            Assert.That(result.Message, Is.EqualTo("Bank transfer initiated successfully"));
        }

        /// <summary>
        /// Tests that bank transfer payment fails when amount is below minimum.
        /// </summary>
        [Test]
        public void BankTransferPayment_WithAmountBelowMinimum_ShouldFail()
        {
            // Act - Use checkout service with amount below minimum
            var result = _checkoutService.ProcessBankTransferPayment(5.00m, "1234567890");

            // Assert
            Assert.That(result.IsSuccessful, Is.False, "Bank transfer should fail with amount below minimum");
            Assert.That(result.Message, Is.EqualTo("Bank transfer minimum amount is $10"));
        }

        /// <summary>
        /// Tests that checkout service fails with zero amount.
        /// </summary>
        [Test]
        public void CheckoutService_WithZeroAmount_ShouldFail()
        {
            // Act - Test zero amount through checkout service
            var result = _checkoutService.ProcessCreditCardPayment(0m, "1234567890123456");

            // Assert
            Assert.That(result.IsSuccessful, Is.False, "Payment should fail with zero amount");
            Assert.That(result.Message, Is.EqualTo("Payment amount must be greater than zero"));
        }

        /// <summary>
        /// Tests that checkout service switches between payment strategies correctly.
        /// </summary>
        [Test]
        public void CheckoutService_ShouldSwitchStrategiesCorrectly()
        {
            // Act - Multiple payment methods using checkout service (strategy switching)
            var creditCardResult = _checkoutService.ProcessCreditCardPayment(100m, "1234567890123456");
            var bankResult = _checkoutService.ProcessBankTransferPayment(200m, "9876543210");

            // Assert - All should succeed with correct transaction ID prefixes
            Assert.That(creditCardResult.IsSuccessful, Is.True, "Credit card payment should succeed");
            Assert.That(bankResult.IsSuccessful, Is.True, "Bank transfer should succeed");

            Assert.That(creditCardResult.TransactionId, Does.StartWith("CC_"));
            Assert.That(bankResult.TransactionId, Does.StartWith("BT_"));
        }

        /// <summary>
        /// Tests that the factory method creates working components.
        /// </summary>
        [Test]
        public void FactoryMethod_ShouldCreateWorkingComponents()
        {
            // Arrange & Act - Factory method creates working instance
            var checkout = StrategyPattern.Create();

            // Assert - Component should be created and functional
            Assert.That(checkout, Is.Not.Null, "Factory should create checkout service");
            Assert.That(checkout, Is.InstanceOf<ICheckoutService>());
        }

        /// <summary>
        /// Tests the core Strategy pattern behavior with different algorithms.
        /// </summary>
        [Test]
        public void StrategyPattern_DemonstrationOfCorePattern()
        {
            // This test demonstrates the core Strategy pattern behavior:
            // Same context (payment processor) with different algorithms (payment strategies)
            
            // Test that we can process different payment types with consistent interface
            var results = new[]
            {
                _checkoutService.ProcessCreditCardPayment(100m, "1234567890123456"),
                _checkoutService.ProcessBankTransferPayment(100m, "1234567890")
            };

            // Assert - All use same interface but different implementations
            Assert.That(results.All(r => r.IsSuccessful), Is.True, "All strategies should work");
            Assert.That(results.All(r => r.ProcessedAmount == 100m), Is.True, "All should process same amount");
            
            // Each strategy should have unique transaction ID prefixes
            var prefixes = results.Select(r => r.TransactionId[..3]).ToArray();
            Assert.That(prefixes, Is.EquivalentTo(new[] { "CC_", "BT_" }));
        }
    }
}
