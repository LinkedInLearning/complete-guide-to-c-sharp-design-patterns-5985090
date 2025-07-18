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

    internal class RedState : ITrafficLightState
    {
        private readonly TrafficLight _trafficLight;

        public string Name => "Red";
        public int Duration => 30;

        public RedState(TrafficLight trafficLight)
        {
            _trafficLight = trafficLight;
        }

        public ITrafficLightState Next()
        {
            return _trafficLight.GetGreenState();
        }

        public string GetInstruction()
        {
            return "STOP";
        }
    }

    internal class GreenState : ITrafficLightState
    {
        private readonly TrafficLight _trafficLight;

        public string Name => "Green";
        public int Duration => 45;

        public GreenState(TrafficLight trafficLight)
        {
            _trafficLight = trafficLight;
        }

        public ITrafficLightState Next()
        {
            return _trafficLight.GetYellowState();
        }

        public string GetInstruction()
        {
            return "GO";
        }
    }

    internal class YellowState : ITrafficLightState
    {
        private readonly TrafficLight _trafficLight;

        public string Name => "Yellow";
        public int Duration => 5;

        public YellowState(TrafficLight trafficLight)
        {
            _trafficLight = trafficLight;
        }

        public ITrafficLightState Next()
        {
            return _trafficLight.GetRedState();
        }

        public string GetInstruction()
        {
            return "CAUTION";
        }
    }

    internal class TrafficLight : ITrafficLight
    {
        private ITrafficLightState _currentState;
        private readonly RedState _redState;
        private readonly GreenState _greenState;
        private readonly YellowState _yellowState;

        public string CurrentState => _currentState.Name;

        public TrafficLight()
        {
            _redState = new RedState(this);
            _greenState = new GreenState(this);
            _yellowState = new YellowState(this);

            _currentState = _redState;
        }

        public void Next()
        {
            _currentState = _currentState.Next();
        }

        public string GetStatus()
        {
            return $"Light: {CurrentState} - {_currentState.GetInstruction()} ({_currentState.Duration}s)";
        }

        internal RedState GetRedState() => _redState;
        internal GreenState GetGreenState() => _greenState;
        internal YellowState GetYellowState() => _yellowState;

    }

    public static class StatePattern
    {
        public static ITrafficLight Create()
        {
            return new TrafficLight();
        }
    }

    /// <summary>
    /// Factory for creating the state pattern with traffic light and its states.
    /// </summary>
    public static class StatePatternFactory
    {
        public static (ITrafficLight light, ITrafficLightState redState, ITrafficLightState greenState) Create()
        {
            var light = new TrafficLight();
            var redState = light.GetRedState();
            var greenState = light.GetGreenState();

            return (light, redState, greenState);
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
