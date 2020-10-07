﻿namespace TestFSM.FiniteStateMachine {
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// The main Finite State Machine Class....
    /// instances of the FSM class family represent a 'running' FSM
    /// which consumes events and changes its state over time.
    /// NOTE:  this is an abstract class so we never get instances of FSM, 
    /// only ASYNCH_FSM and SYNCH_FSM.
    /// 
    /// A business class instance can delegate its state behaviour to an FSM.
    /// Each FSM will be 'bound' by its 'registeredInstance' attribute to an instance
    /// of a business/operational class of the application like ACTOR, CDPLAYER, 
    /// PARTICLE etc ( and vice-versa ).
    /// 
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
    public abstract class FSM {

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
        /// Creates a new instance of the <see cref="FSM"/> class.
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
                throw new System.ArgumentException("ERROR in FSM constructor: Registering instance and FSM_STT " +
                                                    "refClassName do not refer to the same class");
            }
        }

        /// <summary>
        /// The instanceList accessor.
        /// </summary>
        /// <returns>The list of FSMs as a <see cref="Dictionary{String, FSM}"/>.</returns>
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
        /// Gets the current state of the FSM.
        /// </summary>
        /// <returns>The <see cref="STT_State"/>.</returns>
        public STT_State getCurrentState() {
            return this.currentState;
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

        public abstract void initialiseWithEvent(FSM_Event evt);
        public abstract void initialise();
        public abstract STT_State takeEvent(FSM_Event evt);

    }

    public class SYNCH_FSM : FSM {

        public SYNCH_FSM(string newId, FSM_STT fsmSTT, object registeringInstance)
            : base(newId, fsmSTT, registeringInstance) {

            // Is there anything else I need to do ?       
        }

        /// <summary>
        /// Needs to be called once a SYNCH_FSM has been created to set its current state
        /// and run its onEntry() method for its initial state.  This version takes an event and thus
        /// information can be passed to an initialising SYNCH_FSM in the event's data bundle for use
        /// in the State__onEntry() method.
        /// </summary>
        /// <param name="evt">.</param>
        public override void initialiseWithEvent(FSM_Event evt) //  can send initial conditions in the event Bundle
        {
            if(this.stt != null) {
                this.currentState = this.stt.getInitialState();

                if(this.currentState.methodForOnEntry == null) {
                    this.currentState.onEntry(evt);
                } else {
                    STT_State.execInstMethodWithEvent(this.currentState.methodForOnEntry, evt);
                }
            } else {
                Debug.WriteLine("SYNCH_FSM.initialiseWithEvent() Fail: no State Transition Table\n");
            }
        }

        /// <summary>
        /// Needs to be called once an FSM has been created to set its current state
        /// and run its onEntry() method for its initial state.
        /// </summary>
        public override void initialise() {
            if(this.stt != null) {
                this.currentState = this.stt.getInitialState();

                FSM_Event evt = new FSM_Event(this, "start", this); // just so the event isn't null

                if(this.currentState.methodForOnEntry == null) {
                    this.currentState.onEntry(evt);
                } else {
                    STT_State.execInstMethodWithEvent(this.currentState.methodForOnEntry, evt);
                }
            } else {
                Debug.WriteLine("SYNCH_FSM.initialise() Fail: no STT for FSM " + this.fsmName);
            }
        }


        /// <summary>
        /// Processes an event sent to the FSM.  As a by-product calls the appropriate methods
        /// for onEntry(), onExit(), transition() and transitionGuard() in
        /// the Object Model class to which this FSM is bound, if they exist.
        /// </summary>
        /// <param name="evt">.</param>
        /// <returns>an STT_State representing the new state.</returns>
        public override STT_State takeEvent(FSM_Event evt) {

            STT_State retVal;
            if(evt.checkEvent()) {
                retVal = this.currentState.takeEvent(evt);
            } else {
                retVal = this.currentState;
            }
            this.currentState = retVal;
            return retVal;
        }

    }


    public delegate void UICallbackDelegate(FSM fsm);
    public class ASYNCH_FSM : FSM {


        private readonly ConcurrentQueue<FSM_Event> eventQ = new ConcurrentQueue<FSM_Event>();
        //private Task fsmTask;
        private UICallbackDelegate cbDel;
        private bool stopped;

        public ASYNCH_FSM(string newId, FSM_STT fsmSTT, object registeringInstance)
            : base(newId, fsmSTT, registeringInstance) {

        }

        public void setCallBackUIDelegate(UICallbackDelegate cbDel) {
            this.cbDel = cbDel;
        }
        public override void initialise() {

            if(this.stt != null) {
                this.currentState = this.stt.getInitialState();
                FSM_Event evt = new FSM_Event(null, "start", this);  // just so it's not null
                                                                     // Could be used to send initial data for action in the initial state's
                                                                     // State__onEntry() method but probably better to do init in that ...

                if(this.currentState.methodForOnEntry == null) {
                    this.currentState.onEntry(evt);
                } else {
                    STT_State.execInstMethodWithEvent(this.currentState.methodForOnEntry, evt);
                }

                // This is interntionally fire and forget ...  
                Task t = Task.Run(() => this.processEvents());
                
                Debug.WriteLine("ASYNCH_FSM.initialise() processEvents() task " + t.Id + " spawned for FSM " + this.fsmName);
            
            } else {
                Debug.WriteLine("ASYNCH_FSM.initialise() Fail: no STT for FSM " + this.fsmName);
            }
        }


        private void processEvents() {

            while(!this.stopped) {
                if(!this.eventQ.IsEmpty) {

                    if(this.eventQ.TryDequeue(out FSM_Event nextEvent)) { // just in case ...

                        // we could make this a task too ???
                        this.currentState = this.currentState.takeEvent(nextEvent);

                        // IF the event we have just processed was received from a 
                        // UI component ( a Control ) then call the method that has
                        // been registered as our UICallbackDelegate ( if one has been set )
                        // passing back this instance.  From that reference the UI should be
                        // able to find all the info it needs ...

                        if(nextEvent.getSource() is Control sourceControl && this.cbDel != null) {
                            object[] myArray = new object[1];
                            myArray[0] = this;
                            sourceControl.BeginInvoke(this.cbDel, myArray);
                        }
                    }
                }
            }
        }

        // TODO we need a way to stop the FSM asynchronously, so some sort of callback function ?
        // TODO this should probably be on a worker thread

        public void stopProcessingEvents() {
            this.stopped = true;
        }

        public override STT_State takeEvent(FSM_Event evt) {

            // TODO shoud we check thsat its for thsi FSM ??

            if(evt.checkEvent()) {
                this.eventQ.Enqueue(evt);
            } else {
                Debug.WriteLine("FSM.takeEvent() Failure: illegal event name or null target FSM");
            }
            return null;
        }


        public override void initialiseWithEvent(FSM_Event evt) {

            if(this.stt != null) {
                this.currentState = this.stt.getInitialState();

                if(this.currentState.methodForOnEntry == null) {
                    this.currentState.onEntry(evt);
                } else {
                    STT_State.execInstMethodWithEvent(this.currentState.methodForOnEntry, evt);
                }
                this.processEvents();
            } else {
                Debug.WriteLine("ASYNCH_FSM.initialiseWithEvent() Fail: no STT for FSM " + this.fsmName);
            }
        }

    }
}