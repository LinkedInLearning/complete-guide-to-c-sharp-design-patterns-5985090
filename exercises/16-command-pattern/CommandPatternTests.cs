using NUnit.Framework;
using DotNetDesignPatterns.Patterns.Behavioral;

namespace DotNetDesignPatterns.Tests.Patterns.Behavioral
{
    /// <summary>
    /// Tests for the Command Pattern implementation.
    /// 
    /// The Command pattern encapsulates requests as objects, allowing parameterization
    /// of clients, queuing operations, and supporting undo functionality. It decouples
    /// the invoker from the receiver and enables flexible request handling.
    /// 
    /// Required implementation components:
    /// 1. Command interface (ICommand) with Execute() and Undo() methods
    /// 2. Concrete commands (OnCommand, OffCommand, BrightnessCommand) for specific operations
    /// 3. Receiver class (ILight) that performs the actual work
    /// 4. Invoker class (IRemoteControl) that stores and executes commands
    /// 5. Factory method to create the remote control system
    /// 
    /// This implementation demonstrates a remote control system for smart home devices
    /// with full undo capability for all operations.
    /// </summary>
    [TestFixture]
    public class CommandPatternTests
    {
        /// <summary>
        /// Remote control interface that executes commands.
        /// </summary>
        private IRemoteControl _remote = null!;
        
        /// <summary>
        /// Light device that receives and executes commands.
        /// </summary>
        private ILight _light = null!;

        /// <summary>
        /// Setup method that creates the command pattern system.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Use the tuple-returning factory so the test controls both references
            // This creates the entire command system: remote control, commands, and light
            (_light, _remote) = CommandPattern.Create();
        }

        /// <summary>
        /// Tests that the ON button command works correctly.
        /// </summary>
        [Test]
        public void RemoteControl_OnButton_ShouldTurnLightOn()
        {
            // Act - Press the ON button on the remote
            _remote.PressOnButton();
            
            // Assert - Verify the light is now on
            Assert.That(_light.IsOn, Is.True, "Light should be turned on");
        }

        /// <summary>
        /// Tests that the OFF button command works correctly.
        /// </summary>
        [Test]
        public void RemoteControl_OffButton_ShouldTurnLightOff()
        {
            // Arrange - Ensure light is initially on
            _light.TurnOn();
            
            // Act - Press the OFF button on the remote
            _remote.PressOffButton();
            
            // Assert - Verify the light is now off
            Assert.That(_light.IsOn, Is.False, "Light should be turned off");
        }

        /// <summary>
        /// Tests that the brightness command works correctly with parameters.
        /// </summary>
        [Test]
        public void RemoteControl_BrightnessButton_ShouldSetBrightness()
        {
            // Act - Set brightness to 42 via remote
            _remote.PressBrightnessButton(42);
            
            // Assert - Verify brightness is set and light is on
            Assert.That(_light.Brightness, Is.EqualTo(42));
            Assert.That(_light.IsOn, Is.True);
        }

        /// <summary>
        /// Tests the undo functionality for brightness commands.
        /// </summary>
        [Test]
        public void RemoteControl_BrightnessButton_Undo_ShouldRestorePreviousBrightness()
        {
            // Arrange & Act - Execute two brightness commands
            _remote.PressBrightnessButton(80);  // First command
            _remote.PressBrightnessButton(20);  // Second command (this will be undone)
            
            // Act - Undo the last command
            _remote.PressUndoButton();
            
            // Assert - Should restore brightness to the value before the last command
            Assert.That(_light.Brightness, Is.EqualTo(80));
        }

        /// <summary>
        /// Tests undo functionality for the ON command.
        /// </summary>
        [Test]
        public void RemoteControl_OnCommand_Undo_ShouldTurnLightOff()
        {
            // Initially light is off
            Assert.That(_light.IsOn, Is.False, "Light should initially be off");

            // Turn light on
            _remote.PressOnButton();
            Assert.That(_light.IsOn, Is.True, "Light should be on after pressing ON button");

            // Undo the ON command should turn light off
            _remote.PressUndoButton();
            Assert.That(_light.IsOn, Is.False, "Light should be off after undoing ON command");
        }

        /// <summary>
        /// Tests undo functionality for the OFF command.
        /// 
        /// This verifies:
        /// - Undo restores the light to its previous state (on)
        /// - Commands remember their context for proper undo behavior
        /// - OffCommand Undo() method calls light.TurnOn()
        /// - State preservation for correct undo operations
        /// </summary>
        [Test]
        public void RemoteControl_OffCommand_Undo_ShouldTurnLightOn()
        {
            // Turn light on first
            _remote.PressOnButton();
            Assert.That(_light.IsOn, Is.True, "Light should be on initially");

            // Turn light off
            _remote.PressOffButton();
            Assert.That(_light.IsOn, Is.False, "Light should be off after pressing OFF button");

            // Undo the OFF command should turn light on
            _remote.PressUndoButton();
            Assert.That(_light.IsOn, Is.True, "Light should be on after undoing OFF command");
        }

        /// <summary>
        /// Tests that undo works correctly with multiple commands in sequence.
        /// 
        /// This demonstrates:
        /// - Command history management with multiple operations
        /// - LIFO (Last In, First Out) undo behavior
        /// - Only the most recent command should be undone
        /// 
        /// Required implementation:
        /// - Command stack or history list in RemoteControl
        /// - Proper tracking of command execution order
        /// - Undo only affects the last executed command
        /// </summary>
        [Test]
        public void RemoteControl_MultipleCommands_Undo_ShouldUndoLastCommand()
        {
            // Execute multiple commands in sequence
            _remote.PressOnButton();           // 1. Turn on
            _remote.PressBrightnessButton(50); // 2. Set brightness to 50
            _remote.PressOffButton();          // 3. Turn off (this should be undone)

            // Verify final state after all commands
            Assert.That(_light.IsOn, Is.False, "Light should be off");
            Assert.That(_light.Brightness, Is.EqualTo(50), "Brightness should be 50");

            // Undo should reverse only the last command (OFF)
            _remote.PressUndoButton();
            Assert.That(_light.IsOn, Is.True, "Light should be on after undoing OFF command");
            Assert.That(_light.Brightness, Is.EqualTo(50), "Brightness should remain 50");
        }

        /// <summary>
        /// Tests undo behavior when light starts in OFF state.
        /// 
        /// This verifies:
        /// - Initial state handling in command undo operations
        /// - Proper state restoration to the original condition
        /// </summary>
        [Test]
        public void RemoteControl_OnFromOffState_Undo_ShouldReturnToOffState()
        {
            // Ensure light starts off
            Assert.That(_light.IsOn, Is.False, "Light should initially be off");

            // Turn on and verify
            _remote.PressOnButton();
            Assert.That(_light.IsOn, Is.True, "Light should be on after pressing ON button");

            // Undo should return to original off state
            _remote.PressUndoButton();
            Assert.That(_light.IsOn, Is.False, "Light should return to off state after undo");
        }

        /// <summary>
        /// Tests undo behavior when light starts in ON state.
        /// 
        /// This verifies:
        /// - Commands can handle different initial states
        /// - Undo properly restores the pre-command state
        /// </summary>
        [Test]
        public void RemoteControl_OffFromOnState_Undo_ShouldReturnToOnState()
        {
            // Turn light on first
            _light.TurnOn();
            Assert.That(_light.IsOn, Is.True, "Light should be on initially");

            // Turn off via remote
            _remote.PressOffButton();
            Assert.That(_light.IsOn, Is.False, "Light should be off after pressing OFF button");

            // Undo should return to original on state
            _remote.PressUndoButton();
            Assert.That(_light.IsOn, Is.True, "Light should return to on state after undo");
        }

        /// <summary>
        /// Tests a complex sequence of commands with undo.
        /// 
        /// This demonstrates:
        /// - Multiple different command types in sequence
        /// - Brightness command undo restoring previous brightness (default value)
        /// - Integration of all command types working together
        /// 
        /// Note:
        /// - Default brightness behavior when undoing brightness commands
        /// - How different command types interact in the system
        /// </summary>
        [Test]
        public void RemoteControl_On_Off_Brightness_Undo()
        {
            // Execute a sequence of different command types
            _remote.PressOnButton();
            Assert.That(_light.IsOn, Is.True, "Light should be on after pressing ON button");
            
            _remote.PressOffButton();
            Assert.That(_light.IsOn, Is.False, "Light should be off after pressing OFF button");
            
            _remote.PressBrightnessButton(55);
            Assert.That(_light.Brightness, Is.EqualTo(55));
            
            // Undo the brightness command - should restore default brightness
            _remote.PressUndoButton();
            Assert.That(_light.Brightness, Is.EqualTo(100)); // default brightness value
        }
    }
}
