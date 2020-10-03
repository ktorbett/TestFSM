namespace TestFSM.FiniteStateMachine {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    /// <summary>
    /// Models the states of a State Transistion table.  In this implementation
    /// a state has an onEntry method and an onExit Method - these need to be implemented in
    /// the class that is referred to by the 'refClassName' of the STT to which a state belongs.
    /// IF appropriately named methods exist on the business class ( e.g. Start__onEntry() ), they will
    /// be called by the actions of the FSM.
    /// For each event that can transition out of this state the 'refClassName' class should have 2
    /// further Methods - StateName__eventName() and StateName__EventNameGuard(). If implemented they
    /// again will be called to execute actions on the transition and to prevent the transition
    /// being executed if the guard method returns false.
    /// </summary>
    public class STT_State {
        /// <summary>
        /// A delegate that defines an instance method that takes an FSM_event as a parameter.
        /// </summary>
        /// <param name="evt">The evt<see cref="FSM_Event"/>.</param>
        public delegate void ExecInstMethodWithEventDelegate(FSM_Event evt);

        /// <summary>
        /// A delegate that defines an instance method that takes an FSM_event as a parameter and returns a bool
        /// </summary>
        /// <param name="evt">The evt<see cref="FSM_Event"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public delegate bool ExecBoolInstMethodWithEventDelegate(FSM_Event evt);

        /// <summary>
        /// The name of the state - Needs only to be unique within the context
        /// of the STT to which this State belongs.. so different STTs can both contains a state 
        /// named 'Start' if necessary..
        /// </summary>
        internal string stateName;

        /// <summary>
        /// Models the the STT this State is a part of - States can only be in one STT..
        /// </summary>
        internal FSM_STT stt;

        /// <summary>
        /// List ( as a Dictionary ) of the allowed transitions out of this state.
        /// events to be processed must be unique within the context of a State, as we can't  
        /// do 2 things at once... so the Dictionary uses the eventName as a Key..
        /// </summary>
        internal Dictionary<string, STT_Transition> allowedTransitions = new Dictionary<string, STT_Transition>();

        /// <summary>
        /// DERIVED attribute - a reflection pointer to the method in an Object Model class that matches
        /// this state's STT's classRefName and implements actions when an FSM transitions INTO this state.
        /// If the OM does not contain a method with the right name in the right class ( e.g. ACTOR.OnStage__OnEntry() )
        /// then a default method is called which merely notifies the entry into the state in a debug message...
        /// </summary>
        internal MethodInfo methodForOnEntry;

        /// <summary>
        /// DERIVED attribute - a reflection pointer to the method in an Object Model class that matches
        /// this state's STT's classRefName and implements actions when an FSM transitions OUT of this state.
        /// If the OM does not contain a method with the right name in the right class ( e.g. ACTOR.OnStage__OnExit() )
        /// then a default method is called which merely notifies the exit from the state in a debug message...
        /// </summary>
        internal MethodInfo methodForOnExit;

        /// <summary>
        /// DERIVED atribute.  Looks at the list of transistions OUT of a state
        /// if there are none it must be a final state.
        /// </summary>
        /// <returns>.</returns>
        public bool getIsFinalState() {
            bool retVal = false;
            if(this.allowedTransitions.Count == 0 || this.allowedTransitions == null) {
                retVal = true;
            }
            return retVal;
        }

        /// <summary>
        /// Adds an STT_Transition to this State.  Also adds the name of the transition to the
        /// eventList of the STT to which this State is attached.
        /// </summary>
        /// <param name="eventName">.</param>
        /// <param name="toState">.</param>
        internal void addTransition(string eventName, STT_State toState) {
            STT_Transition aTrans = new STT_Transition(this, eventName, toState);
            this.stt.addToEventsList(eventName);

            if(!this.allowedTransitions.TryAdd(eventName, aTrans)) {
                Debug.WriteLine("STT_State.addTransition - duplicate event name - ignoring");
            }
        }

        public Dictionary<string, STT_Transition> getAllowedTransitions() {
            return this.allowedTransitions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="STT_State"/> class.
        /// </summary>
        /// <param name="fsmSTT">.</param>
        /// <param name="stateName">.</param>
        internal STT_State(FSM_STT fsmSTT, string stateName) {
            this.stateName = stateName;
            this.stt = fsmSTT;
            this.setReflectionVariables();
        }

        /// <summary>
        /// Accepts the event BUT DOES NOT process it.
        /// returns the next State that would be reached if it were processed.
        /// NOT SURE when this would be used .
        /// </summary>
        /// <param name="evt">.</param>
        /// <returns>.</returns>
        internal STT_State testEvent(FSM_Event evt) {
            STT_State retVal;
            STT_Transition trans;

            if(this.allowedTransitions != null && this.allowedTransitions.ContainsKey(evt.getEventName())) {   // find the transition by its key which
                // is the event name, eg 'recieveCue'
                trans = this.allowedTransitions.GetValueOrDefault(evt.getEventName());
                // transition occurs only if the guard method is not present or evaluates to True
                if(trans.methodForGuard == null) {  // there is no guard, go ahead
                    retVal = trans.toState;
                } else {
                    if(STT_State.execBoolMethodWithEvent(trans.methodForGuard, evt)) {
                        retVal = trans.toState;
                    } else {
                        retVal = this; // we're still in this state
                    }
                }
            } else {
                retVal = this;  // we're still in this state
            }
            return retVal;
        }

        /// <summary>
        /// Accepts the event and processes it.
        /// returns the next State and 'this' if it can't make the transition.
        /// As a by-product calls the necessary StateName__onExit(), StateName__eventName()
        /// StateName__OnEntry() and StateName__eventNameGuard() methods if they exist.
        /// in the class that the STT refers to in its 'refClassName' attribute.
        /// </summary>
        /// <param name="evt">.</param>
        /// <returns>.</returns>
        internal STT_State takeEvent(FSM_Event evt) {
            STT_State retVal;
            STT_Transition trans;

            if(this.allowedTransitions != null && this.allowedTransitions.ContainsKey(evt.getEventName())) {  // find the transition by its key which
                                                                                                              // is the event refClassName, eg 'recieveCue'
                trans = this.allowedTransitions.GetValueOrDefault(evt.getEventName());

                // do we need a check for null ??

                // do the actions, but only if the guard method is not present or evaluates to True
                if(trans.methodForGuard == null) {  // there is no guard, go ahead
                    this.executeTransition(evt, trans);
                    retVal = trans.toState;
                } else {
                    if(STT_State.execBoolMethodWithEvent(trans.methodForGuard, evt)) {
                        this.executeTransition(evt, trans);
                        retVal = trans.toState;
                    } else {
                        retVal = this; // we're still in this state, the guard said no
                        Debug.WriteLine("STT_State.takeEvent() refusing event " + evt.getEventName() +
                            " in state " + this.stateName + " as Guard method says no\n");
                    }
                }
            } else {
                // should report an error ?
                Debug.WriteLine("STT_State.takeEvent() event " + evt.getEventName() + " ignored in state " +
                    this.stateName + "\n");
                retVal = this;  // we're still in this state
            }
            return retVal;
        }

        /// <summary>
        /// Uses delegates to execute the method that 'method' points at passing the event evt
        /// for the Object Model instance that the destination FSM is bound to.
        /// </summary>
        /// <param name="method">The methodInfo<see cref="MethodInfo"/>.</param>
        /// <param name="evt">The evt<see cref="FSM_Event"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private static bool execBoolMethodWithEvent(MethodInfo method, FSM_Event evt) {
            bool retVal = false;
            try {
                ExecBoolInstMethodWithEventDelegate del =
                    (ExecBoolInstMethodWithEventDelegate)Delegate.CreateDelegate(
                                (typeof(ExecBoolInstMethodWithEventDelegate)),
                                evt.getDestFSM().getRegisteredInstance(), method);
                retVal = del.Invoke(evt);
            } catch(Exception ex) {
                Debug.WriteLine(ex.Message);
            }

            return retVal;
        }

        /// <summary>
        /// gets the state name.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        internal string getStateName() {
            return this.stateName;
        }

        /// <summary>
        /// Default onEntry() method for states.  If the business object model for an STT does not
        /// contain a method to implement the onEntry() actions, thsi is called instead and posts
        /// a debug message.
        /// </summary>
        /// <param name="evt">The evt<see cref="FSM_Event"/>.</param>
        internal void onEntry(FSM_Event evt) {
            Debug.WriteLine("STT_State.onEntry() Default method doing nothing for state " + this.stateName +
                " in response to event " + evt.getEventName() + "\n");
        }

        /// <summary>
        /// Default onExit() method for states.  If the business object model for an STT does not
        /// contain a method to implement the onExit() actions, thsi is called instead and posts
        /// a debug message.
        /// </summary>
        /// <param name="evt">The evt<see cref="FSM_Event"/>.</param>
        internal void onExit(FSM_Event evt) {
            Debug.WriteLine("STT_State.onExit()  Default method doing nothing for state " + this.stateName +
                " in response to event " + evt.getEventName() + "\n");
        }

        /// <summary>
        /// Execites the methods associated with traversing a transition from one state to another.
        /// </summary>
        /// <param name="evt">The evt<see cref="FSM_Event"/>.</param>
        /// <param name="trans">The trans<see cref="STT_Transition"/>.</param>
        private void executeTransition(FSM_Event evt, STT_Transition trans) {
            // we set up with reflection in the constructor/loader/finder methods
            // of the state and transition classes.. Should be using delegates ?
            if(this.methodForOnExit == null) {
                this.onExit(evt);
            } else {
                STT_State.execInstMethodWithEvent(this.methodForOnExit, evt);
            }

            if(trans.methodForTransition == null) {
                trans.onTransition(evt);
            } else {
                STT_State.execInstMethodWithEvent(trans.methodForTransition, evt);
            }
            if(trans.toState.methodForOnEntry == null) {
                trans.toState.onEntry(evt);
            } else {
                STT_State.execInstMethodWithEvent(trans.toState.methodForOnEntry, evt);
            }
        }

        /// <summary>
        /// Sets the reflection variables liek the methodInbfo pointers for thsi State.
        /// </summary>
        private void setReflectionVariables() {
            if(this.stt.getOMClass() != null) {  // if it ain't there we can't continue
                Type[] t = new Type[] { typeof(FSM_Event) };// for the search  to only get methods
                                                            // with a single FSM_Event as a parameter

                this.methodForOnEntry = this.stt.getOMClass().GetMethod(this.stateName + "__onEntry",
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, t, null);

                if(this.methodForOnEntry == null) {
                    Debug.WriteLine("STT_State.setReflectionVariables() method " + this.stateName +
                    "__onEntry() missing from class " + this.stt.getRefClassName());
                } //

                this.methodForOnExit = this.stt.getOMClass().GetMethod(this.stateName + "__onExit",
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, t, null);
                if(this.methodForOnExit == null && !this.getIsFinalState()) {
                    Debug.WriteLine("STT_State.setReflectionVariables() method " + this.stateName +
                    "__onExit() missing from class " + this.stt.getRefClassName());
                }
            }
        }

        /// <summary>
        /// Executes a method, passing an event, for the instance defined in the destFSM registered Instance
        /// </summary>
        /// <param name="method">The methodInfo pointing to the method in the Object Model class<see cref="MethodInfo"/>.</param>
        /// <param name="evt">The FSM_Event to be processed<see cref="FSM_Event"/>.</param>
        internal static void execInstMethodWithEvent(MethodInfo method, FSM_Event evt) {
            try {
                ExecInstMethodWithEventDelegate del =
                    (ExecInstMethodWithEventDelegate)Delegate.CreateDelegate(
                                (typeof(ExecInstMethodWithEventDelegate)),
                                evt.getDestFSM().getRegisteredInstance(), method);
                del.Invoke(evt);

            } catch(Exception ex) {
                Debug.WriteLine("STT_State.execInstMethodWithEvent - " + ex.Message);
            }
        }
    }
}
