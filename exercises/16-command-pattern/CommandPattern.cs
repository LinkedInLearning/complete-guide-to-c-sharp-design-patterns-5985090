/*
Command Pattern

Summary:
The Command Pattern encapsulates a request as an object, allowing you to parameterize clients with different requests, queue operations, and support undo functionality. It decouples the object that invokes the operation from the object that performs it.

Problem to Solve:
In a smart home remote control system, you need to control various devices (lights, fans, etc.) with different operations (on, off, brightness adjustment) while supporting undo functionality. The challenge is to decouple the remote control buttons from the specific device operations and maintain command history for undo operations.
*/

using System;

namespace DotNetDesignPatterns.Patterns.Behavioral
{
    /// <summary>
    /// Remote control interface that encapsulates device commands and supports undo operations.
    /// </summary>
    public interface IRemoteControl
    {
        void PressOnButton();
        void PressOffButton();
        void PressBrightnessButton(int brightness);
        void PressUndoButton();
    }

    /// <summary>
    /// Light device interface that represents the receiver in the command pattern.
    /// </summary>
    public interface ILight
    {
        bool IsOn { get; }
        int Brightness { get; }

        void SetBrightness(int brightness);
        void TurnOff();
        void TurnOn();
    }

    /// <summary>
    /// Factory for creating the command pattern with remote control and light device.
    /// </summary>
    public static class CommandPattern
    {
        public static (ILight light, IRemoteControl remote) Create()
        {
            throw new NotImplementedException();
        }
    }

    /*
    Requirements:

    To pass the tests, implement the following:

    1. Light class implementing ILight:
       - Track on/off state and brightness level
       - TurnOn/TurnOff methods change the IsOn state
       - SetBrightness method sets brightness and turns light on

    2. Command classes implementing a command interface:
       - OnCommand: turns light on and supports undo
       - OffCommand: turns light off and supports undo  
       - BrightnessCommand: sets brightness and supports undo

    3. RemoteControl class implementing IRemoteControl:
       - Encapsulates commands and executes them when buttons are pressed
       - Maintains command history for undo functionality
       - PressUndoButton undoes the last executed command

    4. Update Create method to:
       - Create a light instance
       - Create a remote control with command objects
       - Return both the light and remote control

    The tests will verify that:
    - Remote control buttons execute the correct device operations
    - Undo functionality reverses the last command executed
    - Multiple commands can be executed and undone in sequence
    - Brightness commands turn the light on and set the correct level
    */

}