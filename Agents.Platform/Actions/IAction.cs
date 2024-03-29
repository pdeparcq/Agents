﻿using Proto;

namespace Agents.Platform.Actions;

public interface IAction
{
    string Category { get; }
    string Name { get; }
    List<Parameter> Parameters { get; }
    Task Execute(IContext context, IDictionary<string, string> parameters);
}