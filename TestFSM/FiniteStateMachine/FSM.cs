namespace TestFSM.FiniteStateMachine {
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// The main Finite State Machine Class....
    /// instances of this class represent a 'running' FSM
    /// which consumes events and changes its state over time.
    /// A business class instance can delegate its state behaviour
    /// to an instance of FSM.
    /// Each instance of FSM will be 'bound' to an instance
    /// of a business/operational class of the application
    /// like ACTOR, CDPLAYER, PARTICLE etc ( and vice-versa ).
    /// To do so the STT that an FSM traverses must have the
    /// same refClassName as the business class it is for and we should set
    /// the fsmName to include the refClassName of the
    /// business class it models.  ACTOR:actor1 for example.
    /// The constructor adds the ACTOR: part to whatever is passed in
    ///
    /// The business classes are required to have a member
    /// variable of type FSM ( or one of its subclasses ) and
    /// when they receive events they should pass them on to
    /// their FSM.
    ///
    /// Creation of an instance of the business class should
    /// chain the creation of an instance of FSM with the same
    /// instance refClassName, and set the FSM_STT that the FSM traverses.
    /// there is a code generator capability to assist with this in the
    /// related class FSM_CodeBuilder.
    ///
    /// During its operation as it transitions from state to state, an FSM will call
    /// methods of the business class to which the STT refers in its refClassName attribute.
    ///
    /// For example we may require an ACTOR to make a speech when
    /// moving from the state OffStage to OnStage, which happens when
    /// the event receiveCue occurs. as the FSM moves from OffStage to
    /// OnStage, it will try to call methods named OffStage__onExit()
    /// OffStage__receiveCueGuard(), OffStage__receiveCue() and OnStage__onEntry().
    /// If they don't exist as methods of the ACTOR class, a message will
    /// be generated.
    ///
    /// Thus all the programmer needs to do is to implement these methods according
    /// to the naming convention - similar to many other frameworks like windows forms, 
    /// Hibernate or Android intents.
    /// </summary>
    public class FSM {
        /// <summary>
        /// The current state of this FSM
        /// </summary>
        internal STT_State currentState;

        /// <summary>
        /// Defines the State Transition Table that this FSM is traversing
        /// </summary>
        internal FSM_STT stt;

        /// <summary>
        /// Shouls be of the form refClassName:objName of the business class instance that needs FSM abilities
        /// i.e If we have an ACTOR with a name attribute set to "actor1" the FSM will be named "ACTOR:actor1".
        /// </summary>
        internal string fsmName;

        /// <summary>
        /// A Dictionary of instances of the class.  Supports findByFSMName() and other search methods.
        /// </summary>
        internal static Dictionary<string, FSM> instanceList = new Dictionary<string, FSM>();

        /// <summary>
        /// The instance of business object ( ACTOR, CDPLAYER, PARTICLE  etc ) for which this
        /// instance of FSM is managing the state.  TODO - Generics.....
        /// </summary>
        internal object registeredInstance;// the instance of the business class this FSM is for

        /// <summary>
        /// Initializes a new instance of the <see cref="FSM"/> class.
        /// </summary>
        /// <param name="newId">.</param>
        /// <param name="fsmSTT">.</param>
        /// <param name="registeringInstance">.</param>
        public FSM(string newId, FSM_STT fsmSTT, object registeringInstance) {
            string riClassName = registeringInstance.GetType().Name;
            if(riClassName == fsmSTT.refClassName) {
                this.fsmName = riClassName + ":" + newId;
                this.stt = fsmSTT;
                this.registeredInstance = registeringInstance;

                if(!FSM.instanceList.TryAdd(this.fsmName, this)) {
                    throw new System.ArgumentException("FSM constructor - Duplicate refClassName - not added");
                }
            } else {
                throw new System.ArgumentException("ERROR in FSM constructor: Registering instance and FSM_STT refClassName do not refer to the same class");
            }
        }

        /// <summary>
        /// The instanceList accessor.
        /// </summary>
        /// <returns>The <see cref="Dictionary{String, FSM}"/>.</returns>
        public static Dictionary<string, FSM> getInstanceList() {
            return FSM.instanceList;
        }

        /// <summary>
        /// Removes an FSM from the <ref>instanceList</ref>.
        /// </summary>
        /// <param name="fsm">The fsm<see cref="FSM"/>.</param>
        public static void removeFromInstanceList(FSM fsm) {
            FSM.instanceList.Remove(fsm.fsmName);
        }

        /// <summary>
        /// Allows an external class to find an FSM based on the fsmName - which is composed of 
        /// the class name of the registered instance plus that registered instances 'instanceName'.
        /// i.e. ACTOR:fred
        /// </summary>
        /// <param name="fsmName">The fsmName<see cref="string"/>.</param>
        /// <returns>.</returns>
        public static FSM findByFSMName(string fsmName) {
            FSM retVal = null;
            if(FSM.instanceList.ContainsKey(fsmName)) {
                retVal = FSM.instanceList.GetValueOrDefault(fsmName);  // Can we simplify ?
            } else {
                Debug.WriteLine("FSM:findByFSMName() Can't find FSM named " + fsmName);
            }
            return retVal;
        }

        /// <summary>
        /// Allows an external class to find and FSM based on the registeredInstance.   So an external 
        /// class can find the FSM that it needs to send events to based on the business model instance.
        /// </summary>
        /// <param name="myOMInstance">.</param>
        /// <returns>.</returns>
        public static FSM findByRegisteredInstance(object myOMInstance) {
            FSM retVal = null;
            foreach(FSM fsm in FSM.instanceList.Values) {
                if(fsm.registeredInstance.Equals(myOMInstance)) {
                    retVal = fsm;
                    break;
                }
            }

            if(retVal == null) {
                Debug.WriteLine("FSM:findByInstance() Can't find FSM with registered instance " + myOMInstance.ToString() + "\n");
            }
            return retVal;
        }

        /// <summary>
        /// Sends an event.  Source is an Object rather than an FSM so that anything can send an event - it
        /// doesn't have to come from another instance of FSM.
        /// </summary>
        /// <param name="source">.</param>
        /// <param name="eventName">.</param>
        /// <param name="toFSM">.</param>
        public static void createAndSendEvent(object source, string eventName, FSM toFSM) {
            FSM_Event newEvent = new FSM_Event(source, eventName, toFSM);
            toFSM.takeEvent(newEvent);
        }

        /// <summary>
        /// Needs to be called once an FSM has been created to set its current state
        /// and run its onEntry() method for its initial state.  This version takes an event and thus
        /// information can be passed to an initialising FSM in the event's data bundle for use in the 
        /// State__onEntry() method.
        /// </summary>
        /// <param name="evt">.</param>
        public void initialiseWithEvent(FSM_Event evt) //  can send initial conditions in the event Bundle
        {
            if(this.stt != null) {
                this.currentState = this.stt.getInitialState();

                if(this.currentState.methodForOnEntry == null) {
                    this.currentState.onEntry(evt);
                } else {
                    STT_State.execInstMethodWithEvent(this.currentState.methodForOnEntry, evt);
                }
            } else {
                Debug.WriteLine("FSM.initialiseSynch() Fail: no State Transition Table\n");
            }
        }

        /// <summary>
        /// Needs to be called once an FSM has been created to set its current state
        /// and run its onEntry() method for its initial state.
        /// </summary>
        public void initialise() {
            if(this.stt != null) {
                this.currentState = this.stt.getInitialState();

                FSM_Event evt = new FSM_Event(this, "start", this); // Allows for an FSM to 'initialise itself
                                                                    // by putting code into its initial state's onEntry() method.

                if(this.currentState.methodForOnEntry == null) {
                    this.currentState.onEntry(evt);
                } else {
                    STT_State.execInstMethodWithEvent(this.currentState.methodForOnEntry, evt);
                }
            } else {
                Debug.Write("FSM.initialiseSynch() Fail: no State Transition Table\n");
            }
        }

        /// <summary>
        /// Gets the current state of the FSM.
        /// </summary>
        /// <returns>The <see cref="STT_State"/>.</returns>
        public STT_State getCurrentState() {
            return this.currentState;
        }

        /// <summary>
        /// Processes an event sent to the FSM.  As a by-product calls the appropriate methods
        /// for onEntry(), onExit(), transition() and transitionGuard() in
        /// the Object Model class to which this FSM is bound, if they exist.
        /// </summary>
        /// <param name="evt">.</param>
        /// <returns>an STT_State representing the new state.</returns>
        public STT_State takeEvent(FSM_Event evt) {

            STT_State retVal;
            if(evt.checkEvent()) {
                retVal = this.currentState.takeEvent(evt);
            } else {
                retVal = this.currentState;
            }
            this.currentState = retVal;
            return retVal;
        }

        /// <summary>
        /// Gets the FSM name.  This will be of the form ACTOR:fred
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string getFSMName() {
            return this.fsmName;
        }

        /// <summary>
        /// Gets the State Transition table this FSM is traversing.
        /// </summary>
        /// <returns>The <see cref="FSM_STT"/>.</returns>
        public FSM_STT getSTT() {
            return this.stt;
        }

        /// <summary>
        /// Gets the Registered Instance of the object model class to which this FSM is bound and 
        /// for which it is processing events.
        /// </summary>
        /// <returns>The <see cref="object"/>.</returns>
        public object getRegisteredInstance() {
            return this.registeredInstance;
        }
    }
}
