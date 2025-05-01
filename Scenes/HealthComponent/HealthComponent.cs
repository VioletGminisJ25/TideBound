using Godot;
using System;

[GlobalClass]
public partial class HealthComponent : Node
{
    [Export] public int Health { get; set; } = 100;

    [Signal] public delegate void HealthChangedEventHandler(int newHealth);
    [Signal] public delegate void ObjectDestroyedEventHandler();

    public void TakeDamage(int amount)
    {
        Health -= amount;
        EmitSignal(SignalName.HealthChanged, Health);
        GD.Print("HealthComponent: Taking damage : " + Health);

        if (Health <= 0)
        {
            EmitSignal(SignalName.ObjectDestroyed);
            // Opcionalmente, puedes añadir lógica para la destrucción aquí
        }
    }
}