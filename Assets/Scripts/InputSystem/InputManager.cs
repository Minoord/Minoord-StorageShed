using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private readonly static InputActions _inputAction = new();
    private readonly static Dictionary<Action<InputAction.CallbackContext>, InputAction> _subscribedStartedInputs = new();
    private readonly static Dictionary<Action<InputAction.CallbackContext>, InputAction> _subscribedPreformedInputs = new();
    private readonly static Dictionary<Action<InputAction.CallbackContext>, InputAction> _subscribedCanceledInputs = new();

    /// <summary>
    /// Enables or Disables Input based on isActive
    /// </summary>
    public static void ToggleAllInputs(bool isActive)
    {
        List<InputAction> actions = new();//_subscribedInputs.Values.ToList();

        foreach (InputAction action in actions)
        {
            if (isActive)
                action.Enable();
            else
                action.Disable();
        }
    }

    /// <summary>
    /// Tries to get an action based on the given name
    /// </summary>
    /// <param name="actionName"> The name of the action you want </param>
    /// <returns>Returns the input action you are looking for if it exists </returns>
    public static InputAction TryGetAction(string actionName) => _inputAction.FindAction(actionName);

    /// <summary>
    /// Enables the given action and add the function to the preformed
    /// </summary>
    /// <param name="inputAction"> The action you want to subscribe to</param>
    /// <param name="function"> The function you want to have subscribed</param>
    public static void SubscribeToAction(InputAction inputAction, InputActionPhase inputPhase, Action<InputAction.CallbackContext> function)
    {
        inputAction.Enable();
        inputAction.performed += function;

        _subscribedPreformedInputs.Add(function, inputAction);
    }

    /// <summary>
    /// Searches for the input based on the inputName, enables it and add the function to the preformed
    /// </summary>
    /// <param name="inputName">The name of the action you want</param>
    /// <param name="function"> The function you want to have subscribed</param>
    /// <param name="inputAction"> Gives back the action it subsribed to</param>
    public static void SubscribeToAction(string inputName, Action<InputAction.CallbackContext> function, out InputAction inputAction)
    {
        inputAction = TryGetAction(inputName);

        inputAction.Enable();
        inputAction.performed += function;

        _subscribedPreformedInputs.Add(function, inputAction);
    }

    public static void SubscribeToStartedAction(InputAction inputAction, InputActionPhase inputPhase, Action<InputAction.CallbackContext> function)
    {
        inputAction.Enable();
        inputAction.started += function;

        _subscribedStartedInputs.Add(function, inputAction);
    }

    public static void SubscribeToStartedAction(string inputName, Action<InputAction.CallbackContext> function, out InputAction inputAction)
    {
        inputAction = TryGetAction(inputName);

        inputAction.Enable();
        inputAction.started += function;

        _subscribedStartedInputs.Add(function, inputAction);
    }

    public static void SubscribeToCanceledAction(InputAction inputAction, InputActionPhase inputPhase, Action<InputAction.CallbackContext> function)
    {
        inputAction.Enable();
        inputAction.canceled += function;

        _subscribedCanceledInputs.Add(function, inputAction);
    }

    public static void SubscribeToCanceledAction(string inputName, Action<InputAction.CallbackContext> function, out InputAction inputAction)
    {
        inputAction = TryGetAction(inputName);

        inputAction.Enable();
        inputAction.canceled += function;

        _subscribedCanceledInputs.Add(function, inputAction);
    }

    /// <summary>
    /// Removes the function from the preformed of the given action
    /// </summary>
    /// <param name="inputAction"> The action you want to unsubscribe to</param>
    /// <param name="function"> The function you want to have unsubscribed</param>
    public static void UnsubscribeToAction(InputAction inputAction, InputActionPhase inputPhase, Action<InputAction.CallbackContext> function)
    {
        inputAction.performed -= function;

        _subscribedPreformedInputs.Remove(function, out inputAction);
    }

    /// <summary>
    /// Searches for the input based on the inputName and removes the function from the preformed
    /// </summary>
    /// <param name="inputName">The name of the action you want</param>
    /// <param name="function"> The function you want to have unsubscribed</param>
    public static void UnsubscribeToAction(string inputName, InputActionPhase inputPhase, Action<InputAction.CallbackContext> function)
    {
        InputAction inputAction = TryGetAction(inputName);
        inputAction.performed -= function;

        _subscribedPreformedInputs.Remove(function, out inputAction);
    }

    public static void UnsubscribeToStartedAction(InputAction inputAction, InputActionPhase inputPhase, Action<InputAction.CallbackContext> function)
    {
        inputAction.started -= function;

        _subscribedStartedInputs.Remove(function, out inputAction);
    }

    /// <summary>
    /// Searches for the input based on the inputName and removes the function from the preformed
    /// </summary>
    /// <param name="inputName">The name of the action you want</param>
    /// <param name="function"> The function you want to have unsubscribed</param>
    public static void UnsubscribeToStartedAction(string inputName, InputActionPhase inputPhase, Action<InputAction.CallbackContext> function)
    {
        InputAction inputAction = TryGetAction(inputName);
        inputAction.started -= function;

        _subscribedStartedInputs.Remove(function, out inputAction);
    }

    public static void UnsubscribeToCanceledAction(InputAction inputAction, InputActionPhase inputPhase, Action<InputAction.CallbackContext> function)
    {
        inputAction.canceled -= function;

        _subscribedCanceledInputs.Remove(function, out inputAction);
    }

    /// <summary>
    /// Searches for the input based on the inputName and removes the function from the preformed
    /// </summary>
    /// <param name="inputName">The name of the action you want</param>
    /// <param name="function"> The function you want to have unsubscribed</param>
    public static void UnsubscribeToCanceledAction(string inputName, InputActionPhase inputPhase, Action<InputAction.CallbackContext> function)
    {
        InputAction inputAction = TryGetAction(inputName);
        inputAction.canceled -= function;

        _subscribedCanceledInputs.Remove(function, out inputAction);
    }

    /// <summary>
    /// Unsubscribes each subscribed function from their actions
    /// </summary>
    public static void UnsubscribeToAllActions()
    {
        Dictionary<Action< InputAction.CallbackContext >,InputAction> allInputs = _subscribedStartedInputs;
        allInputs.AddRange(_subscribedPreformedInputs);
        allInputs.AddRange(_subscribedCanceledInputs);
        
        foreach (KeyValuePair<Action<InputAction.CallbackContext>, InputAction> input in allInputs)
            input.Value.performed -= input.Key;

        _subscribedStartedInputs.Clear();
        _subscribedPreformedInputs.Clear();
        _subscribedCanceledInputs.Clear();
    }

    public static void UnsubscribeToAllPreformedActions()
    {
        foreach (KeyValuePair<Action<InputAction.CallbackContext>, InputAction> input in _subscribedPreformedInputs)
            input.Value.performed -= input.Key;

        _subscribedPreformedInputs.Clear();
    }

    public static void UnsubscribeToAllStartedActions()
    {
        foreach (KeyValuePair<Action<InputAction.CallbackContext>, InputAction> input in _subscribedStartedInputs)
            input.Value.performed -= input.Key;

        _subscribedStartedInputs.Clear();
    }

    public static void UnsubscribeToAllCanceledActions()
    {
        foreach (KeyValuePair<Action<InputAction.CallbackContext>, InputAction> input in _subscribedCanceledInputs)
            input.Value.performed -= input.Key;

        _subscribedCanceledInputs.Clear();
    }
}
