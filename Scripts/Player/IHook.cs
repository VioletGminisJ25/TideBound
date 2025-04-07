using Godot;
using System;
public interface IHook{
    public Rope Line{get;set;}
    public StaticBody2D Cursor {get;set;}
}