namespace TestFSM.FiniteStateMachine {
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Delegate type for a method that writes the code for a class.  Allows the developer
    /// to modify the default template code if they implement a method with this signature
    /// and register it using setWriteClassCode().  The method must append the code to the 
    /// StringBuilder retVal.
    /// </summary>
    /// <param name="theSTT"></param>
    /// <param name="retVal"></param>
    public delegate void WriteClassCodeDelegate(FSM_STT theSTT, StringBuilder retVal);

    /// <summary>
    /// Delegate type for a method that writes the code for a state.  Allows the developer
    /// to modify the default template code if they implement a method with this signature
    /// and register it using setWriteOnEntryCode() and setWriteOnExitCode().  The method must append the code to the 
    /// StringBuilder retVal
    /// </summary>
    /// <param name="theSTT"></param>
    /// <param name="retVal"></param>
    /// <param name="state"></param>
    public delegate void WriteStateMethodCodeDelegate(FSM_STT theSTT, StringBuilder retVal,
                                                        STT_State state);

    /// <summary>
    /// Delegate type for a method that writes the code for a transition.  Allows the developer
    /// to modify the default template code if they implement a method with this signature
    /// and register it using setWriteTransCode() and setWriteTransGuardCode.  The method must 
    /// append the code to the StringBuilder retVal.
    /// </summary>
    /// <param name="theSTT"></param>
    /// <param name="retVal"></param>
    /// <param name="state"></param>
    public delegate void WriteTransMethodCodeDelegate(FSM_STT theSTT, StringBuilder retVal,
                                                           STT_State state, STT_Transition trans);

    /// <summary>
    /// This class holds the default methods that can be used in the developer/debug environment
    /// to generate code for a class given that we have loaded the state transition table that
    /// describes its behaviour.  Developers can write a simple app that has a UI to load the STTs
    /// and then create code for them, if it does not already exist.  Ensures the convention that
    /// the FSM that expects methods to implement states to already be present in the business object
    /// model, and have specific names according to a naming convention.
    /// </summary>
    internal class FSM_CodeBuilder {
        /// <summary>
        /// Pointer ( delegate ) to the method that writes the class code and sets it to the 
        /// default value pointing to the static method WriteClassCode() in this 
        /// class
        /// </summary>
        private static WriteClassCodeDelegate writeClassCode = WriteClassCode;

        /// <summary>
        /// Pointer ( delegate ) to the method that writes the onEntry code for a state. 
        /// Default value points to the static method WriteOnEntryMethodCode() in this 
        /// class
        /// </summary>
        private static WriteStateMethodCodeDelegate writeOnEntryCode = WriteOnEntryMethodCode;

        /// <summary>
        /// Pointer ( delegate ) to the method that writes the onExit code for a state. 
        /// Default value points to the static method WriteOnExitMethodCode() in this 
        /// class
        /// </summary>
        private static WriteStateMethodCodeDelegate writeOnExitCode = WriteOnExitMethodCode;

        /// <summary>
        /// Pointer ( delegate ) to the method that writes the onTransition code for a transition. 
        /// Default value points to the static method WriteTransMethodCode() in this 
        /// class
        /// </summary>
        private static WriteTransMethodCodeDelegate writeTransCode = WriteTransMethodCode;

        /// <summary>
        /// Pointer ( delegate ) to the method that writes the Guard code for a transition. 
        /// Default value points to the static method WriteTransGuardMethodCode() in this 
        /// class
        /// </summary>
        private static WriteTransMethodCodeDelegate writeTransGuardCode = WriteTransGuardMethodCode;

        /// <summary>
        /// If true, code generation will skip classes and methods that already exist in the Object
        /// Model classes..
        /// </summary>
        private static bool onlyGenerateMissingCode = true;

        /// <summary>
        /// Allows developer to override the default code generation for business classes.
        /// This section is for the class declaration and methods that all business classes
        /// must share, such as a constructor that also creates a bound FSM.
        /// Create a method in your own code that matches the type signature defined in 
        /// <ref>WriteClassCodeDelegate</ref> - i.e. public void XXX(FSM_STT theSTT, StringBuilder retVal);
        /// Your method should append your code to retVal.  Pass it as a parameter to this method
        /// to register it as a replacement for the default code generator.
        /// </summary>
        /// <param name="del">.</param>
        public static void setWriteClassCode(WriteClassCodeDelegate del) {
            FSM_CodeBuilder.writeClassCode = del;
        }

        /// <summary>
        /// Call this method passing in 'true' to tell the code generation system to ignore business 
        /// object model classes and methods that already exist when it generates code.
        /// By default 'ignoreExisting' is true so set to 'falsefor classes and methods 
        /// that are missing ).
        /// </summary>
        /// <param name="ignore">The ignore<see cref="bool"/>.</param>
        public static void setOnlyGenerateMissingCode(bool ignore) {
            FSM_CodeBuilder.onlyGenerateMissingCode = ignore;
        }

        /// <summary>
        /// Allows developer to override the default code generation for business classes.
        /// This section is for the State__onEntry() method that the FSM calls on entry
        /// to a state.
        /// Create a method in your own code that matches the type signature defined in 
        /// <ref>WriteStateMethodCodeDelegate</ref> - i.e. public void XXX(FSM_STT theSTT, 
        /// StringBuilder retVal, STT_State state);
        /// Your method should append your code to retVal.  Pass it as a parameter to this method
        /// to register it as a replacement for the default code generator.
        /// </summary>
        /// <param name="del">.</param>
        public static void setWriteOnEntryCode(WriteStateMethodCodeDelegate del) {
            FSM_CodeBuilder.writeOnEntryCode = del;
        }

        /// <summary>
        /// Allows developer to override the default code generation for business classes.
        /// This section is for the State__onExit() method that the FSM calls on exit
        /// from a state.
        /// Create a method in your own code that matches the type signature defined in 
        /// <ref>WriteStateMethodCodeDelegate</ref> - i.e. public void XXX(FSM_STT theSTT, 
        /// StringBuilder retVal, STT_State state);
        /// Your method should append your code to retVal.  Pass it as a parameter to this method
        /// to register it as a replacement for the default code generator.
        /// </summary>
        /// <param name="del">.</param>
        public static void setWriteOnExitCode(WriteStateMethodCodeDelegate del) {
            FSM_CodeBuilder.writeOnExitCode = del;
        }

        /// <summary>
        /// Allows developer to override the default code generation for business classes.
        /// This section is for the State__eventName() method that the FSM calls on exit
        /// from one state to another.
        /// Create a method in your own code that matches the type signature defined in 
        /// <ref>WriteTransMethodCodeDelegate</ref> - i.e. public void XXX(FSM_STT theSTT, 
        /// StringBuilder retVal, STT_State state, STT_Transition trans);
        /// Your method should append your code to retVal.  Pass it as a parameter to this method
        /// to register it as a replacement for the default code generator.
        /// </summary>
        /// <param name="del">.</param>
        public static void setWriteTransCode(WriteTransMethodCodeDelegate del) {
            FSM_CodeBuilder.writeTransCode = del;
        }

        /// <summary>
        /// Allows developer to override the default code generation for business classes.
        /// This section is for the State__eventNameGuard() method that the FSM calls when it
        /// receives an event to decide whether or not to act upon it.
        /// Create a method in your own code that matches the type signature defined in 
        /// <ref>WriteTransMethodCodeDelegate</ref> - i.e. public void XXX(FSM_STT theSTT, 
        /// StringBuilder retVal, STT_State state, STT_Transition trans);
        /// Your method should append your code to retVal.  Pass it as a parameter to this method
        /// to register it as a replacement for the default code generator.
        /// </summary>
        /// <param name="del">.</param>
        public static void setWriteTransGuardCode(WriteTransMethodCodeDelegate del) {
            FSM_CodeBuilder.writeTransGuardCode = del;
        }

        /// <summary>
        /// The createOMCodeFromSTT.
        /// </summary>
        /// <param name="theSTT">The theSTT<see cref="FSM_STT"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string createOMCodeFromSTT(FSM_STT theSTT) {
            StringBuilder retVal = new StringBuilder("   //FSM Methods Required for class  ");
            retVal.Append(theSTT.getRefClassName()).Append("  generated by FSM_CodeBuilder ").
                Append(DateTime.Now.ToString()).Append("\n\n");

            if(theSTT.getOMClass() == null || FSM_CodeBuilder.onlyGenerateMissingCode) {
                FSM_CodeBuilder.writeClassCode(theSTT, retVal);
            }

            foreach(STT_State state in theSTT.getStatesList()) {  // loop over all the states in this FSM
                retVal.Append("      // Implementation of State ").Append(state.stateName).Append("\n\n");
                if(state.methodForOnEntry == null || FSM_CodeBuilder.onlyGenerateMissingCode) {
                    FSM_CodeBuilder.writeOnEntryCode(theSTT, retVal, state);
                }
                if((state.methodForOnExit == null || FSM_CodeBuilder.onlyGenerateMissingCode) && !state.getIsFinalState()) // OR its not a final state
                {
                    FSM_CodeBuilder.writeOnExitCode(theSTT, retVal, state);
                }

                if(state.getIsFinalState() != true)  // final state has no transitions
                {
                    foreach(STT_Transition trans in state.allowedTransitions.Values) {
                        if(trans.methodForTransition == null || FSM_CodeBuilder.onlyGenerateMissingCode) {
                            FSM_CodeBuilder.writeTransCode(theSTT, retVal, state, trans);
                        }

                        if(trans.methodForGuard == null || FSM_CodeBuilder.onlyGenerateMissingCode) {
                            FSM_CodeBuilder.writeTransGuardCode(theSTT, retVal, state, trans);
                        }
                    }
                    retVal.Append("\n");
                }
                retVal.Append("\n");
            }
            if(theSTT.getOMClass() == null) {
                retVal.Append("}\n\n");
            }
            return retVal.ToString();
        }

        /// <summary>
        /// WriteClassCode() is the default method that writes the code for the class definition if a class
        /// that is named in a <ref>FSM_STT</ref> ( by means of its <ref>refClassName</ref> attribute )
        /// is missing from the Object Model namespace.  You can override this in the developer environment
        /// by writing your own version with the same signature and using the <ref>setWriteClassCode</ref> 
        /// method to register it with the code generator.
        /// </summary>
        /// <param name="theSTT">The theSTT<see cref="FSM_STT"/>.</param>
        /// <param name="retVal">The retVal<see cref="StringBuilder"/>.</param>
        private static void WriteClassCode(FSM_STT theSTT, StringBuilder retVal) {

            // TODO want code gen to provide a switch on the type sync/async passed in as a parameter
            // to the constructor.  Maybe a second constructor for initialising with an event  ?

            string className = theSTT.getRefClassName();
            retVal.Append("\n\n   // ADD CLASS\n");
            retVal.Append("   public class ").Append(className).Append("\n");
            retVal.Append("   {\n\n");
            string instNameName = className.ToLower() + "Name";
            retVal.Append("      protected string ").Append(instNameName).Append(";\n");
            retVal.Append("      protected FSM fsm;\n\n");
            retVal.Append("      public ").Append(className).Append("( string ")
                .Append(instNameName).Append(", FSM_STT stt, FSMType fsmType )\n");
            retVal.Append("      {\n");
            retVal.Append("         this.").Append(instNameName).Append(" = ").Append(instNameName).Append(";\n");
            retVal.Append("         this.fsm = FSM.createFSM(this.").Append(instNameName).
                                    Append(", stt, this, fsmType);\n");
            retVal.Append("         this.fsm.setInitialState();\n");
            retVal.Append("      }\n\n");
            retVal.Append("      public FSM getFSM()\n");
            retVal.Append("      {\n");
            retVal.Append("         return this.fsm;\n");
            retVal.Append("      }\n\n");
            retVal.Append("      public STT_State getCurrentState()\n");
            retVal.Append("      {\n");
            retVal.Append("         return this.fsm.getCurrentState();\n");
            retVal.Append("      }\n\n");
            retVal.Append("      // Use this in the body of your StateName__onEntry() methods for the end states\n");
            retVal.Append("      // of the FSM ( the ones with no exit transitions ) and want to 'delete the FSM'\n");
            retVal.Append("      // and references to tidy stuff up.\n");
            retVal.Append("      protected void dereferenceFSM()\n");
            retVal.Append("      {\n");
            retVal.Append("         FSM.removeFromInstanceList(this.fsm);\n");
            retVal.Append("         this.fsm = null;\n");
            retVal.Append("      }\n\n");
            retVal.Append("      // Processes an event.  Passes it on to the FSM\n");
            retVal.Append("      public void takeEvent( FSM_Event evt)\n");
            retVal.Append("      {\n");
            retVal.Append("         this.fsm.takeEvent( evt);\n");
            retVal.Append("      }\n\n");

        }

        /// <summary>
        /// WriteOnEntryMethodCode() is the default method that writes the code for the StateName__onEntry()
        /// actions of a class if it is missing from the Object Model namespace. You can override this in 
        /// the developer environment by writing your own version with the same signature and using the 
        /// <ref>setWriteOnEntryMethodCode</ref> method to register it with the code generator.
        /// </summary>
        /// <param name="theSTT">The theSTT<see cref="FSM_STT"/>.</param>
        /// <param name="retVal">The retVal<see cref="StringBuilder"/>.</param>
        /// <param name="state">The state<see cref="STT_State"/>.</param>
        private static void WriteOnEntryMethodCode(FSM_STT theSTT, StringBuilder retVal, STT_State state) {
            retVal.Append("      // Method for Entry \n\n");
            retVal.Append("      public void ").Append(state.stateName);
            retVal.Append("__onEntry(FSM_Event evt)\n");
            retVal.Append("      {\n");
            retVal.Append("         Debug.WriteLine( \"");
            retVal.Append(theSTT.refClassName).Append(".").Append(state.stateName);
            retVal.Append("__onEntry() Executing in response to event \" + evt.getEventName());\n");
            if(theSTT.getDeleteWhenEndStateReached() && state.getIsFinalState()) {
                retVal.Append("         // delete references as this is an end state and the STT demands it\n");
                retVal.Append("         this.dereferenceFSM();\n");
            }
            retVal.Append("      }\n\n");
        }

        /// <summary>
        /// The WriteTransMethodCode.
        /// </summary>
        /// <param name="theSTT">The theSTT<see cref="FSM_STT"/>.</param>
        /// <param name="retVal">The retVal<see cref="StringBuilder"/>.</param>
        /// <param name="state">The state<see cref="STT_State"/>.</param>
        /// <param name="trans">The trans<see cref="STT_Transition"/>.</param>
        private static void WriteTransMethodCode(FSM_STT theSTT, StringBuilder retVal, STT_State state, STT_Transition trans) {
            retVal.Append("      // Method for Transition ").Append(trans.getInstanceName()).Append("\n\n");
            retVal.Append("      public void ").Append(state.getStateName()).Append("__");
            retVal.Append(trans.getEventName()).Append("(FSM_Event evt)\n");
            retVal.Append("      {\n");
            retVal.Append("         Debug.WriteLine( \"");
            retVal.Append(theSTT.getRefClassName()).Append(".").Append(state.getStateName());
            retVal.Append("__").Append(trans.getEventName()).Append("() Executing in response to event \" + evt.getEventName());\n");
            retVal.Append("      }\n\n");
        }

        /// <summary>
        /// The WriteTransGuardMethodCode.
        /// </summary>
        /// <param name="theSTT">The theSTT<see cref="FSM_STT"/>.</param>
        /// <param name="retVal">The retVal<see cref="StringBuilder"/>.</param>
        /// <param name="state">The state<see cref="STT_State"/>.</param>
        /// <param name="trans">The trans<see cref="STT_Transition"/>.</param>
        private static void WriteTransGuardMethodCode(FSM_STT theSTT, StringBuilder retVal, STT_State state, STT_Transition trans) {
            retVal.Append("      // GuardMethod for transition ").Append(trans.getInstanceName()).Append("\n\n");
            retVal.Append("      public bool ").Append(state.getStateName()).Append("__");
            retVal.Append(trans.getEventName()).Append("Guard(FSM_Event evt)\n");
            retVal.Append("      {\n");
            retVal.Append("         Debug.WriteLine( \"").Append(theSTT.getRefClassName()).Append(".");
            retVal.Append(state.getStateName()).Append("__").Append(trans.getEventName());
            retVal.Append("Guard() allowing event \" + evt.getEventName());\n");
            retVal.Append("         return true;\n");
            retVal.Append("      }\n");
        }

        /// <summary>
        /// The WriteOnExitMethodCode.
        /// </summary>
        /// <param name="theSTT">The theSTT<see cref="FSM_STT"/>.</param>
        /// <param name="retVal">The retVal<see cref="StringBuilder"/>.</param>
        /// <param name="state">The state<see cref="STT_State"/>.</param>
        private static void WriteOnExitMethodCode(FSM_STT theSTT, StringBuilder retVal, STT_State state) {
            retVal.Append("      // Method for Exit \n\n");
            retVal.Append("      public void ").Append(state.getStateName());
            retVal.Append("__onExit(FSM_Event evt)\n");
            retVal.Append("      {\n");
            retVal.Append("         Debug.WriteLine( \"");
            retVal.Append(theSTT.getRefClassName()).Append(".").Append(state.getStateName());
            retVal.Append("__onExit() Executing in response to event \" + evt.getEventName());\n");
            retVal.Append("      }\n\n");
        }

        /// <summary>
        /// The writeCodeToFile.
        /// </summary>
        /// <param name="theSTT">The theSTT<see cref="FSM_STT"/>.</param>
        public static void writeCodeToFile(string fileDir, FSM_STT theSTT) {
            // File.Delete(@".\NewCode.txt");
            // if filedir doesn't have trailing slash, put one in.
            fileDir = fileDir + "\\";
            string fileName = fileDir + theSTT.getRefClassName() + ".csx";
            File.Delete(fileName);
            File.WriteAllText(fileName, createOMCodeFromSTT(theSTT));
        }
    }
}
