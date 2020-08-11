using Prism.Events;
using ProgressBarManager.Events;
using ProgressBarManager.Events.Payloads;
using ShellProgressBar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProgressBarManager
{
    public class ShellProgressBarManager : IDisposable
    {
        IEventAggregator _ea;
        private NamedProgressBarOptions defaultOptions = new NamedProgressBarOptions
        {
            Name = "MainBarOptions",
            BackgroundColor = ConsoleColor.DarkGray,
            EnableTaskBarProgress = RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
        };

        private NamedProgressBarOptions defaultChildOptions1 = new NamedProgressBarOptions
        {
            Name = "defaultChildOptions1",
            ForegroundColor = ConsoleColor.Cyan,
            ForegroundColorDone = ConsoleColor.DarkGreen,
            ProgressCharacter = '─',
            BackgroundColor = ConsoleColor.DarkGray,
        };
        private NamedProgressBarOptions defaultChildOptions2 = new NamedProgressBarOptions
        {
            Name = "defaultChildOptions2",
            ForegroundColor = ConsoleColor.Yellow,
            ProgressCharacter = '─',
            BackgroundColor = ConsoleColor.DarkGray,
        };
        const string MainBarName = "main";
        ProgressBar progressBar { get; set; }
        Dictionary<string,Tuple<ChildProgressBar,string>> childProgressBars { get; set; }
        public ShellProgressBarManager(IEventAggregator ea, bool CollapseFinishedChildren = false)
        {
            _ea = ea;
            if (CollapseFinishedChildren)
            {
                defaultChildOptions1.CollapseWhenFinished = true;
                defaultChildOptions2.CollapseWhenFinished = true;
            }

            _ea.GetEvent<SpawnProgressBarEvent>().Subscribe(SpawnProgressBar);
            _ea.GetEvent<SpawnChildProgressBarEvent>().Subscribe(SpawnChildProgressBar);
            _ea.GetEvent<TickBarEvent>().Subscribe(TickBar);
            _ea.GetEvent<ProgressBarMessageEvent>().Subscribe(ProgressBarMessage);
            _ea.GetEvent<ProgressBarTotalTicksChangeEvent>().Subscribe(ProgressBarTotalTicksChange);
            childProgressBars = new Dictionary<string, Tuple<ChildProgressBar, string>>();

        }

        private void ProgressBarTotalTicksChange(ProgressBarTotalTicksPayload obj)
        {
            if (obj.Name == "main")
            {
                progressBar.MaxTicks = obj.TotalTicks;
            }
            else
            {
                if (childProgressBars is null)
                {
                    throw new NullReferenceException("No children spawned.");
                }
                try
                {
                    childProgressBars[obj.Name].Item1.MaxTicks = obj.TotalTicks;
                }
                catch (NullReferenceException)
                {
                    throw new NullReferenceException("No such progressbar.");
                }
            }
        }

        private void ProgressBarMessage(TickPayload obj)
        {
            if (obj.IsMain)
            {
                progressBar.Message = obj.Message;
            }
            else
            {
                if (childProgressBars is null)
                {
                    throw new NullReferenceException("No children spawned.");
                }
                try
                {
                    childProgressBars[obj.Name].Item1.Message = obj.Message;
                }
                catch (NullReferenceException)
                {
                    throw new NullReferenceException("No such progressbar.");
                }
            }
        }

        private void SpawnChildProgressBar(SpawnChildProgressBarPayload obj)
        {
            if(obj.ParentName == "main")
            {
                var child = progressBar.Spawn(obj.TotalTicks, obj.InitialMessage, defaultChildOptions1);
                childProgressBars.Add(obj.Name, new Tuple<ChildProgressBar, string>(child, defaultChildOptions1.Name));
            } else
            {
                try
                {
                    NamedProgressBarOptions options = childProgressBars[obj.ParentName].Item2 == defaultChildOptions1.Name ? defaultChildOptions2 : defaultChildOptions1;
                    var child = childProgressBars[obj.ParentName].Item1.Spawn(obj.TotalTicks, obj.InitialMessage, options);
                    childProgressBars.Add(obj.Name, new Tuple<ChildProgressBar, string>(child,options.Name));
                } catch (NullReferenceException)
                {
                    throw new NullReferenceException("Parentbar not found.");
                }
            }

        }

        

        private void TickBar(TickPayload obj)
        {
            if (obj.IsMain)
            {
                progressBar.Tick(obj.Message);
            }
            else
            {
                if(childProgressBars is null)
                {
                    throw new NullReferenceException("No children spawned.");
                }
                try
                {
                    childProgressBars[obj.Name].Item1.Tick(obj.Message);
                }
                catch (NullReferenceException)
                {
                    throw new NullReferenceException("No such progressbar.");
                }
            }
        }

        private void SpawnProgressBar(SpawnProgressBarPayload obj)
        {
            if(progressBar != null)
            {
                throw new Exception("Only one main progress bar allowed at any point in time. Invoke dispose to create a new one.");
            }
            progressBar = new ProgressBar(obj.TotalTicks, obj.InitialMessage, defaultOptions);

        }

        public void Dispose()
        {
            ((IDisposable)progressBar)?.Dispose();
        }
    }
}
