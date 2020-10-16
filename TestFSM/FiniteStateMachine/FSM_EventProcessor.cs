using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TestFSM.FiniteStateMachine {
    internal class FSM_EventProcessor {
        private readonly BlockingCollection<FSM_Event> eventBC;
        internal string caller;
        private readonly Task task;

        public FSM_EventProcessor(int size, string callerName) {
            this.caller = callerName;
            this.eventBC = new BlockingCollection<FSM_Event>(size);
            // initialise it and start the thread running..
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            this.task = Task.Run(() => {
                // Were we already canceled before we even got going ?
                token.ThrowIfCancellationRequested();
                bool first = true;
                while(!token.IsCancellationRequested) {  // maybe should be while true ?

                    if(first) {
                        Debug.WriteLine("EventProcessor loop started for " + callerName);
                        first = false;
                    }

                    FSM_Event nextEvent = this.eventBC.Take(); // blocking.  Will wait if there's nothing in the queue
                    FSM target = nextEvent.getDestFSM();
                    target.currentState = target.currentState.takeEvent(nextEvent);
                    // IF the source in nextEvent was a UI component, this call can notify it
                    target.notifyUIEventComplete(nextEvent);

                }

                // Poll on this property if we have to do
                // other cleanup before throwing the exception.  not sure ... 
                // perhaps we should process all the tasks that remain in the queue ?
                // and set the BlockingCollection to accet no more events
                if(token.IsCancellationRequested) {
                    // Clean up here, then...
                    token.ThrowIfCancellationRequested();
                }
            }, token);
            // how do we catch the cancellation exception if its thrown? 
        }

        internal void enQueue(FSM_Event evt) {
            this.eventBC.Add(evt);
        }
    }
}
