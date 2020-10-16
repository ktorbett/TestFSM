namespace TestFSM.FiniteStateMachine {
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// FSM_Event models an event in the run-time of your application, that can be sent
    /// as a notification to an object with Finite State Machine ( aka FSM ) behaviour.
    /// For example, a timer in your app might create an event named 'timerComplete'.  To send this
    /// event to an FSM you will need to create an event and send it - specifying a destination FSM
    /// in the FSM_Event and an event name ( in this case - probably 'timerComplete' ).  Usually by
    /// calling an FSM or a business objects's takeEvent() method.
    /// </summary>
    public class FSM_Event {
        /// <summary>
        /// Defines the eventName.
        /// </summary>
        private readonly string eventName;

        /// <summary>
        /// Defines the source.
        /// </summary>
        private readonly object source;

        /// <summary>
        /// Defines the destFSM.
        /// </summary>
        private readonly FSM destFSM;

        /// <summary>
        /// Defines the eventDataBundle.
        /// </summary>
        private Dictionary<string, object> eventDataBundle = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FSM_Event"/> class.
        /// </summary>
        /// <param name="source">The source<see cref="object"/>.</param>
        /// <param name="eventName">The eventName<see cref="string"/>.</param>
        /// <param name="destFSM">The destFSM<see cref="FSM"/>.</param>
        public FSM_Event(object source, string eventName, FSM destFSM) {
            this.source = source;
            this.eventName = eventName;
            this.destFSM = destFSM;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FSM_Event"/> class.
        /// </summary>
        /// <param name="source">The source<see cref="object"/>.</param>
        /// <param name="eventName">The eventName<see cref="string"/>.</param>
        /// <param name="destFSM">The destFSM<see cref="FSM"/>.</param>
        /// <param name="eventDataBundle">The eventDataBundle<see cref="Dictionary{String, object}"/>.</param>
        public FSM_Event(object source, string eventName,
                        FSM destFSM, Dictionary<string, object> eventDataBundle) {
            this.source = source;
            this.eventName = eventName;
            this.destFSM = destFSM;
            this.eventDataBundle = eventDataBundle;
        }

        /// <summary>
        /// Check the components of an event for validity - for example
        /// does the named event exist in the STT ?  is the dest not null ?
        /// TODO should we check the source isn't null too ?
        /// </summary>
        /// <returns>true if all OK<see cref="bool"/>.</returns>
        public bool checkEvent(FSM fsm) {
            bool retVal = true;

            if(this.destFSM == null || this.destFSM != fsm) {
                retVal = false;
                Debug.WriteLine("FSM_Event.checkEvent() " + this.eventName +
                    " inconsistent - destFSM is null or not the same as the FSM it has been sent to");
            }

            if(!this.destFSM.stt.eventsList.Contains(this.eventName)) {
                retVal = false;
                Debug.WriteLine("FSM_Event.checkEvent() Target STT does not accept event " + this.eventName);
            }

            return retVal;
        }

        /// <summary>
        /// Adds a Bundle ( implemented as a Dictionary ) of arbitrary attributes to be passed
        /// along with an event so that extra information can be processed in the
        /// Business class' onEntry() onExit() transition() and transitionGuard() methods.
        /// </summary>
        /// <param name="bundle">The bundle<see cref="Dictionary{String, object}"/>.</param>
        public void addBundle(Dictionary<string, object> bundle) {
            this.eventDataBundle = bundle;
        }

        /// <summary>
        /// The getEventName.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string getEventName() {
            return this.eventName;
        }

        /// <summary>
        /// The getDestFSM.
        /// </summary>
        /// <returns>The <see cref="FSM"/>.</returns>
        public FSM getDestFSM() {
            return this.destFSM;
        }

        /// <summary>
        /// The getSource.
        /// </summary>
        /// <returns>The <see cref="object"/>.</returns>
        public object getSource() {
            return this.source;
        }
    }
}
