using Godot;
using System;

[Tool]
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

        if (Health <= 0)
        {
            EmitSignal(SignalName.ObjectDestroyed);
            // Opcionalmente, puedes añadir lógica para la destrucción aquí
        }
    }

    public override void _EnterTree()
    {
        if (Engine.IsEditorHint())
        {
            Node parent = GetParent();
            if (parent == null)
            {
                GD.PushWarning("Parent node not found in editor.");
                return;
            }

            if (parent.GetNodeOrNull<HealthComponent>("HealthComponent") == null)
            {
                GD.PushWarning("HealthComponent not found in parent node in editor.");
            }
            return;
        }
    }


    public int GetHealth()
    {
        return Health;
    }

    public void SetHealth(int newHealth)
    {
        Health = newHealth;
        EmitSignal(SignalName.HealthChanged, Health);
    }
}