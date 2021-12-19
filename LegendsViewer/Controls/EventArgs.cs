using System;

namespace LegendsViewer.Controls
{
    public class EventArgs<T> : EventArgs
    {
        public T Arg { get; private set; }

        public EventArgs(T arg)
        {
            Arg = arg;
        }
    }
}
