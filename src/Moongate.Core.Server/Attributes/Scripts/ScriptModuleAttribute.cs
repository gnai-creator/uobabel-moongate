namespace Moongate.Core.Server.Attributes.Scripts;

[AttributeUsage(AttributeTargets.Class)]
public class ScriptModuleAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
