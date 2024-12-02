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
				states[node.Name] = s;
				s.fsm = this;
				s.Ready();
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
    public virtual void PhysicsUpdate(float delta) {
		currentState.PhysicsUpdate(delta);
	}
	public virtual void HandleInput(InputEvent @event)
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
		states[key] = currentState;
		currentState.Enter();
	}
	
}
