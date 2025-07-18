using System;
using NUnit.Framework;
using DotNetDesignPatterns.Patterns.Behavioral;

namespace DotNetDesignPatterns.Tests.Patterns.Behavioral
{
    /// <summary>
    /// Tests for the State Pattern implementation.
    /// 
    /// The State pattern allows an object to alter its behavior when its internal
    /// state changes, appearing as if the object changed its class. It encapsulates
    /// state-specific behavior in separate state classes and promotes cleaner state management.
    /// 
    /// Required implementation components:
    /// 1. State interface (ITrafficLightState) that defines state-specific behavior
    /// 2. Concrete states (RedState, GreenState, YellowState) that implement specific behaviors
    /// 3. Context class (ITrafficLight) that maintains current state and delegates behavior
    /// 4. State transition logic that determines the next state in the sequence
    /// 5. Factory method to create the traffic light system with initial state
    /// 
    /// This implementation demonstrates a traffic light system where each state
    /// handles its own behavior and transition logic.
    /// </summary>
    [TestFixture]
    public class StatePatternTests
    {
        /// <summary>
        /// Context object that maintains current state and delegates behavior.
        /// </summary>
        private ITrafficLight _trafficLight = null!;
        
        /// <summary>
        /// Red state instance for testing state-specific behavior.
        /// </summary>
        private ITrafficLightState _redState = null!;
        
        /// <summary>
        /// Green state instance for testing state-specific behavior.
        /// </summary>
        private ITrafficLightState _greenState = null!;
        
        /// <summary>
        /// Yellow state instance for testing state-specific behavior.
        /// </summary>
        private ITrafficLightState _yellowState = null!;

        /// <summary>
        /// Setup method that creates the state pattern system.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var result = StatePatternFactory.Create();
            _trafficLight = result.light;
            _redState = result.redState;
            _greenState = result.greenState;
            _yellowState = _greenState.Next();
        }

        /// <summary>
        /// Tests that the traffic light starts in the red state.
        /// </summary>
        [Test]
        public void TrafficLight_InitialState_ShouldBeRed()
        {
            // Assert
            Assert.That(_trafficLight.CurrentState, Is.EqualTo(_redState.Name));
        }

        /// <summary>
        /// Tests that the traffic light status includes state-specific information.
        /// </summary>
        [Test]
        public void TrafficLight_InitialStatus_ShouldShowRedState()
        {
            // Act
            var status = _trafficLight.GetStatus();

            // Assert
            Assert.That(status, Does.Contain(_redState.Name));
            Assert.That(status, Does.Contain(_redState.GetInstruction()));
            Assert.That(status, Does.Contain(_redState.Duration.ToString()));
        }

        /// <summary>
        /// Tests the first state transition from red to green.
        /// </summary>
        [Test]
        public void TrafficLight_FromRed_NextShouldBeGreen()
        {
            // Act
            _trafficLight.Next();

            // Assert
            Assert.That(_trafficLight.CurrentState, Is.EqualTo(_greenState.Name));
        }

        /// <summary>
        /// Tests the second state transition from green to yellow.
        /// </summary>
        [Test]
        public void TrafficLight_FromGreen_NextShouldBeYellow()
        {
            // Arrange
            _trafficLight.Next(); // Red -> Green

            // Act
            _trafficLight.Next(); // Green -> Yellow

            // Assert
            Assert.That(_trafficLight.CurrentState, Is.EqualTo(_yellowState.Name));
        }

        /// <summary>
        /// Tests the third state transition from yellow back to red.
        /// </summary>
        [Test]
        public void TrafficLight_FromYellow_NextShouldBeRed()
        {
            // Arrange
            _trafficLight.Next(); // Red -> Green
            _trafficLight.Next(); // Green -> Yellow

            // Act
            _trafficLight.Next(); // Yellow -> Red

            // Assert
            Assert.That(_trafficLight.CurrentState, Is.EqualTo(_redState.Name));
        }

        /// <summary>
        /// Tests a complete state cycle returns to the initial state.
        /// 
        /// This demonstrates:
        /// - Full state cycle functionality
        /// - State pattern maintains proper circular transitions
        /// - System returns to initial state after complete cycle
        /// 
        /// Required implementation:
        /// - Complete transition chain: Red -> Green -> Yellow -> Red
        /// - Consistent state behavior across full cycles
        /// </summary>
        [Test]
        public void TrafficLight_FullCycle_ShouldReturnToRed()
        {
            // Act - Complete full cycle
            _trafficLight.Next(); // Red -> Green
            _trafficLight.Next(); // Green -> Yellow
            _trafficLight.Next(); // Yellow -> Red

            // Assert
            Assert.That(_trafficLight.CurrentState, Is.EqualTo(_redState.Name));
        }

        /// <summary>
        /// Tests that green state properties are correctly displayed.
        /// 
        /// This verifies:
        /// - Context correctly delegates to current state for status information
        /// - GreenState provides proper name, instruction, and duration
        /// - State-specific behavior is properly encapsulated
        /// 
        /// Required implementation:
        /// - GreenState with appropriate Name, Duration, and GetInstruction properties
        /// - Context GetStatus method that queries current state
        /// </summary>
        [Test]
        public void TrafficLight_GreenState_ShouldHaveCorrectProperties()
        {
            // Arrange
            _trafficLight.Next(); // Red -> Green

            // Act
            var status = _trafficLight.GetStatus();

            // Assert
            Assert.That(status, Does.Contain(_greenState.Name));
            Assert.That(status, Does.Contain(_greenState.GetInstruction()));
            Assert.That(status, Does.Contain(_greenState.Duration.ToString()));
        }

        /// <summary>
        /// Tests that yellow state properties are correctly displayed.
        /// 
        /// This verifies:
        /// - YellowState has proper state-specific properties
        /// - Context maintains consistency when transitioning to yellow
        /// - All state types follow the same interface contract
        /// 
        /// Required implementation:
        /// - YellowState with Name, Duration, and GetInstruction properties
        /// - Consistent behavior across all state implementations
        /// </summary>
        [Test]
        public void TrafficLight_YellowState_ShouldHaveCorrectProperties()
        {
            // Arrange
            _trafficLight.Next(); // Red -> Green
            _trafficLight.Next(); // Green -> Yellow

            // Act
            var status = _trafficLight.GetStatus();

            // Assert
            Assert.That(status, Does.Contain(_yellowState.Name));
            Assert.That(status, Does.Contain(_yellowState.GetInstruction()));
            Assert.That(status, Does.Contain(_yellowState.Duration.ToString()));
        }

        /// <summary>
        /// Tests that red state has the required properties.
        /// 
        /// This verifies:
        /// - RedState implements the ITrafficLightState interface correctly
        /// - All required properties are properly initialized
        /// - State provides meaningful name, duration, and instruction
        /// 
        /// Required implementation:
        /// - RedState class with Name property (non-empty string)
        /// - Duration property (positive integer representing time in seconds)
        /// - GetInstruction method that returns driver instruction
        /// </summary>
        [Test]
        public void RedState_ShouldHaveCorrectProperties()
        {
            // Assert
            Assert.That(_redState.Name, Is.Not.Null.And.Not.Empty);
            Assert.That(_redState.Duration, Is.GreaterThan(0));
            Assert.That(_redState.GetInstruction(), Is.Not.Null.And.Not.Empty);
        }

        /// <summary>
        /// Tests that green state has the required properties.
        /// 
        /// This verifies:
        /// - GreenState implements the ITrafficLightState interface correctly
        /// - State provides consistent interface implementation with RedState
        /// - All state types follow the same property contract
        /// 
        /// Required implementation:
        /// - GreenState class with Name property (non-empty string)
        /// - Duration property (positive integer)
        /// - GetInstruction method with appropriate green light instruction
        /// </summary>
        [Test]
        public void GreenState_ShouldHaveCorrectProperties()
        {
            // Assert
            Assert.That(_greenState.Name, Is.Not.Null.And.Not.Empty);
            Assert.That(_greenState.Duration, Is.GreaterThan(0));
            Assert.That(_greenState.GetInstruction(), Is.Not.Null.And.Not.Empty);
        }

        /// <summary>
        /// Tests that yellow state has the required properties.
        /// 
        /// This verifies:
        /// - YellowState implements the ITrafficLightState interface correctly
        /// - State provides consistent interface implementation with other states
        /// - All state types follow the same property contract
        /// 
        /// Required implementation:
        /// - YellowState class with Name property (non-empty string)
        /// - Duration property (positive integer)
        /// - GetInstruction method with appropriate yellow light instruction
        /// </summary>
        [Test]
        public void YellowState_ShouldHaveCorrectProperties()
        {
            // Assert
            Assert.That(_yellowState.Name, Is.Not.Null.And.Not.Empty);
            Assert.That(_yellowState.Duration, Is.GreaterThan(0));
            Assert.That(_yellowState.GetInstruction(), Is.Not.Null.And.Not.Empty);
        }

        /// <summary>
        /// Tests that the factory creates all required components correctly.
        /// 
        /// This verifies:
        /// - Factory method returns all necessary system components
        /// - All components implement their respective interfaces
        /// - Proper object creation and wiring
        /// 
        /// Required implementation:
        /// - StatePatternFactory.Create() method that returns tuple with light and states
        /// - TrafficLight implementation of ITrafficLight
        /// - RedState and GreenState implementations of ITrafficLightState
        /// - Proper initialization and wiring of all components
        /// </summary>
        [Test]
        public void Factory_Create_ShouldReturnValidComponents()
        {
            // Act
            var result = StatePatternFactory.Create();

            // Assert
            Assert.That(result.light, Is.Not.Null);
            Assert.That(result.redState, Is.Not.Null);
            Assert.That(result.greenState, Is.Not.Null);
            Assert.That(result.light, Is.InstanceOf<ITrafficLight>());
            Assert.That(result.redState, Is.InstanceOf<ITrafficLightState>());
            Assert.That(result.greenState, Is.InstanceOf<ITrafficLightState>());
        }

        /// <summary>
        /// Tests that state transitions remain consistent across multiple cycles.
        /// 
        /// This demonstrates:
        /// - State pattern maintains consistency over time
        /// - Multiple cycles produce the same transition sequence
        /// - No state corruption or memory issues across cycles
        /// - Robust state management implementation
        /// 
        /// Required implementation:
        /// - Stateless transition logic (each state always transitions to the same next state)
        /// - Proper state management without side effects
        /// - Consistent behavior across multiple operations
        /// </summary>
        [Test]
        public void TrafficLight_StateTransitions_ShouldBeConsistent()
        {
            // Test multiple cycles to verify consistency
            for (int cycle = 0; cycle < 3; cycle++)
            {
                // Should start at Red (or return to Red after each cycle)
                Assert.That(_trafficLight.CurrentState, Is.EqualTo(_redState.Name));
                
                // Red -> Green
                _trafficLight.Next();
                Assert.That(_trafficLight.CurrentState, Is.EqualTo(_greenState.Name));
                
                // Green -> Yellow
                _trafficLight.Next();
                Assert.That(_trafficLight.CurrentState, Is.EqualTo(_yellowState.Name));
                
                // Yellow -> Red (completes the cycle)
                _trafficLight.Next();
                Assert.That(_trafficLight.CurrentState, Is.EqualTo(_redState.Name));
            }
        }
    }
}

