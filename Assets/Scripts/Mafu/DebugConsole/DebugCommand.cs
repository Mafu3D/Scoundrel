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

    public class DebugCommand<T1, T2> : DebugCommandBase
    {
        public string DefaultParameter;
        private Action<T1, T2> command;

        public DebugCommand(string id, string description, string format, string defaultParameter,Action<T1, T2> command) : base(id, description, format)
        {
            DefaultParameter = defaultParameter;
            this.command = command;
        }

        public void Invoke(T1 value1, T2 value2) => command.Invoke(value1, value2);
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

    public interface IDebugCommand<T1, T2>
    {
        public string ID { get; }
        public string Description { get; }
        public string Format { get; }
        public string DefaultParameter1 { get; }
        public string DefaultParameter2 { get; }

        public void Invoke(T1 value1, T2 value2);
    }

    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class DebugCommandAttribute : System.Attribute {}
}