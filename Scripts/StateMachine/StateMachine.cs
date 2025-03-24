using Godot;
using System;
using System.Collections.Generic;

public partial class StateMachine : Node
{

	[Export] public NodePath initialState;
	private Dictionary<string, State> states;
	private State currentState;
	public override void _Ready()
	{
		states = new Dictionary<string, State>();
		foreach (Node node in this.GetChildren())
		{
			if(node is State s)
			{
				states.Add(node.Name, s);
				s.fsm = this;
				s.Enter();
				s.Exit(); //reset
			}
		}

		currentState = GetNode<State>(initialState);
		currentState.Enter();
	}

	
	public override void _Process(double delta)
	{
		currentState.Update((float)delta);
	}

	public override void _PhysicsProcess(double delta) {
		currentState.PhysicsUpdate((float)delta);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		currentState.HandleInput(@event);
	}
	
	public void TransitionTo(string key)
	{
		if(!states.ContainsKey(key) || currentState == states[key])
		{
			return;
		}
		currentState.Exit();
		currentState = states[key];
		currentState.Enter();
	}
	
}
