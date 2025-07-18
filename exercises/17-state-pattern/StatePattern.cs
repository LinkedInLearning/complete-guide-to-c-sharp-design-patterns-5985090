/*
State Pattern

Summary:
The State Pattern allows an object to alter its behavior when its internal state changes. The object appears to change its class by delegating behavior to state objects that represent different states and their associated behaviors.

Problem to Solve:
In a traffic light system, the light needs to cycle through different states (Red, Green, Yellow) with each state having different timing, instructions, and transition rules. The challenge is to manage state transitions and behaviors without complex conditional logic while keeping the system maintainable and extensible.
*/

using System;

namespace DotNetDesignPatterns.Patterns.Behavioral
{
    /// <summary>
    /// Context interface for the traffic light that delegates behavior to its current state.
    /// </summary>
    public interface ITrafficLight
    {
        string CurrentState { get; }
        void Next();
        string GetStatus();
    }

    /// <summary>
    /// State interface that encapsulates state-specific behavior and knows how to transition to the next state.
    /// </summary>
    public interface ITrafficLightState
    {
        string Name { get; }
        int Duration { get; }
        ITrafficLightState Next();
        string GetInstruction();
    }

    /// <summary>
    /// Factory for creating the state pattern with traffic light and its states.
    /// </summary>
    public static class StatePatternFactory
    {
        public static (ITrafficLight light, ITrafficLightState redState, ITrafficLightState greenState) Create()
        {
            throw new NotImplementedException("Implement the state pattern for traffic light");
        }
    }

    /*
    Requirements:

    To pass the tests, implement the following:

    1. TrafficLight class implementing ITrafficLight:
       - Maintains reference to current state
       - Delegates Next() calls to current state and updates current state
       - GetStatus() returns formatted string with state name, duration, and instruction
       - CurrentState returns the name of the current state

    2. State classes implementing ITrafficLightState:
       - RedState: Name="Red", Duration=30, GetInstruction()="STOP", Next()=GreenState
       - GreenState: Name="Green", Duration=45, GetInstruction()="GO", Next()=YellowState  
       - YellowState: Name="Yellow", Duration=5, GetInstruction()="CAUTION", Next()=RedState

    3. State transition cycle:
       - Red → Green → Yellow → Red (repeating cycle)
       - Each state knows its next state in the sequence
       - Traffic light starts in Red state

    4. Update Create method to:
       - Create instances of all three state classes
       - Wire up the state transitions between them
       - Create traffic light starting in Red state
       - Return traffic light and state instances

    The tests will verify that:
    - Traffic light starts in Red state
    - Next() transitions follow the correct sequence
    - Each state has correct name, duration, and instruction
    - GetStatus() returns properly formatted state information
    - State transitions cycle correctly through all states
    */
}
