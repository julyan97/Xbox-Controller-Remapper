using ControllerRebinder.Common.Enumerations;
using ControllerRebinder.Common.Moddels.Configurations.SubModelsOfConfigurations;
using ControllerRebinder.Core.Caches;
using ControllerRebinder.Core.Events.Versions.v03;
using ControllerRebinder.Core.Helpers;
using ControllerRebinder.Core.Services.Imp;
using DXNET.XInput;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;

namespace ControllerRebinder.Core.Services.v03;

public class JoyStickService_v03 : IJoyStickService
{
    private readonly Controller _controller;
    private readonly InputSimulator _inputSimulator;
    private readonly JoyStick _joyStick;
    private readonly ILogger _logger;
    private Quadrant _currentQuadrant = Quadrant.TopLeft;
    private double _staticYArea;
    private double _currentXArea;

    private Timer _timer;

    private const int ThresholdMultiplier = 100;
    private const int AreaMultiplier = 10_000_000;

    public event JoyStickEventHandler_v03 JoyStickMoved;

    public JoyStickService_v03(
        Controller controller,
        InputSimulator inputSimulator,
        JoyStick joyStick,
        ILogger logger)
    {
        _controller = controller;
        _inputSimulator = inputSimulator;
        _joyStick = joyStick;
        _logger = logger;
    }

    public async Task Start()
    {
        var config = ConfigCache.Configurations.LeftJoyStick;

        CircleHelper.FindArea(
            config.ThreshHoldAreaCal,
            config.MaxValController,
            config.MaxValController,
            out double staticYAngle,
            out _staticYArea);

        _timer = new Timer(CheckJoystickState, null, 0, ConfigCache.Configurations.RefreshRate);

    }

    private void CheckJoystickState(object state)
    {
        HandleException(() =>
        {
            var controllerState = _controller.GetState();
            short stickX = 0;
            short stickY = 0;

            ChooseJoyStick(controllerState, ref stickX, ref stickY);
            OnJoyStickMoved(stickX, stickY);
        });
    }

    private void ChooseJoyStick(State state, ref short stickX, ref short stickY)
    {
        switch (_joyStick)
        {
            case JoyStick.Left:
                stickX = state.Gamepad.LeftThumbX;
                stickY = state.Gamepad.LeftThumbY;
                break;
            case JoyStick.Right:
                stickX = state.Gamepad.RightThumbX;
                stickY = state.Gamepad.RightThumbY;
                break;
        }
    }

    protected virtual void OnJoyStickMoved(int stickX, int stickY)
    {
        JoyStickMoved?.Invoke(this, new JoyStickEventArgs_v03(stickX, stickY));
    }

    public void HandleException(Action action)
    {
        try
        {
            action();
        }
        catch { }
    }
}