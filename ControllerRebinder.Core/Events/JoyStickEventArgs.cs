using ControllerRebinder.Common.Enumerations;
using ControllerRebinder.Common.Moddels.Configurations.SubModelsOfConfigurations;
using ControllerRebinder.Core.Caches;
using ControllerRebinder.Core.Helpers;
using ControllerRebinder.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using WindowsInput;

namespace ControllerRebinder.Core.Events;
public class JoyStickEventArgs : EventArgs
{
    public int StickX { get; }
    public int StickY { get; }

    public JoyStickEventArgs(int stickX, int stickY)
    {
        StickX = stickX;
        StickY = stickY;
    }
}

public delegate void JoyStickEventHandler(object sender, JoyStickEventArgs e);


public class JoyStickHandler
{
    private readonly InputSimulator _inputSimulator;
    private readonly ILogger _logger;
    private double _staticYArea;
    private double _currentXArea;
    private Quadrant _currentQuadrant = Quadrant.TopLeft;

    private const int AreaMultiplier = 10_000_000;

    public JoyStickHandler(InputSimulator inputSimulator, ILogger logger)
    {
        _inputSimulator = inputSimulator;
        _logger = logger;
    }

    public void Subscribe(JoyStickService joyStickService)
    {
        joyStickService.JoyStickMoved += OnJoyStickMoved;
    }

    private async void OnJoyStickMoved(object sender, JoyStickEventArgs e)
    {
        var config = ConfigCache.Configurations.LeftJoyStick;
        double upDown = config.ForwardDown * AreaMultiplier;
        double leftRight = config.LeftRight * AreaMultiplier;
        var controlls = config.Controlls;
        var deadZone = config.DeadZone;
        var keyboard = _inputSimulator.Keyboard;

        Log(e.StickX, e.StickY);

        if (CircleHelper.IsInDeadZone(e.StickX, e.StickY, deadZone, _logger))
        {
            await controlls.ReleaseAll(_inputSimulator).ConfigureAwait(false);
        }
        else
        {
            CircleHelper.FindArea(
                config.ThreshHoldAreaCal,
                Math.Abs(e.StickX), Math.Abs(e.StickY),
                out double currentAngle,
                out _currentXArea);

            HandleStickMovement(upDown, leftRight, controlls, keyboard);
        }

        _currentQuadrant = QuadrantHelper.WhereAmI(e.StickX, e.StickY);
    }

    private void Log(int stickX, int stickY)
    {
        if (ConfigCache.Configurations.Log)
        {
            ConsoleHelper.ClearConsole();
            _logger.LogInformation("Version 3.0\n");
            _logger.LogInformation($"X (left-right): {stickX} : Y (up-down): {stickY}\n");
            _logger.LogInformation($"Static: {_staticYArea} : X: {_currentXArea}\n");
        }
    }

    private void HandleStickMovement(double upDown, double leftRight, Controlls controlls, IKeyboardSimulator keyboard)
    {
        bool isMoving = false;

        if (_currentXArea > leftRight && _currentXArea < upDown)
        {
            switch (_currentQuadrant)
            {
                case Quadrant.TopLeft:
                    keyboard.KeyDown(controlls.Up);
                    keyboard.KeyDown(controlls.Left);
                    isMoving = true;
                    break;
                case Quadrant.TopRight:
                    keyboard.KeyDown(controlls.Up);
                    keyboard.KeyDown(controlls.Right);
                    isMoving = true;
                    break;
                case Quadrant.BottomLeft:
                    keyboard.KeyDown(controlls.Down);
                    keyboard.KeyDown(controlls.Left);
                    isMoving = true;
                    break;
                case Quadrant.BottomRight:
                    keyboard.KeyDown(controlls.Down);
                    keyboard.KeyDown(controlls.Right);
                    isMoving = true;
                    break;
            }
        }

        if (!isMoving)
        {
            keyboard.KeyUp(controlls.Up);
            keyboard.KeyUp(controlls.Left);
            keyboard.KeyUp(controlls.Down);
            keyboard.KeyUp(controlls.Right);
        }

        if ((_currentQuadrant == Quadrant.TopLeft || _currentQuadrant == Quadrant.TopRight) && _currentXArea > upDown)
        {
            keyboard.KeyDown(controlls.Up);
        }
        if ((_currentQuadrant == Quadrant.TopLeft || _currentQuadrant == Quadrant.BottomLeft) && _currentXArea < leftRight)
        {
            keyboard.KeyDown(controlls.Left);
        }
        if ((_currentQuadrant == Quadrant.BottomLeft || _currentQuadrant == Quadrant.BottomRight) && _currentXArea > upDown)
        {
            keyboard.KeyDown(controlls.Down);
        }
        if ((_currentQuadrant == Quadrant.BottomRight || _currentQuadrant == Quadrant.TopRight) && _currentXArea < leftRight)
        {
            keyboard.KeyDown(controlls.Right);
        }
    }
}


