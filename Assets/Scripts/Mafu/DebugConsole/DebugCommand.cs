using System;
using UnityEngine;

namespace Mafu.DebugConsole
{
    public class DebugCommandBase
    {
        public string CommandID { get; private set; }
        public string CommandDescription { get; private set; }
        public string CommandFormat { get; private set; }

        public DebugCommandBase(string id, string description, string format)
        {
            CommandID = id;
            CommandDescription = description;
            CommandFormat = format;
        }
    }

    public class DebugCommand : DebugCommandBase
    {
        private Action command;

        public DebugCommand(string id, string description, string format, Action command) : base(id, description, format)
        {
            this.command = command;
        }

        public void Invoke() => command.Invoke();
    }

    public class DebugCommand<T> : DebugCommandBase
    {
        public string DefaultParameter;
        private Action<T> command;

        public DebugCommand(string id, string description, string format, string defaultParameter, Action<T> command) : base(id, description, format)
        {
            DefaultParameter = defaultParameter;
            this.command = command;
        }

        public void Invoke(T value) => command.Invoke(value);
    }

    public interface IDebugCommand
    {
        public string ID { get; }
        public string Description { get; }
        public string Format { get; }

        public void Invoke();
    }

    public interface IDebugCommand<T>
    {
        public string ID { get; }
        public string Description { get; }
        public string Format { get; }
        public string DefaultParameter { get; }

        public void Invoke(T value);
    }

    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class DebugCommandAttribute : System.Attribute {}
}