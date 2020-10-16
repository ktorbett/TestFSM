using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TestFSM.FiniteStateMachine {
    
    /// <summary>
    /// This class uses an internal BlockingCollection ( which in turn uses a ConcurrentQueue )
    /// to maintain a queue of <ref>FSM_Event</ref>s.  Its constructor also starts a Task using 
    /// the BlockingCollection to monitor the queue of FSM_Events and execute them against the right 
    /// FSM in a FIFO manner.
    /// </summary>
    internal class FSM_EventProcessor {
        private readonly BlockingCollection<FSM_Event> eventBC;
        internal string caller;
        private readonly Task task;

        /// <summary>
        /// This constructor allows the FSM_EventProcessor to set the size of the internal 
        /// BlockingCollection and also to know ( for debug purposes only at present ) whether 
        /// it 'belongs' to an STT or an FSM. 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="callerName"></param>
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
                        Debug.WriteLine("EventProcessor task started for " + callerName);
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
                // and set the BlockingCollection to accept no more events
                if(token.IsCancellationRequested) {
                    // Clean up here, then...
                    token.ThrowIfCancellationRequested();
                }
            }, token);
            // TODO how do we catch the cancellation exception if its thrown? 
            // and how do we stop the process ... 
        }

        /// <summary>
        /// Puts an FSM_Event on the internal Queue of the BlockingCollection inside the Event Processor.
        /// It is a blocking operation and if the collection is full ( see the <ref>size</ref> parameter 
        /// of the constructor ) the thread will block.
        /// </summary>
        /// <param name="evt"></param>
        internal void enQueue(FSM_Event evt) {
            this.eventBC.Add(evt);
        }
    }
}
