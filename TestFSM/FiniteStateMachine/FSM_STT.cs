using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestFSM.FiniteStateMachine {

    /// <summary>
    /// Says how tasks will be allocated for FSMs traversing this State Transition Table.
    /// Either there is one queue of events for each instance of an FSM ( taskPerInstance )
    /// each running on its own Task in parallel - OR we have a single queue of events and a 
    /// single OS Task for all the instances of FSM that use this STT.
    /// This allows a modicum of control on how many tasks are created.  If you have lots of
    /// instances of FSM perhaps you might not want to spawn hundreds of Tasks.  
    /// </summary>
    public enum taskAllocation { taskPerInstance, taskPerClass };

    /// <summary>
    /// FSM_STT is the State Transition Table.  It represents the set of
    /// states, transitions and events that can be drawn in a state machine
    /// diagram.  Represents the behaviour cycle of a state machine object.
    ///
    /// For each business class that wants FSM-like behaviour there should be
    /// an instance of this class ( with its related states and transitions )
    /// to describe the behaviour of all instances of that class.
    /// By convention we MUST set refClassName of the FSM_STT the same as the
    /// corresponding business class whose behaviour it describes, so when we create
    /// an FSM_STT for the business class ACTOR, we shall set its refClassName = "ACTOR"
    ///
    /// The names of the states in the STT will be used to create method names for
    /// actions as the FSM traverses its STT.  The system will expect
    /// to find these named methods in the class named by the 'refClassName' attribute.
    ///
    /// For example if the STT has a state named OnStage and the refClassName is 'ACTOR'
    /// expect to find the named ACTOR.OnStage__onEntry() in the namespace specified in the
    /// nameSpace attribute.
    ///
    /// </summary>
    public class FSM_STT {
        /// <summary>
        /// Should reflect the className of the class this STT models
        /// </summary>
        internal string refClassName;

        /// <summary>
        /// Holds a list of all the instances so they can be found. The String Key is
        /// the STT's refClassName
        /// </summary>
        internal static Dictionary<string, FSM_STT> instanceList = new Dictionary<string, FSM_STT>();

        /// <summary>
        /// Holds a list of the states in this STT, the key is the state's stateName
        /// </summary>
        internal HashSet<STT_State> statesList = new HashSet<STT_State>();

        /// <summary>
        /// DERIVED attribute.  A reflection pointer in to the Object model class this STT is for.
        /// that class must be in the nameSpace attribute.  Used when we execute methods of that class
        /// as the FSM processes events
        /// </summary>
        private Type OMClass;

        /// <summary>
        /// By default we create a Task in the OS for each Business Class ( i.e. each instance of STT )
        /// with all the instances of FSM for that class sharing the same execution thread.
        /// But we can change this to one Task for every instance of FSM instead.
        /// </summary>
        internal taskAllocation taskModel = taskAllocation.taskPerInstance;  //or taskAllocation.taskPerClass

        /// <summary>
        /// IF the taskModel is 'taskPerClass' then there will be one event processing queue here
        /// which will accept and then process all events posted at instances of the class that this 
        /// STT models.  On a single thread.
        /// </summary>
        internal FSM_EventProcessor eventProcessor = null;

        public void setTaskModel(taskAllocation alloc) {
            this.taskModel = alloc;
        }

        /// <summary>
        /// Holds the starting state for this STT and hence for all FSMs
        /// that are created using this STT.  Set by the setInitialState() method.
        /// </summary>
        private STT_State initialState;

        /// <summary>
        /// the namespace that contains the OM classes where the
        /// methods for onEntry, onExit and onTransistion live.
        /// </summary>
        private readonly string nameSpace;

        /// <summary>
        /// Indicates that when an end state of this STT is reached ( i.e. there are no further
        /// events that can be processed ) we want to delete the instance of FSM that relates
        /// to the business object.  Default is false - no FSMs are automatically deleted.
        /// </summary>
        private bool deleteWhenEndStateReached = false;  // TODO - implement a call ...

        /// <summary>
        /// DERIVED list of the the events that this STT can consume.  In effect
        /// it is the union of all the events that can transition out of
        /// all of the states in this STT. Allows us to instantly reject events
        /// if they are sent to the wrong FSM ( and its STT )
        /// </summary>
        internal HashSet<string> eventsList = new HashSet<string>();

        internal void addToEventsList(string eventName) {
            this.eventsList.Add(eventName);
        }

        // Getters and Setters

        public bool getDeleteWhenEndStateReached() {
            return this.deleteWhenEndStateReached;
        }

        /// <summary>
        /// Call this to ensure all FSMs traversing this STT delete themselves when they reach
        /// an end state ( i.e. no further events can be processed ).  By default they are not deleted
        /// </summary>
        public void setDeleteWhenEndStateReached() {
            this.deleteWhenEndStateReached = true;
        }

        public string getRefClassName() {
            return this.refClassName;
        }

        internal Type getOMClass() {
            if(this.OMClass == null) { this.OMClass = Type.GetType(this.nameSpace + "." + this.refClassName); }
            return this.OMClass;
        }

        // Search Methods

        /// <summary>
        /// Find an STT for a given OM className
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static FSM_STT findByRefClassName(string className) {
            return instanceList.GetValueOrDefault(className, null);
        }

        public HashSet<STT_State> getStatesList() {
            return this.statesList;
        }

        // Constructors

        /// <summary>
        /// Builds a new instance of FSM_STT. Also sets the derived attribute OMClass and
        /// adds the new instance to the static instanceList of this class
        /// </summary>
        /// <param name="refClassName"></param>
        /// <param name="nameSpace"></param>
        public FSM_STT(string refClassName, string nameSpace) {
            this.refClassName = refClassName;
            this.nameSpace = nameSpace;
            this.OMClass = Type.GetType(this.refClassName);
            try {
                FSM_STT.instanceList.Add(refClassName, this);
            } catch(Exception ex) {
                Debug.WriteLine("FSM_STT.new() " + ex.Message);
            }
        }

        // Accessors
        internal void addToStatesList(STT_State newState) {
            this.statesList.Add(newState);
        }

        public void setInitialState(STT_State initState) {
            // Maybe check its in the list already if not add ...  ???
            // check its not null as well ...or part of a different STT
            this.initialState = initState;
        }

        public static Dictionary<string, FSM_STT> getInstanceList() {
            return FSM_STT.instanceList;
        }

        public HashSet<string> getEventsList() {
            return this.eventsList;
        }

        /// <summary>
        /// Creates an STT_State linked to this STT and add its to the list of states.
        /// Returns the new STT_State.
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns></returns>
        public STT_State addState(string stateName) {
            STT_State aState = new STT_State(this, stateName);
            this.addToStatesList(aState);
            return aState;
        }

        internal STT_State getInitialState() {
            return this.initialState;
        }

        internal string getNameSpace() {
            return this.nameSpace;
        }
    }
}