namespace TestFSM.FiniteStateMachine {
    using System;
    using System.Diagnostics;
    using System.Reflection;

    /// <summary>
    /// Represents a transition between two states due to an event occurring.
    /// <ref>STT_Transition</ref>s belong to <ref>FSM_State</ref>s in the <ref>FSM_STT</ref>.
    /// </summary>
    public class STT_Transition {
        /// <summary>
        /// Defines the instanceName.
        /// </summary>
        internal string instanceName;// embeds fromstate toState as well as eventName

        // eg PLAYING__endoftrack__STOP
        /// <summary>
        /// Defines the fromState.
        /// </summary>
        private readonly STT_State fromState;

        /// <summary>
        /// Defines the toState.
        /// </summary>
        internal STT_State toState;

        /// <summary>
        /// Defines the eventName.
        /// </summary>
        internal string eventName;// the short version, in context of the STT - e.g. 'endOfTrack'

        /// <summary>
        /// Defines the methodForTransition.
        /// </summary>
        internal MethodInfo methodForTransition;

        /// <summary>
        /// Defines the methodForGuard.
        /// </summary>
        internal MethodInfo methodForGuard;

        // Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="STT_Transition"/> class.
        /// </summary>
        /// <param name="fromState">The fromState<see cref="STT_State"/>.</param>
        /// <param name="eventName">The eventName<see cref="string"/>.</param>
        /// <param name="toState">The toState<see cref="STT_State"/>.</param>
        internal STT_Transition(STT_State fromState, string eventName, STT_State toState) {
            this.fromState = fromState;
            this.toState = toState;
            this.instanceName = fromState.stateName + "__" + eventName + "__" + toState.stateName;
            this.eventName = eventName;
            this.setReflectionVariables();
        }

        // sets the reflection variables we shall use to call methods in the business class
        // this STT is for.   Names of methods are based on the the stateName and the
        // transition eventName
        /// <summary>
        /// The setReflectionVariables.
        /// </summary>
        private void setReflectionVariables() {
            if(this.fromState.stt.getOMClass() != null) {  // if it ain't there in the OM we can't find any methods defined within it.
                Type[] t = new Type[] { typeof(FSM_Event) };// for the search  to only get methods
                                                            // with a single FSM_Event as a parameter

                this.methodForGuard = this.fromState.stt.getOMClass().GetMethod(this.fromState.stateName +
                        "__" + this.eventName + "Guard", BindingFlags.Instance | BindingFlags.Public |
                        BindingFlags.NonPublic, null, t, null); //
                if(this.methodForGuard == null) // we didn't find it.
                {
                    Debug.WriteLine("STT_Transition.setReflectionVariables() method " + this.fromState.stateName +
                    "__" + this.eventName + "Guard() missing from class " + this.fromState.stt.refClassName + "\n");
                }

                this.methodForTransition = this.fromState.stt.getOMClass().GetMethod(this.fromState.stateName +
                        "__" + this.eventName, BindingFlags.Instance | BindingFlags.Public |
                        BindingFlags.NonPublic, null, t, null); //
                if(this.methodForGuard == null)  // we didn't find it
                {
                    Debug.WriteLine("STT_Transition.setReflectionVariables() method " + this.fromState.stateName +
                    "__" + this.eventName + "() missing from class " + this.fromState.stt.refClassName);
                } //
            }
        }

        /// <summary>
        /// Default onTransition method.  s that implements a transition/guard must be named the same as it 
        ///  and be in the class that implements the 'fromState' value in the matching registeredInstance
        /// </summary>
        /// <param name="evt">The evt<see cref="FSM_Event"/>.</param>
        internal void onTransition(FSM_Event evt) {
            Debug.WriteLine("STT_Transition.onTransition() Default method doing nothing for trans " + this.instanceName + " in response to event " + evt.getEventName() + "\n");
        }

        /// <summary>
        /// Returns the full name of the transition comprising the starting state, event and end state
        /// e.g. OnStage__audienceBoo__Exit.
        /// </summary>
        /// <returns>String.</returns>
        public string getInstanceName() {
            return this.instanceName;
        }

        public STT_State getFromState() {
            return this.fromState;
        }

        public STT_State getToState() {
            return this.toState;
        }

        /// <summary>
        /// Returns just the name of the FSM_Event that causes this transition.
        /// </summary>
        /// <returns>String.</returns>
        public string getEventName() {
            return this.eventName;
        }
    }
}
